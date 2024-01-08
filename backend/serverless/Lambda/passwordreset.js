const AWS = require('aws-sdk');
const cognito = new AWS.CognitoIdentityServiceProvider();

exports.handler = async (event, context) => {
  const requestBody = JSON.parse(event.body);
  const  email  = event.email;

  try {
    
    const forgotPasswordResponse = await cognito.forgotPassword({
      ClientId: '35l37eb37u1bknkqleadfbcui5',
      Username: email,
    }).promise();
    

    return {
      statusCode: 200,
      body: JSON.stringify({ message: 'Forgot password request successfully, check the spam folder' }),
    };
  } catch (error) {
    // Handle errors appropriately
    console.error('Error:', error);

    return {
      statusCode: 500,
      body: JSON.stringify({ error: 'Internal Server Error' }),
    };
  }
};