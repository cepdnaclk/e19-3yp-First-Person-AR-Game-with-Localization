const { ApiGatewayManagementApiClient, PostToConnectionCommand } = require("@aws-sdk/client-apigatewaymanagementapi");
const { DynamoDBClient,  GetItemCommand } = require("@aws-sdk/client-dynamodb");
const { IoTDataPlaneClient, PublishCommand } = require("@aws-sdk/client-iot-data-plane");
const dbclient = new DynamoDBClient()
const iotclient = new IoTDataPlaneClient();

const ENDPOINT = 'https://9c0zh8p4oj.execute-api.ap-southeast-1.amazonaws.com/beta/';

const client = new ApiGatewayManagementApiClient({ endpoint: ENDPOINT });

exports.handler = async (event, context) => {
    try {
        const result = event;
        const ammo = result.Ammo;
        const health = result.Health;
        const gunid = result.SSID;
        
        var numericPart = gunid[3];
        var topic = "gun/"+numericPart+"/healthammo";

          
        const msg =JSON.stringify({
            "health":health,
            'ammo':ammo
        })

        const input = { // PublishRequest
            topic: topic, 
            
            payload: msg,
           
          };

        
        const command = new PublishCommand(input);
        const response = await iotclient.send(command)

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
            Data: msg,
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
