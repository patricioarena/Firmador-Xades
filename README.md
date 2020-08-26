## Firmador-Xades


<p>

<img src="https://github.com/patricioarena/images/blob/master/Firmador-Xades/Anotaci%C3%B3n%202020-08-26%20124546.jpg?raw=true" style='margin: 10px auto 20px;
    display: block' >

</p>

---

# Descripción

<p> Aplicacion para firma digital como servicio. La presente es una aplicacion de escritorio que posee un server web embebido el cual expone endpoints para firmar y verificar la firma en un documento mediante el estándar Xades. Dado que el negocio no requeria la utilizacion del nodo ds:Object y el mismo es opcional este no esta presente en el documento firmado resultante.</p>

<br>

```
                              XMLDSIG
                                   |
<ds:Signature ID?>- - - - - - - - -+- - - - -+
  <ds:SignedInfo>                  |         |
    <ds:CanonicalizationMethod/>   |         |
    <ds:SignatureMethod/>          |         |
    (<ds:Reference URI? >          |         |
      (<ds:Transforms>)?           |         |
      <ds:DigestMethod>            |         |
      <ds:DigestValue>             |         |
    </ds:Reference>)+              |         |
  </ds:SignedInfo>                 |         |
  <ds:SignatureValue>              |         |
  (<ds:KeyInfo>)?- - - - - - - - - +         |
</ds:Signature>- - - - - - - - - - - - - - - +
                                             |
                                          XAdES
```
Para lograr esto se propuso crear una aplicacion la cual se instala en el equipo del usuario, la misma recibe el documento en un puerto, accede al almacen de certificados para seleccionar el certificado a utilizar y posteriormente realizar el proceso de firma.

- Para la comunicacion entre la pagina web o aplicacion web con el servidor local embebido corriendo en la maquina del usuario se utiliza una conexion segura HTTPS. Para lograr esto la aplicacion     emite e instala un certificado autofirmado.
- El certificado instalado es asosciado a la aplicacion y al puerto mediante netsh.

## Ejemplo de xml firmado
``` xml
<?xml version="1.0" encoding="utf-8" standalone="no"?>
<Tour>
    <NombreTour>
       The Offspring y Bad Religion
    </NombreTour>
    <Fecha>
       24/10/2019 19:00:00
    </Fecha>
    <Videos>
        <video nombre="Bad Religion - 21st century digital boy - Luna Park - 24/10/2019"> https://www.youtube.com/watch?v=iDVeAAvFb3U
        </video>
        <video nombre="The Offspring - Americana - Luna Park - 24/10/2019"> https://www.youtube.com/watch?v=Zd7bAu7hVZQ
        </video>
    </Videos>
    <ds:Signature xmlns:ds="http://www.w3.org/2000/09/xmldsig#" Id="Signature-8322a69e-1d3a-4496-b4f2-c8ac6f1c4dfb">
        <ds:SignedInfo>
            <ds:CanonicalizationMethod Algorithm="http://www.w3.org/TR/2001/REC-xml-c14n-20010315" />
            <ds:SignatureMethod Algorithm="http://www.w3.org/2001/04/xmldsig-more#rsa-sha512" />
            <ds:Reference Id="Reference-6821c7d4-18bb-4a94-bfb4-c876db313caa" URI="">
                <ds:Transforms>
                    <ds:Transform Algorithm="http://www.w3.org/2000/09/xmldsig#enveloped-signature" />
                    <ds:Transform Algorithm="http://www.w3.org/TR/2001/REC-xml-c14n-20010315" />
                </ds:Transforms>
                <ds:DigestMethod Algorithm="http://www.w3.org/2001/04/xmlenc#sha256" />
                <ds:DigestValue>8wmXdw7NeUFkA8F/wsnP9wpmf1E0RGPI8uDXkwQObROs=</ds:DigestValue>
            </ds:Reference>
        </ds:SignedInfo>
        <ds:SignatureValue Id="SignatureValue-8322a69e-1d3a-4496-b4f2-c8ac6f1c4dfb">adkT4g1HDSvfM9KdKqm+WLDPLp49i46FujRHB4P8S3JjBWNVSaoTpQNRtEdkLtpTpO72McelUcnfYfBM9A...</ds:SignatureValue>
        <ds:KeyInfo Id="KeyInfoId-Signature-8322a69e-1d3a-4496-b4f2-c8ac6f1c4dfb">
            <ds:X509Data>
                <ds:X509Certificate>MIIDCjCCAfKgAwIBAgIQQ0zBDFfEd5NO56EOHLdG2jANBgkqhkiG9w0BAQsFADAeMRwwG...</ds:X509Certificate>
            </ds:X509Data>
            <ds:KeyValue>
                <ds:RSAKeyValue>
                    <ds:Modulus>3IFPnwyrKR24xu5zD0mm1G+ZUskcGAhbgJAOWDGG+SuqO6KZp5Wuq0fr0dx5opBPxIg1rmMyW4aKtB...</ds:Modulus>
                    <ds:Exponent>AQAB</ds:Exponent>
                </ds:RSAKeyValue>
            </ds:KeyValue>
        </ds:KeyInfo>
    </ds:Signature>
</Tour>
```


# Dependencias
1.   NET Framework 4.8
2.   NodeJS v10.16.3
3.	 Angular 9.1.12


# Compilar y probar
**Aplicacion de escritorio**
- La aplicación final se divide en dos aplicaciones realmente, por un
lado el firmador, aplicacion principal que expone los enpoint y por otro una aplicacion auxiliar que se encarga de controlar, emitir, instalar y setear a un puerto especifico el certificado autofirmado para utilizar transferencia segura entre la aplicacion web y la aplicacion de escritorio.

- Para compilar y ejecutar por primera vez se debe realizar como administrador ya que la instalación del certificado autofirmado y ejecucion de comandos netsh requieren privilegios elevados.

    #### Endpoints

    - [POST] https://localhost:8400/api/Signature/1 _Xades con ds:Object_
        - Request : ObjectModel = { Archivo: String; Extension: String; }
        - Response : XML
    - [POST] https://localhost:8400/api/Signature/2 _Xades sin ds:Object_
        - Request : ObjectModel = { Archivo: String; Extension: String; }
        - Response : XML
    - [POST] https://localhost:8400/api/Verify/1 _Xades con ds:Object_
        - Request : ObjectModel = { Archivo: String; Extension: String; }
        - Response : JSON
    - [POST] https://localhost:8400/api/Verify/2 _Xades sin ds:Object_
        - Request : ObjectModel = { Archivo: String; Extension: String; }
        - Response : JSON
    - [GET] https://localhost:8400/api/isAlive
        - Response : true or Error

**Aplicacion web**
- Compilar y utilizar la pagina web dentro del directorio `/Firmador`. **Es necesario compilar la libreria**
    - Situarce en `/Firmador` y Ejecutar `ng build lib-digitalsignature --prod` al finalizar la compilación utilizar `ng serve` con normalidad.
- Para realizar las pruebas de firma y verificación se creo una inferfaz web en Angular 9.1.12. a la cual se puede acceder en la direccion https://localhost:4200/home

# Referencias
El proyecto esta basado en la libreria https://github.com/ctt-gob-es/FirmaXadesNet45 y fue agregada funcionalidad para cumplir con las necesidades del negocio actual, el objetivo era crear una aplicacion que permita firmar documentos que estan alojados en un servidor web pero utilizando los recursos del equipo local del usuario.
- https://www.w3.org/TR/2003/NOTE-XAdES-20030220/