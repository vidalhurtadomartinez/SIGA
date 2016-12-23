using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.EGRE.Models
{
    public class gatbl_PresidenteColegioProfesional:BaseModelo
    {
        public gatbl_PresidenteColegioProfesional()
        {
            this.dtFechaFinCargo_dt = DateTime.Now;
            this.dtFefhaInicioCargo_dt = DateTime.Now;
            this.sNombreCompleto_desc = "";
            this.sNroDocumento_desc = "";
            this.sEstadoActivo_fl = ParEstadoActivoPresidenteColProfesional.ACTIVO;
        }

        [Key]
        public int iPresidenteColProfesional_id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(100, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.", MinimumLength = 3)]
        [Display(Name = "Nombre Presidente")]
        public string sNombreCompleto_desc { get; set; }

        [StringLength(50)]
        [Display(Name = "Numero documento")]
        public string sNroDocumento_desc { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Fecha inicio del cargo.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime dtFefhaInicioCargo_dt { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Fecha fin al cargo.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime dtFechaFinCargo_dt { get; set; }


        [Display(Name = "Estado")]
        public ParEstadoActivoPresidenteColProfesional sEstadoActivo_fl { get; set; }

        [StringLength(50)]
        [Display(Name = "Numero de telefono(s)")]
        public string sTelefono_desc { get; set; }

        [StringLength(50)]
        [Display(Name = "Correo electrónico")]
        public string sEmail_desc { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public int iCProfesional_id { get; set; } //llave foranea de colegio profesional


        //propiedades de navegacion
        public virtual gatbl_CProfesionales ColegioProfesional { get; set; }
        public virtual IEnumerable<gatbl_ComunicacionExtColProf> ComExternasColegiosProfesionales { get; set; }
    }
}