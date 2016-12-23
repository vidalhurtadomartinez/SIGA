using SIGAA.Areas.EGRE.Models;
using System.Collections.Generic;

namespace SIGAA.Areas.EGRE.ViewModel
{
    public class RecepcionTFGView
    {
        public RecepcionTFGView()
        {
            this.RecepcionTFG = new gatbl_RecepcionesTFG();
            this.DetalleRecepcionesTFG = new List<gatbl_DRecepcionesTFG>();
        }
        public gatbl_RecepcionesTFG RecepcionTFG { get; set; }
        public List<gatbl_DRecepcionesTFG> DetalleRecepcionesTFG { get; set; }

    }
}