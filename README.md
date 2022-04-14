# Job scheduling

## The problem
To simulate a realistic problem, This small repository will be implementing a minimal Task scheduling solution exposed via a web API. In short, This solution would make it possible for clients to enqueue an filter through a file to keep only German phone numbers and store them for later retrieval in the background and to query the state of any previously enqueued job.

For sake of minimizing the complexity, we are choosing to process a file but it could be any time consuming process (eg: Image processing & facial regonition/mapping, Data Mining etc.). We have also chosen to use In-memory object instead of database to keep it simple and not dependent on any back-end (eg. SQL). 

### Functional requirements
Develop a web API that supports the following operations:
1. The client can enqueue a new Task, by providing an txt file as input
2. The client can retrieve an overview of all Task (both pending and completed)
3. The client can retrieve a specific Task by its ID (see below), including the output (with phone number list) if the Task has completed

Task 1
German phones parser

German phone numbers always start with +49 or 0049 when dialed from aboard. Should build an API which would filter through a file to keep only German phone numbers and store them for later retrieval. For simplicity, should only consider phone numbers which have 11 digits after +49 or 0049. For example: +4915201365263 or 004915201365263 are valid phone numbers.

Service should allow the following:

A). Submit a file to the endpoint to be processed. This endpoint should return a Task ID which could be used to fetch or delete the results. Example files are provided, see phone_numbers_1.txt phone_numbers_2.txt phone_numbers_3.txt ...
B). Using the Task ID, a user should be able to retrieve the results of the submitted file
C). Using the Task ID, a user should be allowed to delete the results associated to the Task ID
D). An endpoint to fetch all Task IDs
E). The API should be explorable locally with swagger. Please provide the link to visit once the API is up and running.
F). Be aware of duplicates.

Task 2: Testing
Every good application should be tested. Please provide instructions on how to run the tests in a README file.

Task 3: Deployment
Now that have solution up and running. How would deploy it?

Deliverable
A README file which includes instructions on which links to open for the Web Application and Swagger. Instructions on how to run the tests.

A Task scheduling takes an text file as input, process the input file and parsing using german parser algorithm, and outputs german phone number. Apart from the input and output a Task It also includes the following metadata: 
1. Id - a unique GUID assigned by the application
2. EnqueuedUtcTimeStamp - when was the job enqueued.
3. CompletedUtcTimeStamp - when was the job completed.
4. Duration - How much time did it take to execute the job.
5. Status - Status of the job (eg: Completed, Pending etc.)

Focus area for this solution is in JobSchedule.External where it's using Channel approach for running async background service.

#### Delivery includes
1. Web API including background job processing.
2. Logging features for background processing and web controller events and middleware logger. 
4. Unit Test and Integration test covering key components.
5. Publishing and deployment


#### Pre-Requisite:
1. Visual Studio 2019 with common packages installed (eg: .Net Framework / .Net Core 3.1 etc.) & nuget feed.

#### Solution consists of following projects
1. Jobschedule.API : .Net Core 3.1 Web Api based project. It also includes Background service within for the sake of simplicity and demo purposes. At later stage, Background service can be extracted and hosted as independent background service.
2. JobSchedule.Shared : Containing all the common/shared models which can be used across the projects.
3. JobSchedule.External : Containing classes to retreive data which might be fetched via some other external services etc.
  3.1 JobSchedule.External.UnitTest : Unit Test Project
4. Jobschedule.Data : Containing all methods to store and get data in inmemory
4 JobSchedule.IntegrationTest : Integration Test of Web Api Project (JobSchedule) with use case of common features being covered & acceptance criteria.

#### How to build:

1. Clone the project using Visual Studio 2019.
2. Go to menu -> Build -> Build Solution.
3. Go to menu -> Test -> Run All Tests
4. Hit F5 (Run) or go to menu -> Debug -> Start Debugging
  4.1 Please ensure startup solution is JobSchedule
  4.2 Please ensure running configuration profile is 'JobSchedule - Development'. Staging and Production environment will also be able to run but please do not run under IIS Projects.
5. Once run, it should open the web browser with url : http://localhost:30680/swagger/index.html 

#### How to build and running using docker
1. Install https://dotnet.microsoft.com/en-us/download/dotnet/3.1
2. Install https://www.docker.com/products/docker-desktop/
3. Download and extract ZIP file
4. Open command prompt and “cd <root folder of extracted folder>”
5.	To build unit test cases execute following command
		docker build --target unittestrunner -t jobschedule-service-tests:latest .
6. 5To build integration test cases execute following command
		docker build --target integrationtestrunner -t jobschedule-service-tests:latest .
6.	To run test cases, execute following command
		docker run -t jobschedule-service-tests:latest
7.	To build API
		docker build -t jobschedule-service:latest .
8.	To run API
		docker run --rm -it -p 30680:80 jobschedule-service:latest
9.	Open in browser http://localhost:30680/swagger/index.html 

#### Deployment in AWS EC2
1. The application is deployed in amazon EC2 : http://ec2-65-1-114-4.ap-south-1.compute.amazonaws.com/swagger/index.html

#### Design Process
1. projects libraries are small and independent as possible.
2. Keeping reusable code to be developed using class library so later on we can push them into artifactory as nuget packages and consume directly from nuget packages instead of referencing in project dependency manually.
3. Using Industry best practices to keep the seperation of concerns.
4. Web Api service is running Background Service.
5. Unit testing and integration testings are done by XUnit and Moq
6. Solution can extensible for any other phone number parsing
