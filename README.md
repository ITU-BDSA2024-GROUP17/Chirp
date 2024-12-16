# Chirp
Twitter Clone made from ITU course BDSA

## Usage

### Deployment
Chirp can be ran directly from the terminal.
```bash
dotnet example-file.dll
```

Or by being deployed to a cloud service provider.
A example for an Azure pipeline can be seen in this repo.

### Environment variables

| Environment variable | Description                            |
|:--------------------:|----------------------------------------|
|    GHUB_CLIENT_ID    | Id of the GitHub OAuth application     |
|  GHUB_CLIENT_SECRET  | Secret of the GitHub OAuth application |

## Contribute to the project
### Branches
#### Naming convention
##### Feature Branches
if the branch is a feature branch, use the following naming convention:
`feature/<issueID>/<issue-title>`
- Example: `feature/1/Implement-Login-Page`
##### Bug Branches
if the branch is a bugfix branch, use the following naming convention:
`fix/<issueID>/<issue-title>`
- Example: `fix/2/fix-login-page`
##### Independent Branches
_If the branch has **no**_ issue, use the following naming convention:
`fix/<branch-name>`
or `feature/<branch-title>` depending on the branch type.


### Posting Issues
- Use the issue template
- Use the correct user story format: `As a <role>, I want <goal> [, so that <benefit>].`
- Use the correct labels
- Link the issue to the correct milestone & branch
- If the issue is a `requirement` link it to the correct requirement, and use the right nameing convention.
#### Naming convention for issue title
- Reqirement: `<reqID>/<Requirement Title>`
    - Example: `2d/A user can create a tweet`
- Normal Issue: `<Issue Title>`

#### Issue Description template
```markdown
# Description

# Requirement Description

# Acceptence criteria
- [ ] item
- [ ] item
# Resources
- [Link to resource](https://google.com)
- [Link to requirement session notes - iff a requirement issue](URL)
```

### Generating the report

Generate PlantUML SVG diagrams
```sh
plantuml -tsvg ./docs/diagrams/ -o ../images/
```
> **_NOTE:_** Needs to be ran from root of project

Generate PDF - Needs to be ran from root of project
```sh
pandoc ./docs/report.md -o ./docs/2024_itubdsa_group_17_report.pdf
```
> **_NOTE:_** Needs to be ran from root of project
