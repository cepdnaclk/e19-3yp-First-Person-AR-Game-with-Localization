const { CognitoIdentityProviderClient, ConfirmForgotPasswordCommand } = require("@aws-sdk/client-cognito-identity-provider");
const client = new CognitoIdentityProviderClient();

exports.handler = async (event, context, callback) => {
    const requestBody = JSON.parse(event.body);
    const email = requestBody.email;
    const password = requestBody.password;
    const confirmcode = requestBody.confirmcode;
    try {
        const input = { // ConfirmForgotPasswordRequest
            ClientId: '35l37eb37u1bknkqleadfbcui5',
            Username: email, 
            ConfirmationCode: confirmcode,
            Password: password,
          };
          const command = new ConfirmForgotPasswordCommand(input);
          const response = await client.send(command);

        return {
            statusCode: 200,
            body: JSON.stringify({ message: 'change password success' }),
          };

    }catch (error) {
        return {
            statusCode: 500,
            body: JSON.stringify('Validation failed')
        };
    }
}