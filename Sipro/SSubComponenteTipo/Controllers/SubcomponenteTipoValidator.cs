using System;
using FluentValidation;
using Newtonsoft.Json.Linq;
using Utilities;

namespace SSubComponenteTipo.Controllers
{
    /// <summary>
    /// A class which represents the SubcomponenteTipoValidator.
	/// Generated by SIPRO TEAM. May 2018. 
    /// </summary>

	public class SubcomponenteTipoValidator : AbstractValidator<JObject>
	{
		public SubcomponenteTipoValidator() 
		{
			//RuleFor(subcomponente_tipo => subcomponente_tipo["id"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(subcomponente_tipo => { return GenericValidatorType.ValidateType(subcomponente_tipo, typeof(Int32)); }).WithMessage("Error when trying to parse the property 'id' to type " + typeof(Int32));
			RuleFor(subcomponente_tipo => subcomponente_tipo["nombre"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(subcomponente_tipo => { return GenericValidatorType.ValidateType(subcomponente_tipo, typeof(String)); }).WithMessage("Error when trying to parse the property 'nombre' to type " + typeof(String)).MaximumLength(1000);
			RuleFor(subcomponente_tipo => subcomponente_tipo["descripcion"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).Must(subcomponente_tipo => { return GenericValidatorType.ValidateType(subcomponente_tipo, typeof(String)); }).WithMessage("Error when trying to parse the property 'descripcion' to type " + typeof(String)).MaximumLength(2000);
			//RuleFor(subcomponente_tipo => subcomponente_tipo["usuarioCreo"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(subcomponente_tipo => { return GenericValidatorType.ValidateType(subcomponente_tipo, typeof(String)); }).WithMessage("Error when trying to parse the property 'usuarioCreo' to type " + typeof(String)).MaximumLength(30);
			//RuleFor(subcomponente_tipo => subcomponente_tipo["usuarioActualizo"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).Must(subcomponente_tipo => { return GenericValidatorType.ValidateType(subcomponente_tipo, typeof(String)); }).WithMessage("Error when trying to parse the property 'usuarioActualizo' to type " + typeof(String)).MaximumLength(30);
			//RuleFor(subcomponente_tipo => subcomponente_tipo["fechaCreacion"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(subcomponente_tipo => { return GenericValidatorType.ValidateType(subcomponente_tipo, typeof(DateTime)); }).WithMessage("Error when trying to parse the property 'fechaCreacion' to type " + typeof(DateTime));
			//RuleFor(subcomponente_tipo => subcomponente_tipo["fechaActualizacion"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).Must(subcomponente_tipo => { return GenericValidatorType.ValidateType(subcomponente_tipo, typeof(DateTime)); }).WithMessage("Error when trying to parse the property 'fechaActualizacion' to type " + typeof(DateTime));
			//RuleFor(subcomponente_tipo => subcomponente_tipo["estado"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(subcomponente_tipo => { return GenericValidatorType.ValidateType(subcomponente_tipo, typeof(Int32)); }).WithMessage("Error when trying to parse the property 'estado' to type " + typeof(Int32));
			
		}
	}
}
