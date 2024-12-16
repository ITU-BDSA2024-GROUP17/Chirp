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

![](./docs/diagrams/domain-model.svg)

## Architecture — In the small

## Architecture of deployed application

## User activities

## Sequence of functionality/calls trough _Chirp!_

# Process

## Build, test, release, and deployment

When a commit is pushed to a branch which currently is used for a pull request a workflow is ran on the commit. The workflow, builds, run all the tests and lints the code. If any of these fail or if any warning occour then the workflow will fail. Preventing the proposed changes from being merged into the main branch.
![](./docs/diagrams/build-test-lint-workflow.svg)

The following diagram shows one of the workflows that is being ran when a tag is pushed to the main branch. This workflow is building the artifacts for Linux, MacOS and Windows, which is then included in the GitHub release for that tag.
![](./docs/diagrams/build-release-workflow.svg)

The other workflow that is being ran when a tag is pushed to main, is the workflow responsible for Azure deployments.
![](./docs/diagrams/deploy-workflow.svg)

## Team work

When requirements from the course were published, the group would either delegate the different issues and users stories to be created by different group members, or one or two individuals would write them out by themselves as quickly as possible. This allowed the group to quickly be aware of what the current issues were, and allowed members to assign themselves to these issues.

Similarly, if a bug was found, or group members agreed on a new functionality of the program, someone was quickly assigned to make an issue. A single issue was usually worked on by anywhere between 1 and 3 group members according to the scale of the issue.

The group utilized some pair programming at the start of the project, although it developed into more individually focused programming. If a member needed help on they would usually contact other members on some communication platform.
The primary communication platfrom utilized by the group was a discord server. This server also included a bot that sent a message whenever there was activity with issues or pull requests on the repository.

When a feature was done, an effort was made to test this feature. Unfortunately this was not done for every feature, and is something the group has agreed to work more on in future projects. This resulted a backlog of testing that had to be worked on by all group members towards the end of the project.

When a feature was deemed ready by the creator, a pull request would be made. If any other group member had good knowledge of the part of the program the feature would interact with, they would be requested as a reviewer, although this was not always possible. The reviewer would ensure that the program worked as intended, and then approve it if no further changes were needed. An approval was needed to merge into main, as branch protection was in place.

## How to make _Chirp!_ work locally

Clone repository
```sh
git clone https://github.com/ITU-BDSA2024-GROUP17/Chirp
```

Change directory
```sh
cd Chirp
```

Run the 'Chirp.Web' project
```sh
dotnet run --project ./src/Chirp.Web
```

Chirp will now begin to build and then run when it is finished.

Then website can then be accessed via http://localhost:5163 in a browser.

_Note: To enable login via GitHub OAuth, the following environment variables needs to be set._

| Environment variable | Description                            |
|:--------------------:|----------------------------------------|
|    GHUB_CLIENT_ID    | Id of the GitHub OAuth application     |
|  GHUB_CLIENT_SECRET  | Secret of the GitHub OAuth application |

## How to run test suite locally

While being in the Chirp directory, which was cloned previously, the following command can be used to run all the tests.
```sh
dotnet test
```

# Ethics

## License

Chirp is licensed under [MIT](https://opensource.org/license/mit)

## LLMs, ChatGPT, CoPilot, and others
