
namespace SiproModelCore.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the OBJETO_RIESGO table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("OBJETO_RIESGO")]
	public partial class ObjetoRiesgo
	{
		[Key]
	    [ForeignKey("Riesgo")]
        public virtual Int32 riesgoid { get; set; }
		[Key]
	    [Column("OBJETO_ID")]
	    public virtual Int64 objetoId { get; set; }
		[Key]
	    [Column("OBJETO_TIPO")]
	    public virtual Int64 objetoTipo { get; set; }
	    [Column("USUARIO_CREO")]
	    public virtual string usuarioCreo { get; set; }
	    [Column("USUARIO_ACTUALIZO")]
	    public virtual string usuarioActualizo { get; set; }
	    [Column("FECHA_CREACION")]
	    public virtual DateTime? fechaCreacion { get; set; }
	    [Column("FECHA_ACTUALIZACION")]
	    public virtual DateTime? fechaActualizacion { get; set; }
		public virtual Riesgo riesgos { get; set; }
		public virtual IEnumerable<ObjetoRiesgo> objetoriesgoes { get; set; }
	}
}
