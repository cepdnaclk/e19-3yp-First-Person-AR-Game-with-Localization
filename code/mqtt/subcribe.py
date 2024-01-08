# python3.6

import random
import matplotlib.pyplot as plt
import json
from paho.mqtt import client as mqtt_client

# Create empty lists for each of the values
acceleration_x = [0] * 20
acceleration_y = [0] * 20
acceleration_z = [0] * 20
rotation_x = [0] * 20
rotation_y = [0] * 20
rotation_z = [0] * 20
roll = [0] * 20
pitch = [0] * 20

correction_rot_x = 0.063418068
correction_rot_y = 0.030643184
correction_rot_z = -0.518802464

broker = 'localhost'
port = 1883
topic = "gyro/pub"

# Generate a Client ID with the subscribe prefix.
client_id = f'subscribe-{random.randint(0, 100)}'
# username = 'emqx'
# password = 'public'


def connect_mqtt() -> mqtt_client:
    def on_connect(client, userdata, flags, rc):
        if rc == 0:
            print("Connected to MQTT Broker!")
        else:
            print("Failed to connect, return code %d\n", rc)

    client = mqtt_client.Client(client_id)
    # client.username_pw_set(username, password)
    client.on_connect = on_connect
    client.connect(broker, port)
    return client

def subscribe(client: mqtt_client):
    def on_message(client, userdata, msg):
        global acceleration_x, acceleration_y, acceleration_z
        global rotation_x, rotation_y, rotation_z
        global roll, pitch
    
        print(f"Received `{msg.payload.decode()}` from `{msg.topic}` topic")
        # data = json.loads(msg.payload.decode())  # decode JSON data

        # # Append new values to the lists
        # acceleration_x.append(data['Acceleration X: '])
        # acceleration_y.append(data['Acceleration Y: '])
        # acceleration_z.append(data['Acceleration Z: '])
        # rotation_x.append(data['Rotation X: '] + correction_rot_x)
        # rotation_y.append(data['Rotation Y: '] + correction_rot_y)
        #rotation_z.append(data['Rotation Z: '] + correction_rot_z)
        # roll.append(data['Roll: '])
        # pitch.append(data['Pitch: '])

        # Keep only the last 20 data points in each array
        # acceleration_x = acceleration_x[-20:]
        # acceleration_y = acceleration_y[-20:]
        # acceleration_z = acceleration_z[-20:]
        # rotation_x = rotation_x[-20:]
        # rotation_y = rotation_y[-20:]
        # rotation_z = rotation_z[-20:]
        # roll = roll[-20:]
        # pitch = pitch[-20:]

        # Clear the current figure
        # plt.clf()

        # # Plot the new values
        # plt.plot(acceleration_x, label='Acceleration X')
        #plt.plot(acceleration_y, label='Acceleration Y')
        #plt.plot(acceleration_z, label='Acceleration Z')
        # plt.plot(rotation_x, label='Rotation X')
        # plt.plot(rotation_y, label='Rotation Y')
        # plt.plot(rotation_z, label='Rotation Z')
        # plt.plot(roll, label='Roll')
        # plt.plot(pitch, label='Pitch')

        # Add a legend
        # plt.legend()

        # # Draw the plot
        # plt.pause(0.01)  # pause a bit so that plots are updated

    client.subscribe(topic)
    #plt.show(block=False)  # start the plot outside the function
    client.on_message = on_message

def run():
    client = connect_mqtt()
    subscribe(client)
    client.loop_forever()


if __name__ == '__main__':
    run()
