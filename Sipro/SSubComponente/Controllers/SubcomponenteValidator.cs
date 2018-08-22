using System;
using FluentValidation;
using Newtonsoft.Json.Linq;
using Utilities;

namespace SSubComponente.Controllers
{
    /// <summary>
    /// A class which represents the SubcomponenteValidator.
	/// Generated by SIPRO TEAM. May 2018. 
    /// </summary>

	public class SubcomponenteValidator : AbstractValidator<JObject>
	{
		public SubcomponenteValidator() 
		{
			//RuleFor(subcomponente => subcomponente["id"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(subcomponente => { return GenericValidatorType.ValidateType(subcomponente, typeof(Int32)); }).WithMessage("Error when trying to parse the property 'id' to type " + typeof(Int32));
			RuleFor(subcomponente => subcomponente["nombre"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(subcomponente => { return GenericValidatorType.ValidateType(subcomponente, typeof(String)); }).WithMessage("Error when trying to parse the property 'nombre' to type " + typeof(String)).MaximumLength(1000);
			RuleFor(subcomponente => subcomponente["descripcion"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).Must(subcomponente => { return GenericValidatorType.ValidateType(subcomponente, typeof(String)); }).WithMessage("Error when trying to parse the property 'descripcion' to type " + typeof(String)).MaximumLength(2000);
			RuleFor(subcomponente => subcomponente["componenteid"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(subcomponente => { return GenericValidatorType.ValidateType(subcomponente, typeof(Int32)); }).WithMessage("Error when trying to parse the property 'componenteid' to type " + typeof(Int32));
			RuleFor(subcomponente => subcomponente["subcomponenteTipoid"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(subcomponente => { return GenericValidatorType.ValidateType(subcomponente, typeof(Int32)); }).WithMessage("Error when trying to parse the property 'subcomponenteTipoid' to type " + typeof(Int32));
			//RuleFor(subcomponente => subcomponente["usuarioCreo"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(subcomponente => { return GenericValidatorType.ValidateType(subcomponente, typeof(String)); }).WithMessage("Error when trying to parse the property 'usuarioCreo' to type " + typeof(String)).MaximumLength(30);
			//RuleFor(subcomponente => subcomponente["usuarioActualizo"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).Must(subcomponente => { return GenericValidatorType.ValidateType(subcomponente, typeof(String)); }).WithMessage("Error when trying to parse the property 'usuarioActualizo' to type " + typeof(String)).MaximumLength(30);
			//RuleFor(subcomponente => subcomponente["fechaCreacion"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(subcomponente => { return GenericValidatorType.ValidateType(subcomponente, typeof(DateTime)); }).WithMessage("Error when trying to parse the property 'fechaCreacion' to type " + typeof(DateTime));
			//RuleFor(subcomponente => subcomponente["fechaActualizacion"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).Must(subcomponente => { return GenericValidatorType.ValidateType(subcomponente, typeof(DateTime)); }).WithMessage("Error when trying to parse the property 'fechaActualizacion' to type " + typeof(DateTime));
			//RuleFor(subcomponente => subcomponente["estado"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(subcomponente => { return GenericValidatorType.ValidateType(subcomponente, typeof(Int32)); }).WithMessage("Error when trying to parse the property 'estado' to type " + typeof(Int32));
			RuleFor(subcomponente => subcomponente["ueunidadEjecutora"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).Must(subcomponente => { return GenericValidatorType.ValidateType(subcomponente, typeof(Int32)); }).WithMessage("Error when trying to parse the property 'ueunidadEjecutora' to type " + typeof(Int32));
			RuleFor(subcomponente => subcomponente["snip"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).Must(subcomponente => { return GenericValidatorType.ValidateType(subcomponente, typeof(Int32)); }).WithMessage("Error when trying to parse the property 'snip' to type " + typeof(Int32));
			RuleFor(subcomponente => subcomponente["programa"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).Must(subcomponente => { return GenericValidatorType.ValidateType(subcomponente, typeof(Int32)); }).WithMessage("Error when trying to parse the property 'programa' to type " + typeof(Int32));
			RuleFor(subcomponente => subcomponente["subprograma"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).Must(subcomponente => { return GenericValidatorType.ValidateType(subcomponente, typeof(Int32)); }).WithMessage("Error when trying to parse the property 'subprograma' to type " + typeof(Int32));
			RuleFor(subcomponente => subcomponente["proyecto"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).Must(subcomponente => { return GenericValidatorType.ValidateType(subcomponente, typeof(Int32)); }).WithMessage("Error when trying to parse the property 'proyecto' to type " + typeof(Int32));
			RuleFor(subcomponente => subcomponente["actividad"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).Must(subcomponente => { return GenericValidatorType.ValidateType(subcomponente, typeof(Int32)); }).WithMessage("Error when trying to parse the property 'actividad' to type " + typeof(Int32));
			RuleFor(subcomponente => subcomponente["obra"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).Must(subcomponente => { return GenericValidatorType.ValidateType(subcomponente, typeof(Int32)); }).WithMessage("Error when trying to parse the property 'obra' to type " + typeof(Int32));
			RuleFor(subcomponente => subcomponente["latitud"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).Must(subcomponente => { return GenericValidatorType.ValidateType(subcomponente, typeof(String)); }).WithMessage("Error when trying to parse the property 'latitud' to type " + typeof(String)).MaximumLength(30);
			RuleFor(subcomponente => subcomponente["longitud"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).Must(subcomponente => { return GenericValidatorType.ValidateType(subcomponente, typeof(String)); }).WithMessage("Error when trying to parse the property 'longitud' to type " + typeof(String)).MaximumLength(30);
			RuleFor(subcomponente => subcomponente["costo"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).Must(subcomponente => { return GenericValidatorType.ValidateType(subcomponente, typeof(decimal)); }).WithMessage("Error when trying to parse the property 'costo' to type " + typeof(decimal));
			RuleFor(subcomponente => subcomponente["acumulacionCostoid"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(subcomponente => { return GenericValidatorType.ValidateType(subcomponente, typeof(Int64)); }).WithMessage("Error when trying to parse the property 'acumulacionCostoid' to type " + typeof(Int64));
			RuleFor(subcomponente => subcomponente["renglon"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).Must(subcomponente => { return GenericValidatorType.ValidateType(subcomponente, typeof(Int32)); }).WithMessage("Error when trying to parse the property 'renglon' to type " + typeof(Int32));
			RuleFor(subcomponente => subcomponente["ubicacionGeografica"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).Must(subcomponente => { return GenericValidatorType.ValidateType(subcomponente, typeof(Int32)); }).WithMessage("Error when trying to parse the property 'ubicacionGeografica' to type " + typeof(Int32));
			RuleFor(subcomponente => subcomponente["fechaInicio"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).Must(subcomponente => { return GenericValidatorType.ValidateType(subcomponente, typeof(DateTime)); }).WithMessage("Error when trying to parse the property 'fechaInicio' to type " + typeof(DateTime));
			RuleFor(subcomponente => subcomponente["fechaFin"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).Must(subcomponente => { return GenericValidatorType.ValidateType(subcomponente, typeof(DateTime)); }).WithMessage("Error when trying to parse the property 'fechaFin' to type " + typeof(DateTime));
			RuleFor(subcomponente => subcomponente["duracion"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(subcomponente => { return GenericValidatorType.ValidateType(subcomponente, typeof(Int32)); }).WithMessage("Error when trying to parse the property 'duracion' to type " + typeof(Int32));
			RuleFor(subcomponente => subcomponente["duracionDimension"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).Must(subcomponente => { return GenericValidatorType.ValidateType(subcomponente, typeof(String)); }).WithMessage("Error when trying to parse the property 'duracionDimension' to type " + typeof(String)).MaximumLength(1);
			//RuleFor(subcomponente => subcomponente["orden"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).Must(subcomponente => { return GenericValidatorType.ValidateType(subcomponente, typeof(Int32)); }).WithMessage("Error when trying to parse the property 'orden' to type " + typeof(Int32));
			//RuleFor(subcomponente => subcomponente["treepath"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).Must(subcomponente => { return GenericValidatorType.ValidateType(subcomponente, typeof(String)); }).WithMessage("Error when trying to parse the property 'treepath' to type " + typeof(String)).MaximumLength(1000);
			//RuleFor(subcomponente => subcomponente["nivel"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).Must(subcomponente => { return GenericValidatorType.ValidateType(subcomponente, typeof(Int32)); }).WithMessage("Error when trying to parse the property 'nivel' to type " + typeof(Int32));
			RuleFor(subcomponente => subcomponente["entidad"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).Must(subcomponente => { return GenericValidatorType.ValidateType(subcomponente, typeof(Int32)); }).WithMessage("Error when trying to parse the property 'entidad' to type " + typeof(Int32));
			RuleFor(subcomponente => subcomponente["ejercicio"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).Must(subcomponente => { return GenericValidatorType.ValidateType(subcomponente, typeof(Int32)); }).WithMessage("Error when trying to parse the property 'ejercicio' to type " + typeof(Int32));
			RuleFor(subcomponente => subcomponente["fechaInicioReal"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).Must(subcomponente => { return GenericValidatorType.ValidateType(subcomponente, typeof(DateTime)); }).WithMessage("Error when trying to parse the property 'fechaInicioReal' to type " + typeof(DateTime));
			RuleFor(subcomponente => subcomponente["fechaFinReal"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).Must(subcomponente => { return GenericValidatorType.ValidateType(subcomponente, typeof(DateTime)); }).WithMessage("Error when trying to parse the property 'fechaFinReal' to type " + typeof(DateTime));
			RuleFor(subcomponente => subcomponente["inversionNueva"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(subcomponente => { return GenericValidatorType.ValidateType(subcomponente, typeof(Int32)); }).WithMessage("Error when trying to parse the property 'inversionNueva' to type " + typeof(Int32));
			
		}
	}
}
