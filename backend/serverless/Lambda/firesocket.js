const { ApiGatewayManagementApiClient, PostToConnectionCommand } = require("@aws-sdk/client-apigatewaymanagementapi");
const { DynamoDBClient,  GetItemCommand } = require("@aws-sdk/client-dynamodb");
const dbclient = new DynamoDBClient()

const ENDPOINT = 'https://9c0zh8p4oj.execute-api.ap-southeast-1.amazonaws.com/beta/';

const client = new ApiGatewayManagementApiClient({ endpoint: ENDPOINT });

exports.handler = async (event, context) => {
    try {
        const fireresult = event;
    

        //horzontal cumulative roll -- need to be fixed
        //const gyrox = gunresult.gyrox;

        const gyroy = fireresult.fire;
        const gunid = fireresult.gunid;
        

        const playerParams = {
            "TableName": 'arcombat-gunid',
            "Key": {
                "gunid": {"S": gunid},
                
            }
        };


        const commandPlayer = new GetItemCommand(playerParams);
        
        const playerResponse = await dbclient.send(commandPlayer);
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
       
    
        


        // console.log(event);
        const Msg = {
           // Data : hitsocketmsg,
            Data: "fire",
            ConnectionId: playerId
        };
     

       

        const commandSend = new PostToConnectionCommand(Msg);
        await client.send(commandSend);


        return "success"
    } catch (error) {
        console.error('Error:', error);

        return {
            statusCode: 500,
            body: JSON.stringify('Error starting connectio.')
        };
    }
};
