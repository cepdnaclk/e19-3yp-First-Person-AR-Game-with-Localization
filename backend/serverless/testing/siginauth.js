const AWS = require('aws-sdk');
const cognito = new AWS.CognitoIdentityServiceProvider();

exports.handler = async  (event, context, callback) => {
  // Process authentication request
  // Use Cognito API to authenticate the user
  const cognitoResponse = await cognito.initiateAuth({
    AuthFlow: 'USER_PASSWORD_AUTH',
    AuthParameters: {
      Username: event.username,
      Password: event.password,
    },
    ClientId: '1q2aum3ptjv1hpb4u3spldal8r',
  }).promise();

  // Generate access token
  const accessToken = cognitoResponse.AuthenticationResult.AccessToken;

  // Include the access token in the response
  return {
    statusCode: 200,
    body: JSON.stringify({ accessToken }),
  };
}