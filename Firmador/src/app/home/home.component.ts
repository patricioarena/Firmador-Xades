import { Component, isDevMode, OnInit } from "@angular/core";
import {
  DigitalSignatureService,
  TiposDeFirma,
  XmlModel,
} from "../digitalsignature/digitalsignature.service";

import { saveAs } from "file-saver";
import { ToastrService } from "ngx-toastr";
import { NotificationService } from "../service/notification.service";

@Component({
  selector: "app-home",
  templateUrl: "./home.component.html",
  styleUrls: ["./home.component.scss"],
})

// Basic Usage
// export class HomeComponent implements OnInit {

//   constructor(
//   ) { }

//   public ngOnInit() {
//   }

// }

// Advanced Usage - Service
export class HomeComponent implements OnInit {
  public TiposDeFirma: any = [
    {
      key: "Xades-BES - Sin ds:Object",
      value: TiposDeFirma.Xades_BES_Sin_ds_Object,
    },
    {
      key: "Xades-BES - Con ds:Object",
      value: TiposDeFirma.Xades_BES_Con_ds_Object,
    },
  ];
  public TipoDeFirma = TiposDeFirma.Xades_BES_Sin_ds_Object;
  public ocsp = true;
  public text: String;
  public textPreview: String = "";
  public objeto: XmlModel;
  public isEnabled = false;
  public showPreview = false;
  public fileUrl;
  public responseFirma;
  public Base64pdf;

  constructor(
    public signatureService: DigitalSignatureService,
    private notificationService: NotificationService,
    private toastr: ToastrService
  ) {}

  public ngOnInit() {
    this.text =
      '<?xml version="1.0" encoding="UTF-8" standalone="no" ?>\n' +
      "<Tour>\n" +
      "   <NombreTour> The Offspring y Bad Religion </NombreTour> \n" +
      "   <Fecha> 24/10/2019 19:00:00 </Fecha>\n" +
      "   <Videos>\n" +
      // tslint:disable-next-line: max-line-length
      '       <video nombre="Bad Religion - 21st century digital boy - Luna Park - 24/10/2019">https://www.youtube.com/watch?v=iDVeAAvFb3U</video> \n' +
      '       <video nombre="The Offspring - Americana - Luna Park - 24/10/2019">https://www.youtube.com/watch?v=Zd7bAu7hVZQ</video> \n' +
      "   </Videos>\n" +
      "</Tour>";
  }

  public FirmarDigital() {
    this.isEnabled = false;
    this.showPreview = false;
    this.textPreview = "";
    this.objeto = new XmlModel();
    this.objeto.XmlFile = btoa(this.text.toString());
    this.objeto.Extension = ".xml";
    isDevMode() && (this.ocsp = false);
    this.signatureService.firmaDigital(this.objeto, this.ocsp).subscribe(
      (resp) => {
        // tslint:disable-next-line: no-empty
        if (resp === "-1") {
        } else if (resp === "-2") {
          this.notificationService.showInfo(
            "Certificado",
            "Certificado no valido"
          );
        } else {
          this.responseFirma = resp;
          this.isEnabled = true;
        }
      },
      (err) => {
        const message = JSON.parse(err.error).ExceptionMessage;
        this.notificationService.showError("Certificado", message);
      }
    );
  }

  public FirmarElectronica() {
    this.isEnabled = false;
    this.showPreview = false;
    this.textPreview = "";
    this.objeto = new XmlModel();
    this.objeto.XmlFile = btoa(this.text.toString());
    this.objeto.Extension = ".xml";
    isDevMode() && (this.ocsp = false);

    this.signatureService.firmaElectronica(this.objeto, this.ocsp).subscribe(
      (resp) => {
        isDevMode() && console.log({ resp });
        // tslint:disable-next-line: no-empty
        if (resp === "-1") {
        } else if (resp === "-2") {
          this.notificationService.showInfo(
            "Certificado",
            "Certificado no valido"
          );
        } else {
          this.responseFirma = resp;
          this.isEnabled = true;
        }
      },
      (err) => {
        const message = JSON.parse(err.error).ExceptionMessage;
        this.notificationService.showInfo("Certificado", message);
      }
    );
  }

  public Verificar() {
    this.objeto = new XmlModel();
    this.objeto.XmlFile = this.text;
    this.objeto.Extension = ".xml";
    this.signatureService.verificar(this.objeto).subscribe(
      (resp) => {
        const data = JSON.parse(JSON.stringify(resp.data));
        const cantTotalDeFirmas = data.length;
        let firmasValidas = 0;
        let firmasInvalidas = 0;

        data.forEach((element) => {
          if (data[data.indexOf(element)].IsValid === true) {
            firmasValidas = firmasValidas + 1;
          }
        });

        firmasInvalidas = cantTotalDeFirmas - firmasValidas;

        if (cantTotalDeFirmas === firmasValidas) {
          // tslint:disable-next-line: max-line-length
          this.notificationService.showVerify(
            "Verificación de firmas satisfactoria",
            `Validas: ${firmasValidas} / Invalidas: ${firmasInvalidas}`,
            data
          );
        } else {
          // tslint:disable-next-line: max-line-length
          this.notificationService.showNoVerify(
            "Verificación de firmas no satisfactoria",
            `Validas: ${firmasValidas} / Invalidas: ${firmasInvalidas}`,
            data
          );
        }
      },
      (err) => {
        const message = err.error.ExceptionMessage;
        this.notificationService.showError("Firmas", message);
      }
    );
  }

  public downloadFile() {
    const blob = new Blob([this.responseFirma], {
      type: "text/xml; charset=utf-8",
    });
    saveAs(blob, "Document.xml");
  }

  public preview() {
    this.showPreview = true;
    this.textPreview = this.responseFirma;
  }

  public changeType(event) {
    const key = event.target.value;
    const value = this.TiposDeFirma.find((item) => item.key === key).value;
    this.TipoDeFirma = value;
  }

  public copyMessage() {
    this.notificationService.showInfo("", "Documento copiado");
  }

  public ping() {
    this.signatureService.ping().subscribe(
      (resp) => {
        console.log("Ping exitoso:", resp); // Aquí haces algo con la respuesta
        this.toastr.success("El firmador funciona correctamente.");
      },
      (err) => {
        console.error("Error en el ping:", err); // Aquí manejas el error
        this.toastr.error(
          "No se logro conectar con el firmador, por favor reinicie la computadora."
        );
      }
    );
  }

  public pdfSignature() {
    this.signatureService.pdfSignature().subscribe(() => {});
  }

  triggerFileInput() {
    const fileInput = document.getElementById(
      "pdfFileInput"
    ) as HTMLInputElement;
    fileInput.click();
  }

  onFileSelected(event: any) {
    const file: File = event.target.files[0];
    const reader = new FileReader();

    reader.onload = () => {
      const base64String = (reader.result as string).split(",")[1]; // Remueve el prefijo de base64
      const model = {
        PdfBase64: base64String,
        Reason: "Prueba",
      };

      // Llama al servicio con el PDF en base64
      this.pdfSign64(model);
    };

    reader.readAsDataURL(file);
  }

  public pdfSign64(model: any) {
    this.signatureService.pdfSign64(model).subscribe(
      (response: any) => {
        this.Base64pdf = response.data.data[0];
        console.log("PDF firmado exitosamente:", response);
        this.toastr.success("PDF firmado exitosamente");
      },
      (error) => {
        console.error("Error al firmar el PDF:", error);
        this.toastr.error("Error al firmar el PDF:", error);
      }
    );
  }

  public descargarbase64() {
    if (this.Base64pdf != "") {
      // Convierte la cadena base64 a un arreglo de bytes
      const byteCharacters = atob(this.Base64pdf); // Decodifica el base64 a bytes
      const byteNumbers = new Array(byteCharacters.length);

      for (let i = 0; i < byteCharacters.length; i++) {
        byteNumbers[i] = byteCharacters.charCodeAt(i);
      }

      // Crea un array de bytes (Uint8Array) y un Blob con el tipo MIME de PDF
      const byteArray = new Uint8Array(byteNumbers);
      const blob = new Blob([byteArray], { type: "application/pdf" });

      // Crea una URL temporal para el archivo Blob
      const blobUrl = window.URL.createObjectURL(blob);

      // Crea un enlace para descargar el archivo
      const a = document.createElement("a");
      a.href = blobUrl;
      a.download = "documento_firmado.pdf"; // Nombre del archivo
      document.body.appendChild(a);
      a.click(); // Simula el clic en el enlace

      // Elimina el enlace temporal después de la descarga
      document.body.removeChild(a);
      window.URL.revokeObjectURL(blobUrl);
      this.toastr.success("PDF descargado exitosamente");
    } else {
      console.log("Haga primero Firmar PDF(Base64)");
      this.toastr.error("Error al descargar PDF");
    }
  }

  verificarVersion() {
    this.signatureService.ping().subscribe(
      (resp) => {
        console.log("Ping exitoso:", resp); // Aquí haces algo con la respuesta
        if (resp) {
          this.signatureService.versionSignature().subscribe({
            next: (response) => {
              console.log("A", response);
              const versionAuthenticaFirmador = response.data.version;
              if (
                versionAuthenticaFirmador <
                this.signatureService.versionAuthentica
              ) {
                const showConfirm = () => {
                  const result = window.confirm(
                    "Authentica no se encuentra actualizado.\nPara continuar debe descargarlo nuevamente o cerrarlo y abrirlo si ya lo posee, para luego reiniciar la computadora.\nSi no lo posee puede descargarlo apretando aceptar."
                  );
                  if (result) {
                    window.location.replace("https://authentica.fepba.gov.ar/");
                  }
                };
                showConfirm();
              } else {
                this.toastr.success(
                  "La versión del firmador esta actualizada."
                );
              }
            },
          });
        }
      },
      (err) => {
        console.error("Error en el ping:", err); // Aquí manejas el error
        this.toastr.error("No se logro hacer conectar.");
      }
    );
  }
}
