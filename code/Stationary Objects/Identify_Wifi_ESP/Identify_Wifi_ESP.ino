#include <WiFi.h>
#include <WiFiClientSecure.h>
#include <PubSubClient.h>

// WiFi credentials
const char* ssid = "";
const char* password = "";

// AWS IoT credentials
const char* aws_endpoint = ""; // Your AWS IoT endpoint
const char* aws_topic = ""; // Your MQTT topic

// Certificates and private key
const char* root_ca = R"EOF(
// Your root CA certificate
)EOF";

const char* client_cert = R"KEY(
// Your client certificate
)KEY";

const char* private_key = R"KEY(
// Your private key
)KEY";

WiFiClientSecure wifiClient;
PubSubClient client(aws_endpoint, 8883, wifiClient); // Set the MQTT port to 8883 for TLS

const int pirPin = 33;  // GPIO pin for the PIR sensor
const int LEDPin = 32;  // GPIO pin for the LED

void setup() {
  Serial.begin(115200);
  delay(10);

  pinMode(pirPin, INPUT);
  pinMode(LEDPin, OUTPUT);

  // Connect to WiFi
  WiFi.mode(WIFI_STA);
  WiFi.begin(ssid, password);  

  while (WiFi.status() != WL_CONNECTED) {
    delay(250);
    Serial.print(".");
  }
  Serial.println("\nConnected to WiFi");

  // Load certificate and private key for AWS IoT
  wifiClient.setCACert(root_ca);
  wifiClient.setCertificate(client_cert);
  wifiClient.setPrivateKey(private_key);

  // Connect to AWS IoT
  if (client.connect("pir1")) {
    Serial.println("Connected to AWS IoT");
    // Subscribe or publish here
  } else {
    Serial.print("AWS IoT connection failed, rc=");
    Serial.println(client.state());
  }
}

void loop() {
  if (!client.connected()) {
    // Reconnect to AWS IoT
  }

  client.loop(); // MQTT loop

  // PIR sensor logic
  if (digitalRead(pirPin) == HIGH) {
    digitalWrite(LEDPin, HIGH);
    displayWiFiNetworks();
    delay(5000); // Avoid continuous scanning
  } else {
    digitalWrite(LEDPin, LOW);
  }
}

void displayWiFiNetworks() {
  Serial.println("Scanning Wi-Fi networks...");
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
    }
    Serial.println("-----------------------");
  }
}
