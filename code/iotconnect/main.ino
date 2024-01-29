#include "secrets.h"
#include <WiFiClientSecure.h>
#include <PubSubClient.h>
#include <ESP8266WiFi.h>
#include <WiFiClient.h>

#define DHTPIN 14     // Digital pin connected to the DHT sensor
#define DHTTYPE DHT11   // DHT 11
 
#define AWS_IOT_PUBLISH_TOPIC   "gun/1/test"
#define AWS_IOT_SUBSCRIBE_TOPIC "esp32/sub"

 
float h ;
float t;
 

 
WiFiClient net;;
PubSubClient client(net);
 
void connectAWS()
{
  WiFi.mode(WIFI_STA);
  WiFi.begin(WIFI_SSID, WIFI_PASSWORD);
 
  Serial.println("Connecting to Wi-Fi");
 
  while (WiFi.status() != WL_CONNECTED)
  {
    delay(500);
    Serial.print(".");
  }
 
  // Configure WiFiClientSecure to use the AWS IoT device credentials

  // size_t privateKeySize = AWS_CERT_CA.size();
  // char *privateKey = new char[privateKeySize + 1];

  // AWS_CERT_CA.readBytes(privateKey, privateKeySize);
  // privateKey[privateKeySize] = '\0'; 

  // net.loadCACert(privateKey);
  // net.loadCertificate(AWS_CERT_CRT);
  // net.loadPrivateKey(AWS_CERT_PRIVATE);

  net.setCACert(AWS_CERT_CA);
  net.setCertificate(AWS_CERT_CRT);
  net.setPrivateKey(AWS_CERT_PRIVATE);
 
  // Connect to the MQTT broker on the AWS endpoint we defined earlier
  client.setServer(AWS_IOT_ENDPOINT, 8883);
 
  // Create a message handler
  client.setCallback(messageHandler);
 
  Serial.println("Connecting to AWS IOT");
 
  while (!client.connect(THINGNAME))
  {
    Serial.print(".");
    delay(100);
  }
 
  if (!client.connected())
  {
    Serial.println("AWS IoT Timeout!");
    return;
  }
 
  // Subscribe to a topic

 
  Serial.println("AWS IoT Connected!");
}
 
void publishMessage()
{
 
  client.publish(AWS_IOT_PUBLISH_TOPIC, "he");
}
 
void messageHandler(char* topic, byte* payload, unsigned int length)
{
  Serial.print("incoming: ");
  Serial.println(topic);

}
 
void setup()
{
  Serial.begin(115200);
  connectAWS();
 
}
 
void loop()
{
  h = 9;
  t =7;
 
 
  if (isnan(h) || isnan(t) )  // Check if any reads failed and exit early (to try again).
  {
    Serial.println(F("Failed to read from DHT sensor!"));
    return;
  }
 
  Serial.print(F("Humidity: "));
  Serial.print(h);
  Serial.print(F("%  Temperature: "));
  Serial.print(t);
  Serial.println(F("°C "));
 
  publishMessage();
  client.loop();
  delay(1000);
}