# Lambda .NET Example

This is a simple example project bootstrapping a CDK project with C#. The goal is to create and deploy a lambda function.

## Prerequisites
* [NodeJS / NPM](https://nodejs.org/en/)
* [Docker](https://www.docker.com/)
* [Dotnet](https://dotnet.microsoft.com/download)

## Getting started

Before deploying the application, make sure you have AWS credentials in your terminal.

```sh
$ git clone git@github.com:superluminar-io/lambda-dotnet-example
$ cd lambda-dotnet-example

# Install dependencies
$ npm i

# Deploy
$ npx cdk deploy
```

Use the URL from the stack output:
```sh
# Create note
$ curl -X POST https://XXXXXXXXXX.execute-api.eu-central-1.amazonaws.com/notes --data '{ "title": "Hello World", "content": "Mollit adipisicing ut dolore aliqua." }' -H 'Content-Type: application/json'

# List notes
$ curl https://XXXXXXXXXX.execute-api.eu-central-1.amazonaws.com/notes

# Get note
$ curl https://XXXXXXXXXX.execute-api.eu-central-1.amazonaws.com/notes/:id
```

## Credits

* [AWS CDK Workshop](https://cdkworkshop.com/40-dotnet.html).
* [Lambda .NET Example](https://github.com/aws-samples/aws-cdk-examples/tree/master/csharp/capitalize-string)