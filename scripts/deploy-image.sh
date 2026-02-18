#!/bin/bash
set -e
set -o pipefail

echo "=========================================="
echo "AWS ECS Fargate Deployment Script"
echo "=========================================="
echo ""

# Project configuration
PROJECT_NAME="hospitalcontainer18cmp"
TASK_FAMILY="hospitalcontainer18cmp-task"
SERVICE_NAME="hospitalcontainer18cmp-service"

# Prompt for AWS configuration
read -p "Enter AWS Region (e.g., us-east-1): " AWS_REGION
read -p "Enter ECS Cluster Name (e.g., my-ecs-cluster): " CLUSTER_NAME

echo ""
echo "=== Network Configuration ==="
read -p "Enter VPC ID (e.g., vpc-0abc123def456): " VPC_ID
read -p "Enter Subnet IDs comma-separated (e.g., subnet-0abc123,subnet-0def456): " SUBNET_IDS
read -p "Enter Security Group ID (e.g., sg-0abc123def): " SECURITY_GROUP

# Parse subnets
IFS=',' read -ra SUBNETS <<< "$SUBNET_IDS"
SUBNET_1="${SUBNETS[0]}"
SUBNET_2="${SUBNETS[1]:-$SUBNET_1}"

echo ""
read -p "Enter Docker Image URI (e.g., 123456789.dkr.ecr.us-east-1.amazonaws.com/app:latest): " IMAGE_URI

# Get AWS Account ID
echo ""
echo "Retrieving AWS Account ID..."
ACCOUNT_ID=$(aws sts get-caller-identity --query Account --output text)
echo "AWS Account ID: $ACCOUNT_ID"

# Check/create ECS cluster
echo ""
echo "Checking if ECS cluster exists..."
aws ecs describe-clusters --clusters "$CLUSTER_NAME" --region "$AWS_REGION" >/dev/null 2>&1 || {
    echo "Cluster does not exist. Creating ECS cluster..."
    aws ecs create-cluster --cluster-name "$CLUSTER_NAME" --region "$AWS_REGION"
}

# Load balancer configuration
echo ""
read -p "Do you need a load balancer for this service? (y/n): " NEED_LB

if [[ "$NEED_LB" =~ ^[Yy]$ ]]; then
    echo ""
    echo "=== Creating Application Load Balancer ==="
    
    ALB_NAME="$PROJECT_NAME-alb"
    TG_NAME="$PROJECT_NAME-tg"
    
    echo "Creating Application Load Balancer: $ALB_NAME"
    ALB_ARN=$(aws elbv2 create-load-balancer \
        --name "$ALB_NAME" \
        --subnets "$SUBNET_1" "$SUBNET_2" \
        --security-groups "$SECURITY_GROUP" \
        --scheme internet-facing \
        --type application \
        --ip-address-type ipv4 \
        --region "$AWS_REGION" \
        --query 'LoadBalancers[0].LoadBalancerArn' \
        --output text 2>/dev/null || aws elbv2 describe-load-balancers --names "$ALB_NAME" --region "$AWS_REGION" --query 'LoadBalancers[0].LoadBalancerArn' --output text)
    
    echo "Load Balancer ARN: $ALB_ARN"
    
    echo "Creating Target Group: $TG_NAME"
    TARGET_GROUP_ARN=$(aws elbv2 create-target-group \
        --name "$TG_NAME" \
        --protocol HTTP \
        --port 8080 \
        --vpc-id "$VPC_ID" \
        --target-type ip \
        --health-check-enabled \
        --health-check-path "/health" \
        --health-check-interval-seconds 30 \
        --health-check-timeout-seconds 5 \
        --healthy-threshold-count 2 \
        --unhealthy-threshold-count 3 \
        --region "$AWS_REGION" \
        --query 'TargetGroups[0].TargetGroupArn' \
        --output text 2>/dev/null || aws elbv2 describe-target-groups --names "$TG_NAME" --region "$AWS_REGION" --query 'TargetGroups[0].TargetGroupArn' --output text)
    
    echo "Target Group ARN: $TARGET_GROUP_ARN"
    
    echo "Creating ALB Listener..."
    aws elbv2 create-listener \
        --load-balancer-arn "$ALB_ARN" \
        --protocol HTTP \
        --port 80 \
        --default-actions Type=forward,TargetGroupArn="$TARGET_GROUP_ARN" \
        --region "$AWS_REGION" >/dev/null 2>&1 || echo "Listener already exists"
    
    ALB_DNS=$(aws elbv2 describe-load-balancers --load-balancer-arns "$ALB_ARN" --region "$AWS_REGION" --query 'LoadBalancers[0].DNSName' --output text)
    echo "Load Balancer DNS: $ALB_DNS"
else
    TARGET_GROUP_ARN=""
    echo "Skipping load balancer configuration"
fi

# Replace placeholders in task definition
echo ""
echo "=== Preparing Task Definition ==="
cp ecs/task-definition.json /tmp/task-definition.json

sed -i "s|{{IMAGE_URI}}|$IMAGE_URI|g" /tmp/task-definition.json
sed -i "s|{{AWS_REGION}}|$AWS_REGION|g" /tmp/task-definition.json
sed -i "s|{{ACCOUNT_ID}}|$ACCOUNT_ID|g" /tmp/task-definition.json

echo "Registering ECS task definition..."
TASK_DEF_ARN=$(aws ecs register-task-definition \
    --cli-input-json file:///tmp/task-definition.json \
    --region "$AWS_REGION" \
    --query 'taskDefinition.taskDefinitionArn' \
    --output text)

echo "Task Definition ARN: $TASK_DEF_ARN"

# Prepare service definition
echo ""
echo "=== Preparing Service Definition ==="
cp ecs/service-definition.json /tmp/service-definition.json

sed -i "s|{{CLUSTER_NAME}}|$CLUSTER_NAME|g" /tmp/service-definition.json
sed -i "s|{{SUBNET_1}}|$SUBNET_1|g" /tmp/service-definition.json
sed -i "s|{{SUBNET_2}}|$SUBNET_2|g" /tmp/service-definition.json
sed -i "s|{{SECURITY_GROUP}}|$SECURITY_GROUP|g" /tmp/service-definition.json

if [ -z "$TARGET_GROUP_ARN" ]; then
    # Remove loadBalancers section if no ALB
    jq 'del(.loadBalancers, .healthCheckGracePeriodSeconds)' /tmp/service-definition.json > /tmp/service-definition-final.json
else
    sed -i "s|{{TARGET_GROUP_ARN}}|$TARGET_GROUP_ARN|g" /tmp/service-definition.json
    cp /tmp/service-definition.json /tmp/service-definition-final.json
fi

# Check if service exists
echo ""
echo "=== Deploying Service ==="
SERVICE_EXISTS=$(aws ecs describe-services \
    --cluster "$CLUSTER_NAME" \
    --services "$SERVICE_NAME" \
    --region "$AWS_REGION" \
    --query 'services[0].serviceName' \
    --output text 2>/dev/null)

if [ "$SERVICE_EXISTS" = "$SERVICE_NAME" ]; then
    echo "Service exists. Updating service..."
    aws ecs update-service \
        --cluster "$CLUSTER_NAME" \
        --service "$SERVICE_NAME" \
        --task-definition "$TASK_DEF_ARN" \
        --desired-count 2 \
        --region "$AWS_REGION" \
        --force-new-deployment
else
    echo "Service does not exist. Creating service..."
    aws ecs create-service \
        --cli-input-json file:///tmp/service-definition-final.json \
        --region "$AWS_REGION"
fi

echo ""
echo "Waiting for service to stabilize..."
aws ecs wait services-stable \
    --cluster "$CLUSTER_NAME" \
    --services "$SERVICE_NAME" \
    --region "$AWS_REGION"

echo ""
echo "=== Deployment Verification ==="
aws ecs describe-services \
    --cluster "$CLUSTER_NAME" \
    --services "$SERVICE_NAME" \
    --region "$AWS_REGION" \
    --query 'services[0].[serviceName,status,runningCount,desiredCount]' \
    --output table

echo ""
echo "=========================================="
echo "Deployment Completed Successfully!"
echo "=========================================="
echo "Service Name: $SERVICE_NAME"
echo "Cluster: $CLUSTER_NAME"
echo "Region: $AWS_REGION"
[ -n "$ALB_DNS" ] && echo "Application URL: http://$ALB_DNS"
echo "CloudWatch Logs: /ecs/$PROJECT_NAME"
echo "========================================="