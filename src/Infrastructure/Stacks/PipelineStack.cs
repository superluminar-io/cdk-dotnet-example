using Amazon.CDK;
using Amazon.CDK.AWS.CodePipeline;
using Amazon.CDK.AWS.CodePipeline.Actions;
using Amazon.CDK.Pipelines;

namespace Infrastructure.Stacks
{
    public class PipelineStack : Stack
    {
        public PipelineStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            var sourceArtifact = new Artifact_();
            var cloudAssemblyArtifact = new Artifact_();

            var pipeline = new CdkPipeline(this, "Pipeline", new CdkPipelineProps
            {
                CloudAssemblyArtifact = cloudAssemblyArtifact,

                SourceAction = new GitHubSourceAction(new GitHubSourceActionProps
                {
                    ActionName = "GitHub",
                    Output = sourceArtifact,
                    OauthToken = SecretValue.SecretsManager("CDK_DOTNET_EXAMPLE_GITHUB_TOKEN"),
                    Owner = "superluminar-io",
                    Repo = "cdk-dotnet-example",
                    Branch = "main"
                }),

                SynthAction = SimpleSynthAction.StandardNpmSynth(new StandardNpmSynthOptions
                {
                    SourceArtifact = sourceArtifact,
                    CloudAssemblyArtifact = cloudAssemblyArtifact,
                    InstallCommand = "apt-get install -y dotnet-sdk-5.0 && npm ci",
                })
            });

            pipeline.AddApplicationStage(new ApiStage(this, "ApiStage"));
        }
    }

    class ApiStage : Stage
    {
        public ApiStage(Construct scope, string id, Amazon.CDK.StageProps props = null) : base(scope, id, props)
        {
            new ApiStack(this, "ApiStack");
        }
    }
}