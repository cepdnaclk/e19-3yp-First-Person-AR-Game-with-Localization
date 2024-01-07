const AWS = require('aws-sdk');
const cognito = new AWS.CognitoIdentityServiceProvider();

exports.handler = async  (event, context, callback) => {
 

  // Use Cognito API to authenticate the user
  const cognitoResponse = await cognito.initiateAuth({
    AuthFlow: 'USER_PASSWORD_AUTH',
    AuthParameters: {
      USERNAME: event.username,
      PASSWORD: event.password,
    },
    ClientId: '35l37eb37u1bknkqleadfbcui5',
  }).promise();

  // Generate access token
  const { AccessToken, RefreshToken } = cognitoResponse.AuthenticationResult

  // Include the tokens in the response
  return {
    statusCode: 200,
    body: {
      accessToken: AccessToken,
      refreshToken: RefreshToken
    }
  };
}