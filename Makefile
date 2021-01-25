dependencies:
	npm i
	dotnet restore

deploy:
	npx cdk deploy

diff: build
	npx cdk diff