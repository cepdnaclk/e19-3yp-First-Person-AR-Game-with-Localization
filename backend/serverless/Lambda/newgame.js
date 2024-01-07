const { DynamoDBClient, PutItemCommand } = require("@aws-sdk/client-dynamodb");
const client = new DynamoDBClient()

exports.handler = async (event, context, callback) => {
    try {
        
        const requestBody = event;
        const email = requestBody.email;
        const users = requestBody.users;
        const stationid = requestBody.stationid;
        const dynamoDBParamsEnv = {
            "TableName": 'Arcombat-env',
            "Item": {
                "email": {"S": email},
                "users": {"L":users},
                "stationid": {"L":stationid}
                           
            }
        };
        const commandEnv = new PutItemCommand(dynamoDBParamsEnv);
        const responsedbEnv = await client.send(commandEnv);
        return {
            statusCode: 200,
            body: JSON.stringify({ message: 'new game created successfully' }),
        };

    //error handleing
    } catch (error) {
        console.error('Error:', error);
        return {
            statusCode: 500,
            body: JSON.stringify('Internal Server Error')
        };
    }
};
