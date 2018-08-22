
namespace SiproModelCore.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the FORMULARIO_ITEM table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("FORMULARIO_ITEM")]
	public partial class FormularioItem
	{
		[Key]
	    public virtual Int32 id { get; set; }
	    public virtual string texto { get; set; }
	    [ForeignKey("Formulario")]
        public virtual Int32 formularioid { get; set; }
	    public virtual Int32 orden { get; set; }
	    [Column("USUARIO_CREO")]
	    public virtual string usuarioCreo { get; set; }
	    [Column("USUARIO_ACTUALIZACION")]
	    public virtual Int32? usuarioActualizacion { get; set; }
	    [Column("FECHA_CREACION")]
	    public virtual DateTime fechaCreacion { get; set; }
	    [Column("FECHA_ACTUALIZACION")]
	    public virtual DateTime? fechaActualizacion { get; set; }
	    public virtual Int32 estado { get; set; }
	    [Column("FORMULARIO_ITEM_TIPOID")]
	    [ForeignKey("FormularioItemTipo")]
        public virtual Int32 formularioItemTipoid { get; set; }
		public virtual FormularioItemTipo formularioItemTipos { get; set; }
		public virtual Formulario formularios { get; set; }
		public virtual IEnumerable<FormularioItem> formularioitems { get; set; }
	}
}
