# tfl Road Status

Test to consume tfl web api

## Getting Started

The document is aimed to describe how to download, setup and run the tests and console client from command prompt.

### Prerequisites

Visual Studio Community Edition 2017 

### Installing

1. Clone the solution from Git
2. Open the solution file RoadStatus.sln
3. Restore Nuget packages 

Update the app_id and app_key values in the projects listed below.
```
Projects to update:
RoadStatus.Client
RoadStatus.Test
```

### Running the tests

After the Test Explorer is opened in VS the following three tests can be tested
- RoadStatusRoadStatusConsoleClientTest - Moq and Unit Testing.
- RoadStatusRoadStatusIntegrationTest - Integration Tests to test integration with tfl Web Api.
- RoadStatusRoadStatusServiceTest - Tests the HttpRequest for valid json response and handling.


### Executing the application from Command Prompt

1. Open Command prompt and navigate to the \RoadStatus.Client\bin\Debug folder
2. Run RoadStatus.exe A2 
The expected result is:
```
        The status of the A2 is as follows
        Road Status is Good
        Road Status Description is No Exceptional Delays
```
3. execute 
4. echo %errorlevel% and you should get back 0 as the exit code.

