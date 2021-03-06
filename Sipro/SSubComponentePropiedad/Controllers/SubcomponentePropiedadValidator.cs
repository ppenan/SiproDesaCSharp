using System;
using FluentValidation;
using Newtonsoft.Json.Linq;
using Utilities;

namespace SSubComponentePropiedad.Controllers
{
    /// <summary>
    /// A class which represents the SubcomponentePropiedadValidator.
	/// Generated by SIPRO TEAM. May 2018. 
    /// </summary>

	public class SubcomponentePropiedadValidator : AbstractValidator<JObject>
	{
		public SubcomponentePropiedadValidator() 
		{
			//RuleFor(subcomponente_propiedad => subcomponente_propiedad["id"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(subcomponente_propiedad => { return GenericValidatorType.ValidateType(subcomponente_propiedad, typeof(Int32)); }).WithMessage("Error when trying to parse the property 'id' to type " + typeof(Int32));
			RuleFor(subcomponente_propiedad => subcomponente_propiedad["nombre"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(subcomponente_propiedad => { return GenericValidatorType.ValidateType(subcomponente_propiedad, typeof(String)); }).WithMessage("Error when trying to parse the property 'nombre' to type " + typeof(String)).MaximumLength(1000);
			RuleFor(subcomponente_propiedad => subcomponente_propiedad["descripcion"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).Must(subcomponente_propiedad => { return GenericValidatorType.ValidateType(subcomponente_propiedad, typeof(String)); }).WithMessage("Error when trying to parse the property 'descripcion' to type " + typeof(String)).MaximumLength(2000);
			//RuleFor(subcomponente_propiedad => subcomponente_propiedad["usuarioCreo"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(subcomponente_propiedad => { return GenericValidatorType.ValidateType(subcomponente_propiedad, typeof(String)); }).WithMessage("Error when trying to parse the property 'usuarioCreo' to type " + typeof(String)).MaximumLength(30);
			//RuleFor(subcomponente_propiedad => subcomponente_propiedad["usuarioActualizo"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).Must(subcomponente_propiedad => { return GenericValidatorType.ValidateType(subcomponente_propiedad, typeof(String)); }).WithMessage("Error when trying to parse the property 'usuarioActualizo' to type " + typeof(String)).MaximumLength(30);
			//RuleFor(subcomponente_propiedad => subcomponente_propiedad["fechaCreacion"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(subcomponente_propiedad => { return GenericValidatorType.ValidateType(subcomponente_propiedad, typeof(DateTime)); }).WithMessage("Error when trying to parse the property 'fechaCreacion' to type " + typeof(DateTime));
			//RuleFor(subcomponente_propiedad => subcomponente_propiedad["fechaActualizacion"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).Must(subcomponente_propiedad => { return GenericValidatorType.ValidateType(subcomponente_propiedad, typeof(DateTime)); }).WithMessage("Error when trying to parse the property 'fechaActualizacion' to type " + typeof(DateTime));
			RuleFor(subcomponente_propiedad => subcomponente_propiedad["datoTipoid"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(subcomponente_propiedad => { return GenericValidatorType.ValidateType(subcomponente_propiedad, typeof(Int32)); }).WithMessage("Error when trying to parse the property 'datoTipoid' to type " + typeof(Int32));
			//RuleFor(subcomponente_propiedad => subcomponente_propiedad["estado"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(subcomponente_propiedad => { return GenericValidatorType.ValidateType(subcomponente_propiedad, typeof(Int32)); }).WithMessage("Error when trying to parse the property 'estado' to type " + typeof(Int32));
			
		}
	}
}
