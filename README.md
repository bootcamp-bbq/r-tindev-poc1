# r-tindev-poc1

referals to publish to heroku docker see the article 

`https://rmauro.dev/2019/08/18/deploying-net-container-app-to-heroku-cloud/`


## Setup your appsettings.json

  "GithubApi": {
    "Uri": "https://api.github.com",
    "UserAgent": "bootcamp-bbq",
    "Token": ""
  },
  "ConnectionStrings": {
    "Mongo": ""
  },
  "MongoDb": {
    "Database": ""
  },
  "AppSettings": {
    "Secret": ""
  },
  "HealthChecksUI": {
    "HealthChecks": [
      {
        "Name": "HTTP-Api-Basic",
        "Uri": "http://localhost:9000/healthz"
      }
    ],
    "EvaluationTimeOnSeconds": 10,
    "MinimumSecondsBetweenFailureNotifications": 60
  }
