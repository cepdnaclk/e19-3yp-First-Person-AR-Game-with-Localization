const { ApiGatewayManagementApiClient, PostToConnectionCommand } = require("@aws-sdk/client-apigatewaymanagementapi");
const { DynamoDBClient,  GetItemCommand } = require("@aws-sdk/client-dynamodb");
const dbclient = new DynamoDBClient()

const ENDPOINT = 'https://9c0zh8p4oj.execute-api.ap-southeast-1.amazonaws.com/beta/';

const client = new ApiGatewayManagementApiClient({ endpoint: ENDPOINT });

exports.handler = async (event, context) => {
    try {
        const opencvresult = event
        const hit = opencvresult.hit
        const shooter = opencvresult.shooter
        console.log(hit)
        console.log(shooter)

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
        const responseHitId = await dbclient.send(commandHitId);
        const hitterId = responseHitId.Item.connectionid.S;

        const commandShootId = new GetItemCommand(shooterIdParams);
        const responseShootId = await dbclient.send(commandShootId);
        const shooterId = responseShootId.Item.connectionid.S;
        console.log(hitterId)
        console.log(shooterId)
        // {
        //     "hit": "1",
        //     "shoot": "0"
        // },
        // console.log(event);
        const hitMsg = {
            Data: "hit",
            ConnectionId: hitterId,
        };
        const shootMsg = {
            Data:"score",
            ConnectionId: shooterId,
        };


        const commandSendHitter = new PostToConnectionCommand(hitMsg);
        await client.send(commandSendHitter);

        const commandSendShooter = new PostToConnectionCommand(shootMsg);
        await client.send(commandSendShooter);

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
