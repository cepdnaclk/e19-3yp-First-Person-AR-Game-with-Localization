# Generate certificate

## Generate a CA key

openssl genrsa -out ca.key 2048


## Generate a self-signed CA certificate

```bash
openssl req -new -x509 -days 365 -key ca.key -out ca.crt
```

## Generate a server key

```bash
openssl genrsa -out server.key 2048
```

## Generate a server certificate signing request

```bash
openssl req -new -key server.key -out server.csr
```

## Sign the server certificate with the CA certificate

```bash
openssl x509 -req -in server.csr -CA ca.crt -CAkey ca.key -CAcreateserial -out server.crt -days 365
```