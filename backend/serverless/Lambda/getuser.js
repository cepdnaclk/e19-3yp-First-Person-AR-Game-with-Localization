const { CognitoIdentityProviderClient, GetUserCommand } = require("@aws-sdk/client-cognito-identity-provider");
const client = new CognitoIdentityProviderClient();

exports.handler = async (event, context, callback) => {
    try {
        const accessToken = event.Authorization; 
        const params = {
            AccessToken: accessToken,
        };
        const userData = await cognito.getUser(params).promise();
        const email = userData.Username
        return {
            statusCode: 200,
            body: {
                "email": email,
            }
        };
    }catch (error) {
        return {
            statusCode: 500,
            body: JSON.stringify('Validation failed')
        };
    }
}