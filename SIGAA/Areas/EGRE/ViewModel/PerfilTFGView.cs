using System.Collections.Generic;
using SIGAA.Areas.EGRE.Models;

namespace SIGAA.Areas.EGRE.ViewModel
{
    public class PerfilTFGView
    {
        public PerfilTFGView()
        {
            this.Perfil = new gatbl_Perfiles();
            this.RecepcionesEstudianteEjemplar1 = new List<gatbl_EntregaTFG>();
            this.EntregaTribuanalEjemplar1 = new List<gatbl_EntregaTribunales>();
            this.RecepcionTribunalEjemplar1 = new List<gatbl_RecepcionesTFG>();
            this.EntregaEstudianteEjemplar1 = new List<gatbl_EntregaAlEst>();

            this.RecepcionesEstudianteEjemplar2 = new List<gatbl_EntregaTFG>();
            this.EntregaTribuanalEjemplar2 = new List<gatbl_EntregaTribunales>();
            this.RecepcionTribunalEjemplar2 = new List<gatbl_RecepcionesTFG>();
            this.EntregaEstudianteEjemplar2 = new List<gatbl_EntregaAlEst>();
        }
        public gatbl_Perfiles Perfil { get; set; }

        public List<gatbl_EntregaTFG> RecepcionesEstudianteEjemplar1 { get; set; }
        public List<gatbl_EntregaTribunales> EntregaTribuanalEjemplar1 { get; set; }
        public List<gatbl_RecepcionesTFG> RecepcionTribunalEjemplar1 { get; set; }
        public List<gatbl_EntregaAlEst> EntregaEstudianteEjemplar1 { get; set; }

        public List<gatbl_EntregaTFG> RecepcionesEstudianteEjemplar2 { get; set; }
        public List<gatbl_EntregaTribunales> EntregaTribuanalEjemplar2 { get; set; }
        public List<gatbl_RecepcionesTFG> RecepcionTribunalEjemplar2 { get; set; }
        public List<gatbl_EntregaAlEst> EntregaEstudianteEjemplar2 { get; set; }
    }
}