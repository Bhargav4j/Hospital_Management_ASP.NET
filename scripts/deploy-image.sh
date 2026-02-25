#!/bin/bash
set -e
set -o pipefail

echo "========================================"
echo "AWS ECS Fargate Deployment Script"
echo "========================================"
echo ""

# Prompt for deployment configuration
read -p "Enter AWS region (e.g., us-east-1): " AWS_REGION
read -p "Enter ECS cluster name (e.g., clinic-management-cluster): " CLUSTER_NAME
read -p "Enter VPC ID (e.g., vpc-0abc123def456): " VPC_ID
read -p "Enter Subnet IDs comma-separated (e.g., subnet-0abc123,subnet-0def456): " SUBNETS_INPUT
read -p "Enter Security Group ID (e.g., sg-0abc123def): " SECURITY_GROUP
read -p "Enter Docker image URI (e.g., 123456789.dkr.ecr.us-east-1.amazonaws.com/clinic-management:latest): " IMAGE_URI

# Parse subnets
IFS=',' read -ra SUBNETS <<< "$SUBNETS_INPUT"
SUBNET_1=${SUBNETS[0]}
SUBNET_2=${SUBNETS[1]:-$SUBNET_1}

echo ""
echo "Getting AWS Account ID..."
ACCOUNT_ID=$(aws sts get-caller-identity --query Account --output text)
echo "Account ID: $ACCOUNT_ID"

echo ""
echo "Checking if ECS cluster exists..."
aws ecs describe-clusters --clusters $CLUSTER_NAME --region $AWS_REGION 2>/dev/null || {
  echo "Cluster not found. Creating ECS cluster: $CLUSTER_NAME"
  aws ecs create-cluster --cluster-name $CLUSTER_NAME --region $AWS_REGION
}

echo ""
read -p "Do you need a load balancer for this service? (y/n): " NEED_LB

if [[ "$NEED_LB" =~ ^[Yy]$ ]]; then
  echo ""
  echo "Creating Application Load Balancer..."
  
  ALB_NAME="clinic-management-alb"
  TG_NAME="clinic-management-tg"
  
  # Create ALB
  ALB_ARN=$(aws elbv2 create-load-balancer \
    --name $ALB_NAME \
    --subnets $SUBNET_1 $SUBNET_2 \
    --security-groups $SECURITY_GROUP \
    --scheme internet-facing \
    --type application \
    --region $AWS_REGION \
    --query 'LoadBalancers[0].LoadBalancerArn' \
    --output text 2>/dev/null || \
    aws elbv2 describe-load-balancers \
      --names $ALB_NAME \
      --region $AWS_REGION \
      --query 'LoadBalancers[0].LoadBalancerArn' \
      --output text)
  
  echo "ALB ARN: $ALB_ARN"
  
  # Create Target Group with target-type ip (required for Fargate)
  TARGET_GROUP_ARN=$(aws elbv2 create-target-group \
    --name $TG_NAME \
    --protocol HTTP \
    --port 8080 \
    --vpc-id $VPC_ID \
    --target-type ip \
    --health-check-enabled \
    --health-check-path /health \
    --health-check-interval-seconds 30 \
    --health-check-timeout-seconds 5 \
    --healthy-threshold-count 2 \
    --unhealthy-threshold-count 3 \
    --region $AWS_REGION \
    --query 'TargetGroups[0].TargetGroupArn' \
    --output text 2>/dev/null || \
    aws elbv2 describe-target-groups \
      --names $TG_NAME \
      --region $AWS_REGION \
      --query 'TargetGroups[0].TargetGroupArn' \
      --output text)
  
  echo "Target Group ARN: $TARGET_GROUP_ARN"
  
  # Create listener
  aws elbv2 create-listener \
    --load-balancer-arn $ALB_ARN \
    --protocol HTTP \
    --port 80 \
    --default-actions Type=forward,TargetGroupArn=$TARGET_GROUP_ARN \
    --region $AWS_REGION 2>/dev/null || echo "Listener already exists"
  
  # Get ALB DNS name
  ALB_DNS=$(aws elbv2 describe-load-balancers \
    --load-balancer-arns $ALB_ARN \
    --region $AWS_REGION \
    --query 'LoadBalancers[0].DNSName' \
    --output text)
  
  echo "ALB DNS Name: $ALB_DNS"
else
  echo "Skipping load balancer creation."
  TARGET_GROUP_ARN=""
fi

echo ""
echo "Preparing ECS task definition..."
cp ecs/task-definition.json ecs/task-definition-temp.json

# Replace placeholders in task definition
sed -i "s|{{IMAGE_URI}}|$IMAGE_URI|g" ecs/task-definition-temp.json
sed -i "s|{{AWS_REGION}}|$AWS_REGION|g" ecs/task-definition-temp.json
sed -i "s|{{ACCOUNT_ID}}|$ACCOUNT_ID|g" ecs/task-definition-temp.json

echo "Registering task definition..."
TASK_DEF_ARN=$(aws ecs register-task-definition \
  --cli-input-json file://ecs/task-definition-temp.json \
  --region $AWS_REGION \
  --query 'taskDefinition.taskDefinitionArn' \
  --output text)

echo "Task Definition ARN: $TASK_DEF_ARN"

echo ""
echo "Preparing ECS service definition..."
cp ecs/service-definition.json ecs/service-definition-temp.json

# Replace placeholders in service definition
sed -i "s|{{CLUSTER_NAME}}|$CLUSTER_NAME|g" ecs/service-definition-temp.json
sed -i "s|{{SUBNET_1}}|$SUBNET_1|g" ecs/service-definition-temp.json
sed -i "s|{{SUBNET_2}}|$SUBNET_2|g" ecs/service-definition-temp.json
sed -i "s|{{SECURITY_GROUP}}|$SECURITY_GROUP|g" ecs/service-definition-temp.json
sed -i "s|{{AWS_REGION}}|$AWS_REGION|g" ecs/service-definition-temp.json

if [[ "$NEED_LB" =~ ^[Yy]$ ]]; then
  sed -i "s|{{TARGET_GROUP_ARN}}|$TARGET_GROUP_ARN|g" ecs/service-definition-temp.json
else
  # Remove loadBalancers section if no LB needed
  jq 'del(.loadBalancers) | del(.healthCheckGracePeriodSeconds)' ecs/service-definition-temp.json > ecs/service-definition-temp2.json
  mv ecs/service-definition-temp2.json ecs/service-definition-temp.json
fi

SERVICE_NAME="clinic-management-service"

# Check if service exists
echo ""
echo "Checking if service exists..."
EXISTING_SERVICE=$(aws ecs describe-services \
  --cluster $CLUSTER_NAME \
  --services $SERVICE_NAME \
  --region $AWS_REGION \
  --query 'services[0].serviceName' \
  --output text 2>/dev/null || echo "None")

if [ "$EXISTING_SERVICE" = "None" ] || [ "$EXISTING_SERVICE" = "" ]; then
  echo "Creating new ECS service..."
  aws ecs create-service \
    --cli-input-json file://ecs/service-definition-temp.json \
    --region $AWS_REGION
else
  echo "Updating existing ECS service..."
  aws ecs update-service \
    --cluster $CLUSTER_NAME \
    --service $SERVICE_NAME \
    --task-definition $TASK_DEF_ARN \
    --force-new-deployment \
    --region $AWS_REGION
fi

echo ""
echo "Waiting for service to stabilize..."
aws ecs wait services-stable \
  --cluster $CLUSTER_NAME \
  --services $SERVICE_NAME \
  --region $AWS_REGION

echo ""
echo "========================================"
echo "Deployment Complete!"
echo "========================================"
echo ""
echo "Service Details:"
aws ecs describe-services \
  --cluster $CLUSTER_NAME \
  --services $SERVICE_NAME \
  --region $AWS_REGION \
  --query 'services[0].{Name:serviceName,Status:status,DesiredCount:desiredCount,RunningCount:runningCount}'

echo ""
echo "CloudWatch Logs:"
echo "  Log Group: /ecs/clinic-management"
echo "  Region: $AWS_REGION"
echo ""

if [[ "$NEED_LB" =~ ^[Yy]$ ]]; then
  echo "Application URL:"
  echo "  http://$ALB_DNS"
  echo ""
fi

echo "To view logs:"
echo "  aws logs tail /ecs/clinic-management --follow --region $AWS_REGION"
echo ""

# Cleanup temp files
rm -f ecs/task-definition-temp.json ecs/service-definition-temp.json

echo "Deployment script completed successfully!"