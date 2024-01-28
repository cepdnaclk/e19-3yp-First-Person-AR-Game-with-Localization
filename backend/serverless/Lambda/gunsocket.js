const { ApiGatewayManagementApiClient, PostToConnectionCommand } = require("@aws-sdk/client-apigatewaymanagementapi");
const { DynamoDBClient,  GetItemCommand } = require("@aws-sdk/client-dynamodb");
const dbclient = new DynamoDBClient()

const ENDPOINT = 'https://9c0zh8p4oj.execute-api.ap-southeast-1.amazonaws.com/beta/';

const client = new ApiGatewayManagementApiClient({ endpoint: ENDPOINT });

exports.handler = async (event, context) => {
    try {
        const gunresult = event
        const screenshot = gunresult.screenshot
        const gyrox = gunresult.gyrox
        const gyroy = gunresult.gyroy
        const gunid = gunresult.gunid
        console.log(event)
        

        const playerParams = {
            "TableName": 'Arcombat-gunid',
            "Key": {
                "gunid": {"S": gunid},
                
            }
        };

        const commandPlayer = new GetItemCommand(playerParams);
        const playerResponse = await dbclient.send(commandPlayer);
        const email = playerResponse.Item.email.S;
       
    
        

        const hitsocketmsg =JSON.stringify({
            "hit": "1",
            "shoot": "0"
        })
        const shootsocketmsg =JSON.stringify({
            "hit": "0",
            "shoot": "1"
        })


        // console.log(event);
        const hitMsg = {
           // Data : hitsocketmsg,
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
