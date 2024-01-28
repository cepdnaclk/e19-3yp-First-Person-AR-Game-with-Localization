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

        const playerIdParams = {
            "TableName": 'Arcombat-socket',
            "Key": {
                "email": {"S": email},
                
            }
        };

        const commandplayerId = new GetItemCommand(playerIdParams);
        const responseplayerId = await dbclient.send(commandplayerId);
        const playerId = responseplayerId.Item.connectionid.S;
       
    
        

        const gyrosocketmsg =JSON.stringify({
            "gyrox": gyrox,
            "gyroy": gyroy
        })
        const shootsocketmsg =JSON.stringify({
            "hit": "0",
            "shoot": "1"
        })


        // console.log(event);
        const gyroMsg = {
           // Data : hitsocketmsg,
            Data: gyrosocketmsg,
            ConnectionId: playerId
        };
        const screenshotMsg = {
            Data:"screenshot",
            ConnectionId: playerId,
        };

        if (screenshot == "1"){
            const commandSendScreenshot = new PostToConnectionCommand(screenshotMsg);
            await client.send(commandSendScreenshot);
        }

        const commandSendGyro = new PostToConnectionCommand(gyroMsg);
        await client.send(commandSendGyro);


        return {
            statusCode: 200,
            body: JSON.stringify('msg passed.')
        };
    } catch (error) {
        console.error('Error:', error);

        return {
            statusCode: 500,
            body: JSON.stringify('Error starting connectio.')
        };
    }
};
