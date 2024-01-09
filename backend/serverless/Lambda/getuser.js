const { CognitoIdentityProviderClient, GetUserCommand } = require("@aws-sdk/client-cognito-identity-provider");
const client = new CognitoIdentityProviderClient();

exports.handler = async (event, context, callback) => {
    try {
        const requestBody = JSON.parse(event.body);
        const accessToken = requestBody.Authorization; 
        const params = {
            AccessToken: accessToken,
        };
        const command = new GetUserCommand(params);
        const response = await client.send(command)
        return {
            statusCode: 200,
            body: JSON.stringify({
                "email": response.UserAttributes,
            })
        };
    }catch (error) {
        return {
            statusCode: 500,
            body: JSON.stringify('Validation failed')
        };
    }
}