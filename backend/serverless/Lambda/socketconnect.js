const { ApiGatewayManagementApiClient, PostToConnectionCommand } = require("@aws-sdk/client-apigatewaymanagementapi");
const client = new ApiGatewayManagementApiClient();
const { DynamoDBClient, UpdateItemCommand, PutItemCommand } = require("@aws-sdk/client-dynamodb");
const dbclient = new DynamoDBClient();

exports.handler = async (event) => {
    try {
        const connectionId = event.requestContext.connectionId;
        const stage = event.requestContext.stage;
        const apiEndpoint = event.requestContext.domainName + '/' + stage;
        
        const jsonString = JSON.stringify({"message": "connection established"});

        const requestBody = event.headers;
        const email = requestBody.email;

        // const inputdb = {
        //     TableName: "Arcombat-user",
        //     Key: {
        //       "email": { S: email },
        //     },
        //     AttributeUpdates: {
        //       connectionid: { Action: "PUT", Value: { S: connectionId } },
        //     },
        //     // ReturnValues: "ALL_NEW",
        //   };


        const dynamoDBParams = {
          TableName: 'Arcombat-socket',
          Item: {
              email: { S: email },
              connectionid: { S: connectionId },
          }
      };

      const commandSock = new PutItemCommand(dynamoDBParams);
      const responsedbEnv = await dbclient.send(commandSock);



        return {
            statusCode: 200,
            body: JSON.stringify('Connection started successfully.')
        };
    } catch (error) {
        console.error('Error starting connection:', error);
        return {
            statusCode: 500,
            body: JSON.stringify('Error starting connection.')
        };
    }
};