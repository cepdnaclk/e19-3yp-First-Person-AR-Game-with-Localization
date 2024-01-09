const { ApiGatewayManagementApiClient, PostToConnectionCommand } = require("@aws-sdk/client-apigatewaymanagementapi");
const client = new ApiGatewayManagementApiClient();
const ApiGatewayManagementApi = require('aws-api-gateway-management-api')

exports.handler = async (event) => {
    try {
        const connectionId = event.requestContext.connectionId;
        const stage = event.requestContext.stage;
        const apiEndpoint = event.requestContext.domainName + '/' + stage;
        
        const jsonString = JSON.stringify({"message": "connection established"})
        const blobData = Buffer.from(jsonString);

        const input = { // PostToConnectionRequest
              "Data": blobData, // required
              "ConnectionId": connectionId
        }
        const command = new PostToConnectionCommand(input);
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