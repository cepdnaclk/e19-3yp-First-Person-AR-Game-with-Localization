const { ApiGatewayManagementApiClient, PostToConnectionCommand } = require("@aws-sdk/client-apigatewaymanagementapi");
const client = new ApiGatewayManagementApiClient();
const { DynamoDBClient, UpdateItemCommand } = require("@aws-sdk/client-dynamodb");
const dbclient = new DynamoDBClient();

exports.handler = async (event) => {
    try {
        const connectionId = event.requestContext.connectionId;
        const stage = event.requestContext.stage;
        const apiEndpoint = event.requestContext.domainName + '/' + stage;
        
        const jsonString = JSON.stringify({"message": "connection established"})
        const blobData = Buffer.from(jsonString);

        const requestBody = JSON.parse(event.body);
        const email = requestBody.email;

        const inputdb = {
            TableName: "Arcombat-user",
            Key: {
              "email": { S: email },
            },
            AttributeUpdates: {
              connectionid: { Action: "PUT", Value: { S: connectionId } },
            },
            // ReturnValues: "ALL_NEW",
          };


        // const input = { // PostToConnectionRequest
        //       "Data": blobData, // required
        //       "ConnectionId": connectionId
        // }
        // const command = new PostToConnectionCommand(input);
        // const response = await client.send(command);
        const command = new UpdateItemCommand(inputdb);
        const response = await client.send(command);

        return {
            statusCode: 200,
            body: 'Connection started successfully.',
        };
    } catch (error) {
        console.error('Error starting connection:', error);
        return {
            statusCode: 500,
            body: 'Error starting connection.',
        };
    }
};