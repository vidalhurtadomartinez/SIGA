using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.EGRE.Models
{
    public class gatbl_RectorUniversidadPublica:BaseModelo
    {
        public gatbl_RectorUniversidadPublica() {
            this.dtFechaFinCargo_dt = DateTime.Now;
            this.dtFefhaInicioCargo_dt = DateTime.Now;
            this.sNombreCompleto_desc = "";
            this.sNroDocumento_desc = "";
            this.sEstadoActivo_fl = ParEstadoActivoRectorUnivPublica.ACTIVO;
        }

        [Key]
        public int iRectorUnivPublica_id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(100, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.", MinimumLength = 3)]
        [Display(Name = "Nombre Rector")]
        public string sNombreCompleto_desc { get; set; }

        [StringLength(50)]
        [Display(Name = "Numero documento")]
        public string sNroDocumento_desc { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Fecha inicio del cargo")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime dtFefhaInicioCargo_dt { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Fecha fin del cargo")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime dtFechaFinCargo_dt { get; set; }

        [Display(Name = "Estado")]
        public ParEstadoActivoRectorUnivPublica sEstadoActivo_fl { get; set; }

        [StringLength(50)]
        [Display(Name = "Numero de telefono(s)")]
        public string sTelefono_desc { get; set; }

        [StringLength(50)]
        [Display(Name = "Correo electrónico")]
        public string sEmail_desc { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Universidad")]
        public string unv_codigo { get; set; } //llave foranea de universidad

       
        //propiedades de navegacion
        public virtual universidades Universidad { get; set; }
        public virtual IEnumerable<gatbl_ComunicacionExtUnivPublica> ComExternasUniversidadesPublicas { get; set; }

    }
}