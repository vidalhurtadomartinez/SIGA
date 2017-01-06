using SIGAA.Areas.CONV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.CONV.ViewModels
{
    public class ProgramaAnaliticoViewModels
    {
        public gatbl_ProgramasAnaliticos gatbl_ProgramasAnaliticos { get; set; }
        public gatbl_DProgramasAnaliticos gatbl_DProgramasAnalitico { get; set; }
        public gatbl_DProgramasAnaliticosTemas gatbl_DProgramasAnaliticosTema { get; set; }
        public List<gatbl_DProgramasAnaliticos> gatbl_DProgramasAnaliticos { get; set; }
        public List<gatbl_DetProgramasAnaliticos> gatbl_DetProgramasAnaliticos { get; set; }
        public List<gatbl_DetProgramasAnaliticos> gatbl_DetProgramasAnaliticosEliminados { get; set; }
        public List<gatbl_DProgramasAnaliticosTemas> gatbl_DProgramasAnaliticosTemas { get; set; }
    }
}