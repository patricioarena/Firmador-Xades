export class Propietario {
    Department: string;
    Email: string;
    Office: string;
    Fullname: string;
    Exist: boolean;

    constructor(propiertario?: any) {
      this.Department = propiertario && propiertario.Department || '';
      this.Email = propiertario && propiertario.Email || '';
      this.Office = propiertario && propiertario.Office || '';
      this.Fullname = propiertario && propiertario.Fullname || '';
      this.Exist = propiertario && propiertario.Exist || false;
    }
  }


  export class Firma {
    nombre: string;
    firma: string;

    constructor(firma?: any) {
      this.nombre = firma && firma.nombre || '';
      this.firma = firma && firma.firma || '';
    }
  }