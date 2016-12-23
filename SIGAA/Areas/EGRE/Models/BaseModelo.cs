using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.EGRE.Models
{
    public class BaseModelo
    {
        public BaseModelo()
        {
            iEstado_fl = true;
            iEliminado_fl = 1;
            sCreado_by = "";
            iConcurrencia_id = 1;
        }

        [Display(Name ="Estado")]
        public bool iEstado_fl { get; set; }  //true=Visible; false=Oculto
        public int iEliminado_fl { get; set; }  //1=Vigente; 2=Eliminado Lógico; 3=Eliminado Físico

        [StringLength(100)]
        public string sCreado_by { get; set; }  //Usuario que creao Ó el ultimo que modificó
        public int iConcurrencia_id { get; set; }  //se utiliza para el control de concurrencia al momento de modificar concurrencia
    }
}