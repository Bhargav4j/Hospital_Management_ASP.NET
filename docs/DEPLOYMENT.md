# Clinic Management Application - Deployment Guide

## Table of Contents

1. [Overview](#overview)
2. [Prerequisites](#prerequisites)
3. [Local Development Setup](#local-development-setup)
4. [Docker Deployment](#docker-deployment)
5. [AWS ECS Fargate Deployment](#aws-ecs-fargate-deployment)
6. [Configuration Management](#configuration-management)
7. [Security Considerations](#security-considerations)
8. [Monitoring and Logging](#monitoring-and-logging)
9. [Troubleshooting](#troubleshooting)
10. [Scaling and Performance](#scaling-and-performance)

---

## Overview

This guide provides comprehensive instructions for deploying the Clinic Management application, a .NET 8.0 ASP.NET Core web application with AWS integrations including CloudWatch, Systems Manager Parameter Store, and S3.

### Application Architecture

- **Framework**: .NET 8.0 ASP.NET Core
- **Application Type**: Razor Pages Web Application
- **Port**: 8080 (HTTP)
- **Health Check Endpoint**: `/health`
- **Logging**: Serilog with CloudWatch integration
- **Configuration**: AWS Systems Manager Parameter Store
- **Storage**: AWS S3
- **Caching**: Redis (optional)

---

## Prerequisites

### Local Development

- .NET 8.0 SDK or later
- Docker Desktop (for containerization)
- Visual Studio 2022, VS Code, or Rider
- Git

### Docker Deployment

- Docker Engine 20.10+
- Docker Compose 2.0+
- 2GB+ available memory
- 10GB+ available disk space

### AWS ECS Fargate Deployment

#### Required AWS Services

- AWS Account with appropriate permissions
- AWS CLI v2 installed and configured
- AWS VPC with public/private subnets
- Security groups configured for HTTP/HTTPS traffic
- IAM roles for ECS task execution and task permissions

#### Required IAM Roles

**1. ecsTaskExecutionRole** (managed by AWS)
```json
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Principal": {
        "Service": "ecs-tasks.amazonaws.com"
      },
      "Action": "sts:AssumeRole"
    }
  ]
}
```

Attach managed policies:
- `AmazonECSTaskExecutionRolePolicy`
- `CloudWatchLogsFullAccess`

**2. ecsTaskRole** (custom for application permissions)
```json
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Action": [
        "ssm:GetParameter",
        "ssm:GetParameters",
        "ssm:GetParametersByPath"
      ],
      "Resource": "arn:aws:ssm:*:*:parameter/clinic-management/*"
    },
    {
      "Effect": "Allow",
      "Action": [
        "s3:GetObject",
        "s3:PutObject",
        "s3:DeleteObject",
        "s3:ListBucket"
      ],
      "Resource": [
        "arn:aws:s3:::hospwithoutdbcontcmp-files",
        "arn:aws:s3:::hospwithoutdbcontcmp-files/*"
      ]
    },
    {
      "Effect": "Allow",
      "Action": [
        "logs:CreateLogGroup",
        "logs:CreateLogStream",
        "logs:PutLogEvents"
      ],
      "Resource": "arn:aws:logs:*:*:log-group:/ecs/clinic-management:*"
    }
  ]
}
```

#### Network Requirements

- **VPC**: Dedicated VPC or use default VPC
- **Subnets**: At least 2 subnets in different availability zones
- **Security Group**: Allow inbound traffic on port 8080 (or 80 if using ALB)
- **Internet Gateway**: Required for FARGATE tasks with public IPs

#### CloudWatch Log Group Setup

Create the log group before deployment:
```bash
aws logs create-log-group --log-group-name /ecs/clinic-management --region us-east-1
```

---

## Local Development Setup

### 1. Clone the Repository

```bash
git clone <repository-url>
cd HospContainerRaviCmp
```

### 2. Restore Dependencies

```bash
cd ClinicManagement
dotnet restore
```

### 3. Update Configuration

Edit `src/ClinicManagement.Web/appsettings.Development.json`:

```json
{
  "AWS": {
    "Region": "us-east-1",
    "SystemsManager": {
      "Enabled": false
    },
    "S3": {
      "Enabled": false
    },
    "Redis": {
      "Enabled": false
    },
    "CloudWatch": {
      "Enabled": false
    },
    "FeatureToggle": {
      "RequireAwsEnvironment": false
    }
  }
}
```

### 4. Run the Application

```bash
cd src/ClinicManagement.Web
dotnet run
```

Access the application at `http://localhost:5000` or `https://localhost:5001`.

---

## Docker Deployment

### 1. Build Docker Image

From the repository root:

```bash
docker build -f Dockerfile -t clinic-management:latest .
```

### 2. Run with Docker Compose

```bash
docker-compose up -d
```

Access the application at `http://localhost:8080`.

### 3. View Logs

```bash
docker-compose logs -f clinic-management-app
```

### 4. Stop Services

```bash
docker-compose down
```

---

## AWS ECS Fargate Deployment

### Step 1: Build and Push Docker Image

#### Using Linux/macOS

```bash
chmod +x scripts/build-push.sh
./scripts/build-push.sh
```

#### Using Windows

```cmd
scripts\build-push.bat
```

**Script will prompt for:**
- Registry type (AWS ECR or Docker Hub)
- AWS region (for ECR)
- AWS Account ID (for ECR)
- ECR repository name
- Docker Hub credentials (for Docker Hub)
- Image tag

### Step 2: Prepare AWS Infrastructure

#### Create VPC and Subnets (if not exists)

```bash
# Create VPC
VPC_ID=$(aws ec2 create-vpc --cidr-block 10.0.0.0/16 --region us-east-1 --query 'Vpc.VpcId' --output text)

# Create subnets
SUBNET_1=$(aws ec2 create-subnet --vpc-id $VPC_ID --cidr-block 10.0.1.0/24 --availability-zone us-east-1a --query 'Subnet.SubnetId' --output text)
SUBNET_2=$(aws ec2 create-subnet --vpc-id $VPC_ID --cidr-block 10.0.2.0/24 --availability-zone us-east-1b --query 'Subnet.SubnetId' --output text)

# Create Internet Gateway
IGW_ID=$(aws ec2 create-internet-gateway --region us-east-1 --query 'InternetGateway.InternetGatewayId' --output text)
aws ec2 attach-internet-gateway --vpc-id $VPC_ID --internet-gateway-id $IGW_ID --region us-east-1
```

#### Create Security Group

```bash
SG_ID=$(aws ec2 create-security-group \
  --group-name clinic-management-sg \
  --description "Security group for Clinic Management application" \
  --vpc-id $VPC_ID \
  --region us-east-1 \
  --query 'GroupId' \
  --output text)

# Allow HTTP traffic
aws ec2 authorize-security-group-ingress \
  --group-id $SG_ID \
  --protocol tcp \
  --port 8080 \
  --cidr 0.0.0.0/0 \
  --region us-east-1
```

#### Configure AWS Systems Manager Parameters

Store sensitive configuration in AWS Systems Manager Parameter Store:

```bash
# Database connection string (if applicable)
aws ssm put-parameter \
  --name "/clinic-management/ConnectionStrings/DefaultConnection" \
  --value "your-connection-string" \
  --type "SecureString" \
  --region us-east-1

# Redis connection string (if using Redis)
aws ssm put-parameter \
  --name "/clinic-management/AWS/Redis/ConnectionString" \
  --value "your-redis-connection-string" \
  --type "SecureString" \
  --region us-east-1
```

### Step 3: Deploy to ECS

#### Using Linux/macOS

```bash
chmod +x scripts/deploy-image.sh
./scripts/deploy-image.sh
```

#### Using Windows

```cmd
scripts\deploy-image.bat
```

**Script will prompt for:**
- AWS region
- ECS cluster name
- VPC ID
- Subnet IDs (comma-separated)
- Security Group ID
- Docker image URI
- Load balancer requirement (y/n)

**The script will automatically:**
- Create ECS cluster (if not exists)
- Create Application Load Balancer and Target Group (if requested)
- Register ECS task definition
- Create or update ECS service
- Wait for service to stabilize
- Display service details and URLs

### Step 4: Verify Deployment

#### Check Service Status

```bash
aws ecs describe-services \
  --cluster clinic-management-cluster \
  --services clinic-management-service \
  --region us-east-1
```

#### Check Task Status

```bash
aws ecs list-tasks \
  --cluster clinic-management-cluster \
  --service-name clinic-management-service \
  --region us-east-1
```

#### View Logs

```bash
aws logs tail /ecs/clinic-management --follow --region us-east-1
```

---

## ECS Task Definition Explained

### CPU and Memory Configuration

Fargate supports specific CPU/memory combinations:

| CPU (vCPU) | Memory Options (MB) |
|------------|---------------------|
| 256 (.25)  | 512, 1024, 2048 |
| 512 (.5)   | 1024, 2048, 3072, 4096 |
| 1024 (1)   | 2048-8192 (1GB increments) |
| 2048 (2)   | 4096-16384 (1GB increments) |
| 4096 (4)   | 8192-30720 (1GB increments) |

**Default Configuration:** CPU: 512, Memory: 1024

### Network Mode

Fargate requires `awsvpc` network mode, which provides each task with its own elastic network interface (ENI).

### Container Health Checks

Health checks monitor application health:

```json
"healthCheck": {
  "command": ["CMD-SHELL", "wget --no-verbose --tries=1 --spider http://localhost:8080/health || exit 1"],
  "interval": 30,
  "timeout": 5,
  "retries": 3,
  "startPeriod": 60
}
```

---

## ECS Service Configuration

### Launch Type

The service uses **FARGATE** launch type for serverless container execution.

### Desired Count

Default: 2 tasks for high availability. Adjust based on load requirements.

### Deployment Configuration

- **Maximum Percent**: 200 (allows 2x desired count during deployment)
- **Minimum Healthy Percent**: 50 (maintains at least 50% of tasks during deployment)

### Load Balancer Integration

When using Application Load Balancer:
- Target type: `ip` (required for Fargate awsvpc mode)
- Health check path: `/health`
- Health check grace period: 300 seconds

---

## Configuration Management

### Environment Variables

Key environment variables:

| Variable | Description | Default |
|----------|-------------|----------|
| ASPNETCORE_ENVIRONMENT | Application environment | Production |
| ASPNETCORE_URLS | Kestrel binding URL | http://+:8080 |
| AWS__Region | AWS region | us-east-1 |
| AWS__SystemsManager__Enabled | Enable SSM Parameter Store | true |
| AWS__S3__Enabled | Enable S3 integration | true |
| AWS__CloudWatch__Enabled | Enable CloudWatch logging | true |

### AWS Systems Manager Parameter Store

The application automatically loads configuration from Parameter Store at path `/clinic-management/` with 5-minute refresh interval.

---

## Security Considerations

### Container Security

- **Non-root User**: Application runs as `appuser` (non-root)
- **Read-only Filesystem**: Consider mounting volumes as read-only where possible
- **Security Scanning**: Regularly scan images for vulnerabilities

### Network Security

- **Security Groups**: Restrict inbound traffic to necessary ports only
- **Private Subnets**: Consider running tasks in private subnets with NAT Gateway
- **VPC Endpoints**: Use VPC endpoints for AWS services to avoid internet traffic

### IAM Security

- **Least Privilege**: Grant minimum required permissions to task role
- **Secrets Management**: Store sensitive data in AWS Secrets Manager or SSM Parameter Store
- **Credential Rotation**: Implement regular credential rotation

---

## Monitoring and Logging

### CloudWatch Logs

All application logs are sent to CloudWatch Logs:
- Log Group: `/ecs/clinic-management`
- Log Stream Prefix: `ecs`

### Application Insights (Optional)

Consider enabling Application Insights for advanced monitoring:

```bash
aws ssm put-parameter \
  --name "/clinic-management/ApplicationInsights/ConnectionString" \
  --value "your-connection-string" \
  --type "SecureString" \
  --region us-east-1
```

### Health Monitoring

Health check endpoint: `http://<host>:8080/health`

---

## Troubleshooting

### Common ECS Issues

#### Task Fails to Start

**Symptoms**: Tasks transition from PENDING to STOPPED immediately

**Solutions**:
1. Check CloudWatch logs for application errors
2. Verify IAM role permissions
3. Ensure container image is accessible
4. Check CPU/memory limits

```bash
# View task stopped reason
aws ecs describe-tasks \
  --cluster clinic-management-cluster \
  --tasks <task-id> \
  --region us-east-1 \
  --query 'tasks[0].stoppedReason'
```

#### Health Check Failures

**Symptoms**: Tasks fail health checks and restart continuously

**Solutions**:
1. Increase `startPeriod` in health check configuration
2. Verify application starts successfully
3. Check application logs for startup errors
4. Test health endpoint locally

#### Network Connectivity Issues

**Symptoms**: Tasks cannot access external services or AWS APIs

**Solutions**:
1. Verify security group allows outbound traffic
2. Ensure subnets have route to Internet Gateway (for public IPs)
3. Check VPC endpoints configuration
4. Verify IAM role has necessary permissions

### Application-Specific Issues

#### AWS Systems Manager Parameter Store Not Loading

**Check**:
1. Verify IAM task role has `ssm:GetParameter*` permissions
2. Ensure parameters exist at path `/clinic-management/`
3. Check AWS region configuration
4. Review application logs for SSM errors

#### S3 Access Denied

**Check**:
1. Verify IAM task role has S3 permissions
2. Ensure bucket name is correct
3. Check bucket policy allows access from task role
4. Verify AWS region matches bucket region

---

## Scaling and Performance

### Service Auto Scaling

Configure ECS Service Auto Scaling based on metrics:

```bash
# Register scalable target
aws application-autoscaling register-scalable-target \
  --service-namespace ecs \
  --resource-id service/clinic-management-cluster/clinic-management-service \
  --scalable-dimension ecs:service:DesiredCount \
  --min-capacity 2 \
  --max-capacity 10 \
  --region us-east-1

# Create scaling policy
aws application-autoscaling put-scaling-policy \
  --service-namespace ecs \
  --resource-id service/clinic-management-cluster/clinic-management-service \
  --scalable-dimension ecs:service:DesiredCount \
  --policy-name cpu-scaling-policy \
  --policy-type TargetTrackingScaling \
  --target-tracking-scaling-policy-configuration file://scaling-policy.json \
  --region us-east-1
```

**scaling-policy.json**:
```json
{
  "TargetValue": 70.0,
  "PredefinedMetricSpecification": {
    "PredefinedMetricType": "ECSServiceAverageCPUUtilization"
  },
  "ScaleInCooldown": 300,
  "ScaleOutCooldown": 60
}
```

### Blue/Green Deployments

For zero-downtime deployments, consider using CodeDeploy with ECS:

1. Create CodeDeploy application and deployment group
2. Configure task definition with multiple task sets
3. Use deployment configuration for traffic shifting
4. Implement automated rollback on failures

---

## Additional Resources

- [AWS ECS Documentation](https://docs.aws.amazon.com/ecs/)
- [AWS Fargate Documentation](https://docs.aws.amazon.com/AmazonECS/latest/developerguide/AWS_Fargate.html)
- [.NET on AWS](https://aws.amazon.com/developer/language/net/)
- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)

---

**Last Updated**: 2026-02-25

**Version**: 1.0.0