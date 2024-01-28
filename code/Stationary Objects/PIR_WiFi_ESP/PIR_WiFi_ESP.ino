#include <WiFi.h>
#include <WiFiClientSecure.h>
#include <PubSubClient.h>
#include <ArduinoJson.h> 

// WiFi credentials
const char* ssid = "SLT-4G_166F33";
const char* password = "F568A2FF";

// AWS IoT credentials
const char* aws_endpoint = "a2leuqp8y2i70g-ats.iot.ap-southeast-1.amazonaws.com"; // Your AWS IoT endpoint
const char* aws_topic = "pir/1/pub"; // Your MQTT topic

// Certificates and private key
const char* root_ca = R"EOF(
-----BEGIN CERTIFICATE-----
MIIDQTCCAimgAwIBAgITBmyfz5m/jAo54vB4ikPmljZbyjANBgkqhkiG9w0BAQsF
ADA5MQswCQYDVQQGEwJVUzEPMA0GA1UEChMGQW1hem9uMRkwFwYDVQQDExBBbWF6
b24gUm9vdCBDQSAxMB4XDTE1MDUyNjAwMDAwMFoXDTM4MDExNzAwMDAwMFowOTEL
MAkGA1UEBhMCVVMxDzANBgNVBAoTBkFtYXpvbjEZMBcGA1UEAxMQQW1hem9uIFJv
b3QgQ0EgMTCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBALJ4gHHKeNXj
ca9HgFB0fW7Y14h29Jlo91ghYPl0hAEvrAIthtOgQ3pOsqTQNroBvo3bSMgHFzZM
9O6II8c+6zf1tRn4SWiw3te5djgdYZ6k/oI2peVKVuRF4fn9tBb6dNqcmzU5L/qw
IFAGbHrQgLKm+a/sRxmPUDgH3KKHOVj4utWp+UhnMJbulHheb4mjUcAwhmahRWa6
VOujw5H5SNz/0egwLX0tdHA114gk957EWW67c4cX8jJGKLhD+rcdqsq08p8kDi1L
93FcXmn/6pUCyziKrlA4b9v7LWIbxcceVOF34GfID5yHI9Y/QCB/IIDEgEw+OyQm
jgSubJrIqg0CAwEAAaNCMEAwDwYDVR0TAQH/BAUwAwEB/zAOBgNVHQ8BAf8EBAMC
AYYwHQYDVR0OBBYEFIQYzIU07LwMlJQuCFmcx7IQTgoIMA0GCSqGSIb3DQEBCwUA
A4IBAQCY8jdaQZChGsV2USggNiMOruYou6r4lK5IpDB/G/wkjUu0yKGX9rbxenDI
U5PMCCjjmCXPI6T53iHTfIUJrU6adTrCC2qJeHZERxhlbI1Bjjt/msv0tadQ1wUs
N+gDS63pYaACbvXy8MWy7Vu33PqUXHeeE6V/Uq2V8viTO96LXFvKWlJbYK8U90vv
o/ufQJVtMVT8QtPHRh8jrdkPSHCa2XV4cdFyQzR1bldZwgJcJmApzyMZFo6IQ6XU
5MsI+yMRQ+hDKXJioaldXgjUkK642M4UwtBV8ob2xJNDd2ZhwLnoQdeXeGADbkpy
rqXRfboQnoZsG4q5WTP468SQvvG5
-----END CERTIFICATE-----
)EOF";

const char* client_cert = R"KEY(
-----BEGIN CERTIFICATE-----
MIIDFDCCAfwCFAnmYnjlR7sM0VI3c5vlVv4yBLmcMA0GCSqGSIb3DQEBCwUAMEgx
CzAJBgNVBAYTAlNMMRMwEQYDVQQIDApTb21lLVN0YXRlMREwDwYDVQQKDAhhcmNv
bWJhdDERMA8GA1UEAwwIYXJjb21iYXQwHhcNMjQwMTI2MTMzNzIyWhcNMjUwMTI1
MTMzNzIyWjBFMQswCQYDVQQGEwJTTDETMBEGA1UECAwKU29tZS1TdGF0ZTERMA8G
A1UECgwIYXJjb21iYXQxDjAMBgNVBAMMBXBpci8xMIIBIjANBgkqhkiG9w0BAQEF
AAOCAQ8AMIIBCgKCAQEApAjTaaTTjzIj+6zpRm++hKO/IxJEreS5jwzE3MG7UEOp
Di5GGm4ZxUHtxG8vYOLjxG/9URkvx2XRf9BZSoXnEt254hUBfncJUazg+GoLz1Jj
ELF25On8UP4u8zaWk0jFXEPElWBEjjvH7IyXcWthmo7Atngd+Vll4efqj2upTc6o
wjB9IKoq/KeJm7gL4j4x04nyy8rGh9PRTRTTEfHwqeCj9qjW0B6XAizp6jkbJe/e
PQgFPqufcheKhY65KfvO7GWk5ox3iNImvNtG3BXBYf9r47b7ZIUjdiuY8JgrHxhE
lyRVSK3NL8UyCfvo7YvTPlSd5nqxgBMi2HkymjOHdQIDAQABMA0GCSqGSIb3DQEB
CwUAA4IBAQAw/0WV0Ucrtw/yuxqowZLpgCGBYInsGMmu+siOxnAGPq/kRumha6Vc
OTVw9HMtvMTuS9/QMqANeLxgc0ALHq6bV/N/pu4PmuoiDLcOaL8OP19eYuENfJNv
fjIBc7W8ouM5CmrIVusDBoNYLpGyvAfxBBXdW1xewiuTYjs1VX4JdUntlh5oKrGH
Q4cCi9SHTJ0vdMDWZY/WAsWzI6N5x8ZpNXsuz24xmAwmMTVxGlRlI7XlMsO9TPsd
2kEnUo6vrDxUq5zoYvnuCDYBciDHaaz+HcJFz1dpG0WiaIedFYuJiqkh9P7ALTc4
Mxwlh0U4eL+s4cLVVIqqdvEGHFRaQgPF
-----END CERTIFICATE-----
-----BEGIN CERTIFICATE-----
MIIDcTCCAlmgAwIBAgIUXJjdI/YD28+QJagw9JNUSYzlvzIwDQYJKoZIhvcNAQEL
BQAwSDELMAkGA1UEBhMCU0wxEzARBgNVBAgMClNvbWUtU3RhdGUxETAPBgNVBAoM
CGFyY29tYmF0MREwDwYDVQQDDAhhcmNvbWJhdDAeFw0yNDAxMTMwNjU4MjlaFw0z
NDAxMTAwNjU4MjlaMEgxCzAJBgNVBAYTAlNMMRMwEQYDVQQIDApTb21lLVN0YXRl
MREwDwYDVQQKDAhhcmNvbWJhdDERMA8GA1UEAwwIYXJjb21iYXQwggEiMA0GCSqG
SIb3DQEBAQUAA4IBDwAwggEKAoIBAQCazSiNESFFAhFTcLL0GuYhqugjU1IZmP95
RaB6QuNAMK8OQ754yF6tDlWxc6ZN7l49nSJuHuKPs4PoAGJr+ydokQQh+1zX4Q4s
PbW44rsfc3gE5b4jZda5b7E/J5cy2hQjNnzfQwiwpAemL+ye3HcESwY9VXMEEXCB
oqKeFqSAsghKMYdKlB71qsL8koAXbETEwbutVYnXBa02FGUhVd6m/0QAuMdY8dK3
2oIshSBVVzIbwN/u/Ofj+FCoT+HN72k0QFXLUlHr03/eKyKeIBcVJxOLgy7AgzCD
oa/I1ERPxfcBK1l2MYSHirIidboizaR7tw6ZBCx3PYEyeSD7BQPbAgMBAAGjUzBR
MB0GA1UdDgQWBBTK/LIGqAK29+fp6WmHreTCEnVaOzAfBgNVHSMEGDAWgBTK/LIG
qAK29+fp6WmHreTCEnVaOzAPBgNVHRMBAf8EBTADAQH/MA0GCSqGSIb3DQEBCwUA
A4IBAQBr/LXSOWrocuEyDuwP4VzrDaYmsDpRZ0AGc71so889F2jUsof3MJSn0iap
Dpw3t/YaREAr3ytuXM188tQYHMMrFzdZRTg7C8SWbmvsGqYCq4nkbwDOPVuL4iPS
3+5Tex6MKPdROQLvmUiMc5O6NmE6tJU4nh965H8q0jwZQfGSXOwDo0tQ0DSN2OzB
6DSvlKA38raArCvMqfQiK+D2N79kk5We+pLh1f92yPV2vbEEN+QWUTyvsu4l5MOn
GMRjV4n9HgZEXQjjcHm64mHGkZZHXZ87xZ7vTzLOrN+xoBhaOaM7iblxSAL9nQ0O
UhhnT477jo51rwYv4uvzfLvrQvX6
-----END CERTIFICATE-----

)KEY";

const char* private_key = R"KEY(
-----BEGIN PRIVATE KEY-----
MIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQCkCNNppNOPMiP7
rOlGb76Eo78jEkSt5LmPDMTcwbtQQ6kOLkYabhnFQe3Eby9g4uPEb/1RGS/HZdF/
0FlKhecS3bniFQF+dwlRrOD4agvPUmMQsXbk6fxQ/i7zNpaTSMVcQ8SVYESOO8fs
jJdxa2GajsC2eB35WWXh5+qPa6lNzqjCMH0gqir8p4mbuAviPjHTifLLysaH09FN
FNMR8fCp4KP2qNbQHpcCLOnqORsl7949CAU+q59yF4qFjrkp+87sZaTmjHeI0ia8
20bcFcFh/2vjtvtkhSN2K5jwmCsfGESXJFVIrc0vxTIJ++jti9M+VJ3merGAEyLY
eTKaM4d1AgMBAAECggEAGMy1y9PN4Wki/A6jERFdZDfOcrq1E/ZP4JL9Y2PN0j98
fGaeFipdro8+Z5OovdTnux/0VDHC4bpgz1pcWZI/goBvtUDYQjq0d7rzo1MDGJPb
4t71uvuuOD+JsR8oajmXDxE21B+jFb7vhbGmxtDFh6S7+YpU+8kaiGoz8f2EsfzF
tY7yVsslMxRsL5EBDxmS0lgLSAqozr1ENYpuK2ubT487K/knKEjomi40yeu3Barr
txB1l7ZWJSPgzxtQYp7N8CK4cSsTHLLZN75wh4QBN4euTLMsB7Rw8LhBOsJl+ReF
zSOLQHvtrO5yKYllheKXXnKiRjhw/JB8LEiU2uZEuQKBgQC/LtI8mVwl1luAOIYU
lO4L+aVcGMmOmHXKfW/30HOtObdxNVkvGHvUVWNIDKSl9VxLzi2V9MatfYMPOjDp
J5SNNi8p7yqrdDHY4V3w7rzUtq4gRWIMDyauadD87ol4/bs7dYQNUNeLd61X/xow
t2IvEFM7Rb6lAlSaD2hyd0/LbQKBgQDbpbv6h51+vUrFLzTEH2C5bz20MkyLOteF
Lb0p6F4cFYELc6FPTfXk4yqqBTNw7FeewfdwcL+lhgXMPF8PM7pT6Lhcd/QHFdkW
9SyMjeXfXmJH/CjnEZyumJyf2C9mR4msBFMM76lLzj7MdYkaeBU8hTsyRw++GfZZ
YWGXUhjfKQKBgH08u6XMxItYvzngTRzgbovTrpoE9sv9XXQW9aj9mTlJjLyROOnb
/QocIVxKQ/UfJKXX2w7XeOqfRKJN+UpSjkJu0ziCHo6QIDM1dqdkjLg/LxOev9qs
didc0/VMgSuL8wHBOo7KfVg0Po5dYxe0mZHA/PTf/EH1o9wD6eTJ+bCFAoGBAK1+
bcroD/kqH8M9FFrNxiLywTRyfo+DPPVPOZm+l4drXPDktsfjdpUC8pI1ZXqO1G7s
GzzPcjGWeUHnddaWL1lT+zve6/wkv8MoibXD54zWCp99h/lsqewnU5/WrSoG27Hq
AIe5Tmo4UNZDCLbFn1CclDOUedTYwQO8rc9O46JxAoGAfM5eRhFXrNa3lhEbmUwx
z5RyL3oSYCbz0KtLBQsqlv0sSs86VpsKvHa4Me+J59eToHcg7GuND8oWiZVguN1R
iW9kxyAEkV7mzP4qBQavHVs+NfZNFJm075m3sw+Iy6pqhEiclqAQjsazdRWpDVBF
SRiCztwU/bjhzySR+2I/f2c=
-----END PRIVATE KEY-----
)KEY";

WiFiClientSecure wifiClient;
PubSubClient client(aws_endpoint, 8883, wifiClient); // Set the MQTT port to 8883 for TLS

const int pirPin = 33;  // GPIO pin for the PIR sensor
const int LEDPin = 32;  // GPIO pin for the LED

String displayWiFiNetworks();

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

    // Get first Wi-Fi network's details and send to AWS
    String wifiDetails = displayWiFiNetworks();
    if(wifiDetails != "") {
      client.publish(aws_topic, wifiDetails.c_str());
    }

    delay(5000); // Avoid continuous scanning
  } else {
    digitalWrite(LEDPin, LOW);
  }
}

String displayWiFiNetworks() {
    Serial.println("Scanning Wi-Fi networks...");
    int numNetworks = WiFi.scanNetworks();
    if (numNetworks == 0) {
        Serial.println("No networks found");
        return "";
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

        // Create a JSON array to hold network details
        StaticJsonDocument<1024> jsonDoc; // Adjust size as needed
        JsonArray networksArray = jsonDoc.createNestedArray("Networks");

        for (int i = 0; i < numNetworks; ++i) {
            String ssid = WiFi.SSID(i);
            // Check if the first three letters of the SSID are "gun"
            if (ssid.substring(0, 3).equalsIgnoreCase("gun")) {
                // Add network details to the JSON array
                JsonObject network = networksArray.createNestedObject();
                network["SSID"] = ssid;
            }
        }

        if (networksArray.size() == 0) {
            Serial.println("No 'gun' networks found");
            return "";
        }

        // Convert JSON object to string
        String jsonStr;
        serializeJson(jsonDoc, jsonStr);

        Serial.println("JSON String:");
        Serial.println(jsonStr);
        return jsonStr;
    }
}








