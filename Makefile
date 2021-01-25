dependencies:
	npm i
	dotnet restore

FUNCTIONS = $(shell ls functions)
build:
	@for f in $(FUNCTIONS); do \
        cd functions/$$f/src/$$f && \
		dotnet build && \
		dotnet lambda package && \
		cd ../../../../; \
    done

deploy: build
	npx cdk deploy

diff: build
	npx cdk diff