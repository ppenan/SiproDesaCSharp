
namespace SiproModelAnalyticCore.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Collections.Generic;

    /// <summary>
    /// A class which represents the MV_ARBOL table.
	/// Generated by SIPRO TEAM. April 2018. 
    /// </summary>
	[Table("MV_ARBOL")]
	public partial class MvArbol
	{
	    public virtual Int32 prestamo { get; set; }
	    public virtual Int32 componente { get; set; }
	    public virtual Int32 producto { get; set; }
	    public virtual Int32 subproducto { get; set; }
	    public virtual Int32 nivel { get; set; }
	    public virtual Int32 actividad { get; set; }
	    public virtual Int32 treelevel { get; set; }
	    public virtual Int32 treepath { get; set; }
	    [Column("FECHA_INICIO")]
	    public virtual Int32 fechaInicio { get; set; }
	}
}
