const AWS = require('aws-sdk');
//const dynamodb = new AWS.DynamoDB.DocumentClient();
const cognito = new AWS.CognitoIdentityServiceProvider();
//import { DynamoDBClient, PutItemCommand } from "@aws-sdk/client-dynamodb"; 
const { DynamoDBClient, PutItemCommand } = require("@aws-sdk/client-dynamodb");
const client = new DynamoDBClient()






exports.handler = async (event, context, callback) => {
    try {
        console.log(event);
        
        const requestBody = event;
        const email = requestBody.email;
        const password = requestBody.password;

        // Add user to Cognito User Pool
        const signUpParams = {
            ClientId: '1q2aum3ptjv1hpb4u3spldal8r',
            Username: email,
            Password: password,
            UserAttributes: [
                {
                    Name: 'email',
                    Value: email
                }
                // Add more user attributes as needed
            ]
        };

        const signUpResponse = await cognito.signUp(signUpParams).promise();

        // Add user to DynamoDB
        const dynamoDBParams = {
            "TableName": 'test',
            "Item": {
                //"UserId": signUpResponse.UserSub,  // Use the Cognito User Sub as the DynamoDB key
                "email": {"S": email},
                "password": {"S": password},
                // Add more attributes as needed
            }
        };
        
//         const input = {
//           "email": {
//             "AlbumTitle": {
//               "S": "Somewhat Famous"
//             },
//             "Artist": {
//               "S": "No One You Know"
//             },
//             "SongTitle": {
//               "S": "Call Me Today"
//             }
//           },
//           "ReturnConsumedCapacity": "TOTAL",
//           "TableName": "Music"
// };

        //await dynamodb.put(dynamoDBParams).promise();
        const command = new PutItemCommand(dynamoDBParams);
        const responsedb = await client.send(command);

        // Return a response
        const response = {
            statusCode: 200,
            body: JSON.stringify('User added successfully to Cognito User Pool and DynamoDB.')
        };

        return response;
    } catch (error) {
        console.error('Error:', error);
        return {
            statusCode: 500,
            body: JSON.stringify('Internal Server Error')
        };
    }
};
