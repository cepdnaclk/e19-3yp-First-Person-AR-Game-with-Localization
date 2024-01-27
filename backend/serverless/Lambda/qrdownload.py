import json
import boto3
import base64
client=boto3.client('s3')

def lambda_handler(event, context):

    bucket_name='arcombat-qr'
    image_file_name=event['body']['email']
   
    response = client.get_object(
    Bucket=bucket_name,
    Key=image_file_name+'.png',
)
    print("Response from s3 : ",response)
    image_file_to_be_downloaded=response['Body'].read()
    return {
        'statusCode': 200,
        'body':base64.b64encode(image_file_to_be_downloaded) ,
        'isBase64Encoded': True
    }
