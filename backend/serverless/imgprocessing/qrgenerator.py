import qrcode

qr = qrcode.QRCode(version=1, error_correction=qrcode.constants.ERROR_CORRECT_L, box_size=50, border =2)
qr.add_data('e19236@eng.pdn.ac.lk')
qr.make(fit=True)

img = qr.make_image(fill_color="black", back_color="white")

img.save('236.png')