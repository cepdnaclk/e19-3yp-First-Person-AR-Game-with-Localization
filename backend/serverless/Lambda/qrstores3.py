import qrcode
import io


# Generate QR code
qr = qrcode.QRCode(
    version=1,
    error_correction=qrcode.constants.ERROR_CORRECT_L,
    box_size=50,
    border=2
)
def lambda_handler(event, context):
    email = event
    qr.add_data(email)
    qr.make(fit=True)

    img = qr.make_image(fill_color="black", back_color="white")

    img_bytes_io = io.BytesIO()
    img.save(img_bytes_io)
    img_bytes = img_bytes_io.getvalue()