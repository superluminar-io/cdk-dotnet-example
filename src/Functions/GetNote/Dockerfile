FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build
WORKDIR /src
COPY "*.csproj" "Function/Function.csproj"
RUN dotnet restore "Function/Function.csproj"

WORKDIR "/src/Function"
COPY . .
RUN dotnet publish "Function.csproj" \
            --configuration Release \ 
            --runtime linux-x64 \
            --self-contained false \ 
            --output /app/publish \
            -p:PublishReadyToRun=true  

FROM public.ecr.aws/lambda/dotnet:5.0
WORKDIR /var/task
COPY --from=build /app/publish .

CMD [ "Function::Function.Function::FunctionHandler" ]