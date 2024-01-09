#include <Adafruit_MPU6050.h>
#include <Adafruit_Sensor.h>
#include <Wire.h>
#include <WiFi.h>
#include <PubSubClient.h>
// #include <WiFiClientSecure.h>
#include <WiFiClient.h>
#include <ArduinoJson.h>

#include "../lib/Secrets.h"

// Define PINs
#define MANUAL_FIRE_PIN 18 // 2 Way Switch terminal 1  
#define AUTOMATIC_FIRE_PIN 19 // 2 Way Switch terminal 2 
// PIN 21 & 22 used for gyrp
#define TRIGER_PIN 23 // Push Button
#define LATCH_PIN 25 // Shift Register
#define CLOCK_PIN 26 // Shift Register 
#define DATA_PIN  27 // Shift Register
#define ZOOM_PIN 32 // Pot 1
#define RELOAD_PIN 33 // IR Sensor
#define BULLET_COUNT_PIN 34 // Pot 2
#define GUN_SELECT_PIN 35 // Pot 3

// Define constants
const int GUN_STEPS = 4095 / 5;
const int BULLET_STEPS = 4095 / 10;

// Variables Declaration
byte bulletsLEDs = 0b1111111111;
int gunCategory = 1;
int bulletVal = 1;
int zoomVal = 100;
int gunMod = 0;
sensors_event_t a, g, temp;
int zoomPotVal; // Pot 1
int bulletPotVal; // Pot 2
int gunPotVal; // Pot 3
double roll;
double pitch;

// Define offsets of Gyro 
float accel_x_offset = 0.0;
float accel_y_offset = 0.0;
float accel_z_offset = 0.0;
float gyro_x_offset = 0.0;
float gyro_y_offset = 0.0;
float gyro_z_offset = 0.0;

// Create MPU Object
Adafruit_MPU6050 mpu;

// WIFI Client
// WiFiClientSecure net = WiFiClientSecure();
WiFiClient net;

// MQTT Client
PubSubClient client(net);

void calibrateSensor() {
  // Calibration variables
  int32_t accel_x, accel_y, accel_z;
  int32_t gyro_x, gyro_y, gyro_z;
  int16_t count = 0;
  double tempSumAccel = 0, tempSumGyro = 0;

  // Calibrate accelerometer
  while (count < 1000) {
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

// Get Potentiameter values
void getPolVal(){
  zoomPotVal = analogRead(ZOOM_PIN);
  bulletPotVal = analogRead(BULLET_COUNT_PIN);
  gunPotVal = analogRead(GUN_SELECT_PIN);

  zoomVal = (zoomPotVal / 4095) * 100;
  bulletVal = bulletPotVal / BULLET_STEPS + 1;
  gunCategory = gunPotVal / GUN_STEPS + 1;
}

void getGyro(){
  mpu.getEvent(&a, &g, &temp);

  // Get data
  double ax = a.acceleration.x - accel_x_offset ;
  double ay = a.acceleration.y - accel_y_offset;
  double az = a.acceleration.z - accel_z_offset;

  // Calculate roll and pitch
  roll = atan2(ay, az) * RAD_TO_DEG;
  pitch = atan2(-ax, sqrt(ay * ay + az * az)) * RAD_TO_DEG;
}

// Show the bullet count on LED bar
void updateBullets(){
  // Deduct the buller
  bulletsLEDs = bulletsLEDs << 1;

  // Write on shift register
  digitalWrite(LATCH_PIN, LOW);
  shiftOut(DATA_PIN, CLOCK_PIN, LSBFIRST, bulletsLEDs);
  digitalWrite(LATCH_PIN, HIGH);
}

// TODO: implement ISR for trigure
void updateShiftRegister()
{
  client.publish(PUBLISH_TOPIC, "ISR");
  updateBullets();
}

// TODO: implement ISR for 2 way switch
// TODO: implement ISR for IR

// Connect to WIFI
void connectWIFI(){
  WiFi.mode(WIFI_STA);
  WiFi.begin(WIFI_SSID, WIFI_PASSWORD);
 
  Serial.println("Connecting to Wi-Fi");
 
  while (WiFi.status() != WL_CONNECTED){
    delay(500);
    Serial.print(".");
  }
  Serial.println();
}

// Connect to AWS
void connectAWS(){
  Serial.println("Connecting to AWS IOT");
 
  while (!client.connect(THINGNAME)){
    Serial.print(".");
    delay(100);
  }
 
  if (!client.connected()){
    Serial.println("AWS IoT Timeout!");
    return;
  }
 
  // Subscribe to a topic
  client.subscribe(SUBSCRIBE_TOPIC);
 
  Serial.println("AWS IoT Connected!");
}

// TODO: Implement message handling
void publishMessage(){
  // Get Data
  getPolVal();
  getGyro();

  StaticJsonDocument<200> doc;

  doc["Roll: "] = roll;
  doc["Pitch: "] = pitch;
  doc["BulletVal: "] = bulletVal;
  doc["ZoomVal: "] = zoomVal;
  doc["GunMod: "] = gunMod;

  char jsonBuffer[512];
  serializeJson(doc, jsonBuffer); // print to client
 
  client.publish(PUBLISH_TOPIC, jsonBuffer);
}

// TODO: Implement to reload action
void messageHandler(char* topic, byte* payload, unsigned int length)
{
  Serial.print("incoming: ");
  Serial.println(topic);
 
  StaticJsonDocument<200> doc;
  deserializeJson(doc, payload);
  const char* message = doc["message"];
  Serial.println(message);
}

void setup(void) {
  Serial.begin(115200);

  // Pin Mode Definition
  pinMode(LATCH_PIN, OUTPUT);
  pinMode(DATA_PIN, OUTPUT);  
  pinMode(CLOCK_PIN, OUTPUT);

  // pinMode(triger, INPUT);

  // attachInterrupt(digitalPinToInterrupt(triger), updateShiftRegister, HIGH);

  Serial.println("Gun Power Up");

  // Try to initialize mpu
  if (!mpu.begin()) {
    Serial.println("Failed to find MPU6050 chip");
    while (1) {
      Serial.print(".");
      delay(10);
    }
  }
  Serial.println("MPU6050 Found!");

  // Configure MPU
  mpu.setAccelerometerRange(MPU6050_RANGE_2_G);
  mpu.setGyroRange(MPU6050_RANGE_250_DEG);
  mpu.setFilterBandwidth(MPU6050_BAND_44_HZ);

  // Calibrate mpu
  calibrateSensor();

  // Connect to WIFI
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

  // Connect to AWS
  connectAWS();

  Serial.println("");

  // Initialize the gun
  client.publish(PUBLISH_TOPIC, "Hello from ESP32");
  getPolVal();
  updateBullets();
  delay(100);
}

void loop() {
  /* Get new sensor events with the readings */
  publishMessage();
  delay(100);
}