FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build
WORKDIR /src
COPY "*.csproj" "ListNotes/ListNotes.csproj"
RUN dotnet restore "ListNotes/ListNotes.csproj"

WORKDIR "/src/ListNotes"
COPY . .
RUN dotnet publish "ListNotes.csproj" \
            --configuration Release \ 
            --runtime linux-x64 \
            --self-contained false \ 
            --output /app/publish \
            -p:PublishReadyToRun=true  

FROM public.ecr.aws/lambda/dotnet:5.0
WORKDIR /var/task
COPY --from=build /app/publish .

CMD [ "ListNotes::ListNotes.Function::FunctionHandler" ]