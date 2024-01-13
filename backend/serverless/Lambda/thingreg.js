var AWS = require('aws-sdk');
    
exports.handler = async  (event, context, callback) => {
    
    //Replace it with the AWS region the lambda will be running in
    var region = "ap-southeast-1";
    
    var accountId = event.awsAccountId.toString().trim();

    var iot = new AWS.Iot({'region': region, apiVersion: '2015-05-28'});
    var certificateId = event.certificateId.toString().trim();
    
     //Replace it with your desired topic prefix
    var topicName = `${certificateId}`;

    var certificateARN = `arn:aws:iot:${region}:${accountId}:cert/${certificateId}`;
    var policyName = `Policy_${certificateId}`;
    
    //Policy that allows connect, publish, subscribe and receive
    var policy = {
        "Version": "2012-10-17",
        "Statement": [
            {
                "Effect": "Allow",
                "Action": [
                    "iot:Connect"
                ],
                "Resource": `arn:aws:iot:${region}:${accountId}:client/${certificateId}`
            },
            {
                "Effect": "Allow",
                "Action": [
                    "iot:Publish",
                    "iot:Receive"
                ],
                "Resource": `arn:aws:iot:${region}:${accountId}:topic/${topicName}/*`
            },
            {
                "Effect": "Allow",
                "Action": [
                    "iot:Subscribe",
                ],
                "Resource": `arn:aws:iot:${region}:${accountId}:topicfilter/${topicName}/#`
            }
        ]
    };

   //create policy
    iot.createPolicy({
        policyDocument: JSON.stringify(policy),
        policyName: policyName
    }, (err, data) => {
        //if the policy exits
        if (err && (!err.code || err.code !== 'ResourceAlreadyExistsException')) {
            console.log(err);
            callback(err, data);
            return;
        }
        console.log(data);
        
        //attach the policy to the certificate
        iot.attachPrincipalPolicy({
            policyName: policyName,
            principal: certificateARN
        }, (err, data) => {
            //Ignore if the policy is already attached
            if (err && (!err.code || err.code !== 'ResourceAlreadyExistsException')) {
                console.log(err);
                callback(err, data);
                return;
            }
            console.log(data);
            //certificate activation
            iot.updateCertificate({
                certificateId: certificateId,
                newStatus: 'ACTIVE'
            }, (err, data) => {
                if (err) {
                    console.log(err, err.stack); 
                    callback(err, data);
                }
                else {
                    console.log(data);   
                    callback(null, "Success, created, attached policy and activated the certificate " + certificateId);
                }
            });
        });
    });
 
}