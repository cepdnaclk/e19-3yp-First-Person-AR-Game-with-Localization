const { ApiGatewayManagementApiClient, PostToConnectionCommand } = require("@aws-sdk/client-apigatewaymanagementapi");
const { DynamoDBClient,  GetItemCommand } = require("@aws-sdk/client-dynamodb");
const dbclient = new DynamoDBClient()

const ENDPOINT = 'https://9c0zh8p4oj.execute-api.ap-southeast-1.amazonaws.com/beta/';
const client = new ApiGatewayManagementApiClient({ endpoint: ENDPOINT });

exports.handler = async (event, context) => {
    try {
        const opencvresult = JSON.parse(event)
        const hit = opencvresult.hit
        const shooter = opencvresult.shooter

        const hitterIdParams = {
            "TableName": 'Arcombat-socket',
            "Key": {
                "email": {"S": hit},
                
            }
        };

        const shooterIdParams = {
            "TableName": 'Arcombat-socket',
            "Key": {
                "email": {"S": shooter},
                
            }
        };
        const commandHitId = new GetItemCommand(hitterIdParams);
       const responseHitId = await client.send(commandHitId);
       const hitterId = response.Item.connectionid.S;

        console.log(event);
        const hitMsg = {
            Data: JSON.stringify({
                "hit": "1",
                "shoot": "0"
            }),
            ConnectionId: hitterId,
        };
        const shootMsg = {
            Data: JSON.stringify({
                "hit": "0",
                "shoot": "1"
            }),
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
