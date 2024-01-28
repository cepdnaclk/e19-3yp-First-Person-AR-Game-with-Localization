#include <WiFi.h>
#include <PubSubClient.h>
#include <WiFiClientSecure.h>

// WiFi credentials
const char* ssid = "";
const char* password = "";

// AWS IoT credentials
const char* aws_endpoint = ""; // Your AWS IoT endpoint
const char* aws_topic = "pir/1/pub"; // Your MQTT topic

// Certificates and private key
const char* root_ca = R"EOF(

)EOF";

const char* client_cert = R"KEY(

)KEY";

const char* private_key = R"KEY(

)KEY";

WiFiClientSecure wifiClient; // Use WiFiClientSecure instead of WiFiClient
PubSubClient client(aws_endpoint, 8883, wifiClient);

void setup() {
  Serial.begin(115200);
  delay(10);

  // Connect to WiFi
  WiFi.mode(WIFI_STA);
  WiFi.begin(ssid, password);  

  while (WiFi.status() != WL_CONNECTED) {
    delay(250);
    Serial.print(".");
  }
  Serial.println("\nConnected to WiFi");

  // Load certificate and private key
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
  // Ensure the client remains connected
  if (!client.connected()) {
    Serial.println("Reconnecting to AWS IoT...");
    // Reconnect code here
  }

  client.loop(); // MQTT loop
  // Rest of your code
}
