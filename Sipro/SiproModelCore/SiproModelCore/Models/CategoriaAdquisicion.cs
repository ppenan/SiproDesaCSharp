
namespace SiproModelCore.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the CATEGORIA_ADQUISICION table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("CATEGORIA_ADQUISICION")]
	public partial class CategoriaAdquisicion
	{
		[Key]
	    public virtual Int64 id { get; set; }
	    public virtual string nombre { get; set; }
	    public virtual string descripcion { get; set; }
	    [Column("USUARIO_CREO")]
	    public virtual string usuarioCreo { get; set; }
	    [Column("USUARIO_ACTUALIZO")]
	    public virtual string usuarioActualizo { get; set; }
	    [Column("FECHA_CREACION")]
	    public virtual DateTime fechaCreacion { get; set; }
	    [Column("FECHA_ACTUALIZACION")]
	    public virtual DateTime? fechaActualizacion { get; set; }
	    public virtual Int32 estado { get; set; }
	}
}
