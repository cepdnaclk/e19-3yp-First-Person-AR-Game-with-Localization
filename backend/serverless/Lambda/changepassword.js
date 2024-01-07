const { CognitoIdentityProviderClient, ConfirmForgotPasswordCommand } = require("@aws-sdk/client-cognito-identity-provider");
const client = new CognitoIdentityProviderClient();

exports.handler = async (event, context, callback) => {
    email = event.email;
    password = event.password;
    confirmcode = event.confirmcode;
    try {
        const input = { // ConfirmForgotPasswordRequest
            ClientId: '35l37eb37u1bknkqleadfbcui5',
            Username: email, 
            ConfirmationCode: confirmcode,
            Password: password,
          };
          const command = new ConfirmForgotPasswordCommand(input);
          const response = await client.send(command);

    }catch (error) {
        return {
            statusCode: 500,
            body: JSON.stringify('Validation failed')
        };
    }
}