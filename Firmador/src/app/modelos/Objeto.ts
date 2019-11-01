  export class Objeto {
    Archivo: String;
    Extension: String;

    constructor(objeto?: any) {
      this.Archivo = objeto && objeto.Archivo || '';
      this.Extension = objeto && objeto.Extension || '.xml';
    }
  }
