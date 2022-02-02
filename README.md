# AWS Beanstalk API Server

This is an API Server template in ASP.NET Core to easily get started developing API Server to AWS Beanstalk.

## Prerequisite:

- AWS.Logger.AspNetCore v3.2.0

- Visual Studio Community v2019

- ASP.NET Core 3.1


## Setup environment

1. Install [.Net Core 3.0 SDK](https://dotnet.microsoft.com/download/dotnet-core/3.0)

2. Install ASP.NET Core and AWS SDK

      `dotnet add package AWS.Logger.AspNetCore --Version 3.2.0`

3. Run the application locally

      `dotnet fake build -t run`

4. Open URL at [http://localhost:8000](http://localhost:8000))


## Deploying API Server to AWS Beanstalk

1. Launch an AWS Elastic Beanstalk environment  
  - Setup environment: single instance
  - Platfrom: Windows Server

2.  Set up CloudWatch Logs

2-1. Set up LogGroup in CloudWatch beforehand.

2.2 Activate Log Stream by Beanstalk Console UI on AWS.

2.3 Create a policy below and attach to arn:aws:I am::[your AWS ID]:role/aws-elasticbeanstalk-ec2-role

```
{
    "Version": "2012-10-17",
    "Statement":
        {
            "Sid": "CloudWatchLogsAccess",
            "Effect": "Allow",
            "Action": "logs:*",
            "Resource": "*"
        }
    ]
}
```

2.4 Modify LogGroup Name

vim appsettings.json

```
{
  "Logging": {
    "Region": "<Your Region>",
    "LogGroup": "/aws/elasticbeanstalk/<your LogGroup>/<your log LogStream Name>",
    "IncludeLogLevel": true,
    "IncludeCategory": true,
    "IncludeNewline": true,
    "IncludeException": true,
    "IncludeEventId": false,
    "IncludeScopes": false,
    "LogLevel": {
      "Default": "Debug",
      "System": "Information",
      "Microsoft": "Information",
      "AWS": "Debug"
    }
  },
  "AllowedHosts": "*"
}
```

3. Build and upload

* Create new bundle `dotnet fake build -t bundle`

* Upload `dotnet-core-eb-api-server.zip` to the EB environment


Please refer to the following for details.

Medium: https://medium.com/game-tech-tutorial/deploy-asp-net-core-to-aws-elastic-beanstalk-with-cloudwatch-log-873b5d88a5dc

