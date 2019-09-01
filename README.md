# r-tindev-poc1

referals to publish to heroku docker see the article 

`https://rmauro.dev/2019/08/18/deploying-net-container-app-to-heroku-cloud/`


## Setup your appsettings.json

    "GithubApi": {
    "Uri": "https://api.github.com",
      "UserAgent": "bootcamp-bbq",
      "Token": "" //create one though developer settings on your github account
    },
    "ConnectionStrings": {
      "Mongo": "" //mongoDb connection String e.g. mongodb://localhost:27038?retryWrites=true&w=majority
    },
    "MongoDb": {
      "Database": "" //name of database in your mongoDb server
    },
    "AppSettings": {
      "Secret": "" //secret with at least 12 bytes
    },
    "HealthChecksUI": {
      "HealthChecks": [
        {
          "Name": "HTTP-Api-Basic",
          "Uri": "http://localhost:9000/healthz" //change to your health check url
        }
      ],
      "EvaluationTimeOnSeconds": 10,
      "MinimumSecondsBetweenFailureNotifications": 60
    }
