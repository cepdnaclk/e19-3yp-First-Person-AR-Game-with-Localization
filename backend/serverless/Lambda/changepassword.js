const { CognitoIdentityProviderClient, ConfirmForgotPasswordCommand } = require("@aws-sdk/client-cognito-identity-provider");
const client = new CognitoIdentityProviderClient();

exports.handler = async (event, context, callback) => {
    const email = event.email;
    const password = event.password;
    const confirmcode = event.confirmcode;
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
            body: { message: 'change password success' },
          };

    }catch (error) {
        return {
            statusCode: 500,
            body: JSON.stringify('Validation failed')
        };
    }
}