const { ApiGatewayManagementApiClient, PostToConnectionCommand } = require("@aws-sdk/client-apigatewaymanagementapi");

const ENDPOINT = 'https://9c0zh8p4oj.execute-api.ap-southeast-1.amazonaws.com/beta/';
const client = new ApiGatewayManagementApiClient({ endpoint: ENDPOINT });

exports.handler = async (event, context) => {
    try {
        console.log(event);
        const input = {
            Data: "hello hhj",
            ConnectionId: 'R1QipefWSQ0CEUQ=', // Replace with your actual ConnectionId
        };

        const command = new PostToConnectionCommand(input);
        await client.send(command);

        return {
            statusCode: 200,
            body: JSON.stringify('Connection started successfully.')
        };
    } catch (error) {
        console.error('Error:', error);

        return {
            statusCode: 500,
            body: JSON.stringify('Error starting connection.')
        };
    }
};
