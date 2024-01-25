#include <ESP8266WiFi.h>

void setup() {
  Serial.begin(115200);
  delay(10);

  // Connect to Wi-Fi
  WiFi.mode(WIFI_STA);
  WiFi.begin("Public WiFi", "16959385");

  while (WiFi.status() != WL_CONNECTED) {
    delay(250);
    Serial.print(".");
  }
  Serial.println("");

}

void loop() {
   // Display Wi-Fi networks and signal strength
  displayWiFiNetworks();
  delay(250);
}

void displayWiFiNetworks() {
  // Perform Wi-Fi scan
  Serial.println("Scanning nearby Wi-Fi networks...");
  int numNetworks = WiFi.scanNetworks();

  if (numNetworks == 0) {
    Serial.println("No networks found");
  } else {
    Serial.print(numNetworks);
    Serial.println(" networks found");

    for (int i = 0; i < numNetworks; i++) {
      Serial.print("Network SSID: ");
      Serial.println(WiFi.SSID(i));
      Serial.print("Signal strength: ");
      Serial.println(WiFi.RSSI(i));
      Serial.println("-----------------------");
    }
  }
}
