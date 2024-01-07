const { DynamoDBClient, PutItemCommand } = require("@aws-sdk/client-dynamodb");
const client = new DynamoDBClient()

exports.handler = async (event, context, callback) => {
    try {
        
        const requestBody = event;
        const email = requestBody.email;
        
    //error handleing
    } catch (error) {
        console.error('Error:', error);
        return {
            statusCode: 500,
            body: JSON.stringify('Internal Server Error')
        };
    }
};
