const AWS = require('aws-sdk');
const cognito = new AWS.CognitoIdentityServiceProvider();

exports.handler = async (event, context) => {
  const { username } = event;

  try {
    
    const forgotPasswordResponse = await cognito.forgotPassword({
      ClientId: '1q2aum3ptjv1hpb4u3spldal8r',
      Username: username,
    }).promise();

    

    return {
      statusCode: 200,
      body: JSON.stringify({ message: 'Forgot password request initiated successfully' }),
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