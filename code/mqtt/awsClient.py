from AWSIoTPythonSDK.MQTTLib import AWSIoTMQTTClient
import time

# For certificate based connection
myMQTTClient = AWSIoTMQTTClient("gun0001")

# For Websocket connection
# myMQTTClient = AWSIoTMQTTClient("myClientID", useWebsocket=True)

# Configurations
# For TLS mutual authentication
myMQTTClient.configureEndpoint("a2leuqp8y2i70g-ats.iot.ap-southeast-1.amazonaws.com", 8883)
# For Websocket
# myMQTTClient.configureEndpoint("yourEndpoint.amazonaws.com", 443)

# For certificate based connection
myMQTTClient.configureCredentials("awsRootCA.pem", "deviceCert.crt", "deviceCert.key")
# For Websocket, we only need to configure the root CA
# myMQTTClient.configureCredentials("root-CA.crt")

# Connect and subscribe to AWS IoT
myMQTTClient.connect()

# Publish to the same topic in a loop forever
loopCount = 0
while True:
    myMQTTClient.publish("myTopic", "New Message " + str(loopCount), 1)
    loopCount += 1
    print(loopCount)
    time.sleep(1)