import json
import boto3
import base64
from botocore.exceptions import ClientError

client=boto3.client('s3')

def lambda_handler(event, context):
    print(event)

    bucket_name='arcombat-qr'
    body_dict = json.loads(event['body'])
    image_file_name=body_dict['email']
    
    try:
        response = client.generate_presigned_url('get_object',
                                                    Params={'Bucket': bucket_name,
                                                            'Key': image_file_name+'.png'},
                                                    ExpiresIn=18000)
    except ClientError as e:
        logging.error(e)
        return None
    
    return {
        'statusCode': 200,
        'body': json.dumps({'presigned_url': response}),
    }
