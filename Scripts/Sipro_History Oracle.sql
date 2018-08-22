DROP TABLE actividad CASCADE CONSTRAINTS;
DROP TABLE actividad_propiedad_valor CASCADE CONSTRAINTS;
DROP TABLE actividad_tipo CASCADE CONSTRAINTS;
DROP TABLE acumulacion_costo CASCADE CONSTRAINTS;
DROP TABLE asignacion_raci CASCADE CONSTRAINTS;
DROP TABLE categoria_adquisicion CASCADE CONSTRAINTS;
DROP TABLE colaborador CASCADE CONSTRAINTS;
DROP TABLE componente CASCADE CONSTRAINTS;
DROP TABLE componente_propiedad_valor CASCADE CONSTRAINTS;
DROP TABLE componente_sigade CASCADE CONSTRAINTS;
DROP TABLE componente_tipo CASCADE CONSTRAINTS;
DROP TABLE desembolso CASCADE CONSTRAINTS;
DROP TABLE desembolso_tipo CASCADE CONSTRAINTS;
DROP TABLE documento CASCADE CONSTRAINTS;
DROP TABLE meta CASCADE CONSTRAINTS;
DROP TABLE meta_avance CASCADE CONSTRAINTS;
DROP TABLE meta_planificado CASCADE CONSTRAINTS;
DROP TABLE meta_tipo CASCADE CONSTRAINTS;
DROP TABLE meta_unidad_medida CASCADE CONSTRAINTS;
DROP TABLE pago_planificado CASCADE CONSTRAINTS;
DROP TABLE plan_adquisicion CASCADE CONSTRAINTS;
DROP TABLE plan_adquisicion_pago CASCADE CONSTRAINTS;
DROP TABLE prestamo CASCADE CONSTRAINTS;
DROP TABLE prestamo_tipo CASCADE CONSTRAINTS;
DROP TABLE prestamo_tipo_prestamo CASCADE CONSTRAINTS;
DROP TABLE producto CASCADE CONSTRAINTS;
DROP TABLE producto_propiedad_valor CASCADE CONSTRAINTS;
DROP TABLE producto_tipo CASCADE CONSTRAINTS;
DROP TABLE proyecto CASCADE CONSTRAINTS;
DROP TABLE proyecto_propiedad_valor CASCADE CONSTRAINTS;
DROP TABLE proyecto_tipo CASCADE CONSTRAINTS;
DROP TABLE ptipo_propiedad CASCADE CONSTRAINTS;
DROP TABLE riesgo CASCADE CONSTRAINTS;
DROP TABLE riesgo_propiedad_valor CASCADE CONSTRAINTS;
DROP TABLE riesgo_tipo CASCADE CONSTRAINTS;
DROP TABLE subcomponente CASCADE CONSTRAINTS;
DROP TABLE subcomponente_propiedad_valor CASCADE CONSTRAINTS;
DROP TABLE subcomponente_tipo CASCADE CONSTRAINTS;
DROP TABLE subproducto CASCADE CONSTRAINTS;
DROP TABLE subproducto_propiedad_valor CASCADE CONSTRAINTS;
DROP TABLE subproducto_tipo CASCADE CONSTRAINTS;
DROP TABLE tipo_adquisicion CASCADE CONSTRAINTS;
DROP TABLE unidad_medida CASCADE CONSTRAINTS;
DROP TABLE objeto_riesgo CASCADE CONSTRAINTS;
DROP TABLE componente_matriz CASCADE CONSTRAINTS;

CREATE TABLE actividad (id number(10) NULL, nombre varchar2(1000) NULL, descripcion varchar2(1000), fecha_inicio timestamp(0) NULL, fecha_fin timestamp(0) NULL, porcentaje_avance number(3) NULL, usuario_creo varchar2(30) NULL, usuario_actualizo varchar2(30), fecha_creacion timestamp(0) NULL, fecha_actualizacion timestamp(0) NULL, estado number(2) NULL, actividad_tipoid number(10) NULL, snip number(10), programa number(4), subprograma number(4), proyecto number(4), actividad number(4), obra number(4), objeto_id number(11) NULL, objeto_tipo number(2) NULL, duracion number(10) NULL, duracion_dimension varchar2(1) NULL, pred_objeto_id number(11), pred_objeto_tipo number(2), latitud varchar2(30), longitud varchar2(30), costo decimal(15, 2), acumulacion_costo number(11) NULL, renglon number(4), ubicacion_geografica number(4), orden number(10), treePath varchar2(1000), nivel number(4), proyecto_base number(11), componente_base number(10), producto_base number(10), fecha_inicio_real timestamp(0) NULL, fecha_fin_real timestamp(0) NULL, inversion_nueva number(1) NULL, version number(10) NULL, linea_base varchar2(500) NULL, actual number(1));
CREATE TABLE actividad_propiedad_valor (actividadid number(10) NULL, actividad_propiedadid number(10) NULL, valor_entero number(10), valor_string varchar2(4000), valor_decimal decimal(15, 2), valor_tiempo timestamp(0) NULL, usuario_creo varchar2(30), usuario_actualizo varchar2(30), fecha_creacion timestamp(0) NULL, fecha_actualizacion timestamp(0) NULL, estado number(2),  version number(10) NULL, linea_base varchar2(500) NULL, actual number(1));
CREATE TABLE actividad_tipo (id number(10) NULL, nombre varchar2(200) NULL, descripcion varchar2(1000), usuario_creo varchar2(30) NULL, usuario_actualizo varchar2(30), fecha_creacion timestamp(0) NULL, fecha_actualizacion timestamp(0) NULL, estado number(2) NULL,  version number(10) NULL, linea_base varchar2(500) NULL, actual number(1));
CREATE TABLE acumulacion_costo (id number(11) NULL, nombre varchar2(45) NULL, usuario_creo varchar2(30) NULL, usuario_actualizo varchar2(30), fecha_creacion timestamp(0) NULL, fecha_actualizacion timestamp(0) NULL, estado number(2) NULL,  version number(10) NULL, linea_base varchar2(500) NULL, actual number(1));
CREATE TABLE asignacion_raci (id number(11) NULL, matriz_raciid number(11) NULL, colaboradorid number(10) NULL, rol_raci varchar2(1) NULL, objeto_id number(11) NULL, objeto_tipo number(11) NULL, usuario_creo varchar2(30) NULL, usuario_actualizo varchar2(30) NULL, fecha_creacion timestamp(0) NULL, fecha_actualizacion timestamp(0) NULL, estado number(2) NULL,  version number(10) NULL, linea_base varchar2(500) NULL, actual number(1));
CREATE TABLE categoria_adquisicion (id number(11) NULL, nombre varchar2(45) NULL, descripcion varchar2(100), usuario_creo varchar2(30) NULL, usuario_actualizo varchar2(30), fecha_creacion timestamp(0) NULL, fecha_actualizacion timestamp(0) NULL, estado number(2) NULL,  version number(10) NULL, linea_base varchar2(500) NULL, actual number(1));
CREATE TABLE colaborador (id number(10) NULL, pnombre varchar2(255) NULL, snombre varchar2(255), papellido varchar2(255) NULL, sapellido varchar2(255), cui number(15) NULL, ueunidad_ejecutora number(10) NULL, usuariousuario varchar2(30) NULL, estado number(1) NULL, usuario_creo varchar2(30) NULL, usuario_actualizo varchar2(30), fecha_creacion timestamp(0) NULL, fecha_actualizacion timestamp(0) NULL, ejercicio number(4) NULL, entidad number(10),  version number(10) NULL, linea_base varchar2(500) NULL, actual number(1));
CREATE TABLE componente (id number(10) NULL, nombre varchar2(1000) NULL, descripcion varchar2(4000), proyectoid number(10) NULL, componente_tipoid number(10) NULL, usuario_creo varchar2(30) NULL, usuario_actualizo varchar2(30), fecha_creacion timestamp(0) NULL, fecha_actualizacion timestamp(0) NULL, estado number(2) NULL, ueunidad_ejecutora number(10) NULL, snip number(10), programa number(4), subprograma number(4), proyecto number(4), actividad number(4), obra number(4), latitud varchar2(30), longitud varchar2(30),  costo decimal(15, 2), acumulacion_costoid number(11), renglon number(4), ubicacion_geografica number(4), fecha_inicio timestamp(0) NULL, fecha_fin timestamp(0) NULL, duracion number(10) NULL, duracion_dimension varchar2(1),  orden number(10), treepath varchar2(1000), nivel number(4), entidad number(10), ejercicio number(4) NULL, es_de_sigade number(1), fuente_prestamo decimal(15, 2), fuente_donacion decimal(15, 2), fuente_nacional decimal(15, 2),   componente_sigadeid number(10) NULL, fecha_inicio_real timestamp(0) NULL, fecha_fin_real timestamp(0) NULL, inversion_nueva number(1) NULL, version number(10) NULL, linea_base varchar2(500) NULL, actual number(1));
CREATE TABLE componente_propiedad_valor (componenteid number(10) NULL, componente_propiedadid number(10) NULL, valor_string varchar2(4000), valor_entero number(10), valor_decimal decimal(15, 2), valor_tiempo timestamp(0) NULL, usuario_creo varchar2(30) NULL, usuario_actualizo varchar2(30), fecha_creacion timestamp(0) NULL, fecha_actualizacion timestamp(0) NULL,  version number(10) NULL, linea_base varchar2(500) NULL, actual number(1));
CREATE TABLE componente_sigade (id number(10) NULL, nombre varchar2(2000) NULL, codigo_presupuestario varchar2(45) NULL, numero_componente number(10) NULL,monto_componente decimal(15, 2) NULL, usuario_creo varchar2(30) NULL, usuario_actualizo varchar2(30), fecha_creacion timestamp(0) NULL,fecha_actualizacion timestamp(0) NULL, estado number(2) NULL, version number(10) NULL, linea_base varchar2(500) NULL, actual number(1));
CREATE TABLE componente_tipo (id number(10) NULL, nombre varchar2(1000) NULL, descripcion varchar2(2000), usuario_creo varchar2(30) NULL, usuario_actualizo varchar2(30), fecha_creacion timestamp(0) NULL, fecha_actualizacion timestamp(0) NULL, estado number(2) NULL,  version number(10) NULL, linea_base varchar2(500) NULL, actual number(1));
CREATE TABLE desembolso (id number(10) NULL, fecha timestamp(0) NULL, estado number(2) NULL, monto decimal(15, 2) NULL, tipo_cambio decimal(15, 4) NULL, monto_moneda_origen number(11), usuario_creo varchar2(30) NULL, usuario_actualizo varchar2(30), fecha_creacion timestamp(0) NULL, fecha_actualizacion timestamp(0) NULL, proyectoid number(10) NULL, desembolso_tipoid number(10) NULL, tipo_monedaid number(10) NULL,  version number(10) NULL, linea_base varchar2(500) NULL, actual number(1));
CREATE TABLE desembolso_tipo (id number(10) NULL, nombre varchar2(1000), descripcion varchar2(4000), estado number(2), usuario_creo varchar2(30) NULL, usuario_actualizo varchar2(30), fecha_creacion timestamp(0) NULL, fecha_actualizacion timestamp(0) NULL,  version number(10) NULL, linea_base varchar2(500) NULL, actual number(1));
CREATE TABLE documento (id number(10) NULL, nombre varchar2(1000) NULL, extension varchar2(45) NULL, id_tipo_objeto number(11) NULL, id_objeto number(11) NULL, usuario_creo varchar2(30) NULL, usuario_actualizo varchar2(30), fecha_creacion timestamp(0) NULL, fecha_actualizacion timestamp(0) NULL, estado number(2) NULL,  version number(10) NULL, linea_base varchar2(500) NULL, actual number(1));
CREATE TABLE meta (id number(10) NULL, nombre varchar2(1000) NULL, descripcion varchar2(4000), usuario_creo varchar2(30) NULL, usuario_actualizo varchar2(30), fecha_creacion timestamp(0) NULL, fecha_actualizacion timestamp(0) NULL, estado number(2), meta_unidad_medidaid number(10) NULL, dato_tipoid number(10) NULL, objeto_id number(11), objeto_tipo number(11), meta_final_entero number(10), meta_final_string varchar2(1000), meta_final_decimal decimal(15, 2), meta_final_fecha timestamp(0) NULL,  version number(10) NULL, linea_base varchar2(500) NULL, actual number(1));
CREATE TABLE meta_avance (metaid number(10) NULL, fecha timestamp(0) NULL, usuario varchar2(30) NULL, valor_entero number(10), valor_string varchar2(4000), valor_decimal decimal(15, 2), valor_tiempo timestamp(0) NULL, estado number(2) NULL, fecha_ingreso timestamp(0) NULL, version number(10) NULL, linea_base varchar2(500) NULL, actual number(1));
CREATE TABLE meta_planificado (metaid number(10) NULL, ejercicio number(4) NULL, enero_entero number(10), enero_string varchar2(1000), enero_decimal decimal(15, 2), enero_tiempo timestamp(0) NULL, febrero_entero number(10), febrero_string varchar2(1000), febrero_decimal decimal(15, 2), febrero_tiempo timestamp(0) NULL, marzo_entero number(10), marzo_string varchar2(1000), marzo_decimal decimal(15, 2), marzo_tiempo timestamp(0) NULL, abril_entero number(10), abril_string varchar2(1000), abril_decimal decimal(15, 2), abril_tiempo timestamp(0) NULL, mayo_entero number(10), mayo_string varchar2(1000), mayo_decimal decimal(15, 2), mayo_tiempo timestamp(0) NULL, junio_entero number(10), junio_string varchar2(1000), junio_decimal decimal(15, 2), junio_tiempo timestamp(0) NULL, julio_entero number(10), julio_string varchar2(1000), julio_decimal decimal(15, 2), julio_tiempo timestamp(0) NULL, agosto_entero number(10), agosto_string varchar2(1000), agosto_decimal decimal(15, 2), agosto_tiempo timestamp(0) NULL, septiembre_entero number(10), septiembre_string varchar2(1000), septiembre_decimal decimal(15, 2), septiembre_tiempo timestamp(0) NULL, octubre_entero number(10), octubre_string varchar2(1000), octubre_decimal decimal(15, 2), octubre_tiempo timestamp(0) NULL, noviembre_entero number(10), noviembre_string varchar2(1000), noviembre_decimal decimal(15, 2), noviembre_tiempo timestamp(0) NULL, diciembre_entero number(10), diciembre_string varchar2(1000), diciembre_decimal decimal(15, 2), diciembre_tiempo timestamp(0) NULL, estado number(2) NULL, usuario varchar2(30) NULL, fecha_ingreso timestamp(0) NULL,  version number(10) NULL, linea_base varchar2(500) NULL, actual number(1));
CREATE TABLE meta_tipo (id number(10) NULL, nombre varchar2(1000) NULL, descripcion varchar2(4000), usuario_creo varchar2(30) NULL, usuario_actualizo varchar2(30), fecha_creacion timestamp(0) NULL, fecha_actualizacion timestamp(0) NULL, estado number(2) NULL,  version number(10) NULL, linea_base varchar2(500) NULL, actual number(1));
CREATE TABLE meta_unidad_medida (id number(10) NULL, nombre varchar2(1000), descripcion varchar2(4000), simbolo varchar2(10), usuario_creo varchar2(30), usuario_actualizo varchar2(30), fecha_creacion timestamp(0) NULL, fecha_actualizacion timestamp(0) NULL, estado number(2),  version number(10) NULL, linea_base varchar2(500) NULL, actual number(1));
CREATE TABLE pago_planificado (id number(10) NULL, fecha_pago timestamp(0) NULL, pago decimal(15,2) NULL, objeto_id number(10) NULL, objeto_tipo number(10) NULL, usuario_creo varchar2(30) NULL, usuario_actualizo varchar2(30) NULL, fecha_creacion timestamp(0) NULL, fecha_actualizacion timestamp(0) NULL, estado number(1) NULL, version number(10) NULL, linea_base varchar2(500) NULL, actual number(1));
CREATE TABLE plan_adquisicion (id number(11) NULL, tipo_adquisicion number(11) NULL, categoria_adquisicion number(11) NULL, unidad_medida varchar2(30), cantidad number(11), total decimal(15, 2), precio_unitario decimal(15, 2), preparacion_doc_planificado timestamp(0) NULL, preparacion_doc_real timestamp(0) NULL, lanzamiento_evento_planificado timestamp(0) NULL, lanzamiento_evento_real timestamp(0) NULL, recepcion_ofertas_planificado timestamp(0) NULL, recepcion_ofertas_real timestamp(0) NULL, adjudicacion_planificado timestamp(0) NULL, adjudicacion_real timestamp(0) NULL, firma_contrato_planificado timestamp(0) NULL, firma_contrato_real timestamp(0) NULL, objeto_id number(11) NULL, objeto_tipo number(11) NULL, usuario_creo varchar2(30) NULL, usuario_actualizo varchar2(30), fecha_creacion timestamp(0) NULL, fecha_actualizacion timestamp(0) NULL, estado number(2), bloqueado number(2), numero_contrato varchar2(45), monto_contrato decimal(15, 2) NULL, nog number(8), tipo_revision number(1),  version number(10) NULL, linea_base varchar2(500) NULL, actual number(1));
CREATE TABLE plan_adquisicion_pago (id number(11) NULL, plan_adquisicionid number(11) NULL, fecha_pago timestamp(0) NULL, pago decimal(15, 2), descripcion varchar2(100) NULL, usuario_creo varchar2(30) NULL, usuario_actualizo varchar2(30), fecha_creacion timestamp(0) NULL, fecha_actualizacion timestamp(0) NULL, estado number(2) NULL,  version number(10) NULL, linea_base varchar2(500) NULL, actual number(1));
CREATE TABLE prestamo (id number(10) NULL, fecha_corte timestamp(0) NULL, codigo_presupuestario number(12) NULL, numero_prestamo varchar2(30) NULL, destino varchar2(1000), sector_economico varchar2(1000), ueunidad_ejecutora number(10) NULL, fecha_firma timestamp(0) NULL, autorizacion_tipoid number(10), numero_autorizacion varchar2(100), fecha_autorizacion timestamp(0) NULL, anios_plazo number(3), anios_gracia number(3), fecha_fin_ejecucion timestamp(0) NULL, perido_ejecucion number(3), interes_tipoid number(10), porcentaje_interes decimal(10, 5), porcentaje_comision_compra decimal(10, 5), tipo_monedaid number(10) NULL, monto_contratado decimal(15, 2) NULL, amortizado decimal(15, 2), por_amortizar decimal(15, 2), principal_anio decimal(15, 2), intereses_anio decimal(15, 2), comision_compromiso_anio decimal(15, 2), otros_gastos decimal(15, 2), principal_acumulado decimal(15, 2), intereses_acumulados decimal(15, 2), comision_compromiso_acumulado decimal(15, 2), otros_cargos_acumulados decimal(15, 2), presupuesto_asignado_func decimal(15, 2), presupuesto_asignado_inv decimal(15, 2), presupuesto_modificado_func decimal(15, 2), presupuesto_modificado_inv decimal(15, 2), presupuesto_vigente_func decimal(15, 2), presupuesto_vigente_inv decimal(15, 2), presupuesto_devengado_func decimal(15, 2), presupuesto_devengado_inv decimal(15, 2), presupuesto_pagado_func decimal(15, 2), presupuesto_pagado_inv decimal(15, 2), saldo_cuentas decimal(15, 2), desembolsado_real decimal(15, 2), ejecucion_estadoid number(10), usuario_creo varchar2(30) NULL, usuario_actualizo varchar2(30), fecha_creacion timestamp(0) NULL, fecha_actualizacion timestamp(0) NULL, estado number(2) NULL, proyecto_programa varchar2(100) NULL, fecha_decreto timestamp(0) NULL, fecha_suscripcion timestamp(0) NULL, fecha_elegibilidad_ue timestamp(0) NULL, fecha_cierre_origianl_ue timestamp(0) NULL, fecha_cierre_actual_ue timestamp(0) NULL, meses_prorroga_ue number(3) NULL, plazo_ejecucion_ue number(3), monto_asignado_ue decimal(15, 2), desembolso_a_fecha_ue decimal(15, 2), monto_por_desembolsar_ue decimal(15, 2), fecha_vigencia timestamp(0) NULL, monto_contratado_usd decimal(15, 2) NULL, monto_contratado_qtz decimal(15, 2) NULL, desembolso_a_fecha_usd decimal(15, 2), monto_por_desembolsar_usd decimal(15, 2) NULL, monto_asignado_ue_usd decimal(15, 2), monto_asignado_ue_qtz decimal(15, 2), desembolso_a_fecha_ue_usd decimal(15, 2), monto_por_desembolsar_ue_usd decimal(15, 2), entidad number(10) NULL, ejercicio number(4) NULL, objetivo varchar2(4000), objetivo_especifico varchar2(4000), porcentaje_avance number(3) NULL, cooperantecodigo number(5) NULL, cooperanteejercicio number(4) NULL, version number(10) NULL, linea_base varchar2(500) NULL, actual number(1));
CREATE TABLE prestamo_tipo (id number(11) NULL, nombre varchar2(1000) NULL, descripcion varchar2(2000), usuario_creo varchar2(30) NULL, usuario_actualizo varchar2(30), fecha_creacion timestamp(0) NULL, fecha_actualizacion timestamp(0) NULL, estado number(2) NULL,  version number(10) NULL, linea_base varchar2(500) NULL, actual number(1));
CREATE TABLE prestamo_tipo_prestamo (prestamoId number(10) NULL, tipoPrestamoId number(11) NULL, usuario_creo varchar2(30) NULL, usuario_actualizo varchar2(30), fecha_creacion timestamp(0) NULL, fecha_actualizacion timestamp(0) NULL, estado number(2) NULL, version number(10) NULL, linea_base varchar2(500) NULL, actual number(1));
CREATE TABLE producto (id number(10) NULL, nombre varchar2(1000) NULL, descripcion varchar2(4000), componenteid number(10), subcomponenteid number(10) NULL, usuario_creo varchar2(30) NULL, usuario_actualizo varchar2(30), fecha_creacion timestamp(0) NULL, fecha_actualizacion timestamp(0) NULL, producto_tipoid number(10) NULL, estado number(2), ueunidad_ejecutora number(10) NULL, snip number(10), programa number(4), subprograma number(4), proyecto number(4), actividad number(4), obra number(4), latitud varchar2(30), longitud varchar2(30), peso number(3), costo decimal(15, 2), acumulacion_costoid number(11) NULL, renglon number(4), ubicacion_geografica number(4), fecha_inicio timestamp(0) NULL, fecha_fin timestamp(0) NULL, duracion number(10) NULL, duracion_dimension varchar2(1), orden number(10), treePath varchar2(1000), nivel number(4), entidad number(10), ejercicio number(4) NULL, fecha_inicio_real timestamp(0) NULL, fecha_fin_real timestamp(0) NULL, inversion_nueva number(1) NULL, version number(10) NULL, linea_base varchar2(500) NULL, actual number(1));
CREATE TABLE producto_propiedad_valor (producto_propiedadid number(10) NULL, productoid number(10) NULL, valor_entero number(10), valor_string varchar2(4000), valor_decimal decimal(15, 2), valor_tiempo timestamp(0) NULL, usuario_creo varchar2(30) NULL, usuario_actualizo varchar2(30), fecha_creacion timestamp(0) NULL, fecha_actualizacion timestamp(0) NULL, estado number(2) NULL,  version number(10) NULL, linea_base varchar2(500) NULL, actual number(1));
CREATE TABLE producto_tipo (id number(10) NULL, nombre varchar2(1000) NULL, descripcion varchar2(2000), usuario_creo varchar2(30) NULL, usuario_actualizo varchar2(30), fecha_creacion timestamp(0) NULL, fecha_actualizacion timestamp(0) NULL, estado number(2) NULL,  version number(10) NULL, linea_base varchar2(500) NULL, actual number(1));
CREATE TABLE proyecto (id number(10) NULL, nombre varchar2(1000) NULL, descripcion varchar2(2000), usuario_creo varchar2(30) NULL, usuario_actualizo varchar2(30), fecha_creacion timestamp(0) NULL, fecha_actualizacion timestamp(0) NULL, estado number(2) NULL, proyecto_tipoid number(10) NULL, ueunidad_ejecutora number(10) NULL, snip number(10), programa number(4), subprograma number(4), proyecto number(4), actividad number(4), obra number(4), latitud varchar2(30), longitud varchar2(30), objetivo varchar2(4000), director_proyecto number(10), enunciado_alcance varchar2(4000), costo number(15, 2), acumulacion_costoid number(11) NULL, objetivo_especifico varchar2(4000), vision_general varchar2(45), renglon number(4), ubicacion_geografica number(4), fecha_inicio timestamp(0) NULL, fecha_fin timestamp(0) NULL, duracion number(10) NULL, duracion_dimension varchar2(1) NULL, orden number(10) NULL, treePath varchar2(1000) NULL, nivel number(4) NULL, ejercicio number(4) NULL, entidad number(10) NULL, ejecucion_fisica_real number(3) NULL, proyecto_clase number(2) NULL, project_cargado number(1) NULL, prestamoid number(10) NULL, observaciones varchar2(2000) NULL, coordinador number(1) NULL, fecha_elegibilidad timestamp(0) NULL, fecha_inicio_real timestamp(0) NULL, fecha_fin_real timestamp(0) NULL, congelado number(1), fecha_cierre timestamp(0) NULL,  version number(10) NULL, linea_base varchar2(1000) NULL, actual number(1));
CREATE TABLE proyecto_propiedad_valor (proyectoid number(10) NULL, proyecto_propiedadid number(10) NULL, valor_string varchar2(4000), valor_entero number(10), valor_decimal decimal(15, 2), valor_tiempo timestamp(0) NULL, usuario_creo varchar2(30) NULL, usuario_actualizo varchar2(30), fecha_creacion timestamp(0) NULL, fecha_actualizacion timestamp(0) NULL, estado number(2) NULL,  version number(10) NULL, linea_base varchar2(500) NULL, actual number(1));
CREATE TABLE proyecto_tipo (id number(10) NULL, nombre varchar2(1000) NULL, descripcion varchar2(2000), usario_creo varchar2(30) NULL, usuario_actualizo varchar2(30), fecha_creacion timestamp(0) NULL, fecha_actualizacion timestamp(0) NULL, estado number(2) NULL,  version number(10) NULL, linea_base varchar2(500) NULL, actual number(1));
CREATE TABLE ptipo_propiedad (proyecto_tipoid number(10) NULL, proyecto_propiedadid number(10) NULL, usuario_creo varchar2(30) NULL, usuario_actualizo varchar2(30), fecha_creacion timestamp(0) NULL, fecha_actualizacion timestamp(0) NULL, estado number(2),  version number(10) NULL, linea_base varchar2(500) NULL, actual number(1));
CREATE TABLE riesgo (id number(10) NULL, nombre varchar2(1000) NULL, descripcion varchar2(4000), riesgo_tipoid number(10) NULL, impacto decimal(3, 2) NULL, probabilidad decimal(3, 2) NULL, impacto_monto decimal(15, 2), impacto_tiempo decimal(15, 2), gatillo varchar2(1000), consecuencia varchar2(1000), solucion varchar2(1000), riesgo_segundarios varchar2(1000), ejecutado number(1) NULL, fecha_ejecucion timestamp(0) NULL, resultado varchar2(1000), observaciones varchar2(1000), colaboradorid number(10) NULL, usuario_creo varchar2(30) NULL, usuario_actualizo varchar2(30), fecha_creacion timestamp(0) NULL, fecha_actualizacion timestamp(0) NULL, estado number(2) NULL,  version number(10) NULL, linea_base varchar2(500) NULL, actual number(1));
CREATE TABLE riesgo_propiedad_valor (riesgoid number(10) NULL, riesgo_propiedadid number(10) NULL, valor_entero number(10), valor_string varchar2(4000), valor_decimal decimal(15, 2), valor_tiempo timestamp(0) NULL, estado number(2) NULL, usuario_creo varchar2(30) NULL, usuario_actualizo varchar2(30), fecha_creacion timestamp(0) NULL, fecha_actualizacion timestamp(0) NULL,  version number(10) NULL, linea_base varchar2(500) NULL, actual number(1));
CREATE TABLE riesgo_tipo (id number(10) NULL, nombre varchar2(1000) NULL, descripcion varchar2(4000), usuario_creo varchar2(30) NULL, usuario_actualizo varchar2(30), fecha_creacion timestamp(0) NULL, fecha_actualizacion timestamp(0) NULL, estado number(2) NULL,  version number(10) NULL, linea_base varchar2(500) NULL, actual number(1));
CREATE TABLE subcomponente (id number(10) NULL, nombre varchar2(1000) NULL, descripcion varchar2(2000), componenteid number(10) NULL, subcomponente_tipoid number(10) NULL, usuario_creo varchar2(30) NULL, usuario_actualizo varchar2(30), fecha_creacion timestamp(0) NULL, fecha_actualizacion timestamp(0) NULL, estado number(2) NULL, ueunidad_ejecutora number(10), snip number(10), programa number(4), subprograma number(4), proyecto number(4), actividad number(4), obra number(4), latitud varchar2(30), longitud varchar2(30), costo decimal(15, 2), acumulacion_costoid number(11) NULL, renglon number(4), ubicacion_geografica number(4), fecha_inicio timestamp(0) NULL, fecha_fin timestamp(0) NULL, duracion number(10) NULL, duracion_dimension varchar2(1), orden number(10), treePath varchar2(1000), nivel number(4), entidad number(10), ejercicio number(4), fecha_inicio_real timestamp(0) NULL, fecha_fin_real timestamp(0) NULL, inversion_nueva number(1) NULL, version number(10) NULL, linea_base varchar2(500) NULL, actual number(1));
CREATE TABLE subcomponente_propiedad_valor (subcomponenteid number(10) NULL, subcomponente_propiedadid number(10) NULL, valor_string varchar2(4000), valor_entero number(10), valor_decimal decimal(15, 2), valor_tiempo timestamp(0) NULL, usuario_creo varchar2(30) NULL, usuario_actualizo varchar2(30), fecha_creacion timestamp(0) NULL, fecha_actualizacion timestamp(0) NULL,  version number(10) NULL, linea_base varchar2(500) NULL, actual number(1));
CREATE TABLE subcomponente_tipo (id number(10) NULL, nombre varchar2(1000) NULL, descripcion varchar2(2000), usuario_creo varchar2(30) NULL, usuario_actualizo varchar2(30), fecha_creacion timestamp(0) NULL, fecha_actualizacion timestamp(0) NULL, estado number(2) NULL,  version number(10) NULL, linea_base varchar2(500) NULL, actual number(1));
CREATE TABLE subproducto (id number(10) NULL, nombre varchar2(1000) NULL, descripcion varchar2(4000), usuario_creo varchar2(30) NULL, usuario_actualizo varchar2(30), fecha_creacion timestamp(0) NULL, fecha_actualizacion timestamp(0) NULL, estado number(2) NULL, snip number(10), programa number(4), subprograma number(4), proyecto number(4), actividad number(4), obra number(4), productoid number(10) NULL, subproducto_tipoid number(10) NULL, ueunidad_ejecutora number(10) NULL, latitud varchar2(30), longitud varchar2(30), costo decimal(15, 2), acumulacion_costoid number(11) NULL, renglon number(4), ubicacion_geografica number(4), fecha_inicio timestamp(0) NULL, fecha_fin timestamp(0) NULL, duracion number(10) NULL, duracion_dimension varchar2(1), orden number(10), treePath varchar2(1000), nivel number(4), entidad number(10), ejercicio number(4) NULL, fecha_inicio_real timestamp(0) NULL, fecha_fin_real timestamp(0) NULL, inversion_nueva number(1) NULL, version number(10) NULL, linea_base varchar2(500) NULL, actual number(1));
CREATE TABLE subproducto_propiedad_valor (subproductoid number(10) NULL, subproducto_propiedadid number(10) NULL, valor_entero number(10), valor_string varchar2(4000), valor_decimal decimal(15, 2), valor_tiempo timestamp(0) NULL, usuario_creo varchar2(30) NULL, usuario_actualizo varchar2(30), fecha_creacion timestamp(0) NULL, fecha_actualizacion timestamp(0) NULL, estado number(2) NULL,  version number(10) NULL, linea_base varchar2(500) NULL, actual number(1));
CREATE TABLE subproducto_tipo (id number(10) NULL, nombre varchar2(1000) NULL, descripcion varchar2(2000), usuario_creo varchar2(30) NULL, usuario_actualizo varchar2(30), fecha_creacion timestamp(0) NULL, fecha_actualizacion timestamp(0) NULL, estado number(2) NULL,  version number(10) NULL, linea_base varchar2(500) NULL, actual number(1));
CREATE TABLE tipo_adquisicion (id number(11) NULL, cooperantecodigo number(5) NULL, cooperanteejercicio number(4) NULL,nombre varchar2(1000) NULL, usuario_creo varchar2(30) NULL, usuario_actualizo varchar2(30), fecha_creacion timestamp(0) NULL, fecha_actualizacion timestamp(0) NULL, estado number(2) NULL,  convenio_cdirecta number(1) NULL, version number(10) NULL, linea_base varchar2(500) NULL, actual number(1));
CREATE TABLE unidad_medida (id number(11) NULL, nombre varchar2(100) NULL, descripcion varchar2(100), usuario_creo varchar2(30) NULL, usuario_actualizacion varchar2(30), fecha_creacion timestamp(0) NULL, fecha_actualizacion timestamp(0) NULL, estado number(2) NULL,  version number(10) NULL, linea_base varchar2(500) NULL, actual number(1));
CREATE TABLE objeto_riesgo (riesgoid number(10) NULL, objeto_id number(11) NULL, objeto_tipo number(11) NULL, usuario_creo varchar2(30) NULL,  usuario_actualizo varchar2(30) NULL,  fecha_creacion timestamp(0) NULL,  fecha_actualizacion timestamp(0) NULL,  version number(10) NULL, linea_base varchar2(500) NULL, actual number(1));
CREATE TABLE componente_matriz (unidad_ejecutoraid number(10) DEFAULT NULL, nombre varchar2(200) DEFAULT NULL, componenteid number(10) DEFAULT NULL, componente_sigadeid number(10) DEFAULT NULL, prestamoid number(10) DEFAULT NULL, proyectoid number(10) DEFAULT NULL, entidadid number(10) DEFAULT NULL, entidad varchar2(200) DEFAULT NULL, ejercicio number(4) DEFAULT NULL, fuente_prestamo decimal(15,2) DEFAULT NULL, fuente_donacion decimal(15,2) DEFAULT NULL, fuente_nacional decimal(15,2) DEFAULT NULL, techo decimal(15,2) DEFAULT NULL, version number(10) DEFAULT NULL, usuario_actualizo varchar2(30) DEFAULT NULL, fecha_actualizacion timestamp(0) NULL);

INSERT INTO sipro_history.actividad SELECT * FROM sipro.actividad, (SELECT 1 as version, NULL as linea_base, 1 as actual FROM DUAL);
INSERT INTO sipro_history.actividad_propiedad_valor SELECT * FROM sipro.actividad_propiedad_valor, (SELECT 1 as version, NULL as linea_base, 1 as actual FROM DUAL);
INSERT INTO sipro_history.actividad_tipo SELECT * FROM sipro.actividad_tipo, (SELECT 1 as version, NULL as linea_base, 1 as actual FROM DUAL);
INSERT INTO sipro_history.acumulacion_costo SELECT * FROM sipro.acumulacion_costo, (SELECT 1 as version, NULL as linea_base, 1 as actual FROM DUAL);
INSERT INTO sipro_history.asignacion_raci SELECT * FROM sipro.asignacion_raci, (SELECT 1 as version, NULL as linea_base, 1 as actual FROM DUAL);
INSERT INTO sipro_history.categoria_adquisicion SELECT * FROM sipro.categoria_adquisicion, (SELECT 1 as version, NULL as linea_base, 1 as actual FROM DUAL);
INSERT INTO sipro_history.colaborador SELECT * FROM sipro.colaborador, (SELECT 1 as version, NULL as linea_base, 1 as actual FROM DUAL);
INSERT INTO sipro_history.componente SELECT * FROM sipro.componente, (SELECT 1 as version, NULL as linea_base, 1 as actual FROM DUAL);
INSERT INTO sipro_history.componente_propiedad_valor SELECT * FROM sipro.componente_propiedad_valor, (SELECT 1 as version, NULL as linea_base, 1 as actual FROM DUAL);
INSERT INTO sipro_history.componente_sigade SELECT * FROM sipro.componente_sigade, (SELECT 1 as version, NULL as linea_base, 1 as actual FROM DUAL);
INSERT INTO sipro_history.componente_tipo SELECT * FROM sipro.componente_tipo, (SELECT 1 as version, NULL as linea_base, 1 as actual FROM DUAL);
INSERT INTO sipro_history.desembolso SELECT * FROM sipro.desembolso, (SELECT 1 as version, NULL as linea_base, 1 as actual FROM DUAL);
INSERT INTO sipro_history.desembolso_tipo SELECT * FROM sipro.desembolso_tipo, (SELECT 1 as version, NULL as linea_base, 1 as actual FROM DUAL);
INSERT INTO sipro_history.documento SELECT * FROM sipro.documento, (SELECT 1 as version, NULL as linea_base, 1 as actual FROM DUAL);
INSERT INTO sipro_history.meta SELECT * FROM sipro.meta, (SELECT 1 as version, NULL as linea_base, 1 as actual FROM DUAL);
INSERT INTO sipro_history.meta_avance SELECT * FROM sipro.meta_avance, (SELECT 1 as version, NULL as linea_base, 1 as actual FROM DUAL);
INSERT INTO sipro_history.meta_planificado SELECT * FROM sipro.meta_planificado, (SELECT 1 as version, NULL as linea_base, 1 as actual FROM DUAL);
INSERT INTO sipro_history.meta_tipo SELECT * FROM sipro.meta_tipo, (SELECT 1 as version, NULL as linea_base, 1 as actual FROM DUAL);
INSERT INTO sipro_history.meta_unidad_medida SELECT * FROM sipro.meta_unidad_medida, (SELECT 1 as version, NULL as linea_base, 1 as actual FROM DUAL);
INSERT INTO sipro_history.plan_adquisicion SELECT * FROM sipro.plan_adquisicion, (SELECT 1 as version, NULL as linea_base, 1 as actual FROM DUAL);
INSERT INTO sipro_history.plan_adquisicion_pago SELECT * FROM sipro.plan_adquisicion_pago, (SELECT 1 as version, NULL as linea_base, 1 as actual FROM DUAL);
INSERT INTO sipro_history.prestamo SELECT * FROM sipro.prestamo, (SELECT 1 as version, NULL as linea_base, 1 as actual FROM DUAL);
INSERT INTO sipro_history.prestamo_tipo SELECT * FROM sipro.prestamo_tipo, (SELECT 1 as version, NULL as linea_base, 1 as actual FROM DUAL);
INSERT INTO sipro_history.prestamo_tipo_prestamo SELECT * FROM sipro.prestamo_tipo_prestamo, (SELECT 1 as version, NULL as linea_base, 1 as actual FROM DUAL);
INSERT INTO sipro_history.producto SELECT * FROM sipro.producto, (SELECT 1 as version, NULL as linea_base, 1 as actual FROM DUAL);
INSERT INTO sipro_history.producto_propiedad_valor SELECT * FROM sipro.producto_propiedad_valor, (SELECT 1 as version, NULL as linea_base, 1 as actual FROM DUAL);
INSERT INTO sipro_history.producto_tipo SELECT * FROM sipro.producto_tipo, (SELECT 1 as version, NULL as linea_base, 1 as actual FROM DUAL);
INSERT INTO sipro_history.proyecto SELECT * FROM sipro.proyecto, (SELECT 1 as version, NULL as linea_base, 1 as actual FROM DUAL);
INSERT INTO sipro_history.proyecto_propiedad_valor SELECT * FROM sipro.proyecto_propiedad_valor, (SELECT 1 as version, NULL as linea_base, 1 as actual FROM DUAL);
INSERT INTO sipro_history.proyecto_tipo SELECT * FROM sipro.proyecto_tipo, (SELECT 1 as version, NULL as linea_base, 1 as actual FROM DUAL);
INSERT INTO sipro_history.ptipo_propiedad SELECT * FROM sipro.ptipo_propiedad, (SELECT 1 as version, NULL as linea_base, 1 as actual FROM DUAL);
INSERT INTO sipro_history.riesgo SELECT * FROM sipro.riesgo, (SELECT 1 as version, NULL as linea_base, 1 as actual FROM DUAL);
INSERT INTO sipro_history.riesgo_propiedad_valor SELECT * FROM sipro.riesgo_propiedad_valor, (SELECT 1 as version, NULL as linea_base, 1 as actual FROM DUAL);
INSERT INTO sipro_history.riesgo_tipo SELECT * FROM sipro.riesgo_tipo, (SELECT 1 as version, NULL as linea_base, 1 as actual FROM DUAL);
INSERT INTO sipro_history.subcomponente SELECT * FROM sipro.subcomponente, (SELECT 1 as version, NULL as linea_base, 1 as actual FROM DUAL);
INSERT INTO sipro_history.subcomponente_propiedad_valor SELECT * FROM sipro.subcomponente_propiedad_valor, (SELECT 1 as version, NULL as linea_base, 1 as actual FROM DUAL);
INSERT INTO sipro_history.subcomponente_tipo SELECT * FROM sipro.subcomponente_tipo, (SELECT 1 as version, NULL as linea_base, 1 as actual FROM DUAL);
INSERT INTO sipro_history.subproducto SELECT * FROM sipro.subproducto, (SELECT 1 as version, NULL as linea_base, 1 as actual FROM DUAL);
INSERT INTO sipro_history.subproducto_propiedad_valor SELECT * FROM sipro.subproducto_propiedad_valor, (SELECT 1 as version, NULL as linea_base, 1 as actual FROM DUAL);
INSERT INTO sipro_history.subproducto_tipo SELECT * FROM sipro.subproducto_tipo, (SELECT 1 as version, NULL as linea_base, 1 as actual FROM DUAL);
INSERT INTO sipro_history.tipo_adquisicion SELECT * FROM sipro.tipo_adquisicion, (SELECT 1 as version, NULL as linea_base, 1 as actual FROM DUAL);
INSERT INTO sipro_history.unidad_medida SELECT * FROM sipro.unidad_medida, (SELECT 1 as version, NULL as linea_base, 1 as actual FROM DUAL);
INSERT INTO sipro_history.objeto_riesgo SELECT * FROM sipro.objeto_riesgo, (SELECT 1 as version, NULL as linea_base, 1 as actual FROM DUAL);
INSERT INTO sipro_history.pago_planificado SELECT * FROM sipro.pago_planificado, (SELECT 1 as version, NULL as linea_base, 1 as actual FROM DUAL);
COMMIT;