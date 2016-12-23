using SIGAA.Areas.OYM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.OYM.ViewModels
{
    public class Plantilla
    {
        public tblPlantilla tblPlantilla { get; set; }
        public tblPlantillaMarcador tblPlantillaMarcador { get; set; }
        public List<tblPlantillaMarcador> tblPlantillaMarcadores { get; set; }
        public List<tblPlantillaMarcador> ItemPlantillaEliminados { get; set; }        
    }
}