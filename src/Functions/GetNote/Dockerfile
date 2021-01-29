FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build
WORKDIR /src
COPY "*.csproj" "GetNote/GetNote.csproj"
RUN dotnet restore "GetNote/GetNote.csproj"

WORKDIR "/src/GetNote"
COPY . .
RUN dotnet publish "GetNote.csproj" \
            --configuration Release \ 
            --runtime linux-x64 \
            --self-contained false \ 
            --output /app/publish \
            -p:PublishReadyToRun=true  

FROM public.ecr.aws/lambda/dotnet:5.0
WORKDIR /var/task
COPY --from=build /app/publish .

CMD [ "GetNote::GetNote.Function::FunctionHandler" ]