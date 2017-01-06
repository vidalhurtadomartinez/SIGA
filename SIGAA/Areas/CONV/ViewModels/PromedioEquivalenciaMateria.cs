using SIGAA.Areas.CONV.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.CONV.ViewModels
{
    public class PromedioEquivalenciaMateria
    {        
        public gatbl_DetAnalisisPreConvalidaciones gatbl_DetAnalisisPreConvalidaciones { get; set; }

        public gatbl_AnalisisConvalidacionesComplemento gatbl_AnalisisConvalidacionesComplemento { get; set; }
    }
}