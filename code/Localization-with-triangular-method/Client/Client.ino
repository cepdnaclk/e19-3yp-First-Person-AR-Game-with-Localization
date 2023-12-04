#include <ESP8266WiFi.h>

const char *ssid = "MyESPAP"; // Same as the Access Point SSID
const char *password = "password"; // Same as the Access Point password

void setup() {
  Serial.begin(115200);
  delay(100);

  // Connect ESP8266 to the access point
  WiFi.begin(ssid, password);

  while (WiFi.status() != WL_CONNECTED) {
    delay(1000);
    Serial.println("Connecting to WiFi...");
  }

  Serial.println("WiFi connected");
}

void loop() {
  int rssi = WiFi.RSSI();
  Serial.print("RSSI: ");
  Serial.println(rssi);

  delay(100); // Read RSSI every second
}
