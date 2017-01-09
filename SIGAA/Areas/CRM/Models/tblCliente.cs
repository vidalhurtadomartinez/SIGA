using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.CRM.Models
{
    public class tblCliente
    {
        [Key]
        public int lCliente_id { get; set; }

        [Display(Name = "Fecha de Registro")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public DateTime? dtFechaRegistro_dt { get; set; }

        [Display(Name = "Nombres", Description = "Nombres", Prompt = "Nombres")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public string Nombres { get; set; }

        [Display(Name = "Apellido Paterno", Description = "Apellido Paterno", Prompt = "Apellido Paterno")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public string ApellidoPaterno { get; set; }

        [Display(Name = "Apellido Materno", Description = "Apellido Materno", Prompt = "Apellido Materno")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public string ApellidoMaterno { get; set; }

        [Display(Name = "Recomendado Por(I)", Description = "Recomendado Por", Prompt = "Recomendado Por")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public string RecomendadoPorInterno { get; set; }

        [Display(Name = "Recomendado Por(E)", Description = "Recomendado Por", Prompt = "Recomendado Por")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public string RecomendadoPorExterno { get; set; }

        [Display(Name = "Asesor", Description = "Asesor", Prompt = "Asesor")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int? lAsesor_id { get; set; }

        [Display(Name = "Relacion con el Postulante", Description = "Relacion con el Postulante", Prompt = "Relacion con el Postulante")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int? lRelacionPostulante_id { get; set; }

        [Display(Name = "Forma de Contacto", Description = "Forma de Contacto", Prompt = "Forma de Contacto")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int lFormaContacto_id { get; set; }

        //[Display(Name = "Categoria de Colegio", Description = "Categoria de Colegio", Prompt = "Categoria de Colegio")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        //public int lCategoriaColegio_id { get; set; }

        [Display(Name = "Categoria de Colegio", Description = "Categoria de Colegio", Prompt = "Categoria de Colegio")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int lTipoColegio_id { get; set; }

        [Display(Name = "Medio de Informacion", Description = "Medio de Informacion", Prompt = "Medio de Informacion")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int lMedioInformacion_id { get; set; }

        [Display(Name = "Datos Adicionales", Description = "Datos Adicionales", Prompt = "Datos Adicionales")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [DataType(DataType.MultilineText)]
        public string InformacionAdicional { get; set; }

        [Display(Name = "Direccion", Description = "Direccion", Prompt = "Direccion")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [DataType(DataType.MultilineText)]
        public string Direccion { get; set; }

        [Display(Name = "Pais de Nacimiento", Description = "Pais de Nacimiento", Prompt = "Pais de Nacimiento")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int lPaisNacimiento_id { get; set; }

        [Display(Name = "Nacionalidad", Description = "Nacionalidad", Prompt = "Nacionalidad")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int lNacionalidad_id { get; set; }

        [Display(Name = "Ciudad", Description = "Ciudad", Prompt = "Ciudad")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int lCiudad_id { get; set; }

        [Display(Name = "Telefono", Description = "Telefono", Prompt = "Telefono")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public string Telefono { get; set; }

        [Display(Name = "Celular", Description = "Celular", Prompt = "Celular")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public string Celular { get; set; }

        [Display(Name = "Referencias", Description = "Referencias", Prompt = "Referencias")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public string Referencia { get; set; }

        [Display(Name = "Correo Electonico Principal", Description = "Correo Electonico Principal", Prompt = "Correo Electonico Principal")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [DataType(DataType.EmailAddress)]
        public string CorreoPrincipal { get; set; }

        [Display(Name = "Correo Electonico Secundario", Description = "Correo Electonico Secundario", Prompt = "Correo Electonico Principal")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [DataType(DataType.EmailAddress)]
        public string CorreoSecundario { get; set; }

        [Display(Name = "Pagina Web", Description = "Pagina Web", Prompt = "Pagina Web")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [DataType(DataType.Url)]
        public string PaginaWeb { get; set; }

        [Display(Name = "Fecha de Nacimiento")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public DateTime? dtFechaNacimiento_dt { get; set; }

        [Display(Name = "Tipo de Documento", Description = "Tipo de Documento", Prompt = "Tipo de Documento")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int? lTipoDocumento_id { get; set; }

        [Display(Name = "Numero de Documento", Description = "Numero de Documento", Prompt = "Numero de Documento")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public string NumeroDocumento { get; set; }

        [Display(Name = "Expedido En", Description = "Expedido En", Prompt = "Expedido En")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int? lLugarExpedido_id { get; set; }

        [Display(Name = "Rubro", Description = "Rubro", Prompt = "Rubro")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int? lRubro_id { get; set; }

        [Display(Name = "Actividad", Description = "Actividad", Prompt = "Actividad")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int? lActividad_id { get; set; }

        [Display(Name = "Ocupacion / Cargo", Description = "Cargo", Prompt = "Cargo")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int? lCargo_id { get; set; }

        [Display(Name = "Institucion donde Trabaja", Description = "Institucion", Prompt = "Institucion")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int? lInstitucion_id { get; set; }

        [Display(Name = "Horario de Trabajo", Description = "Horario de Trabajo", Prompt = "Horario de Trabajo")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public string HorarioTrabajo { get; set; }

        [Display(Name = "Desde que Fecha Trabaja")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public DateTime? dtFechaInicioTrabajo_dt { get; set; }

        [Display(Name = "Direccion Institucion", Description = "Direccion Institucion", Prompt = "Direccion Institucion")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [DataType(DataType.MultilineText)]
        public string DireccionInstitucion { get; set; }

        [Display(Name = "Correo Electronico Institucion", Description = "Correo Electronico Institucion", Prompt = "Correo Electronico Institucion")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [DataType(DataType.EmailAddress)]
        public string CorreoInstitucion { get; set; }

        [Display(Name = "Pagina Web", Description = "Pagina Web", Prompt = "Pagina Web")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [DataType(DataType.Url)]
        public string PaginaWebInstitucion { get; set; }

        [Display(Name = "Telefonos", Description = "Telefonos", Prompt = "Telefonos")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public string TelefonoInstitucion { get; set; }

        [Display(Name = "Contacto Principal", Description = "Contacto Principal", Prompt = "Contacto Principal")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public string ContactoPrincipal { get; set; }

        [Display(Name = "Datos Adicionales", Description = "Datos Adicionales", Prompt = "Datos Adicionales")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [DataType(DataType.MultilineText)]
        public string DatosAdicionalesInstitucion { get; set; }

        [Display(Name = "Colegio", Description = "Colegio", Prompt = "Colegio")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int? lColegio_id { get; set; }

        [Display(Name = "Año de Egreso", Description = "Año de Egreso", Prompt = "Año de Egreso")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public string AnioEgresoColegio { get; set; }

        [Display(Name = "Datos Adicionales", Description = "Datos Adicionales", Prompt = "Datos Adicionales")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [DataType(DataType.MultilineText)]
        public string DatosAdicionalesColegio { get; set; }

        [Display(Name = "Universidad", Description = "Universidad", Prompt = "Universidad")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int? lUniversidad_id { get; set; }

        [Display(Name = "Carrera", Description = "Carrera", Prompt = "Carrera")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int? lCarrera_id { get; set; }

        [Display(Name = "Materias Vencidas", Description = "Materias Vencidas", Prompt = "Materias Vencidas")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public string MateriasVencidas { get; set; }

        [Display(Name = "Año de Egreso", Description = "Año de Egreso", Prompt = "Año de Egreso")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public string AnioEgresoUniversidad { get; set; }

        [Display(Name = "Datos Adicionales", Description = "Datos Adicionales", Prompt = "Datos Adicionales")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [DataType(DataType.MultilineText)]
        public string DatosAdicionalesUniversidad { get; set; }

        [NotMapped]
        public virtual string NombreCompleto { get { return string.Format("{0} {1}, {2}", ApellidoPaterno, ApellidoMaterno, Nombres); } }




        [ForeignKey(name: "lRelacionPostulante_id")]
        public virtual tblRelacionPostulante tblRelacionPostulante { get; set; }

        [ForeignKey(name: "lFormaContacto_id")]
        public virtual tblFormaContacto tblFormaContacto { get; set; }

        //[ForeignKey(name: "lCategoriaColegio_id")]
        //public virtual tblCategoriaColegio tblCategoriaColegio { get; set; }

        [ForeignKey(name: "lTipoColegio_id")]
        public virtual tblTipoColegio tblTipoColegio { get; set; }

        [ForeignKey(name: "lMedioInformacion_id")]
        public virtual tblMedioInformacion tblMedioInformacion { get; set; }

        [ForeignKey(name: "lPaisNacimiento_id")]
        public virtual tblPais tblPaisNacimiento { get; set; }

        [ForeignKey(name: "lNacionalidad_id")]
        public virtual tblPais tblPaisNacionalidad { get; set; }

        [ForeignKey(name: "lCiudad_id")]
        public virtual tblCiudad tblCiudad { get; set; }

        [ForeignKey(name: "lTipoDocumento_id")]
        public virtual tblTipoDocumento tblTipoDocumento { get; set; }

        [ForeignKey(name: "lLugarExpedido_id")]
        public virtual tblCiudad tblCiudadExpedido { get; set; }

        [ForeignKey(name: "lRubro_id")]
        public virtual tblRubroActividad tblRubroActividad { get; set; }

        [ForeignKey(name: "lActividad_id")]
        public virtual tblActividad tblActividad { get; set; }

        [ForeignKey(name: "lCargo_id")]
        public virtual tblCargo tblCargo { get; set; }

        [ForeignKey(name: "lAsesor_id")]
        public virtual tblAsesor tblAsesor { get; set; }

        [ForeignKey(name: "lInstitucion_id")]
        public virtual tblinstitucion tblinstitucion { get; set; }

        [ForeignKey(name: "lColegio_id")]
        public virtual tblColegio tblColegio { get; set; }

        [ForeignKey(name: "lUniversidad_id")]
        public virtual tblUniversidad tblUniversidad { get; set; }

        [ForeignKey(name: "lCarrera_id")]
        public virtual tblCarrera tblCarrera { get; set; }


        public virtual ICollection<tblEvento> tblEvento { get; set; }
    }
}