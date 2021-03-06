using System;
using FluentValidation;
using Newtonsoft.Json.Linq;
using Utilities;

namespace SProyectoTipo.Validators
{
    /// <summary>
    /// A class which represents the ProyectoTipoValidator.
	/// Generated by SIPRO TEAM. May 2018. 
    /// </summary>

	public class ProyectoTipoValidator : AbstractValidator<JObject>
	{
		public ProyectoTipoValidator() 
		{
			//RuleFor(proyecto_tipo => proyecto_tipo["id"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(proyecto_tipo => { return GenericValidatorType.ValidateType(proyecto_tipo, typeof(Int32)); }).WithMessage("Error when trying to parse the property 'id' to type " + typeof(Int32));
			RuleFor(proyecto_tipo => proyecto_tipo["nombre"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(proyecto_tipo => { return GenericValidatorType.ValidateType(proyecto_tipo, typeof(String)); }).WithMessage("Error when trying to parse the property 'nombre' to type " + typeof(String)).MaximumLength(1000);
			RuleFor(proyecto_tipo => proyecto_tipo["descripcion"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).Must(proyecto_tipo => { return GenericValidatorType.ValidateType(proyecto_tipo, typeof(String)); }).WithMessage("Error when trying to parse the property 'descripcion' to type " + typeof(String)).MaximumLength(2000);
			//RuleFor(proyecto_tipo => proyecto_tipo["usarioCreo"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(proyecto_tipo => { return GenericValidatorType.ValidateType(proyecto_tipo, typeof(String)); }).WithMessage("Error when trying to parse the property 'usarioCreo' to type " + typeof(String)).MaximumLength(30);
			//RuleFor(proyecto_tipo => proyecto_tipo["usuarioActualizo"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).Must(proyecto_tipo => { return GenericValidatorType.ValidateType(proyecto_tipo, typeof(String)); }).WithMessage("Error when trying to parse the property 'usuarioActualizo' to type " + typeof(String)).MaximumLength(30);
			//RuleFor(proyecto_tipo => proyecto_tipo["fechaCreacion"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(proyecto_tipo => { return GenericValidatorType.ValidateType(proyecto_tipo, typeof(DateTime)); }).WithMessage("Error when trying to parse the property 'fechaCreacion' to type " + typeof(DateTime));
			//RuleFor(proyecto_tipo => proyecto_tipo["fechaActualizacion"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).Must(proyecto_tipo => { return GenericValidatorType.ValidateType(proyecto_tipo, typeof(DateTime)); }).WithMessage("Error when trying to parse the property 'fechaActualizacion' to type " + typeof(DateTime));
			//RuleFor(proyecto_tipo => proyecto_tipo["estado"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(proyecto_tipo => { return GenericValidatorType.ValidateType(proyecto_tipo, typeof(Int32)); }).WithMessage("Error when trying to parse the property 'estado' to type " + typeof(Int32));
			
		}
	}
}
