#!bin/bash

openssl genrsa 2048 > private.pem

openssl req\
    -x509 \
    -nodes \
    -new \
    -key private.pem\
    -new \
    -out public.pem\
    -config ./openssl-custom.cnf \
    -days 3650

openssl pkcs12 -export -in public.pem -inkey private.pem -out certificate.pfx -nodes -password pass:123

rm private.pem
rm public.pem