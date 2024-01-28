const { ApiGatewayManagementApiClient, PostToConnectionCommand } = require("@aws-sdk/client-apigatewaymanagementapi");
const { DynamoDBClient,  GetItemCommand } = require("@aws-sdk/client-dynamodb");
const dbclient = new DynamoDBClient()

const ENDPOINT = 'https://9c0zh8p4oj.execute-api.ap-southeast-1.amazonaws.com/beta/';

const client = new ApiGatewayManagementApiClient({ endpoint: ENDPOINT });

exports.handler = async (event, context) => {
    try {
        const gunresult = event;
    

        //horzontal cumulative roll -- need to be fixed
        //const gyrox = gunresult.gyrox;

        const gyroy = gunresult.gyroy;
        const gunid = gunresult.gunid;
        console.log(event)
        ;

        const playerParams = {
            "TableName": 'arcombat-gunid',
            "Key": {
                "gunid": {"S": gunid},
                
            }
            
        };


        const commandPlayer = new GetItemCommand(playerParams);
        
        const playerResponse = await dbclient.send(commandPlayer);
        console.log(playerResponse);
        const email = playerResponse.Item.email.S;
    //        const email = "e19163@eng.pdn.ac.lk"

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
            // "gyrox": gyrox,
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

       

        const commandSendGyro = new PostToConnectionCommand(gyroMsg);
        await client.send(commandSendGyro);


        return "success"
    } catch (error) {
        console.error('Error:', error);

        return {
            statusCode: 500,
            body: JSON.stringify('Error starting connectio.')
        };
    }
};
