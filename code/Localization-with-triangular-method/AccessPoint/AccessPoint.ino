#include <ESP8266WiFi.h>

const char *ssid = "MyESPAP";      // Name of the access point (SSID)
const char *password = "password"; // Password for the access point

void setup() {
  Serial.begin(115200);
  delay(100);

  // Set ESP8266 as an access point
  WiFi.mode(WIFI_AP);
  WiFi.softAP(ssid, password);

  Serial.println("Access Point started");
  Serial.print("IP Address: ");
  Serial.println(WiFi.softAPIP());
}

void loop() {
  // Your code here
}
