
namespace SiproModelCore.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the FORMULARIO_ITEM_VALOR table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("FORMULARIO_ITEM_VALOR")]
	public partial class FormularioItemValor
	{
		[Key]
	    [Column("FORMULARIO_ITEMID")]
	    [ForeignKey("FormularioItem")]
        public virtual Int32 formularioItemid { get; set; }
		[Key]
	    [Column("OBJETO_FORMULARIOFORMULARIOID")]
	    [ForeignKey("ObjetoFormulario")]
        public virtual Int32 objetoFormularioformularioid { get; set; }
		[Key]
	    [Column("OBJETO_FORMULARIOOBJETO_TIPOID")]
	    [ForeignKey("ObjetoFormulario")]
        public virtual Int32 objetoFormularioobjetoTipoid { get; set; }
	    [Column("VALOR_ENTERO")]
	    public virtual Int32 valorEntero { get; set; }
	    [Column("VALOR_STRING")]
	    public virtual string valorString { get; set; }
	    [Column("VALOR_TIEMPO")]
	    public virtual DateTime? valorTiempo { get; set; }
	    [Column("VALOR_DECIMAL")]
	    public virtual decimal? valorDecimal { get; set; }
	    public virtual Int32 proyectoid { get; set; }
	    public virtual Int32 componenteid { get; set; }
	    public virtual Int32 productoid { get; set; }
	    [Column("USUARIO_CREO")]
	    public virtual string usuarioCreo { get; set; }
	    [Column("USUARIO_ACTUALIZO")]
	    public virtual string usuarioActualizo { get; set; }
	    [Column("FECHA_CREACION")]
	    public virtual DateTime fechaCreacion { get; set; }
	    [Column("FECHA_ACTUALIZACION")]
	    public virtual DateTime? fechaActualizacion { get; set; }
		[Key]
	    [Column("OBJETO_FORMULARIOOBJETO_ID")]
	    [ForeignKey("ObjetoFormulario")]
        public virtual Int32 objetoFormularioobjetoId { get; set; }
		public virtual FormularioItem formularioItems { get; set; }
		public virtual ObjetoFormulario objetoFormularios { get; set; }
		public virtual IEnumerable<FormularioItemValor> formularioitemvalors { get; set; }
	}
}
