# AWS ECS Fargate Deployment Guide for HospWithoutDBContCmp

## Table of Contents
1. [Prerequisites](#prerequisites)
2. [Local Development Setup](#local-development-setup)
3. [Docker Image Build and Registry Push](#docker-image-build-and-registry-push)
4. [AWS ECS Fargate Prerequisites](#aws-ecs-fargate-prerequisites)
5. [AWS ECS Fargate Setup](#aws-ecs-fargate-setup)
6. [ECS Task Definition Explained](#ecs-task-definition-explained)
7. [ECS Service Configuration](#ecs-service-configuration)
8. [Deployment Walkthrough](#deployment-walkthrough)
9. [Monitoring and Logging](#monitoring-and-logging)
10. [Troubleshooting](#troubleshooting)
11. [Scaling and Performance](#scaling-and-performance)
12. [Security Best Practices](#security-best-practices)
13. [CI/CD Integration](#cicd-integration)

---

## Prerequisites

### Required Software
- **.NET 8.0 SDK** or later
- **Docker Desktop** (Windows/macOS) or Docker Engine (Linux)
- **AWS CLI v2** configured with appropriate credentials
- **Git** for version control
- **PowerShell** (Windows) or **Bash** (Linux/macOS)

### Required AWS Resources
- AWS Account with appropriate permissions
- IAM user with ECS, ECR, EC2, and CloudWatch permissions
- VPC with at least 2 subnets in different availability zones
- Security groups configured for container access
- IAM roles: `ecsTaskExecutionRole` and `ecsTaskRole`

### AWS CLI Configuration
```bash
# Configure AWS CLI
aws configure

# Verify configuration
aws sts get-caller-identity
```

---

## Local Development Setup

### 1. Clone and Build the Application

```bash
# Navigate to project directory
cd /modernize-data/studio-data/TNT1001/APP1018/transformed-code/15/studio-workspace/HospWithoutDBContCmp

# Restore NuGet packages
dotnet restore

# Build the application
dotnet build -c Release

# Run the application locally
dotnet run
```

The application will start on `http://localhost:8080`.

### 2. Test Locally with Docker Compose

```bash
# Build and start the container
docker-compose up --build

# Access the application
curl http://localhost:8080/health

# Stop the container
docker-compose down
```

---

## Docker Image Build and Registry Push

### Option 1: AWS Elastic Container Registry (ECR)

#### Linux/macOS
```bash
chmod +x scripts/build-push.sh
./scripts/build-push.sh
```

#### Windows
```cmd
scripts\build-push.bat
```

**Follow the interactive prompts:**
1. Select registry type: `1` (AWS ECR)
2. Enter AWS region: `us-east-1`
3. Enter AWS Account ID: `123456789012`
4. Enter ECR repository name: `hospwithoutdbcontcmp`
5. Enter image tag: `latest` (or version number)

The script will:
- Authenticate with AWS ECR
- Create the ECR repository if it doesn't exist
- Build the Docker image
- Push the image to ECR

**Example Output:**
```
Image successfully built and pushed:
123456789012.dkr.ecr.us-east-1.amazonaws.com/hospwithoutdbcontcmp:latest
```

### Option 2: Docker Hub

**Follow the interactive prompts:**
1. Select registry type: `2` (Docker Hub)
2. Enter Docker Hub username: `yourusername`
3. Enter Docker Hub password or access token
4. Enter image tag: `latest`

The script will authenticate with Docker Hub and push the image.

---

## AWS ECS Fargate Prerequisites

### 1. Create IAM Roles

#### ECS Task Execution Role

This role allows ECS to pull images from ECR and send logs to CloudWatch.

```bash
# Create trust policy
cat > ecs-task-execution-trust-policy.json << EOF
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
EOF

# Create role
aws iam create-role \
  --role-name ecsTaskExecutionRole \
  --assume-role-policy-document file://ecs-task-execution-trust-policy.json

# Attach managed policy
aws iam attach-role-policy \
  --role-name ecsTaskExecutionRole \
  --policy-arn arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy
```

#### ECS Task Role (Optional)

This role allows your application to access other AWS services.

```bash
# Create role
aws iam create-role \
  --role-name ecsTaskRole \
  --assume-role-policy-document file://ecs-task-execution-trust-policy.json

# Attach policies as needed (e.g., S3, DynamoDB)
aws iam attach-role-policy \
  --role-name ecsTaskRole \
  --policy-arn arn:aws:iam::aws:policy/AmazonS3ReadOnlyAccess
```

### 2. Configure VPC and Networking

#### Create VPC (if needed)
```bash
# Create VPC
VPC_ID=$(aws ec2 create-vpc \
  --cidr-block 10.0.0.0/16 \
  --query 'Vpc.VpcId' \
  --output text)

echo "VPC ID: $VPC_ID"

# Enable DNS hostnames
aws ec2 modify-vpc-attribute \
  --vpc-id $VPC_ID \
  --enable-dns-hostnames
```

#### Create Subnets
```bash
# Create subnet in AZ 1
SUBNET_1=$(aws ec2 create-subnet \
  --vpc-id $VPC_ID \
  --cidr-block 10.0.1.0/24 \
  --availability-zone us-east-1a \
  --query 'Subnet.SubnetId' \
  --output text)

# Create subnet in AZ 2
SUBNET_2=$(aws ec2 create-subnet \
  --vpc-id $VPC_ID \
  --cidr-block 10.0.2.0/24 \
  --availability-zone us-east-1b \
  --query 'Subnet.SubnetId' \
  --output text)

echo "Subnet 1: $SUBNET_1"
echo "Subnet 2: $SUBNET_2"
```

#### Create Internet Gateway
```bash
# Create Internet Gateway
IGW_ID=$(aws ec2 create-internet-gateway \
  --query 'InternetGateway.InternetGatewayId' \
  --output text)

# Attach to VPC
aws ec2 attach-internet-gateway \
  --vpc-id $VPC_ID \
  --internet-gateway-id $IGW_ID

# Create route table
ROUTE_TABLE_ID=$(aws ec2 create-route-table \
  --vpc-id $VPC_ID \
  --query 'RouteTable.RouteTableId' \
  --output text)

# Add route to Internet Gateway
aws ec2 create-route \
  --route-table-id $ROUTE_TABLE_ID \
  --destination-cidr-block 0.0.0.0/0 \
  --gateway-id $IGW_ID

# Associate route table with subnets
aws ec2 associate-route-table \
  --subnet-id $SUBNET_1 \
  --route-table-id $ROUTE_TABLE_ID

aws ec2 associate-route-table \
  --subnet-id $SUBNET_2 \
  --route-table-id $ROUTE_TABLE_ID
```

#### Create Security Group
```bash
# Create security group
SG_ID=$(aws ec2 create-security-group \
  --group-name hospwithoutdbcontcmp-sg \
  --description "Security group for HospWithoutDBContCmp" \
  --vpc-id $VPC_ID \
  --query 'GroupId' \
  --output text)

echo "Security Group ID: $SG_ID"

# Allow inbound HTTP traffic on port 8080
aws ec2 authorize-security-group-ingress \
  --group-id $SG_ID \
  --protocol tcp \
  --port 8080 \
  --cidr 0.0.0.0/0

# Allow inbound HTTP traffic on port 80 (ALB)
aws ec2 authorize-security-group-ingress \
  --group-id $SG_ID \
  --protocol tcp \
  --port 80 \
  --cidr 0.0.0.0/0
```

### 3. Create CloudWatch Log Group

```bash
aws logs create-log-group \
  --log-group-name /ecs/hospwithoutdbcontcmp \
  --region us-east-1
```

---

## AWS ECS Fargate Setup

### 1. Create ECS Cluster

```bash
aws ecs create-cluster \
  --cluster-name hospwithoutdbcontcmp-cluster \
  --region us-east-1
```

### 2. Understanding the Task Definition

The task definition (`ecs/task-definition.json`) defines:
- **Container specifications**: Image, ports, environment variables
- **Resource allocation**: CPU (512 = 0.5 vCPU) and Memory (1024 MB)
- **IAM roles**: Task execution and task roles
- **Logging**: CloudWatch Logs configuration
- **Health checks**: Container health monitoring

**Valid Fargate CPU/Memory Combinations:**
- CPU: 256 (.25 vCPU) → Memory: 512, 1024, 2048 MB
- CPU: 512 (.5 vCPU) → Memory: 1024, 2048, 3072, 4096 MB
- CPU: 1024 (1 vCPU) → Memory: 2048-8192 MB (1 GB increments)
- CPU: 2048 (2 vCPU) → Memory: 4096-16384 MB (1 GB increments)
- CPU: 4096 (4 vCPU) → Memory: 8192-30720 MB (1 GB increments)

### 3. Understanding the Service Definition

The service definition (`ecs/service-definition.json`) defines:
- **Desired count**: Number of tasks to run (2 for high availability)
- **Launch type**: FARGATE
- **Network configuration**: VPC, subnets, security groups
- **Load balancer**: Application Load Balancer integration
- **Deployment settings**: Rolling update strategy
- **Auto Scaling**: Optional service auto scaling

---

## ECS Task Definition Explained

### Key Components

#### 1. Family and Compatibility
```json
{
  "family": "hospwithoutdbcontcmp-task",
  "requiresCompatibilities": ["FARGATE"],
  "networkMode": "awsvpc"
}
```
- **family**: Task definition name and version grouping
- **requiresCompatibilities**: Must be "FARGATE" for Fargate deployment
- **networkMode**: Must be "awsvpc" for Fargate (each task gets its own ENI)

#### 2. Resource Allocation
```json
{
  "cpu": "512",
  "memory": "1024"
}
```
- **cpu**: Task-level CPU units (512 = 0.5 vCPU)
- **memory**: Task-level memory in MB (1024 = 1 GB)
- Must use valid Fargate combinations

#### 3. IAM Roles
```json
{
  "executionRoleArn": "arn:aws:iam::{{ACCOUNT_ID}}:role/ecsTaskExecutionRole",
  "taskRoleArn": "arn:aws:iam::{{ACCOUNT_ID}}:role/ecsTaskRole"
}
```
- **executionRoleArn**: Allows ECS to pull images and write logs
- **taskRoleArn**: Allows containers to access AWS services

#### 4. Container Definition
```json
{
  "name": "hospwithoutdbcontcmp",
  "image": "{{IMAGE_URI}}",
  "essential": true,
  "portMappings": [{"containerPort": 8080, "protocol": "tcp"}]
}
```
- **name**: Container identifier (must match service load balancer config)
- **image**: Full ECR image URI
- **essential**: If true, task stops if container stops
- **portMappings**: Only containerPort is needed for Fargate

#### 5. Health Check
```json
{
  "healthCheck": {
    "command": ["CMD-SHELL", "curl -f http://localhost:8080/health || exit 1"],
    "interval": 30,
    "timeout": 5,
    "retries": 3,
    "startPeriod": 60
  }
}
```
- **command**: Health check command (requires curl in container)
- **interval**: Time between checks (seconds)
- **timeout**: Maximum time for check to complete
- **retries**: Consecutive failures before unhealthy
- **startPeriod**: Grace period during container startup

#### 6. Logging Configuration
```json
{
  "logConfiguration": {
    "logDriver": "awslogs",
    "options": {
      "awslogs-group": "/ecs/hospwithoutdbcontcmp",
      "awslogs-region": "us-east-1",
      "awslogs-stream-prefix": "ecs"
    }
  }
}
```
- Sends container logs to CloudWatch Logs
- Stream format: `ecs/{task-id}`

---

## ECS Service Configuration

### Key Components

#### 1. Service Basics
```json
{
  "serviceName": "hospwithoutdbcontcmp-service",
  "cluster": "hospwithoutdbcontcmp-cluster",
  "taskDefinition": "hospwithoutdbcontcmp-task",
  "desiredCount": 2,
  "launchType": "FARGATE"
}
```

#### 2. Network Configuration
```json
{
  "networkConfiguration": {
    "awsvpcConfiguration": {
      "subnets": ["subnet-xxx", "subnet-yyy"],
      "securityGroups": ["sg-xxx"],
      "assignPublicIp": "ENABLED"
    }
  }
}
```
- **subnets**: At least 2 subnets in different AZs for high availability
- **securityGroups**: Controls inbound/outbound traffic
- **assignPublicIp**: ENABLED for internet access (use NAT Gateway for production)

#### 3. Load Balancer Integration
```json
{
  "loadBalancers": [{
    "targetGroupArn": "arn:aws:elasticloadbalancing:...",
    "containerName": "hospwithoutdbcontcmp",
    "containerPort": 8080
  }],
  "healthCheckGracePeriodSeconds": 300
}
```
- Target group must use `target-type: ip` (required for Fargate)
- Health check grace period allows startup time

#### 4. Deployment Configuration
```json
{
  "deploymentConfiguration": {
    "maximumPercent": 200,
    "minimumHealthyPercent": 50,
    "deploymentCircuitBreaker": {
      "enable": true,
      "rollback": true
    }
  }
}
```
- **maximumPercent**: 200 = deploy new tasks before stopping old ones
- **minimumHealthyPercent**: 50 = keep at least 50% tasks running
- **deploymentCircuitBreaker**: Automatic rollback on failure

---

## Deployment Walkthrough

### Automated Deployment Script

#### Linux/macOS
```bash
chmod +x scripts/deploy-image.sh
./scripts/deploy-image.sh
```

#### Windows
```cmd
scripts\deploy-image.bat
```

### Interactive Deployment Steps

The script will prompt for:

1. **AWS Region**: `us-east-1`
2. **ECS Cluster Name**: `hospwithoutdbcontcmp-cluster`
3. **VPC ID**: `vpc-0abc123def456`
4. **Subnet IDs**: `subnet-0abc123,subnet-0def456`
5. **Security Group ID**: `sg-0abc123def`
6. **Docker Image URI**: `123456789012.dkr.ecr.us-east-1.amazonaws.com/hospwithoutdbcontcmp:latest`
7. **Load Balancer**: `y` or `n`

### What the Script Does

1. **Validates AWS Configuration**
   - Retrieves AWS Account ID
   - Verifies AWS CLI credentials

2. **Creates/Validates Resources**
   - ECS cluster (creates if doesn't exist)
   - CloudWatch log group
   - Application Load Balancer (if requested)
   - Target Group with health checks

3. **Registers Task Definition**
   - Replaces placeholders in JSON
   - Registers new task definition version
   - Captures task definition ARN

4. **Creates/Updates Service**
   - Checks if service exists
   - Creates new service or updates existing
   - Uses full task definition ARN

5. **Waits for Stability**
   - Monitors deployment progress
   - Waits for all tasks to be running and healthy
   - Reports final status

### Manual Deployment (Alternative)

If you prefer manual deployment:

```bash
# 1. Register task definition
aws ecs register-task-definition \
  --cli-input-json file://ecs/task-definition.json

# 2. Create service
aws ecs create-service \
  --cli-input-json file://ecs/service-definition.json

# 3. Wait for stability
aws ecs wait services-stable \
  --cluster hospwithoutdbcontcmp-cluster \
  --services hospwithoutdbcontcmp-service
```

---

## Monitoring and Logging

### CloudWatch Logs

#### View Logs in Console
1. Navigate to CloudWatch → Log Groups
2. Select `/ecs/hospwithoutdbcontcmp`
3. View log streams by task ID

#### View Logs with AWS CLI
```bash
# Tail logs (real-time)
aws logs tail /ecs/hospwithoutdbcontcmp --follow --region us-east-1

# View logs for specific time range
aws logs filter-log-events \
  --log-group-name /ecs/hospwithoutdbcontcmp \
  --start-time $(date -u -d '1 hour ago' +%s)000 \
  --region us-east-1

# Search logs for errors
aws logs filter-log-events \
  --log-group-name /ecs/hospwithoutdbcontcmp \
  --filter-pattern "ERROR" \
  --region us-east-1
```

### CloudWatch Metrics

#### ECS Service Metrics
- **CPUUtilization**: Container CPU usage
- **MemoryUtilization**: Container memory usage
- **TargetResponseTime**: ALB target response time
- **HealthyHostCount**: Number of healthy targets

#### View Metrics
```bash
# Get CPU utilization
aws cloudwatch get-metric-statistics \
  --namespace AWS/ECS \
  --metric-name CPUUtilization \
  --dimensions Name=ServiceName,Value=hospwithoutdbcontcmp-service Name=ClusterName,Value=hospwithoutdbcontcmp-cluster \
  --start-time $(date -u -d '1 hour ago' +%Y-%m-%dT%H:%M:%S) \
  --end-time $(date -u +%Y-%m-%dT%H:%M:%S) \
  --period 300 \
  --statistics Average \
  --region us-east-1
```

### Application Insights (Optional)

For .NET applications, consider adding Application Insights:

```bash
# Install NuGet package
dotnet add package Microsoft.ApplicationInsights.AspNetCore
```

Add to `Program.cs`:
```csharp
builder.Services.AddApplicationInsightsTelemetry();
```

Set environment variable in task definition:
```json
{
  "name": "APPLICATIONINSIGHTS_CONNECTION_STRING",
  "value": "InstrumentationKey=xxx"
}
```

---

## Troubleshooting

### Common Issues

#### 1. Task Fails to Start

**Symptom**: Tasks transition to STOPPED state immediately.

**Possible Causes**:
- Invalid CPU/memory combination
- Image pull errors (ECR permissions)
- Container health check failures
- Missing IAM roles

**Debugging Steps**:
```bash
# Check task status
aws ecs describe-tasks \
  --cluster hospwithoutdbcontcmp-cluster \
  --tasks $(aws ecs list-tasks --cluster hospwithoutdbcontcmp-cluster --service-name hospwithoutdbcontcmp-service --query 'taskArns[0]' --output text) \
  --query 'tasks[0].{Status:lastStatus,Reason:stoppedReason,Exit:containers[0].exitCode}'

# Check CloudWatch logs
aws logs tail /ecs/hospwithoutdbcontcmp --since 30m
```

**Solution**:
- Verify CPU/memory are valid Fargate combinations
- Check executionRoleArn has ECR permissions
- Review container logs for startup errors
- Increase health check startPeriod

#### 2. Service Deployment Stuck

**Symptom**: Deployment doesn't reach steady state.

**Possible Causes**:
- Health check failures
- Insufficient subnet IP addresses
- Security group blocking traffic
- Resource limits (CPU/memory too low)

**Debugging Steps**:
```bash
# Check service events
aws ecs describe-services \
  --cluster hospwithoutdbcontcmp-cluster \
  --services hospwithoutdbcontcmp-service \
  --query 'services[0].events[0:5]'

# Check task health
aws ecs describe-tasks \
  --cluster hospwithoutdbcontcmp-cluster \
  --tasks $(aws ecs list-tasks --cluster hospwithoutdbcontcmp-cluster --service-name hospwithoutdbcontcmp-service --query 'taskArns[0]' --output text) \
  --query 'tasks[0].containers[0].healthStatus'
```

**Solution**:
- Verify health endpoint returns 200 OK
- Ensure security group allows traffic on port 8080
- Check subnet has available IP addresses
- Review application logs for errors

#### 3. Cannot Pull Image from ECR

**Symptom**: "CannotPullContainerError" in task stopped reason.

**Possible Causes**:
- executionRoleArn missing ECR permissions
- Image URI incorrect
- ECR repository policy blocking access

**Solution**:
```bash
# Verify image exists
aws ecr describe-images \
  --repository-name hospwithoutdbcontcmp \
  --region us-east-1

# Check execution role policy
aws iam get-role-policy \
  --role-name ecsTaskExecutionRole \
  --policy-name AmazonECSTaskExecutionRolePolicy

# Attach ECR policy if missing
aws iam attach-role-policy \
  --role-name ecsTaskExecutionRole \
  --policy-arn arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy
```

#### 4. High Memory or CPU Usage

**Symptom**: Tasks restart due to resource exhaustion.

**Solution**:
- Increase CPU/memory in task definition
- Optimize application code
- Review .NET GC settings
- Enable memory profiling

```json
{
  "cpu": "1024",
  "memory": "2048"
}
```

#### 5. Load Balancer Health Checks Failing

**Symptom**: Targets marked unhealthy in target group.

**Debugging Steps**:
```bash
# Check target health
aws elbv2 describe-target-health \
  --target-group-arn arn:aws:elasticloadbalancing:... \
  --query 'TargetHealthDescriptions[*].{Target:Target.Id,Health:TargetHealth.State,Reason:TargetHealth.Reason}'

# Test health endpoint
TASK_IP=$(aws ecs describe-tasks \
  --cluster hospwithoutdbcontcmp-cluster \
  --tasks $(aws ecs list-tasks --cluster hospwithoutdbcontcmp-cluster --service-name hospwithoutdbcontcmp-service --query 'taskArns[0]' --output text) \
  --query 'tasks[0].containers[0].networkInterfaces[0].privateIpv4Address' \
  --output text)

curl http://$TASK_IP:8080/health
```

**Solution**:
- Verify health endpoint URL and response code
- Check security group allows ALB → container traffic
- Increase health check grace period
- Review application startup time

---

## Scaling and Performance

### Manual Scaling

```bash
# Scale to 5 tasks
aws ecs update-service \
  --cluster hospwithoutdbcontcmp-cluster \
  --service hospwithoutdbcontcmp-service \
  --desired-count 5
```

### Auto Scaling

#### 1. Register Scalable Target
```bash
aws application-autoscaling register-scalable-target \
  --service-namespace ecs \
  --resource-id service/hospwithoutdbcontcmp-cluster/hospwithoutdbcontcmp-service \
  --scalable-dimension ecs:service:DesiredCount \
  --min-capacity 2 \
  --max-capacity 10
```

#### 2. Create Scaling Policy (Target Tracking)
```bash
aws application-autoscaling put-scaling-policy \
  --service-namespace ecs \
  --resource-id service/hospwithoutdbcontcmp-cluster/hospwithoutdbcontcmp-service \
  --scalable-dimension ecs:service:DesiredCount \
  --policy-name cpu-target-tracking-policy \
  --policy-type TargetTrackingScaling \
  --target-tracking-scaling-policy-configuration file://scaling-policy.json
```

**scaling-policy.json**:
```json
{
  "TargetValue": 70.0,
  "PredefinedMetricSpecification": {
    "PredefinedMetricType": "ECSServiceAverageCPUUtilization"
  },
  "ScaleOutCooldown": 60,
  "ScaleInCooldown": 60
}
```

### Performance Optimization

#### .NET Runtime Optimizations

Add to Dockerfile:
```dockerfile
# Enable ReadyToRun compilation for faster startup
ENV DOTNET_ReadyToRun=1

# Optimize garbage collection for containers
ENV DOTNET_gcServer=1
ENV DOTNET_GCHeapCount=2
```

Add to task definition environment:
```json
{
  "name": "DOTNET_TieredCompilation",
  "value": "1"
},
{
  "name": "DOTNET_TC_QuickJitForLoops",
  "value": "1"
}
```

#### Application Performance

- Enable response compression
- Use HTTP/2
- Implement caching (Redis, MemoryCache)
- Optimize database queries
- Use asynchronous programming

---

## Security Best Practices

### 1. Use Least Privilege IAM Roles

```json
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Action": [
        "ecr:GetAuthorizationToken",
        "ecr:BatchCheckLayerAvailability",
        "ecr:GetDownloadUrlForLayer",
        "ecr:BatchGetImage"
      ],
      "Resource": "*"
    },
    {
      "Effect": "Allow",
      "Action": [
        "logs:CreateLogStream",
        "logs:PutLogEvents"
      ],
      "Resource": "arn:aws:logs:*:*:log-group:/ecs/hospwithoutdbcontcmp:*"
    }
  ]
}
```

### 2. Enable Container Insights

```bash
aws ecs update-cluster-settings \
  --cluster hospwithoutdbcontcmp-cluster \
  --settings name=containerInsights,value=enabled
```

### 3. Use Secrets Manager for Sensitive Data

```bash
# Store secret
aws secretsmanager create-secret \
  --name hospwithoutdbcontcmp/db-password \
  --secret-string "your-secret-password"

# Reference in task definition
{
  "secrets": [
    {
      "name": "DB_PASSWORD",
      "valueFrom": "arn:aws:secretsmanager:us-east-1:123456789012:secret:hospwithoutdbcontcmp/db-password"
    }
  ]
}
```

### 4. Enable VPC Flow Logs

```bash
aws ec2 create-flow-logs \
  --resource-type VPC \
  --resource-ids $VPC_ID \
  --traffic-type ALL \
  --log-destination-type cloud-watch-logs \
  --log-group-name /aws/vpc/flowlogs
```

### 5. Use Private Subnets with NAT Gateway

For production:
- Deploy tasks in private subnets
- Use NAT Gateway for outbound internet access
- Keep ALB in public subnets
- Set `assignPublicIp: "DISABLED"` in task definition

---

## CI/CD Integration

### GitHub Actions Example

**.github/workflows/deploy.yml**:
```yaml
name: Deploy to ECS

on:
  push:
    branches: [main]

jobs:
  deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      
      - name: Configure AWS credentials
        uses: aws-actions/configure-aws-credentials@v2
        with:
          aws-access-key-id: ${{ secrets.AWS_ACCESS_KEY_ID }}
          aws-secret-access-key: ${{ secrets.AWS_SECRET_ACCESS_KEY }}
          aws-region: us-east-1
      
      - name: Login to Amazon ECR
        id: login-ecr
        uses: aws-actions/amazon-ecr-login@v1
      
      - name: Build and push image
        env:
          ECR_REGISTRY: ${{ steps.login-ecr.outputs.registry }}
          ECR_REPOSITORY: hospwithoutdbcontcmp
          IMAGE_TAG: ${{ github.sha }}
        run: |
          docker build -t $ECR_REGISTRY/$ECR_REPOSITORY:$IMAGE_TAG .
          docker push $ECR_REGISTRY/$ECR_REPOSITORY:$IMAGE_TAG
      
      - name: Deploy to ECS
        env:
          ECR_REGISTRY: ${{ steps.login-ecr.outputs.registry }}
          ECR_REPOSITORY: hospwithoutdbcontcmp
          IMAGE_TAG: ${{ github.sha }}
        run: |
          # Update task definition
          sed -i 's|{{IMAGE_URI}}|'$ECR_REGISTRY/$ECR_REPOSITORY:$IMAGE_TAG'|g' ecs/task-definition.json
          
          # Register task definition
          aws ecs register-task-definition --cli-input-json file://ecs/task-definition.json
          
          # Update service
          aws ecs update-service \
            --cluster hospwithoutdbcontcmp-cluster \
            --service hospwithoutdbcontcmp-service \
            --force-new-deployment
```

---

## Additional Resources

- [AWS ECS Developer Guide](https://docs.aws.amazon.com/ecs/)
- [AWS Fargate Documentation](https://docs.aws.amazon.com/fargate/)
- [.NET on AWS](https://aws.amazon.com/developer/language/net/)
- [Docker Best Practices](https://docs.docker.com/develop/dev-best-practices/)
- [ASP.NET Core Performance Best Practices](https://docs.microsoft.com/aspnet/core/performance/performance-best-practices)

---

## Support and Troubleshooting

For issues or questions:
1. Check CloudWatch Logs for application errors
2. Review ECS service events for deployment issues
3. Verify IAM roles and permissions
4. Consult AWS Support or community forums

**Common Commands Reference**:

```bash
# View service status
aws ecs describe-services --cluster hospwithoutdbcontcmp-cluster --services hospwithoutdbcontcmp-service

# List running tasks
aws ecs list-tasks --cluster hospwithoutdbcontcmp-cluster --service-name hospwithoutdbcontcmp-service

# View task details
aws ecs describe-tasks --cluster hospwithoutdbcontcmp-cluster --tasks <task-id>

# View logs
aws logs tail /ecs/hospwithoutdbcontcmp --follow

# Force new deployment
aws ecs update-service --cluster hospwithoutdbcontcmp-cluster --service hospwithoutdbcontcmp-service --force-new-deployment
```

---

**End of Deployment Guide**