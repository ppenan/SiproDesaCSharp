
namespace SiproModelCore.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the SCTIPO_PROPIEDAD table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("SCTIPO_PROPIEDAD")]
	public partial class SctipoPropiedad
	{
		[Key]
	    [Column("SUBCOMPONENTE_TIPOID")]
	    [ForeignKey("Subcomponente")]
        public virtual Int32 subcomponenteTipoid { get; set; }
		[Key]
	    [Column("SUBCOMPONENTE_PROPIEDADID")]
	    [ForeignKey("SubcomponentePropiedad")]
        public virtual Int32 subcomponentePropiedadid { get; set; }
	    [Column("USUARIO_CREO")]
	    public virtual string usuarioCreo { get; set; }
	    [Column("USUARIO_ACTUALIZO")]
	    public virtual string usuarioActualizo { get; set; }
	    [Column("FECHA_CREACION")]
	    public virtual DateTime fechaCreacion { get; set; }
	    [Column("FECHA_ACTUALIZACION")]
	    public virtual DateTime? fechaActualizacion { get; set; }
		public virtual Subcomponente subcomponentes { get; set; }
		public virtual SubcomponentePropiedad subcomponentePropiedads { get; set; }
		public virtual IEnumerable<SctipoPropiedad> sctipopropiedads { get; set; }
	}
}
