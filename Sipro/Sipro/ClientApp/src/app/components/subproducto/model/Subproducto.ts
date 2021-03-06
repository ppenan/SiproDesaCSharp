export class Subproducto{
    id: number;
    productoid: number;
    subproductoTipoid: number;
    subProductoTipo: string;
    ueunidadEjecutora: number;
    entidadentidad: number;
    ejercicio: number;
    nombreUnidadEjecutora: string;
    entidadnombre: string;
    nombre: string;
    descripcion: string;
    usuarioCreo: string;
    usuarioActualizo: string;
    fechaCreacion: string;
    fechaActualizacion: string;
    estado: number;
    snip: number;
    programa: number;
    subprograma: number;
    proyecto: number;
    actividad: number;
    obra: number;
    renglon: number;
    ubicacionGeografica: number;
    duracion: number;
    duracionDimension: string;
    fechaInicio: string;
    fechaFin: string; // tenía date
    latitud: string;
    longitud: string;
    costo: number;
    acumulacionCostoid: number;
    acumulacionCostoNombre: string;
    tieneHijos: boolean;
    fechaInicioReal: string;
    fechaFinReal: string;
    fechaElegibilidad: string;
    fechaCierre: string;
    inversionNueva: number;
    orden: number;
    treepath: string;
    nivel: number;

    camposDinamicos: string;
}