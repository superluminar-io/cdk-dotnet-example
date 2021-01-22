# Lambda .NET Example

This is a simple example project bootstrapping a CDK project with C#. The goal is to create and deploy a lambda function.

## Prerequisites
* NodeJS / NPM
* AWS CLI
* Dotnet

## Getting started

Before deploying the application, make sure you have AWS credentials in your terminal.
```sh
$ git clone git@github.com:superluminar-io/lambda-dotnet-example
$ cd lambda-dotnet-example
$ npm install
$ npx cdk deploy
```

## Useful commands

* `dotnet build src` compile this app
* `cdk deploy`       deploy this stack to your default AWS account/region
* `cdk diff`         compare deployed stack with current state
* `cdk synth`        emits the synthesized CloudFormation template