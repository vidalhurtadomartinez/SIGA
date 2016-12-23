using Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Spatial;
using System.Linq;

namespace SIGAA.Models
{
    [Table("Usuario")]
    public class Usuario: BaseModel
    {
        public Usuario() {
            this.NombreCompleto = "";
            this.Sexo = sexo.Masculino;
            this.EmailUtepsa = "";
            this.Contrasena = "";
            this.Dierccion = "";
        }

        [Key]
        public int iUsuario_id { get; set; }

        [Required(ErrorMessage = "Debe ingresar valores al campo {0} porque es Obligatorio.")]
        [Display(Name = "Agenda :")]
        public string agd_codigo { get; set; }

        [Required(ErrorMessage = "Debe ingresar valores al campo {0} porque es Obligatorio.")]
        [Display(Name = "Nombre completo :")]
        public string NombreCompleto { get; set; }

        [Required(ErrorMessage = "Debe ingresar valores al campo {0} porque es Obligatorio.")]
        [Display(Name = "Sexo :")]
        public sexo Sexo { get; set; }

        [Required(ErrorMessage = "Debe ingresar valores al campo {0} porque es Obligatorio.")]
        [DataType(DataType.EmailAddress,ErrorMessage ="La dirección de email no es válido")]
        [Display(Name = "Email UTEPSA :")]
        public string EmailUtepsa { set; get;}

        [Required(ErrorMessage = "Debe ingresar valores al campo {0} porque es Obligatorio.")]
        [StringLength(30, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña :")]
        public string Contrasena { get; set; }

        [StringLength(500, ErrorMessage = "El campo {0} debe tener {1} caracteres como máximo.")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Dirección :")]
        public string Dierccion { set; get; }

        [Required(ErrorMessage = "Debe ingresar valores al campo {0} porque es Obligatorio.")]
        [Display(Name = "Rol :")]
        public int iRol_id { get; set; }

        //propiedades de navegacion
        public virtual Rol Rol { get; set; }

        public enum sexo {
            Masculino,
            Femenino
        }

        //public ResponseModel Autenticarse(string emailUtepsa, string contrasena)
        //{
        //    var rm = new ResponseModel();

        //    try
        //    {
        //        using (var ctx = new SeguridadContext())
        //        {
        //            var usuario = ctx.Usuario.Where(x => x.EmailUtepsa == emailUtepsa && x.Contrasena == contrasena).SingleOrDefault();
        //            if (usuario != null)
        //            {
        //                SessionHelper.AddUserToSession(usuario.iUsuario_id.ToString());
        //                rm.SetResponse(true);
        //            }
        //            else
        //            {
        //                rm.SetResponse(false, "Acceso denegado al sistema");
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //    return rm;
        //}

        public bool Autenticarse(string emailUtepsa, string contrasena)
        {
            bool resultado = false;
            try
            {
                using (var ctx = new SeguridadContext())
                {
                    var usuario = ctx.Usuario.Where(x => x.EmailUtepsa == emailUtepsa && x.Contrasena == contrasena && x.iEstado_fl).SingleOrDefault();
                    if (usuario != null)
                    {
                        SessionHelper.AddUserToSession(usuario.iUsuario_id.ToString());
                        resultado = true;
                    }
                    else
                    {
                        resultado = false;
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return resultado;
        }

        public Usuario Obtener(int id)
        {
            var usuario = new Usuario();
            try
            {
                using (var ctx = new SeguridadContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    usuario = ctx.Usuario.Where(x => x.iUsuario_id == id).Include(r => r.Rol).Include(rp => rp.Rol.PermisoDenegadoRoles).SingleOrDefault();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return usuario;
        }
    }
}
