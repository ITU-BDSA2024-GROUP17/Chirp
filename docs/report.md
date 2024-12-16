---
title: _Chirp!_ Project Report
subtitle: ITU BDSA 2024 Group 17
author:
- "Elias Lildholdt <lild@itu.dk>"
- "Kevin Gravesen <kegr@itu.dk>"
- "Nicklas Østerberg <nsio@itu.dk>"
- "Joakim Andreasen <joaan@itu.dk>"
- "Johannes Jørgensen <jgjo@itu.dk>"
numbersections: true
---

# Design and Architecture of _Chirp!_

## Domain model

![](./docs/images/domain-model.svg)

## Architecture — In the small

## Architecture of deployed application

## User activities

## Sequence of functionality/calls trough _Chirp!_

# Process

## Build, test, release, and deployment

When a commit is pushed to a branch which currently is used for a pull request a workflow is ran on the commit. The workflow, builds, run all the tests and lints the code. If any of these fail or if any warning occour then the workflow will fail. Preventing the proposed changes from being merged into the main branch.
![](./docs/images/build-test-lint-workflow.svg)

The following diagram shows one of the workflows that is being ran when a tag is pushed to the main branch. This workflow is building the artifacts for Linux, MacOS and Windows, which is then included in the GitHub release for that tag.
![](./docs/images/build-release-workflow.svg)

The other workflow that is being ran when a tag is pushed to main, is the workflow responsible for Azure deployments.
![](./docs/images/deploy-workflow.svg)

## Team work

## How to make _Chirp!_ work locally

Clone repository
```sh
git clone https://github.com/ITU-BDSA2024-GROUP17/Chirp
```

Change directory
```sh
cd Chirp
```

_Note: To enable login via GitHub OAuth, the following environment variables needs to be set._

| Environment variable | Description                            |
|:--------------------:|----------------------------------------|
|    GHUB_CLIENT_ID    | Id of the GitHub OAuth application     |
|  GHUB_CLIENT_SECRET  | Secret of the GitHub OAuth application |

### How to run development instance

Run the 'Chirp.Web' project
```sh
dotnet run --project ./src/Chirp.Web
```

Chirp will now begin to build and then run when it is finished.

The website can then be accessed via http://localhost:5163 in a browser.

_Note: Informatory logs (some libaries are set to only print warnings) will be printed to standard out (the terminal)._

### How to run production build

Build the project for production
```sh
dotnet publish src/Chirp.Web -c Release
```

Change directory
```sh
cd src/Chirp.Web/bin/Release/net8.0/publish
```

Run production build
```sh
dotnet Chirp.Web.dll
```

The website can then be accessed via http://localhost:5000 in a browser.

_Note: Informatory logs (some libaries are set to only print warnings) will be printed to standard out (the terminal)._

_Note: To expose the site to the general internet, either a port forward or, a deployment to a hosting provider would be required._

## How to run test suite locally

While being in the Chirp directory, which was cloned previously, the following command can be used to run all the tests.
```sh
dotnet test
```

# Ethics

## License

Chirp is licensed under [MIT](https://opensource.org/license/mit)

## LLMs, ChatGPT, CoPilot, and others
