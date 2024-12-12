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

## Architecture — In the small

## Architecture of deployed application

## User activities

## Sequence of functionality/calls trough _Chirp!_

# Process

## Build, test, release, and deployment

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

# Ethics

## License

Chirp is licensed under [MIT](https://opensource.org/license/mit)

## LLMs, ChatGPT, CoPilot, and others
