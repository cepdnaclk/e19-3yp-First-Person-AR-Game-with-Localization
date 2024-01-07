#include <WiFi.h>


const char* ssid = "TP-LINK_C7B62C";
const char* password = "#wifi#slt";
int flashPin = 4;


void setup() {
  Serial.begin(115200);
  Serial.setDebugOutput(true);
  Serial.println();

  pinMode(flashPin, OUTPUT);

  digitalWrite(flashPin, HIGH);
  delay(300);
  digitalWrite(flashPin, LOW);
  delay(300);


  WiFi.begin(ssid, password);
  WiFi.setSleep(false);

  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  Serial.println("");
  Serial.println("WiFi connected");

  digitalWrite(flashPin, HIGH);
  delay(300);
  digitalWrite(flashPin, LOW);
  delay(300);

  Serial.print("Camera Ready! Use 'http://");
  Serial.print(WiFi.localIP());
  Serial.println("' to connect");
}

void loop() {
  digitalWrite(flashPin, HIGH);
  delay(1000);
  digitalWrite(flashPin, LOW);
  delay(300);
}
