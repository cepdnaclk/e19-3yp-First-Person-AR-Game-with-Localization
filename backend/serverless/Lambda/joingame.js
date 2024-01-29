
const { DynamoDBClient,  GetItemCommand } = require("@aws-sdk/client-dynamodb");
const client = new DynamoDBClient()


exports.handler = async (event, context, callback) => {
    try {
        
        
        const requestBody = JSON.parse(event.body);
        const email = requestBody.email;
    
        const gameid = requestBody.gameid;
     

        // retrieval parameters
        const dynamoDBParams = {
            "TableName": 'Arcombat-arcore',
            "Key": {
                "email": {"S": gameid},
                
            }
        };
        const dynamoDBParamsEnv = {
            "TableName": 'Arcombat-env',
            "Key": {
                "email": {"S": gameid},
                
            }
        };
      
        const commandEnv = new GetItemCommand(dynamoDBParamsEnv);
        const responseEnv = await client.send(commandEnv);

        const users = responseEnv.Item.users.L;
        var found = false
        for (let i = 0; i < users.length; i++) {
            let value = users[i]["S"];
            if (value == email) {
                found = true;
                break;
            }
           
        }
        if (found == false) {
            return {
                statusCode: 500,
                body: JSON.stringify('user not in game')
            };
            }
        else{

       const command = new GetItemCommand(dynamoDBParams);
       const response = await client.send(command);
       const anchorid = response.Item.anchorid.S;


        // Return a response
        return {
            statusCode: 200,
            body: JSON.stringify(anchorid)
        };

        
    }
    //error handleing
    } catch (error) {
        console.error('Error:', error);
        return {
            statusCode: 500,
            body: {"msg":JSON.stringify('Internal Server Error')}
        };
    }
};
