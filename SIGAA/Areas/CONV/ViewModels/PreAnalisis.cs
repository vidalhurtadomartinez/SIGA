using SIGAA.Areas.CONV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.CONV.ViewModels
{
    public class PreAnalisis
    {
        public gatbl_AnalisisPreConvalidaciones gatbl_AnalisisPreConvalidaciones { get; set; }
        public gatbl_DetAnalisisPreConvalidaciones gatbl_DetAnalisisPreConvalidacione { get; set; }
        public List<gatbl_DetAnalisisPreConvalidaciones> gatbl_DetAnalisisPreConvalidaciones { get; set; }
        public List<gatbl_DetAnalisisPreConvalidaciones> gatbl_DetAnalisisPreConvalidacionesEliminados { get; set; }
    }
}