using System;
using FluentValidation;
using Newtonsoft.Json.Linq;
using Utilities;

namespace SCategoriaAdquisicion.Controllers
{
    /// <summary>
    /// A class which represents the CategoriaAdquisicionValidator.
	/// Generated by SIPRO TEAM. May 2018. 
    /// </summary>

	public class CategoriaAdquisicionValidator : AbstractValidator<JObject>
	{
		public CategoriaAdquisicionValidator() 
		{
			//RuleFor(categoria_adquisicion => categoria_adquisicion["id"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(categoria_adquisicion => { return GenericValidatorType.ValidateType(categoria_adquisicion, typeof(Int64)); }).WithMessage("Error when trying to parse the property 'id' to type " + typeof(Int64));
			RuleFor(categoria_adquisicion => categoria_adquisicion["nombre"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(categoria_adquisicion => { return GenericValidatorType.ValidateType(categoria_adquisicion, typeof(String)); }).WithMessage("Error when trying to parse the property 'nombre' to type " + typeof(String)).MaximumLength(45);
			RuleFor(categoria_adquisicion => categoria_adquisicion["descripcion"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).Must(categoria_adquisicion => { return GenericValidatorType.ValidateType(categoria_adquisicion, typeof(String)); }).WithMessage("Error when trying to parse the property 'descripcion' to type " + typeof(String)).MaximumLength(100);
			//RuleFor(categoria_adquisicion => categoria_adquisicion["usuarioCreo"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(categoria_adquisicion => { return GenericValidatorType.ValidateType(categoria_adquisicion, typeof(String)); }).WithMessage("Error when trying to parse the property 'usuarioCreo' to type " + typeof(String)).MaximumLength(30);
			//RuleFor(categoria_adquisicion => categoria_adquisicion["usuarioActualizo"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).Must(categoria_adquisicion => { return GenericValidatorType.ValidateType(categoria_adquisicion, typeof(String)); }).WithMessage("Error when trying to parse the property 'usuarioActualizo' to type " + typeof(String)).MaximumLength(30);
			//RuleFor(categoria_adquisicion => categoria_adquisicion["fechaCreacion"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(categoria_adquisicion => { return GenericValidatorType.ValidateType(categoria_adquisicion, typeof(DateTime)); }).WithMessage("Error when trying to parse the property 'fechaCreacion' to type " + typeof(DateTime));
			//RuleFor(categoria_adquisicion => categoria_adquisicion["fechaActualizacion"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).Must(categoria_adquisicion => { return GenericValidatorType.ValidateType(categoria_adquisicion, typeof(DateTime)); }).WithMessage("Error when trying to parse the property 'fechaActualizacion' to type " + typeof(DateTime));
			//RuleFor(categoria_adquisicion => categoria_adquisicion["estado"].ToString()).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(categoria_adquisicion => { return GenericValidatorType.ValidateType(categoria_adquisicion, typeof(Int32)); }).WithMessage("Error when trying to parse the property 'estado' to type " + typeof(Int32));
			
		}
	}
}