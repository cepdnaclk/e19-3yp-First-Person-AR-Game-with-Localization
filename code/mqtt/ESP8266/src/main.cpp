// #include <Arduino.h>

// // put function declarations here:
// int myFunction(int, int);

// void setup() {
//   // put your setup code here, to run once:
//   int result = myFunction(2, 3);
// }

// void loop() {
//   // put your main code here, to run repeatedly:
// }

// // put function definitions here:
// int myFunction(int x, int y) {
//   return x + y;
// }
#include <ESP8266WiFi.h>
#include <PubSubClient.h>
 
const char* ssid = "Dasun's Galaxy M21"; // Enter your WiFi name
const char* password =  "sise9444"; // Enter WiFi password
const char* mqttServer = "192.168.182.122";
const int mqttPort = 1883;
// const char* mqttUser = "otfxknod";
// const char* mqttPassword = "nSuUc1dDLygF";
 
WiFiClient espClient;
PubSubClient client(espClient);

void callback(char* topic, byte* payload, unsigned int length) {
    Serial.print("Message arrived [");
    Serial.print(topic);
    Serial.print("] ");
    for (unsigned int i = 0; i < length; i++) {
        Serial.print((char)payload[i]);
    }
    Serial.println();
}
 
void setup() {
 
  Serial.begin(115200);
 
  WiFi.begin(ssid, password);
 
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.println("Connecting to WiFi..");
  }
  Serial.println("Connected to the WiFi network");
 
  client.setServer(mqttServer, mqttPort);
  client.setCallback(callback);
 
  while (!client.connected()) {
    Serial.println("Connecting to MQTT...");
 
    if (client.connect("ESP8266Client" )) {
 
      Serial.println("connected");  
 
    } else {
 
      Serial.print("failed with state ");
      Serial.print(client.state());
      delay(2000);
 
    }
  }
 
  client.publish("python/mqtt", "hello"); //Topic name
  client.subscribe("python/mqtt");
 
}
 
// void callback(char* topic, byte* payload, unsigned int length) {
 
//   Serial.print("Message arrived in topic: ");
//   Serial.println(topic);
 
//   Serial.print("Message:");
//   for (int i = 0; i < length; i++) {
//     Serial.print((char)payload[i]);
//   }
 
//   Serial.println();
//   Serial.println("-----------------------");
 
// }
 
void loop() {
  client.loop();
}