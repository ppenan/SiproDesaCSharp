export class Actividad {
    id: number;
    nombre: string;
    descripcion: string;
    usuarioCreo: string;
    usuarioActualizo: string;
    fechaCreacion: string;
    fechaActualizacion: string;
    fechaInicio: string;
    fechaFin: string;
    actividadtipoid: number;
    actividadtiponombre: string;
    porcentajeavance: number;
    programa: number;
    subprograma: number;
    proyecto: number;
    actividad: number;
    obra: number;
    renglon: number;
    ubicacionGeografica: number;
    longitud: string;
    latitud: string;
    prececesorId: number;
    predecesorTipo: number;
    duracion: number;
    duracionDimension: string;
    costo: number;
    acumulacionCostoId: number;
    acumulacionCostoNombre: string;
    presupuestoModificado: number;
    presupuestoPagado: number;
    presupuestoVigente: number;
    presupuestoDevengado: number;
    avanceFinanciero: number;
    estado: number;
    proyectoBase: number;
    tieneHijos: boolean;
    fechaInicioReal: string;
    fechaFinReal: string;
    congelado: number;
    fechaElegibilidad: string;
    fechaCierre: string;
    inversionNueva: number;
}