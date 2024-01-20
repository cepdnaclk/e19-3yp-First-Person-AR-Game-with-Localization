import AWS from 'aws-sdk';

const ENDPOINT = '9c0zh8p4oj.execute-api.ap-southeast-1.amazonaws.com/beta/';
const client = new AWS.ApiGatewayManagementApi({ endpoint: ENDPOINT });

const sendToOne = async (id, body) => {
  try {
    await client.postToConnection({
      'ConnectionId': id,
      'Data': Buffer.from(JSON.stringify(body)),
    }).promise();
  } catch (err) {
    console.error(err);
  }
};

exports.handler = async (event, context, callback) => {
    try {
        await sendToOne('R02GBdCDSQ0CGgA=', { message: 'hello' });
        return {
            statusCode: 200,
            body: JSON.stringify('Connection started successfully.')
        };

    }catch (error) {
        return {
            statusCode: 500,
            body: JSON.stringify('Error starting connection.')
        };

    }
}