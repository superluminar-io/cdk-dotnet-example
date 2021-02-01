# CDK .NET Example

This is a simple example project bootstrapping a CDK project with C#.

## Prerequisites
* [NodeJS / NPM](https://nodejs.org/en/)
* [Docker](https://www.docker.com/)
* [Dotnet](https://dotnet.microsoft.com/download)

## Bootstrap

This project comes with a CD pipeline built-in. We have to bootstrap the pipeline once. After that, we can merge our changes to the main branch, and the pipeline will automatically deploy our application. In order to connect the pipeline with the GitHub repository, we need to store a [GitHub access token](https://docs.github.com/en/github/authenticating-to-github/creating-a-personal-access-token) in the [secrets manager](https://aws.amazon.com/secrets-manager/). The name of the secret is `CDK_DOTNET_EXAMPLE_GITHUB_TOKEN`.

After setting up the secret, we can initialize the pipeline. Make sure you have AWS credentials in your terminal.

```sh
$ git clone git@github.com:superluminar-io/cdk-dotnet-example
$ cd cdk-dotnet-example

# Install dependencies
$ npm i

# Bootstrap CDK in the target account
$ npx cdk bootstrap --cloudformation-execution-policies arn:aws:iam::aws:policy/AdministratorAccess

# Deploy the pipeline
$ npx cdk deploy PipelineStack
```

The pipeline should deploy your application. In CloudFormation, you should find a new `ApiStack`. Take the URL from the stack output and run the following requests:

```sh
# Create note
$ curl -X POST https://XXXXXXXXXX.execute-api.eu-central-1.amazonaws.com/notes --data '{ "title": "Hello World", "content": "Mollit adipisicing ut dolore aliqua." }' -H 'Content-Type: application/json'

# List notes
$ curl https://XXXXXXXXXX.execute-api.eu-central-1.amazonaws.com/notes

# Get note
$ curl https://XXXXXXXXXX.execute-api.eu-central-1.amazonaws.com/notes/:id
```

## Credits

* [AWS CDK Workshop](https://cdkworkshop.com/40-dotnet.html)
* [Lambda .NET Example](https://github.com/aws-samples/aws-cdk-examples/tree/master/csharp/capitalize-string)