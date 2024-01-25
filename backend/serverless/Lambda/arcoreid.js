
const { DynamoDBClient, PutItemCommand } = require("@aws-sdk/client-dynamodb");
const client = new DynamoDBClient()


exports.handler = async (event, context, callback) => {
    try {
        
        
        const requestBody = JSON.parse(event.body);
        const email = requestBody.email;
    
        const anchorid = requestBody.anchorid;
     

        // Add user to DynamoDB arcore
        const dynamoDBParams = {
            "TableName": 'Arcombat-arcore',
            "Item": {
                "email": {"S": email},
                "anchorid": {"S": anchorid},
                
            }
        };
      

        //add to user db
        const command = new PutItemCommand(dynamoDBParams);
        const responsedbUser = await client.send(command);


        // Return a response
        const response = {
            statusCode: 200,
            body: JSON.stringify('arcore anchor stored success.')
        };

        return response;

    //error handleing
    } catch (error) {
        console.error('Error:', error);
        return {
            statusCode: 500,
            body: {"msg":JSON.stringify('Internal Server Error')}
        };
    }
};
