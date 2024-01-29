import qrcode
import io
import boto3

client = boto3.client('s3')


# Generate QR code
qr = qrcode.QRCode(
    version=1,
    error_correction=qrcode.constants.ERROR_CORRECT_L,
    box_size=50,
    border=2
)

bucket_name = 'arcombat-qr'
def lambda_handler(event, context):
    email = event['email']
    qr.add_data(email)
    qr.make(fit=True)

    img = qr.make_image(fill_color="black", back_color="white")

    img_bytes_io = io.BytesIO()
    img.save(img_bytes_io)
    img_bytes = img_bytes_io.getvalue()

    client.put_object(Bucket=bucket_name, Key=email+'.png', Body=img_bytes)
    return "success"