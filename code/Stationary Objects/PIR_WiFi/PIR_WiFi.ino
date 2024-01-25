#include <ESP8266WiFi.h>

const int pirPin = D6;  // Assuming the PIR sensor is connected to GPIO pin D2
const int LEDPin = D7; 

void setup() {
  Serial.begin(115200);
  delay(10);

  pinMode(pirPin, INPUT);
  digitalWrite(pirPin, LOW);  // Set initial value to LOW
  pinMode(LEDPin, OUTPUT);

  // Connect to Wi-Fi
  WiFi.mode(WIFI_STA);
  WiFi.begin("sltfiber", "45763259");  

  while (WiFi.status() != WL_CONNECTED) {
    delay(250);
    Serial.print(".");
  }
  Serial.println("");
}

void loop() {
  
  if (digitalRead(pirPin) == 1) {
    // PIR sensor detected motion, display Wi-Fi networks
    displayWiFiNetworks();
    digitalWrite(LEDPin, HIGH);
    delay(5000);  // Wait for 5 seconds to avoid continuous scanning
  }
  else{
    digitalWrite(LEDPin, LOW);
  }
  
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
