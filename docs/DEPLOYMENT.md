# HospitalContainer18Cmp - AWS ECS Fargate Deployment Guide

## Table of Contents
1. [Prerequisites](#prerequisites)
2. [Local Development Setup](#local-development-setup)
3. [Docker Deployment](#docker-deployment)
4. [AWS ECS Fargate Prerequisites](#aws-ecs-fargate-prerequisites)
5. [ECS Fargate Setup](#ecs-fargate-setup)
6. [ECS Task Definition Explained](#ecs-task-definition-explained)
7. [ECS Service Configuration](#ecs-service-configuration)
8. [Deployment Walkthrough](#deployment-walkthrough)
9. [Troubleshooting](#troubleshooting)
10. [Scaling and Management](#scaling-and-management)
11. [Security Considerations](#security-considerations)

---

## Prerequisites

### Required Tools
- **.NET SDK 8.0 or higher**: [Download](https://dotnet.microsoft.com/download)
- **Docker Desktop**: [Download](https://www.docker.com/products/docker-desktop)
- **AWS CLI v2**: [Download](https://aws.amazon.com/cli/)
- **Git**: Version control

### Verify Installations
```bash
dotnet --version
docker --version
aws --version
```

---

## Local Development Setup

### 1. Clone Repository
```bash
git clone <repository-url>
cd HospitalContainer18Cmp
```

### 2. Restore Dependencies
```bash
dotnet restore
```

### 3. Build Application
```bash
dotnet build -c Release
```

### 4. Run Locally
```bash
dotnet run --project HospitalContainer18Cmp.csproj
```

Application should be running at `http://localhost:8080`

### 5. Test Health Endpoint
```bash
curl http://localhost:8080/health
```

---

## Docker Deployment

### Build Docker Image
```bash
docker build -t hospitalcontainer18cmp:latest -f Dockerfile .
```

### Run Container Locally
```bash
docker run -d \
  --name hospitalcontainer18cmp \
  -p 8080:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  hospitalcontainer18cmp:latest
```

### Test Container
```bash
curl http://localhost:8080/health
```

### Using Docker Compose
```bash
docker-compose up -d
docker-compose logs -f
docker-compose down
```

---

## AWS ECS Fargate Prerequisites

### 1. AWS Account Setup
- Active AWS account with appropriate permissions
- IAM user with ECS, ECR, EC2, and CloudWatch permissions

### 2. Configure AWS CLI
```bash
aws configure
# Enter:
# - AWS Access Key ID
# - AWS Secret Access Key
# - Default region (e.g., us-east-1)
# - Default output format (json)
```

### 3. Create VPC and Networking (if not exists)
```bash
# Create VPC
aws ec2 create-vpc --cidr-block 10.0.0.0/16 --region us-east-1

# Create Subnets (at least 2 in different AZs)
aws ec2 create-subnet --vpc-id vpc-xxxxx --cidr-block 10.0.1.0/24 --availability-zone us-east-1a
aws ec2 create-subnet --vpc-id vpc-xxxxx --cidr-block 10.0.2.0/24 --availability-zone us-east-1b

# Create Internet Gateway
aws ec2 create-internet-gateway
aws ec2 attach-internet-gateway --vpc-id vpc-xxxxx --internet-gateway-id igw-xxxxx

# Create Route Table
aws ec2 create-route-table --vpc-id vpc-xxxxx
aws ec2 create-route --route-table-id rtb-xxxxx --destination-cidr-block 0.0.0.0/0 --gateway-id igw-xxxxx
```

### 4. Create Security Group
```bash
aws ec2 create-security-group \
  --group-name hospitalcontainer18cmp-sg \
  --description "Security group for HospitalContainer18Cmp" \
  --vpc-id vpc-xxxxx

# Allow inbound HTTP traffic
aws ec2 authorize-security-group-ingress \
  --group-id sg-xxxxx \
  --protocol tcp \
  --port 8080 \
  --cidr 0.0.0.0/0

# Allow ALB health checks
aws ec2 authorize-security-group-ingress \
  --group-id sg-xxxxx \
  --protocol tcp \
  --port 80 \
  --cidr 0.0.0.0/0
```

### 5. Create IAM Roles

#### ECS Task Execution Role
```bash
aws iam create-role \
  --role-name ecsTaskExecutionRole \
  --assume-role-policy-document '{
    "Version": "2012-10-17",
    "Statement": [{
      "Effect": "Allow",
      "Principal": {"Service": "ecs-tasks.amazonaws.com"},
      "Action": "sts:AssumeRole"
    }]
  }'

aws iam attach-role-policy \
  --role-name ecsTaskExecutionRole \
  --policy-arn arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy
```

#### ECS Task Role (optional, for application permissions)
```bash
aws iam create-role \
  --role-name ecsTaskRole \
  --assume-role-policy-document '{
    "Version": "2012-10-17",
    "Statement": [{
      "Effect": "Allow",
      "Principal": {"Service": "ecs-tasks.amazonaws.com"},
      "Action": "sts:AssumeRole"
    }]
  }'
```

### 6. Create CloudWatch Log Group
```bash
aws logs create-log-group \
  --log-group-name /ecs/hospitalcontainer18cmp \
  --region us-east-1
```

---

## ECS Fargate Setup

### 1. Create ECR Repository
```bash
aws ecr create-repository \
  --repository-name hospitalcontainer18cmp \
  --region us-east-1
```

### 2. Build and Push Image

**Linux/macOS:**
```bash
chmod +x scripts/build-push.sh
./scripts/build-push.sh
```

**Windows:**
```cmd
scripts\build-push.bat
```

Follow the prompts:
1. Select registry type (1 for AWS ECR)
2. Enter AWS region
3. Enter AWS account ID
4. Enter ECR repository name
5. Enter image tag (default: latest)

---

## ECS Task Definition Explained

### Key Components

#### Launch Type Configuration
```json
"requiresCompatibilities": ["FARGATE"],
"networkMode": "awsvpc"
```
- **FARGATE**: Serverless container execution
- **awsvpc**: Each task gets its own ENI

#### CPU and Memory
```json
"cpu": "512",
"memory": "1024"
```
**Valid Fargate CPU/Memory Combinations:**
- CPU: 256 (.25 vCPU) → Memory: 512, 1024, 2048 MB
- CPU: 512 (.5 vCPU) → Memory: 1024, 2048, 3072, 4096 MB
- CPU: 1024 (1 vCPU) → Memory: 2048-8192 MB
- CPU: 2048 (2 vCPU) → Memory: 4096-16384 MB
- CPU: 4096 (4 vCPU) → Memory: 8192-30720 MB

#### Execution Role
```json
"executionRoleArn": "arn:aws:iam::{{ACCOUNT_ID}}:role/ecsTaskExecutionRole"
```
Allows ECS to:
- Pull images from ECR
- Write logs to CloudWatch
- Retrieve secrets from Secrets Manager/Parameter Store

#### Container Definition
```json
"containerDefinitions": [{
  "name": "hospitalcontainer18cmp",
  "image": "{{IMAGE_URI}}",
  "essential": true,
  "portMappings": [{"containerPort": 8080, "protocol": "tcp"}],
  "environment": [...],
  "healthCheck": {...},
  "logConfiguration": {...}
}]
```

---

## ECS Service Configuration

### Service Definition Components

#### Launch Type and Networking
```json
"launchType": "FARGATE",
"networkConfiguration": {
  "awsvpcConfiguration": {
    "subnets": ["subnet-xxxxx", "subnet-yyyyy"],
    "securityGroups": ["sg-xxxxx"],
    "assignPublicIp": "ENABLED"
  }
}
```

#### Deployment Configuration
```json
"deploymentConfiguration": {
  "maximumPercent": 200,
  "minimumHealthyPercent": 50,
  "deploymentCircuitBreaker": {
    "enable": true,
    "rollback": true
  }
}
```
- **maximumPercent**: Max tasks during deployment (200% = 2x desired count)
- **minimumHealthyPercent**: Min healthy tasks during deployment (50%)
- **deploymentCircuitBreaker**: Auto-rollback on failure

#### Load Balancer Integration
```json
"loadBalancers": [{
  "targetGroupArn": "arn:aws:elasticloadbalancing:...",
  "containerName": "hospitalcontainer18cmp",
  "containerPort": 8080
}],
"healthCheckGracePeriodSeconds": 300
```

---

## Deployment Walkthrough

### Step 1: Build and Push Docker Image
```bash
# Linux/macOS
./scripts/build-push.sh

# Windows
scripts\build-push.bat
```

### Step 2: Deploy to ECS Fargate
```bash
# Linux/macOS
chmod +x scripts/deploy-image.sh
./scripts/deploy-image.sh

# Windows
scripts\deploy-image.bat
```

### Deployment Script Prompts:
1. **AWS Region**: e.g., us-east-1
2. **ECS Cluster Name**: e.g., my-ecs-cluster
3. **VPC ID**: e.g., vpc-0abc123def456
4. **Subnet IDs**: Comma-separated, e.g., subnet-0abc123,subnet-0def456
5. **Security Group ID**: e.g., sg-0abc123def
6. **Docker Image URI**: Full ECR image URI
7. **Load Balancer**: y/n (script will create ALB if yes)

### Step 3: Verify Deployment
```bash
# Check service status
aws ecs describe-services \
  --cluster my-ecs-cluster \
  --services hospitalcontainer18cmp-service \
  --region us-east-1

# List running tasks
aws ecs list-tasks \
  --cluster my-ecs-cluster \
  --service-name hospitalcontainer18cmp-service \
  --region us-east-1

# View task details
aws ecs describe-tasks \
  --cluster my-ecs-cluster \
  --tasks <task-id> \
  --region us-east-1
```

### Step 4: Access Application
If load balancer was created:
```bash
# Get ALB DNS name
aws elbv2 describe-load-balancers \
  --names hospitalcontainer18cmp-alb \
  --region us-east-1 \
  --query 'LoadBalancers[0].DNSName' \
  --output text

# Access application
curl http://<alb-dns-name>/health
```

---

## Troubleshooting

### Common Issues

#### 1. Task Fails to Start
**Error**: Task stopped with reason "Essential container exited"

**Solutions**:
- Check CloudWatch logs: `/ecs/hospitalcontainer18cmp`
- Verify environment variables in task definition
- Ensure health check endpoint is accessible
- Check application startup errors

```bash
aws logs tail /ecs/hospitalcontainer18cmp --follow --region us-east-1
```

#### 2. Invalid CPU/Memory Combination
**Error**: "Invalid CPU or memory value specified"

**Solution**: Use valid Fargate combinations:
```json
"cpu": "512",
"memory": "1024"
```

#### 3. Network Issues
**Error**: "CannotPullContainerError"

**Solutions**:
- Verify subnets have internet access (Internet Gateway)
- Check security group allows outbound traffic
- Ensure ECS execution role has ECR permissions

#### 4. Health Check Failures
**Error**: Target health check failed

**Solutions**:
- Verify `/health` endpoint returns 200 OK
- Increase `healthCheckGracePeriodSeconds` for slow startup
- Check security group allows ALB → container traffic
- Review health check interval and timeout settings

```bash
# Test health endpoint from task
aws ecs execute-command \
  --cluster my-ecs-cluster \
  --task <task-id> \
  --container hospitalcontainer18cmp \
  --interactive \
  --command "curl http://localhost:8080/health"
```

#### 5. Service Not Reaching Steady State
**Solutions**:
- Check CloudWatch logs for application errors
- Verify task definition is valid
- Ensure desired count matches available resources
- Review deployment circuit breaker events

---

## Scaling and Management

### Manual Scaling
```bash
aws ecs update-service \
  --cluster my-ecs-cluster \
  --service hospitalcontainer18cmp-service \
  --desired-count 4 \
  --region us-east-1
```

### Auto Scaling
```bash
# Register scalable target
aws application-autoscaling register-scalable-target \
  --service-namespace ecs \
  --resource-id service/my-ecs-cluster/hospitalcontainer18cmp-service \
  --scalable-dimension ecs:service:DesiredCount \
  --min-capacity 2 \
  --max-capacity 10 \
  --region us-east-1

# Create scaling policy (CPU-based)
aws application-autoscaling put-scaling-policy \
  --service-namespace ecs \
  --resource-id service/my-ecs-cluster/hospitalcontainer18cmp-service \
  --scalable-dimension ecs:service:DesiredCount \
  --policy-name cpu-scaling-policy \
  --policy-type TargetTrackingScaling \
  --target-tracking-scaling-policy-configuration '{
    "TargetValue": 70.0,
    "PredefinedMetricSpecification": {
      "PredefinedMetricType": "ECSServiceAverageCPUUtilization"
    },
    "ScaleInCooldown": 60,
    "ScaleOutCooldown": 60
  }' \
  --region us-east-1
```

### Blue/Green Deployments
```bash
# Using AWS CodeDeploy
aws deploy create-deployment \
  --application-name hospitalcontainer18cmp-app \
  --deployment-group-name hospitalcontainer18cmp-dg \
  --deployment-config-name CodeDeployDefault.ECSAllAtOnce \
  --region us-east-1
```

### Rolling Updates
```bash
aws ecs update-service \
  --cluster my-ecs-cluster \
  --service hospitalcontainer18cmp-service \
  --force-new-deployment \
  --region us-east-1
```

---

## Security Considerations

### 1. Network Security
- Use private subnets with NAT Gateway for production
- Restrict security group ingress to ALB only
- Enable VPC Flow Logs for monitoring

### 2. IAM Best Practices
- Use separate task and execution roles
- Apply principle of least privilege
- Rotate credentials regularly
- Use IAM roles instead of hardcoded credentials

### 3. Secrets Management
```json
"secrets": [
  {
    "name": "DB_PASSWORD",
    "valueFrom": "arn:aws:secretsmanager:region:account-id:secret:db-password"
  }
]
```

### 4. Container Security
- Run as non-root user (already configured)
- Scan images for vulnerabilities
- Keep base images updated
- Use read-only root filesystem where possible

### 5. Logging and Monitoring
- Enable CloudWatch Container Insights
- Set up CloudWatch alarms for critical metrics
- Use AWS X-Ray for distributed tracing
- Configure log retention policies

```bash
# Enable Container Insights
aws ecs update-cluster-settings \
  --cluster my-ecs-cluster \
  --settings name=containerInsights,value=enabled \
  --region us-east-1
```

---

## Additional Resources

- [AWS ECS Fargate Documentation](https://docs.aws.amazon.com/AmazonECS/latest/developerguide/AWS_Fargate.html)
- [.NET on AWS](https://aws.amazon.com/developer/language/net/)
- [ASP.NET Core Best Practices](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/best-practices)
- [ECS Task Definition Parameters](https://docs.aws.amazon.com/AmazonECS/latest/developerguide/task_definition_parameters.html)

---

## Support

For issues or questions:
1. Check CloudWatch logs: `/ecs/hospitalcontainer18cmp`
2. Review ECS service events
3. Consult AWS ECS troubleshooting guide
4. Contact your DevOps team

---

**Last Updated**: 2026-02-18  
**Version**: 1.0.0  
**Platform**: AWS ECS Fargate  
**.NET Version**: 8.0