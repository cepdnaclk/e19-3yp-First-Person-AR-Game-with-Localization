#include <ESP8266WiFi.h>
#include <WiFiUdp.h>

const char* ssid = "SLT_FIBER_XXXXX";
const char* password = "43266@abc";

WiFiUDP udp;
unsigned int localPort = 12345; // Must match the port number in Unity

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
  int packetSize = udp.parsePacket();
  if (packetSize) {
    char packetData[packetSize];
    udp.read(packetData, packetSize);
    Serial.write(packetData, packetSize);
    // Process received data here
  }
}
