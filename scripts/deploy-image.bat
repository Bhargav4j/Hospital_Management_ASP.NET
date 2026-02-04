@echo off
setlocal enabledelayedexpansion

echo ========================================
echo AWS ECS Fargate Deployment Script
echo ========================================
echo.

set PROJECT_NAME=hospwithoutdbcontcmp
set TASK_FAMILY=!PROJECT_NAME!-task
set SERVICE_NAME=!PROJECT_NAME!-service

echo === AWS Configuration ===
set /p AWS_REGION="Enter AWS region (e.g., us-east-1): "
set AWS_DEFAULT_REGION=!AWS_REGION!

set /p CLUSTER_NAME="Enter ECS cluster name (e.g., my-ecs-cluster): "

echo.
echo === Network Configuration ===
set /p VPC_ID="Enter VPC ID (e.g., vpc-0abc123def456): "
set /p SUBNET_INPUT="Enter Subnet IDs comma-separated (e.g., subnet-0abc123,subnet-0def456): "
set /p SECURITY_GROUP="Enter Security Group ID (e.g., sg-0abc123def): "

for /f "tokens=1,2 delims=," %%a in ("!SUBNET_INPUT!") do (
    set SUBNET_1=%%a
    set SUBNET_2=%%b
)
if "!SUBNET_2!"==" " set SUBNET_2=!SUBNET_1!

echo.
echo === Container Configuration ===
set /p IMAGE_URI="Enter Docker image URI (e.g., 123456789.dkr.ecr.us-east-1.amazonaws.com/app:latest): "

echo.
echo === Load Balancer Configuration ===
set /p NEED_LB="Do you need a load balancer for this service? (y/n): "

echo.
echo Retrieving AWS Account ID...
for /f "delims=" %%i in ('aws sts get-caller-identity --query Account --output text') do set ACCOUNT_ID=%%i
echo AWS Account ID: !ACCOUNT_ID!

echo.
echo Checking if ECS cluster exists...
aws ecs describe-clusters --clusters !CLUSTER_NAME! --region !AWS_REGION! >nul 2>&1
if !ERRORLEVEL! neq 0 (
    echo Cluster does not exist. Creating ECS cluster...
    aws ecs create-cluster --cluster-name !CLUSTER_NAME! --region !AWS_REGION!
)
echo ECS cluster ready: !CLUSTER_NAME!

echo.
echo Creating CloudWatch log group...
aws logs create-log-group --log-group-name "/ecs/!PROJECT_NAME!" --region !AWS_REGION! >nul 2>&1
if !ERRORLEVEL! neq 0 echo Log group already exists

set TARGET_GROUP_ARN=
if /i "!NEED_LB!"=="y" (
    echo.
    echo Creating Application Load Balancer and Target Group...
    
    set TARGET_GROUP_NAME=!PROJECT_NAME!-tg
    
    for /f "delims=" %%i in ('aws elbv2 create-target-group --name !TARGET_GROUP_NAME! --protocol HTTP --port 8080 --vpc-id !VPC_ID! --target-type ip --health-check-enabled --health-check-protocol HTTP --health-check-path "/health" --health-check-interval-seconds 30 --health-check-timeout-seconds 5 --healthy-threshold-count 2 --unhealthy-threshold-count 3 --region !AWS_REGION! --query "TargetGroups[0].TargetGroupArn" --output text 2^>nul') do set TARGET_GROUP_ARN=%%i
    
    if "!TARGET_GROUP_ARN!"==" " (
        for /f "delims=" %%i in ('aws elbv2 describe-target-groups --names !TARGET_GROUP_NAME! --region !AWS_REGION! --query "TargetGroups[0].TargetGroupArn" --output text') do set TARGET_GROUP_ARN=%%i
    )
    
    echo Target Group ARN: !TARGET_GROUP_ARN!
    
    set ALB_NAME=!PROJECT_NAME!-alb
    
    for /f "delims=" %%i in ('aws elbv2 create-load-balancer --name !ALB_NAME! --subnets !SUBNET_1! !SUBNET_2! --security-groups !SECURITY_GROUP! --scheme internet-facing --type application --ip-address-type ipv4 --region !AWS_REGION! --query "LoadBalancers[0].LoadBalancerArn" --output text 2^>nul') do set ALB_ARN=%%i
    
    if "!ALB_ARN!"==" " (
        for /f "delims=" %%i in ('aws elbv2 describe-load-balancers --names !ALB_NAME! --region !AWS_REGION! --query "LoadBalancers[0].LoadBalancerArn" --output text') do set ALB_ARN=%%i
    )
    
    echo Load Balancer ARN: !ALB_ARN!
    
    aws elbv2 create-listener --load-balancer-arn !ALB_ARN! --protocol HTTP --port 80 --default-actions Type=forward,TargetGroupArn=!TARGET_GROUP_ARN! --region !AWS_REGION! >nul 2>&1
    
    for /f "delims=" %%i in ('aws elbv2 describe-load-balancers --load-balancer-arns !ALB_ARN! --region !AWS_REGION! --query "LoadBalancers[0].DNSName" --output text') do set ALB_DNS=%%i
)

echo.
echo Preparing ECS task definition...
copy ecs\task-definition.json %TEMP%\task-definition.json >nul

powershell -Command "(Get-Content %TEMP%\task-definition.json) -replace '{{IMAGE_URI}}', '!IMAGE_URI!' | Set-Content %TEMP%\task-definition.json"
powershell -Command "(Get-Content %TEMP%\task-definition.json) -replace '{{AWS_REGION}}', '!AWS_REGION!' | Set-Content %TEMP%\task-definition.json"
powershell -Command "(Get-Content %TEMP%\task-definition.json) -replace '{{ACCOUNT_ID}}', '!ACCOUNT_ID!' | Set-Content %TEMP%\task-definition.json"

echo Registering task definition...
for /f "delims=" %%i in ('aws ecs register-task-definition --cli-input-json file://%TEMP%/task-definition.json --region !AWS_REGION! --query "taskDefinition.taskDefinitionArn" --output text') do set TASK_DEF_ARN=%%i
echo Task definition registered: !TASK_DEF_ARN!

echo.
echo Preparing ECS service definition...
copy ecs\service-definition.json %TEMP%\service-definition.json >nul

powershell -Command "(Get-Content %TEMP%\service-definition.json) -replace '{{CLUSTER_NAME}}', '!CLUSTER_NAME!' | Set-Content %TEMP%\service-definition.json"
powershell -Command "(Get-Content %TEMP%\service-definition.json) -replace '{{SUBNET_1}}', '!SUBNET_1!' | Set-Content %TEMP%\service-definition.json"
powershell -Command "(Get-Content %TEMP%\service-definition.json) -replace '{{SUBNET_2}}', '!SUBNET_2!' | Set-Content %TEMP%\service-definition.json"
powershell -Command "(Get-Content %TEMP%\service-definition.json) -replace '{{SECURITY_GROUP}}', '!SECURITY_GROUP!' | Set-Content %TEMP%\service-definition.json"

if /i "!NEED_LB!"=="y" (
    powershell -Command "(Get-Content %TEMP%\service-definition.json) -replace '{{TARGET_GROUP_ARN}}', '!TARGET_GROUP_ARN!' | Set-Content %TEMP%\service-definition.json"
) else (
    powershell -Command "$json = Get-Content %TEMP%\service-definition.json | ConvertFrom-Json; $json.PSObject.Properties.Remove('loadBalancers'); $json.PSObject.Properties.Remove('healthCheckGracePeriodSeconds'); $json | ConvertTo-Json -Depth 10 | Set-Content %TEMP%\service-definition.json"
)

echo.
echo Checking if service exists...
for /f "delims=" %%i in ('aws ecs describe-services --cluster !CLUSTER_NAME! --services !SERVICE_NAME! --region !AWS_REGION! --query "services[?status==`ACTIVE`].serviceName" --output text') do set EXISTING_SERVICE=%%i

if "!EXISTING_SERVICE!"==" " (
    echo Service does not exist. Creating new service...
    aws ecs create-service --cli-input-json file://%TEMP%/service-definition.json --region !AWS_REGION!
) else (
    echo Service exists. Updating service...
    aws ecs update-service --cluster !CLUSTER_NAME! --service !SERVICE_NAME! --task-definition !TASK_DEF_ARN! --force-new-deployment --region !AWS_REGION!
)

echo.
echo Waiting for service to become stable (this may take several minutes)...
aws ecs wait services-stable --cluster !CLUSTER_NAME! --services !SERVICE_NAME! --region !AWS_REGION!

echo.
echo ========================================
echo Deployment Status
echo ========================================

aws ecs describe-services --cluster !CLUSTER_NAME! --services !SERVICE_NAME! --region !AWS_REGION! --query "services[0].{ServiceName:serviceName,Status:status,DesiredCount:desiredCount,RunningCount:runningCount}" --output table

echo.
echo ========================================
echo Deployment Complete!
echo ========================================
echo Cluster: !CLUSTER_NAME!
echo Service: !SERVICE_NAME!
echo Task Definition: !TASK_DEF_ARN!
echo CloudWatch Logs: /ecs/!PROJECT_NAME!

if /i "!NEED_LB!"=="y" (
    echo Load Balancer DNS: !ALB_DNS!
    echo Application URL: http://!ALB_DNS!
)

echo.
echo To view logs:
echo aws logs tail /ecs/!PROJECT_NAME! --follow --region !AWS_REGION!
echo ========================================

endlocal