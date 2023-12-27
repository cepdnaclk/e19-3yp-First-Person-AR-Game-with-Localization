#include <ESP8266WiFi.h>
#include <PubSubClient.h>
#include "../lib/credentials.h"
WiFiServer server(80);

void setup() {
  Serial.begin(115200);
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    delay(1000);
    Serial.println("Connecting to WiFi...");
  }
  server.begin();
  Serial.println("Server started");
}

void loop() {
  WiFiClient client = server.available();
  if (client) {
    Serial.println("Client connected");
    String currentLine = "";
    while (client.connected()) {
      if (client.available()) {
        char c = client.read();
        currentLine += c;
        if (c == '\n') {
          if (currentLine.length() == 1) {
            client.println("HTTP/1.1 200 OK");
            client.println("Content-type:text/html");
            client.println();
            client.println("<h1>Hello from ESP8266!</h1>");
            break;
          }
          currentLine = "";
        }
      }
    }
    client.stop();
    Serial.println("Client disconnected");
  }
}
