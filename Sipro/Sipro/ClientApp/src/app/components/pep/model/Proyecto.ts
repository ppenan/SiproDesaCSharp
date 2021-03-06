export class Proyecto{
    id: number;
    nombre: string;
    objetivo: string;
    descripcion: string;
    snip: number;
    proyectoTipoid: number;
    proyectotipo: string;
    unidadejecutora: string;
    ueunidadEjecutora: number;
    entidad: number;
    entidadnombre: string;
    ejercicio: number;
    fechaCreacion: string;
    usuarioCreo: string;
    fechaActualizacion: string;
    usuarioActualizo: string;
    programa?: number;
    subprograma?: number;
    proyecto?: number;
    actividad?: number;
    obra?: number;
    renglon?: number;
    ubicacionGeografica?: number;
    longitud: string;
    latitud: string;
    directorProyecto: number;
    directorProyectoNmbre: string;
    costo: number;
    acumulacionCostoid: number;
    acumulacionCostoNombre: string;
    objetivoEspecifico: string;
    visionGeneral: string;
    ejecucionFisicaReal: number;
    proyectoClase: number;
    projectCargado: number;
    prestamoid: number;
    fechaInicio: Date;
    fechaFin: Date;
    observaciones: string;
    fechaInicioReal: Date;
    fechaFinReal: Date;
    congelado: number;
    coordinador: number;
    porcentajeAvance: number;
    permisoEditarCongelar: boolean;
    lineaBaseId: number;
    duracion: number;
    fechaElegibilidad: Date;
    fechaCierre: Date;

    camposDinamicos: string;
    miembros: string;
    impactos: string;
}