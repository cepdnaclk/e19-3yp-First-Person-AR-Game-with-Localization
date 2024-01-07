const AWS = require('aws-sdk');
const cognito = new AWS.CognitoIdentityServiceProvider();
const { DynamoDBClient, PutItemCommand } = require("@aws-sdk/client-dynamodb");
const client = new DynamoDBClient()


exports.handler = async (event, context, callback) => {
    try {
        
        const requestBody = event;
        const email = requestBody.email;
        const password = requestBody.password;
        const gunid = requestBody.gunid;
        const gloveid = requestBody.gloveid;
        const headsetid = requestBody.headsetid;

        // Add user to Cognito User Pool
        const signUpParams = {
            ClientId: '35l37eb37u1bknkqleadfbcui5',
            Username: email,
            Password: password,
            UserAttributes: [
                {
                    Name: 'email',
                    Value: email
                }
            ]
        };

        const signUpResponse = await cognito.signUp(signUpParams).promise();

        // Add user to DynamoDBUser model
        const dynamoDBParamsUser = {
            "TableName": 'Arcombat-user',
            "Item": {
                //"UserId": signUpResponse.UserSub,  // Use the Cognito User Sub as the DynamoDB key
                "email": {"S": email},
                "gunid": {"S": gunid},
                "gloveid": {"S": gloveid},
                "headsetid": {"S": headsetid},
                
            }
        };
        //add environment to DynamoDB model
        const dynamoDBParamsEnv = {
            "TableName": 'Arcombat-env',
            "Item": {
                "email": {"S": email},
                "users": {"L":{}},
                "stationid": {"L":{}}
                           
            }
        };

        //add to user db
        const commandUser = new PutItemCommand(dynamoDBParamsUser);
        const responsedbUser = await client.send(command);

        const commandEnv = new PutItemCommand(dynamoDBParamsEnv);
        const responsedbEnv = await client.send(command);

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
            body: JSON.stringify('Internal Server Error')
        };
    }
};
