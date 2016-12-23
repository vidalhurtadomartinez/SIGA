using SIGAA.Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Models
{
    public class BaseModel
    {
        public BaseModel()
        {
            iEstado_fl = true;
            iEliminado_fl = 1;
            sCreado_by = "";
            iConcurrencia_id = 1;
        }

        [Display(Name ="Estado habilitado :")]
        public bool iEstado_fl { get; set; }

        [Display(Name = "Estado eliminado :")]
        public int iEliminado_fl { get; set; }

        [StringLength(100)]
        [Display(Name = "Creado Por: ")]
        public string sCreado_by { get; set; }

        [Display(Name = "Concurrencia")]
        public int iConcurrencia_id { get; set; }
    }
}