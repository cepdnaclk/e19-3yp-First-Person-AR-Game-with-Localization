const AWS = require('aws-sdk');

exports.handler = async (event, context, callback) => {
    const region = "ap-southeast-1";
    const accountId = event.awsAccountId.toString().trim();
    console.log(event);

    const iot = new AWS.Iot({ 'region': region, apiVersion: '2015-05-28' });
    const certificateId = event.certificateId.toString().trim();
    const topicName = `${certificateId}`;
    const certificateARN = `arn:aws:iot:${region}:${accountId}:cert/${certificateId}`;
    const policyName = `Policy_${certificateId}`;

    const policy = {
        "Version": "2012-10-17",
        "Statement": [
            {
                "Effect": "Allow",
                "Action": ["iot:Connect"],
                "Resource": `arn:aws:iot:${region}:${accountId}:client/${certificateId}`
            },
            {
                "Effect": "Allow",
                "Action": ["iot:Publish", "iot:Receive"],
                "Resource": `arn:aws:iot:${region}:${accountId}:topic/*`
            },
            {
                "Effect": "Allow",
                "Action": ["iot:Subscribe"],
                "Resource": `arn:aws:iot:${region}:${accountId}:topicfilter/${topicName}/#`
            }
        ]
    };

    try {
        console.log("Started creating policy");
        const policyResult = await iot.createPolicy({
            policyDocument: JSON.stringify(policy),
            policyName: policyName
        }).promise();

        console.log("Policy created:", policyResult);

        console.log("Attaching policy to certificate");
        const attachPolicyResult = await iot.attachPrincipalPolicy({
            policyName: policyName,
            principal: certificateARN
        }).promise();

        console.log("Policy attached to certificate:", attachPolicyResult);

        console.log("Activating certificate");
        const activateCertificateResult = await iot.updateCertificate({
            certificateId: certificateId,
            newStatus: 'ACTIVE'
        }).promise();

        console.log("Certificate activated:", activateCertificateResult);

        callback(null, "Success, created, attached policy, and activated the certificate " + certificateId);
    } catch (err) {
        console.error("Error:", err);
        callback(err);
    }
};
