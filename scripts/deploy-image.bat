@echo off
setlocal enabledelayedexpansion

echo ==========================================
echo AWS ECS Fargate Deployment Script
echo ==========================================
echo.

set PROJECT_NAME=hospitalcontainer18cmp
set TASK_FAMILY=hospitalcontainer18cmp-task
set SERVICE_NAME=hospitalcontainer18cmp-service

set /p AWS_REGION="Enter AWS Region (e.g., us-east-1): "
set /p CLUSTER_NAME="Enter ECS Cluster Name (e.g., my-ecs-cluster): "

echo.
echo === Network Configuration ===
set /p VPC_ID="Enter VPC ID (e.g., vpc-0abc123def456): "
set /p SUBNET_IDS="Enter Subnet IDs comma-separated (e.g., subnet-0abc123,subnet-0def456): "
set /p SECURITY_GROUP="Enter Security Group ID (e.g., sg-0abc123def): "

for /f "tokens=1,2 delims=," %%a in ("!SUBNET_IDS!") do (
    set SUBNET_1=%%a
    set SUBNET_2=%%b
)
if "!SUBNET_2!"=="" set SUBNET_2=!SUBNET_1!

echo.
set /p IMAGE_URI="Enter Docker Image URI (e.g., 123456789.dkr.ecr.us-east-1.amazonaws.com/app:latest): "

echo.
echo Retrieving AWS Account ID...
for /f "delims=" %%a in ('aws sts get-caller-identity --query Account --output text') do set ACCOUNT_ID=%%a
echo AWS Account ID: !ACCOUNT_ID!

echo.
echo Checking if ECS cluster exists...
aws ecs describe-clusters --clusters !CLUSTER_NAME! --region !REGION! >nul 2>&1
if !ERRORLEVEL! neq 0 (
    echo Cluster does not exist. Creating ECS cluster...
    aws ecs create-cluster --cluster-name !CLUSTER_NAME! --region !AWS_REGION!
)

echo.
set /p NEED_LB="Do you need a load balancer for this service? (y/n): "

if /i "!NEED_LB!"=="y" (
    echo.
    echo === Creating Application Load Balancer ===
    
    set ALB_NAME=!PROJECT_NAME!-alb
    set TG_NAME=!PROJECT_NAME!-tg
    
    echo Creating Target Group: !TG_NAME!
    for /f "delims=" %%a in ('aws elbv2 create-target-group --name !TG_NAME! --protocol HTTP --port 8080 --vpc-id !VPC_ID! --target-type ip --health-check-path "/health" --region !AWS_REGION! --query "TargetGroups[0].TargetGroupArn" --output text 2^>nul') do set TARGET_GROUP_ARN=%%a
    
    if "!TARGET_GROUP_ARN!"=="" (
        for /f "delims=" %%a in ('aws elbv2 describe-target-groups --names !TG_NAME! --region !AWS_REGION! --query "TargetGroups[0].TargetGroupArn" --output text') do set TARGET_GROUP_ARN=%%a
    )
    
    echo Target Group ARN: !TARGET_GROUP_ARN!
) else (
    set TARGET_GROUP_ARN=
    echo Skipping load balancer configuration
)

echo.
echo === Preparing Task Definition ===
copy ecs\task-definition.json %TEMP%\task-definition.json >nul

powershell -Command "(Get-Content %TEMP%\task-definition.json) -replace '{{IMAGE_URI}}', '!IMAGE_URI!' -replace '{{AWS_REGION}}', '!AWS_REGION!' -replace '{{ACCOUNT_ID}}', '!ACCOUNT_ID!' | Set-Content %TEMP%\task-definition.json"

echo Registering ECS task definition...
for /f "delims=" %%a in ('aws ecs register-task-definition --cli-input-json file://%TEMP%/task-definition.json --region !AWS_REGION! --query "taskDefinition.taskDefinitionArn" --output text') do set TASK_DEF_ARN=%%a

echo Task Definition ARN: !TASK_DEF_ARN!

echo.
echo === Preparing Service Definition ===
copy ecs\service-definition.json %TEMP%\service-definition.json >nul

powershell -Command "(Get-Content %TEMP%\service-definition.json) -replace '{{CLUSTER_NAME}}', '!CLUSTER_NAME!' -replace '{{SUBNET_1}}', '!SUBNET_1!' -replace '{{SUBNET_2}}', '!SUBNET_2!' -replace '{{SECURITY_GROUP}}', '!SECURITY_GROUP!' -replace '{{TARGET_GROUP_ARN}}', '!TARGET_GROUP_ARN!' | Set-Content %TEMP%\service-definition.json"

if "!TARGET_GROUP_ARN!"=="" (
    powershell -Command "$json = Get-Content %TEMP%\service-definition.json | ConvertFrom-Json; $json.PSObject.Properties.Remove('loadBalancers'); $json.PSObject.Properties.Remove('healthCheckGracePeriodSeconds'); $json | ConvertTo-Json -Depth 10 | Set-Content %TEMP%\service-definition.json"
)

echo.
echo === Deploying Service ===
for /f "delims=" %%a in ('aws ecs describe-services --cluster !CLUSTER_NAME! --services !SERVICE_NAME! --region !AWS_REGION! --query "services[0].serviceName" --output text 2^>nul') do set SERVICE_EXISTS=%%a

if "!SERVICE_EXISTS!"=="!SERVICE_NAME!" (
    echo Service exists. Updating service...
    aws ecs update-service --cluster !CLUSTER_NAME! --service !SERVICE_NAME! --task-definition !TASK_DEF_ARN! --desired-count 2 --region !AWS_REGION! --force-new-deployment
) else (
    echo Service does not exist. Creating service...
    aws ecs create-service --cli-input-json file://%TEMP%/service-definition.json --region !AWS_REGION!
)

echo.
echo Waiting for service to stabilize...
aws ecs wait services-stable --cluster !CLUSTER_NAME! --services !SERVICE_NAME! --region !AWS_REGION!

echo.
echo === Deployment Verification ===
aws ecs describe-services --cluster !CLUSTER_NAME! --services !SERVICE_NAME! --region !AWS_REGION! --query "services[0].[serviceName,status,runningCount,desiredCount]" --output table

echo.
echo ==========================================
echo Deployment Completed Successfully!
echo ==========================================
echo Service Name: !SERVICE_NAME!
echo Cluster: !CLUSTER_NAME!
echo Region: !AWS_REGION!
echo CloudWatch Logs: /ecs/!PROJECT_NAME!
echo ==========================================

endlocal