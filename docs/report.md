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
geometry: margin=2.5cm
output: pdf_document
toc: true
---

\newpage

# Design and Architecture of _Chirp!_

## Domain model

The `Author` entity in Chirp! is extending the already existing `IdentityUser` entity. Properties for the `IdentityUser` can be found on Microsoft's [documentation](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.entityframeworkcore.identityuser){ width=400px }.

![Class diagram showing the domain model of Chirp!](./docs/images/domain-model.svg){ width=550px }

\newpage

## Architecture — In the small
```{=latex}
\begin{center}
```

![Onion Architecture of Chirp!](./docs/images/ChirpOnionDiagram.drawio-cropped.svg){ width=650px }

```{=latex}
\end{center}
```

Each layer of the onion architecture represents a dotnet project or dotnet test project - A dotnet project may have mutible sub _projects/folders_ that only can _use_ or _implicite use_ from other projects or sub-project.
Only individual files (classes, interfaces, .cshtml, ...) can inherit from each others, but may only be shown implicitly in the diagram.

\newpage

## Architecture of deployed application

The diagram below shows the architecture of our program after it has been deployed.
The only external service that is used is github, which is used for authenticating our users.
The http endpoint could also be https, depending on if certificates are provided or not.

![Architecture diagram](./docs/images/deployed-architecture.svg){ width=650px }

\newpage

## User activities
To ensure an entuitive website we have created an activity diagram that also works as a sitemap. This diagram shows the features available to users when first visiting the site and when logged in.

![User Activity](./docs/images/UserActivityDiagram.svg){ width=365px }

\newpage

## Sequence of functionality/calls trough _Chirp!_

The sequence diagram below shows the calls that is being made, both internally and externally, when a client requests a profile page on the _Chirp!_ webpage.

It becomes very clear when looking at the diagram, that all the C# calls are are awaited. Most of the calls shown in the diagram could be ran in parralel leading to improved loading time.

![Sequence of events when a client fetches a profile page](./docs/images/page-fetch-sequence-diagram.svg){ width=300px }

\newpage

# Process

## Build, test, release, and deployment

The Chirp repo is currently relying on three workflows that are managing, tests, releases and deployments.

### Build, test and linting workflow

When a commit is pushed to a branch which currently is used for a pull request a workflow is ran on the commit. The workflow, builds, run all the tests and lints the code. If any of these fail or if any warning occour then the workflow will fail. Preventing the proposed changes from being merged into the main branch.

![Diagram illustrating the build, test and linting workflow](./docs/images/build-test-lint-workflow.svg){ width=625px }

\newpage

### Release workflow

The following diagram shows one of the workflows that is being ran when a tag is pushed to the main branch. This workflow is building the artifacts for Linux, MacOS and Windows, which is then included in the GitHub release for that tag.

![Diagram illustrating the release workflow](./docs/images/build-release-workflow.svg){ width=625px }

\newpage

### Deployement workflow

The other workflow that is being ran when a tag is pushed to main, is the workflow responsible for Azure deployments.

![Diagram illustrating the deployment workflow](./docs/images/deploy-workflow.svg){ width=250px }

\newpage

## Team work

![Teamwork flowdiagram](./docs/images/teamwork-flowdiagram.svg){ width=260px }

\newpage

When requirements from the course were published, the group would either delegate the different issues and users stories to be created by different group members, or one or two individuals would write them out by themselves as quickly as possible. This allowed the group to quickly be aware of what the current issues were, and allowed members to assign themselves to these issues.

Similarly, if a bug was found, or group members agreed on a new functionality of the program, someone was quickly assigned to make an issue. A single issue was usually worked on by anywhere between 1 and 3 group members according to the scale of the issue.

The group utilized some pair programming at the start of the project, although it developed into more individually focused programming. If a member needed help on they would usually contact other members on a discord server, which was the primary communication platfrom utilized by the group. This server also included a bot that sent a message whenever there was activity with issues or pull requests on the repository.

When a feature was done, an effort was made to test this feature. Unfortunately this was not done for every feature, and is something the group has agreed to work more on in future projects. This resulted in a backlog of testing that had to be worked on by all group members towards the end of the project.

When a feature was deemed ready by the assigned member(s), a pull request would be made.
If any other group member had good knowledge of the part of the program the
feature would interact with, they would be requested as a reviewer. Otherwise any willing group members would be requested as a reviewer. The reviewer would then ensure that the program worked as intended, and then approve it if no further changes were needed. An approval was needed to merge into main, as branch protection was in place.

![Project Board](./docs/images/team_planning_board.png)

Time zones are currently displayed in UTC. JavaScript could be used instead, to let the client choose the time zone automatically. Due to the agreed low priority, this issue have been pushed to allow for time to be allocated elsewhere.
As applies to the issue of FormatDateTime.

\newpage

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

\newpage

## How to run test suite locally

_Note: The following steps should be performed from the root of the repo._

Build the solution
```sh
dotnet build
```

Install required browsers
```sh
pwsh test/Chirp.Web.Playwright/bin/Debug/net8.0/playwright.ps1 install
```

Start a development server
```sh
dotnet run --project src/Chirp.Web
```

_Note: The tests needs to be ran from another terminal, since Playwright needs to access an actual running instance of Chirp!._

```sh
dotnet test
```

### Run individual tests

Change directory to desired test directory
```sh
cd test/Chirp.Web.Tests
```

_Note: See "The different test suites" section for available test suites._

Run tests in the desired directory
```sh
dotnet test
```

### The different test suites

Chirp currently includes 3 suites.

* Chirp.Infrastructure.Tests
* Chirp.Web.Playwright
* Chirp.Web.Tests

#### Chirp.Infrastructure.Tests

Contains Unit tests regarding the infrastructure project. Along with integration tests that focuses on how the infrastructure project and the database is working togheter.

#### Chirp.Web.Playwright

All of the end-to-end tests are located in this suite. To run these tests a development server needs to be running before the tests are started.

#### Chirp.Web.Tests

Unit tests and integration tests involving the Web project is location in this test suite.

These includes various helper methods and endpoints.

\newpage

# Ethics

## License

Chirp is licensed under [MIT](https://opensource.org/license/mit)

## LLMs, ChatGPT, CoPilot, and others
As it gets normalized to use LLM's while coding in a project, it gets more important to be transparent when using these tools to create. Therefore we have taken steps to document when using LLM's, both directly and indirectly.
To be transparent about these tools, the members of our group are obilgated to co-author every commit and pull-request that was realized with the help of these tools. But as a general non-formal consensus, we did not extensively
make use of LLM's and generative tools to directly write code for our program, but rather used it for inspiration for what solutions exists.
