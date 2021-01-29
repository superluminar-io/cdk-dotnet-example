# Dockerfile to build Lambda functions
FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build
ARG FUNCTION_NAME
WORKDIR /src
COPY . .
RUN dotnet publish "Functions/$FUNCTION_NAME/$FUNCTION_NAME.csproj" \
            --configuration Release \ 
            --runtime linux-x64 \
            --self-contained false \ 
            --output /app/publish \
            -p:PublishReadyToRun=true  

FROM public.ecr.aws/lambda/dotnet:5.0
WORKDIR /var/task
COPY --from=build /app/publish .