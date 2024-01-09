#include <Adafruit_MPU6050.h>
#include <Adafruit_Sensor.h>
#include <Wire.h>
#include <WiFi.h>
#include <PubSubClient.h>
// #include <WiFiClientSecure.h>
#include <WiFiClient.h>
#include <ArduinoJson.h>

#include "../lib/Secrets.h"

#define AWS_IOT_PUBLISH_TOPIC   "gyro/pub"
#define AWS_IOT_SUBSCRIBE_TOPIC "gyro/sub"
#define ZOOM_PUBLISH_TOPIC   "gyro/zoom"

#define GUN_PIN 34
#define ZOOM_PIN 36

// Define constants
const int GUN_STEPS = 4095 / 5;

// Pin Definitions
int latchPin = 33;
int clockPin = 25;
int dataPin = 32;
int triger = 35;

Adafruit_MPU6050 mpu;

// WiFiClientSecure net = WiFiClientSecure();
WiFiClient net;
PubSubClient client(net);

// Define offsets
float accel_x_offset = 0.0;
float accel_y_offset = 0.0;
float accel_z_offset = 0.0;
float gyro_x_offset = 0.0;
float gyro_y_offset = 0.0;
float gyro_z_offset = 0.0;

// Variables Declaration
byte bulletsLEDs = 0b1111111111;

int getGun(){
  int zoomVal = analogRead(GUN_PIN);
  int zoomCategory = zoomVal / GUN_STEPS + 1;
  return zoomCategory;
}

void updateBullets()
{
  bulletsLEDs = bulletsLEDs << 1;
  digitalWrite(latchPin, LOW);
  shiftOut(dataPin, clockPin, LSBFIRST, bulletsLEDs);
  digitalWrite(latchPin, HIGH);
}

void updateShiftRegister()
{
  bulletsLEDs = bulletsLEDs << 1;
  client.publish(AWS_IOT_PUBLISH_TOPIC, "ISR");
  digitalWrite(latchPin, LOW);
  shiftOut(dataPin, clockPin, LSBFIRST, bulletsLEDs);
  digitalWrite(latchPin, HIGH);
}

void messageHandler(char* topic, byte* payload, unsigned int length)
{
  Serial.print("incoming: ");
  Serial.println(topic);
 
  StaticJsonDocument<200> doc;
  deserializeJson(doc, payload);
  const char* message = doc["message"];
  Serial.println(message);
}

void connectWIFI(){
  WiFi.mode(WIFI_STA);
  WiFi.begin(WIFI_SSID, WIFI_PASSWORD);
 
  Serial.println("Connecting to Wi-Fi");
 
  while (WiFi.status() != WL_CONNECTED)
  {
    delay(500);
    Serial.print(".");
  }
  Serial.println();
}

void connectAWS(){
  Serial.println("Connecting to AWS IOT");
 
  while (!client.connect(THINGNAME))
  {
    Serial.print(".");
    delay(100);
  }
 
  if (!client.connected())
  {
    Serial.println("AWS IoT Timeout!");
    return;
  }
 
  // Subscribe to a topic
  client.subscribe(AWS_IOT_SUBSCRIBE_TOPIC);
 
  Serial.println("AWS IoT Connected!");
}
 
void publishMessage(sensors_event_t a,sensors_event_t g)
{
  StaticJsonDocument<200> doc;
  double ax = a.acceleration.x - accel_x_offset ;
  double ay = a.acceleration.y - accel_y_offset;
  double az = a.acceleration.z - accel_z_offset;
  double roll = atan2(ay, az) * RAD_TO_DEG;
  double pitch = atan2(-ax, sqrt(ay * ay + az * az)) * RAD_TO_DEG;

  doc["Acceleration X: "] = ax;
  doc["Acceleration Y: "] = ay;
  doc["Acceleration Z: "] = ay;
  doc["Rotation X: "] = g.gyro.x;
  doc["Rotation Y: "] = g.gyro.y;
  doc["Rotation Z: "] = g.gyro.z;
  doc["Roll: "] = roll;
  doc["Pitch: "] = pitch;

  char jsonBuffer[512];
  serializeJson(doc, jsonBuffer); // print to client
 
  client.publish(AWS_IOT_PUBLISH_TOPIC, jsonBuffer);
}

void calibrateSensor() {
  // Calibration variables
  int32_t accel_x, accel_y, accel_z;
  int32_t gyro_x, gyro_y, gyro_z;
  int16_t count = 0;
  double tempSumAccel = 0, tempSumGyro = 0;

  // Calibrate accelerometer
  while (count < 1000) {
    sensors_event_t a, g, temp;
    mpu.getEvent(&a, &g, &temp);

    accel_x += a.acceleration.x;
    accel_y += a.acceleration.y;
    accel_z += a.acceleration.z;

    gyro_x += g.gyro.x;
    gyro_y += g.gyro.y;
    gyro_z += g.gyro.z;

    count++;
  }

  // Calculate average offset
  accel_x_offset = accel_x / count;
  accel_y_offset = accel_y / count;
  accel_z_offset = (accel_z / count) + 16384;

  gyro_x_offset = gyro_x / count;
  gyro_y_offset = gyro_y / count;
  gyro_z_offset = gyro_z / count;
}

void setup(void) {
  Serial.begin(115200);

  pinMode(latchPin, OUTPUT);
  pinMode(dataPin, OUTPUT);  
  pinMode(clockPin, OUTPUT);

  // pinMode(triger, INPUT);

  // attachInterrupt(digitalPinToInterrupt(triger), updateShiftRegister, HIGH);

  Serial.println("Adafruit MPU6050 test!");

  // Try to initialize!
  // if (!mpu.begin()) {
  //   Serial.println("Failed to find MPU6050 chip");
  //   while (1) {
  //     delay(10);
  //   }
  // }
  Serial.println("MPU6050 Found!");

  mpu.setAccelerometerRange(MPU6050_RANGE_2_G);
  // Serial.print("Accelerometer range set to: ");
  // switch (mpu.getAccelerometerRange()) {
  // case MPU6050_RANGE_2_G:
  //   Serial.println("+-2G");
  //   break;
  // case MPU6050_RANGE_4_G:
  //   Serial.println("+-4G");
  //   break;
  // case MPU6050_RANGE_8_G:
  //   Serial.println("+-8G");
  //   break;
  // case MPU6050_RANGE_16_G:
  //   Serial.println("+-16G");
  //   break;
  // }
  mpu.setGyroRange(MPU6050_RANGE_250_DEG);
  // Serial.print("Gyro range set to: ");
  // switch (mpu.getGyroRange()) {
  // case MPU6050_RANGE_250_DEG:
  //   Serial.println("+- 250 deg/s");
  //   break;
  // case MPU6050_RANGE_500_DEG:
  //   Serial.println("+- 500 deg/s");
  //   break;
  // case MPU6050_RANGE_1000_DEG:
  //   Serial.println("+- 1000 deg/s");
  //   break;
  // case MPU6050_RANGE_2000_DEG:
  //   Serial.println("+- 2000 deg/s");
  //   break;
  // }

  mpu.setFilterBandwidth(MPU6050_BAND_44_HZ);
  // Serial.print("Filter bandwidth set to: ");
  // switch (mpu.getFilterBandwidth()) {
  // case MPU6050_BAND_260_HZ:
  //   Serial.println("260 Hz");
  //   break;
  // case MPU6050_BAND_184_HZ:
  //   Serial.println("184 Hz");
  //   break;
  // case MPU6050_BAND_94_HZ:
  //   Serial.println("94 Hz");
  //   break;
  // case MPU6050_BAND_44_HZ:
  //   Serial.println("44 Hz");
  //   break;
  // case MPU6050_BAND_21_HZ:
  //   Serial.println("21 Hz");
  //   break;
  // case MPU6050_BAND_10_HZ:
  //   Serial.println("10 Hz");
  //   break;
  // case MPU6050_BAND_5_HZ:
  //   Serial.println("5 Hz");
  //   break;
  // }

  calibrateSensor();

  connectWIFI();

  // Configure the Broker
  // Configure WiFiClientSecure to use the AWS IoT device credentials
  // net.setCACert(AWS_CERT_CA);
  // net.setCertificate(AWS_CERT_CRT);
  // net.setPrivateKey(AWS_CERT_PRIVATE);
 
  // Connect to the MQTT broker on the AWS endpoint we defined earlier
  client.setServer(AWS_IOT_ENDPOINT, AWS_IOT_PORT);
 
  // Create a message handler
  client.setCallback(messageHandler);

  connectAWS();

  Serial.println("");

  updateBullets();
  client.publish(AWS_IOT_PUBLISH_TOPIC, "Hello from ESP32");
  delay(100);
}

void loop() {
  /* Get new sensor events with the readings */
  // sensors_event_t a, g, temp;
  // mpu.getEvent(&a, &g, &temp);

  // publishMessage(a, g);
  // delay(50);
  int zoomVal = analogRead(ZOOM_PIN);
  client.publish(ZOOM_PUBLISH_TOPIC, String(zoomVal).c_str());
  delay(100);
}