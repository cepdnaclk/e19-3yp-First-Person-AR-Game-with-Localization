const { CognitoIdentityProviderClient, GetUserCommand } = require("@aws-sdk/client-cognito-identity-provider");
const client = new CognitoIdentityProviderClient();

exports.handler = async (event, context, callback) => {
    try {
        const accessToken = event.Authorization; 
        const params = {
            AccessToken: accessToken,
        };
        const command = new GetUserCommand(params);
        const response = await client.send(command)
        return {
            statusCode: 200,
            body: {
                "email": response.username,
            }
        };
    }catch (error) {
        return {
            statusCode: 500,
            body: JSON.stringify('Validation failed')
        };
    }
}