# MetaMoodAWSAPI
This is a Web API that retrieves both raw data and the results of sentiment analysis from a MySQL DB and sends it to a Blazor WASM instance hosted on an 
EC2 instance with NGINX. This API uses AWS API Gateway and AWS Lambda.

This project was done in Visual Studio with AWS Toolkit and AWS credentials set up, so all of that is necessary before making this API work.

From there, download this GitHub repository. To make any changes to the function, change the code then republish to AWS using the proper credentials, permissions, etc. 

To integrate a function into AWS API Gateway, create a route with the appropriate HTTP verb (GET, PUT, POST, DELETE, etc.) and attach an AWS Lambda function integration.
Whenever the appropriate HTTP verb is sent to that route, it will trigger the lambda function and return the results. 
