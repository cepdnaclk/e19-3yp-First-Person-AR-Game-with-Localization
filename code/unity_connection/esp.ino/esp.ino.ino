#include <ESP8266WiFi.h>
#include <WiFiUdp.h>

const char* ssid = "SLT_FIBER_XXXXX";
const char* password = "43266@abc";
const IPAddress unityIP(192, 168, 1, 80);  // Unity's IP address
const unsigned int unityPort = 12345;       // Unity's port

WiFiUDP udp;
unsigned int localPort = 12345;

void setup() {
  Serial.begin(115200);
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    delay(1000);
    Serial.println("Connecting to WiFi...");
  }
  Serial.println("Connected to WiFi");
  Serial.println(WiFi.localIP());
  udp.begin(localPort);
}

void loop() {
  String message = "Hello from ESP8266!";
  
  udp.beginPacket(unityIP, unityPort);
  udp.write((const uint8_t*)message.c_str(), message.length());
  udp.endPacket();

  delay(1000); // Adjust delay as needed
}
