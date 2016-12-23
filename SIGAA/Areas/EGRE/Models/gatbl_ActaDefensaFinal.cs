using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.EGRE.Models
{
    public class gatbl_ActaDefensaFinal : BaseModelo
    {
        public gatbl_ActaDefensaFinal()
        {
            sGestion_desc = "2016";
            sPeriodo_desc = "2016-1";
            iNumeracion_num = 0;
            dtSorteo_dt = DateTime.Today;
            dtHora_dt = DateTime.Now;
            dtDefensa_dt = DateTime.Today;
            dtHoraDefensa_dt = DateTime.Now;
            sLugar_desc = string.Empty;
            dtFinalizacionDefensa_dt = DateTime.Today;
            dtHoraFinalizacion_dt = DateTime.Now.ToLocalTime();
            sResultadoDefensa_fl = ParResultadoActaDefensaFinal.PRELIMINAR;
            sObs_desc = string.Empty;
        }

        [Key]
        public int iActaDefensaFinal_id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(50, ErrorMessage = "El campo {0} debe tener {1} caracteres como máximo.")]
        [Display(Name = "Gestión")]
        public string sGestion_desc { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(50, ErrorMessage = "El campo {0} debe tener {1} caracteres como máximo.")]
        [Display(Name = "Periodo")]
        public string sPeriodo_desc { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Numeración")]
        public int iNumeracion_num { get; set; }


        [Display(Name = "Fecha sorteo")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime dtSorteo_dt { get; set; }
        
        [Display(Name = "Hora sorteo")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime dtHora_dt { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Fecha defensa")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime dtDefensa_dt { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Hora defensa")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime dtHoraDefensa_dt { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(50, ErrorMessage = "El campo {0} debe tener {1} caracteres como máximo.")]
        [Display(Name = "Lugar defensa")]
        public string sLugar_desc { get; set; } // se refiere a la sala, se de guardar el nombre de la sala, no relacionar

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Fecha Finalización")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime dtFinalizacionDefensa_dt { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Hora finalización")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime dtHoraFinalizacion_dt { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Resultado")]
        public ParResultadoActaDefensaFinal sResultadoDefensa_fl { get; set; } // 01=aprobado; 02=reprobado


        [StringLength(250, ErrorMessage = "El campo {0} debe tener {1} caracteres como máximo.")]
        [Display(Name = "Calificación")]
        public string sCalificacion_desc { get; set; }


        [StringLength(250, ErrorMessage = "El campo {0} debe tener {1} caracteres como máximo.")]
        [Display(Name = "Observación")]
        public string sObs_desc { get; set; }


        [Display(Name = "Acta Digitalizada")]
        public Byte[] bActa_digital { get; set; }


        public int iPerfil_id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string alm_registro { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string lPresidente_id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string lSecretario_id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string lEvaluador1I_id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string lEvaluador2I_id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string lEvaluador1E_id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string lEvaluador2E_id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string lRepresentante_id { get; set; }

        //propiedades de navegacion
        [ForeignKey("lPresidente_id")]
        public virtual agenda Presidente { get; set; }

        [ForeignKey("lSecretario_id")]
        public virtual agenda Secretario { get; set; }

        [ForeignKey("lEvaluador1I_id")]
        public virtual agenda Evaluador1I { get; set; }

        [ForeignKey("lEvaluador2I_id")]
        public virtual agenda Evaluador2I { get; set; }

        [ForeignKey("lEvaluador1E_id")]
        public virtual agenda Evaluador1E { get; set; }

        [ForeignKey("lEvaluador2E_id")]
        public virtual agenda Evaluador2E { get; set; }

        [ForeignKey("lRepresentante_id")]
        public virtual agenda Representante { get; set; }

        public virtual alumnos_agenda Alumno { get; set; }
        public virtual gatbl_Perfiles Perfil { get; set; }

        //Paginador
        //public List<gatbl_ActaDefensaFinal> Paginar(int indiceDePagina, int tamanoDePagina, out int cantidadDePaginas)
        //{
        //    List<gatbl_ActaDefensaFinal> salas = new List<gatbl_ActaDefensaFinal>();
        //    using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["EgresadossConnection"].ConnectionString))
        //    {
        //        con.Open();
        //        using (SqlCommand cmd = new SqlCommand("Sp_Paginar", con))
        //        {
        //            cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //            cmd.CommandTimeout = 0;

        //            System.Object[] args = new System.Object[3];
        //            args[0] = indiceDePagina;
        //            args[1] = tamanoDePagina;
        //            args[2] = 0;

        //            SqlCommandBuilder.DeriveParameters(cmd);

        //            int limite = cmd.Parameters.Count;
        //            for (int i = 1; i < limite; i++)
        //            {
        //                SqlParameter p = (SqlParameter)cmd.Parameters[i];
        //                if (i <= args.Length)
        //                    p.Value = args[i - 1];
        //                else
        //                    p.Value = null;
        //            }
        //            SqlDataReader dr = cmd.ExecuteReader();



        //            /////
        //            //SqlDataReader reader = cmd.ExecuteReader();
        //            //DataTable schemaTable = reader.GetSchemaTable();
        //            //foreach (DataRow row in schemaTable.Rows)
        //            //{
        //            //    gatbl_ActaDefensaFinal actaFinal = new gatbl_ActaDefensaFinal();
        //            //    foreach (DataColumn column in schemaTable.Columns)
        //            //    {                           
        //            //        if (dr[column.ColumnName] != DBNull.Value) actaFinal.iActaDefensaFinal_id = (int)dr[column.ColumnName];
        //            //    }
        //            //    salas.Add(actaFinal);
        //            //}

        //            ///////
                   


        //            if (dr != null)
        //            {
        //                while (dr.Read())
        //                {
        //                    gatbl_ActaDefensaFinal actaFinal = new gatbl_ActaDefensaFinal();





        //                    if (dr["iSala_id"] != DBNull.Value) actaFinal.iActaDefensaFinal_id = (int)dr["iSala_id"];
        //                    if (dr["sEncargado_nm"] != DBNull.Value) actaFinal.iConcurrencia_id = (String)dr[DataC;
        //                    if (dr["sNombre_nm"] != DBNull.Value) actaFinal.iEliminado_fl = (String)dr["sNombre_nm"];
        //                    if (dr["sTelefono_desc"] != DBNull.Value) actaFinal.iEstado_fl = (String)dr["sTelefono_desc"];
        //                    if (dr["sUbicacion_desc"] != DBNull.Value) actaFinal.iNumeracion_num = (String)dr["sUbicacion_desc"];
        //                    if (dr["iConcurrencia_id"] != DBNull.Value) actaFinal.iPerfil_id = (int)dr["iConcurrencia_id"];
        //                    if (dr["iEliminado_fl"] != DBNull.Value) actaFinal.lEvaluador1E_id = (int)dr["iEliminado_fl"];
        //                    if (dr["iEstado_fl"] != DBNull.Value) actaFinal.lEvaluador1E_id = (bool)dr["iEstado_fl"];
        //                    if (dr["sCreado_by"] != DBNull.Value) actaFinal.lEvaluador2E_id = (String)dr["sCreado_by"];
        //                    if (dr["sCreado_by"] != DBNull.Value) actaFinal.lEvaluador2I_id = (String)dr["sCreado_by"];
        //                    if (dr["sCreado_by"] != DBNull.Value) actaFinal.lPresidente_id = (String)dr["sCreado_by"];
        //                    if (dr["sCreado_by"] != DBNull.Value) actaFinal.lRepresentante_id = (String)dr["sCreado_by"];
        //                    if (dr["sCreado_by"] != DBNull.Value) actaFinal.lSecretario_id = (String)dr["sCreado_by"];

        //                    if (dr["sCreado_by"] != DBNull.Value) actaFinal.lEvaluador1I_id = (String)dr["sCreado_by"];
        //                    if (dr["sCreado_by"] != DBNull.Value) actaFinal.lEvaluador1I_id = (String)dr["sCreado_by"];
        //                    if (dr["sCreado_by"] != DBNull.Value) actaFinal.lEvaluador1I_id = (String)dr["sCreado_by"];
        //                    if (dr["sCreado_by"] != DBNull.Value) actaFinal.lEvaluador1I_id = (String)dr["sCreado_by"];
        //                    if (dr["sCreado_by"] != DBNull.Value) actaFinal.lEvaluador1I_id = (String)dr["sCreado_by"];

        //                    salas.Add(actaFinal);
        //                }
        //            }
        //            dr.Close();
        //            cantidadDePaginas = (int)cmd.Parameters["@cantidadDePaginas"].Value;
        //        }
        //    }
        //    return salas;
        //}
    }
}