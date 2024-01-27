const AWS = require('aws-sdk');
const cognito = new AWS.CognitoIdentityServiceProvider();
const { DynamoDBClient, PutItemCommand } = require("@aws-sdk/client-dynamodb");
const client = new DynamoDBClient()
const lambda = new AWS.Lambda();

exports.handler = async (event, context, callback) => {
    try {
        
        
        const requestBody = JSON.parse(event.body);
        const email = requestBody.email;
        const password = requestBody.password;
        const gunid = requestBody.gunid;

        // Add user to Cognito User Pool
        const signUpParams = {
            ClientId: '35l37eb37u1bknkqleadfbcui5',
            "Username": email,
            "Password": password,
            "UserAttributes": [
                {
                    Name: 'email',
                    Value: email
                }
            ]
        };

        //invoke qr generate and store in s3
        const qrdata = {"email": email}
        const qrinput = { 
            FunctionName: "qrstore",
            InvocationType: "RequestResponse",
            Payload: JSON.stringify(qrdata)
          };

        lambda.invoke(qrinput, (err, data) => {
            if (err) {
              console.error(err);
            } else {
              console.log('Lambda Invocation Response:', data);
              return {
                statusCode: 500,
                body: {"msg":JSON.stringify('Internal Server Error')}
            };
            }
          });

        const signUpResponse = await cognito.signUp(signUpParams).promise();

        // Add user to DynamoDBUser model
        const dynamoDBParamsUser = {
            "TableName": 'Arcombat-user',
            "Item": {
                //"UserId": signUpResponse.UserSub,  // Use the Cognito User Sub as the DynamoDB key
                "email": {"S": email},
                "gunid": {"S": gunid},
                
            }
        };
        //add environment to DynamoDB model
        const dynamoDBParamsEnv = {
            "TableName": 'Arcombat-env',
            "Item": {
                "email": {"S": email},
                // "users": {"L":{}},
                // "stationid": {"L":{}}
                           
            }
        };
        

        //add to user db
        const commandUser = new PutItemCommand(dynamoDBParamsUser);
        const responsedbUser = await client.send(commandUser);

        const commandEnv = new PutItemCommand(dynamoDBParamsEnv);
        const responsedbEnv = await client.send(commandEnv);
        
        


        // Return a response
        const response = {
            statusCode: 200,
            body: JSON.stringify('User added successfully to Cognito User Pool and DynamoDB.')
        };

        return response;

    //error handleing
    } catch (error) {
        console.error('Error:', error);
        return {
            statusCode: 500,
            body: {"msg":JSON.stringify('Internal Server Error')}
        };
    }
};
