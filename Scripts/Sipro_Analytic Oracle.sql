DROP TABLE dtm_avance_fisfinan_cmp CASCADE CONSTRAINTS;
DROP TABLE dtm_avance_fisfinan_det_dti CASCADE CONSTRAINTS;
DROP TABLE dtm_avance_fisfinan_dti CASCADE CONSTRAINTS;
DROP TABLE dtm_avance_fisfinan_enp CASCADE CONSTRAINTS;
DROP TABLE mv_arbol CASCADE CONSTRAINTS;
DROP TABLE mv_ejecucion_presupuestaria CASCADE CONSTRAINTS;
DROP TABLE mv_ep_ejec_asig_vige CASCADE CONSTRAINTS;
DROP TABLE mv_ep_estructura CASCADE CONSTRAINTS;
DROP TABLE mv_ep_metas CASCADE CONSTRAINTS;
DROP TABLE mv_ep_prestamo CASCADE CONSTRAINTS;
DROP TABLE mv_gc_adquisiciones CASCADE CONSTRAINTS;
DROP TABLE mv_pr_estructura CASCADE CONSTRAINTS;

DROP SEQUENCE seq_mv_ep_metas;
DROP SEQUENCE seq_mv_pr_estructura;

CREATE SEQUENCE seq_mv_ep_metas START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE seq_mv_pr_estructura START WITH 1 INCREMENT BY 1;

CREATE TABLE dtm_avance_fisfinan_cmp (codigo_presupuestario varchar2(45) DEFAULT NULL, numero_componente number(10) DEFAULT NULL, nombre_componente varchar2(1000) DEFAULT NULL, moneda_componente varchar2(45) DEFAULT NULL, monto_componente decimal(15,2) DEFAULT NULL);
CREATE TABLE dtm_avance_fisfinan_det_dti (ejercicio_fiscal decimal(15,0) NOT NULL, mes_desembolso varchar2(45)  DEFAULT NULL, codigo_presupuestario varchar2(45)  DEFAULT NULL, entidad_sicoin number(10) DEFAULT NULL, unidad_ejecutora_sicoin number(10) DEFAULT NULL, moneda_desembolso varchar2(45)  DEFAULT NULL, desembolsos_mes_moneda decimal(15,2) DEFAULT NULL, tc_mon_usd decimal(15,2) DEFAULT NULL, desembolsos_mes_usd decimal(15,2) DEFAULT NULL, tc_usd_gtq decimal(15,2) DEFAULT NULL, desembolsos_mes_gtq decimal(15,2) DEFAULT NULL);
CREATE TABLE dtm_avance_fisfinan_dti (fecha_corte timestamp(0) DEFAULT NULL, no_prestamo varchar2(45)  DEFAULT NULL, codigo_presupuestario varchar2(45)  DEFAULT NULL, nombre_programa varchar2(500)  DEFAULT NULL, codigo_organismo_finan number(10) DEFAULT NULL, siglas_organismo_finan varchar2(45)  DEFAULT NULL, nombre_organismo_finan varchar2(200)  DEFAULT NULL, moneda_prestamo varchar2(100)  DEFAULT NULL, monto_contratado decimal(15,2) DEFAULT NULL, monto_contratado_usd decimal(15,2) DEFAULT NULL, monto_contratado_gtq decimal(15,2) DEFAULT NULL, desembolsos decimal(15,2) DEFAULT NULL, desembolsos_usd decimal(15,2) DEFAULT NULL, desembolsos_gtq decimal(15,2) DEFAULT NULL, fecha_decreto timestamp(0) DEFAULT NULL, fecha_suscripcion timestamp(0) DEFAULT NULL, fecha_vigencia timestamp(0) DEFAULT NULL, por_desembolsar decimal(15,2) DEFAULT NULL, por_desembolsar_usd decimal(15,2) DEFAULT NULL, por_desembolsar_gtq decimal(15,2) DEFAULT NULL, estado_prestamo varchar2(45)  DEFAULT NULL, objetivo varchar2(4000)  DEFAULT NULL);
CREATE TABLE dtm_avance_fisfinan_enp (codigo_presupuestario varchar2(45) DEFAULT NULL, ejercicio_fiscal number(4) DEFAULT NULL, entidad_presupuestaria number(10) DEFAULT NULL, unidad_ejecutora number(10) DEFAULT NULL);
CREATE TABLE mv_arbol (prestamo NUMBER(4) NOT NULL, componente NUMBER(4) NOT NULL, producto NUMBER(4) NOT NULL, subproducto NUMBER(4) NOT NULL, nivel NUMBER(4) NOT NULL, actividad NUMBER(4) NOT NULL, treelevel NUMBER(4) NOT NULL, treepath NUMBER(4) NOT NULL, fecha_inicio NUMBER(4) NOT NULL);
CREATE TABLE mv_ejecucion_presupuestaria (ejercicio number(4) DEFAULT NULL, mes number(2) DEFAULT NULL, entidad number(10) DEFAULT NULL, unidad_ejecutora number(4) DEFAULT NULL, programa number(4) DEFAULT NULL, subprograma number(4) DEFAULT NULL, proyecto number(4) DEFAULT NULL, actividad number(4) DEFAULT NULL, obra number(4) DEFAULT NULL, renglon number(4) DEFAULT NULL, renglon_nombre varchar2(200)  DEFAULT NULL, fuente number(2) DEFAULT NULL, grupo number(4) DEFAULT NULL, grupo_nombre varchar2(200)  DEFAULT NULL, subgrupo number(4) DEFAULT NULL, subgrupo_nombre varchar2(200)  DEFAULT NULL, ejecucion_presupuestaria decimal(15,2) DEFAULT NULL, organismo number(4) DEFAULT NULL, correlativo number(6) DEFAULT NULL, geografico number(4) DEFAULT NULL);
CREATE TABLE mv_ep_ejec_asig_vige (ejercicio number(4) NOT NULL, mes number(2) DEFAULT NULL, entidad number(10) DEFAULT NULL, unidad_ejecutra number(4) DEFAULT NULL, programa number(4) DEFAULT NULL, subprograma number(4) DEFAULT NULL, proyecto number(4) DEFAULT NULL, actividad number(4) DEFAULT NULL, obra number(4) DEFAULT NULL, renglon number(4) DEFAULT NULL, geografico number(4) DEFAULT NULL, correlativo number(6) DEFAULT NULL, organismo number(4) DEFAULT NULL, fuente number(4) DEFAULT NULL, ejecutado decimal(15,2) DEFAULT NULL, asignado decimal(15,2) DEFAULT NULL, modificaciones decimal(15,2) DEFAULT NULL);
CREATE TABLE mv_ep_estructura (ejercicio number(4) DEFAULT NULL, fuente number(2) DEFAULT NULL, organismo number(4) DEFAULT NULL, correlativo number(6) DEFAULT NULL, programa number(4) DEFAULT NULL, subprograma number(4) DEFAULT NULL, proyecto number(4) DEFAULT NULL, actividad number(4) DEFAULT NULL, obra number(4) DEFAULT NULL, renglon number(4) DEFAULT NULL, geografico number(4) DEFAULT NULL, enero decimal(38,2) DEFAULT NULL, febrero decimal(38,2) DEFAULT NULL, marzo decimal(38,2) DEFAULT NULL, abril decimal(38,2) DEFAULT NULL, mayo decimal(38,2) DEFAULT NULL, junio decimal(38,2) DEFAULT NULL, julio decimal(38,2) DEFAULT NULL, agosto decimal(38,2) DEFAULT NULL, septiembre decimal(38,2) DEFAULT NULL, octubre decimal(38,2) DEFAULT NULL, noviembre decimal(38,2) DEFAULT NULL, diciembre decimal(38,2) DEFAULT NULL);
CREATE TABLE mv_ep_metas (id number(10) NOT NULL, ejercicio number(4) NOT NULL, objeto_id number(10) NOT NULL, objeto_tipo number(11) NOT NULL, meta_unidad_medidaid number(10) NOT NULL, eneroP decimal(15,2) DEFAULT 0.00, eneroR decimal(15,2) DEFAULT 0.00, febreroP decimal(15,2) DEFAULT 0.00, febreroR decimal(15,2) DEFAULT 0.00, marzoP decimal(15,2) DEFAULT 0.00, marzoR decimal(15,2) DEFAULT 0.00, abrilP decimal(15,2) DEFAULT 0.00, abrilR decimal(15,2) DEFAULT 0.00, mayoP decimal(15,2) DEFAULT 0.00, mayoR decimal(15,2) DEFAULT 0.00, junioP decimal(15,2) DEFAULT 0.00, junioR decimal(15,2) DEFAULT 0.00, julioP decimal(15,2) DEFAULT 0.00, julioR decimal(15,2) DEFAULT 0.00, agostoP decimal(15,2) DEFAULT 0.00, agostoR decimal(15,2) DEFAULT 0.00, septiembreP decimal(15,2) DEFAULT 0.00, septiembreR decimal(15,2) DEFAULT 0.00, octubreP decimal(15,2) DEFAULT 0.00, octubreR decimal(15,2) DEFAULT 0.00, noviembreP decimal(15,2) DEFAULT 0.00, noviembreR decimal(15,2) DEFAULT 0.00, diciembreP decimal(15,2) DEFAULT 0.00, diciembreR decimal(15,2) DEFAULT 0.00, lineaBase decimal(15,2) DEFAULT 0.00, metaFinal decimal(15,2) DEFAULT 0.00, PRIMARY KEY (id));
CREATE TABLE mv_ep_prestamo (ejercicio number(4) DEFAULT NULL, fuente number(2) DEFAULT NULL, organismo number(4) DEFAULT NULL, correlativo number(6) DEFAULT NULL, enero decimal(38,2) DEFAULT NULL, febrero decimal(38,2) DEFAULT NULL, marzo decimal(38,2) DEFAULT NULL, abril decimal(38,2) DEFAULT NULL, mayo decimal(38,2) DEFAULT NULL, junio decimal(38,2) DEFAULT NULL, julio decimal(38,2) DEFAULT NULL, agosto decimal(38,2) DEFAULT NULL, septiembre decimal(38,2) DEFAULT NULL, octubre decimal(38,2) DEFAULT NULL, noviembre decimal(38,2) DEFAULT NULL, diciembre decimal(38,2) DEFAULT NULL);
CREATE TABLE mv_gc_adquisiciones (nog number(10) DEFAULT NULL, numero_contrato varchar2(100)  DEFAULT NULL, monto_contrato decimal(15,2) DEFAULT NULL, preparacion_documentos timestamp(0) DEFAULT NULL, lanzamiento_evento timestamp(0) DEFAULT NULL, recepcion_ofertas timestamp(0) DEFAULT NULL, adjudicacion timestamp(0) DEFAULT NULL, firma_contrato timestamp(0) DEFAULT NULL);
CREATE TABLE mv_pr_estructura (id number(11) NOT NULL, objeto_id number(10) DEFAULT NULL, objeto_tipo number(2) DEFAULT NULL, nombre varchar(300) DEFAULT NULL, fuente number(4) DEFAULT NULL, organismo number(4) DEFAULT NULL, correlativo number(4) DEFAULT NULL, programa number(4) DEFAULT NULL, subprograma number(4) DEFAULT NULL, proyecto number(4) DEFAULT NULL, actividad number(4) DEFAULT NULL, obra number(4) DEFAULT NULL, objeto_id_padre number(10) DEFAULT NULL, objeto_tipo_padre number(4) DEFAULT NULL, PRIMARY KEY (id));