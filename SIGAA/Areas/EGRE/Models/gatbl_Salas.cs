using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.EGRE.Models
{
    public class gatbl_Salas : BaseModelo
    {
        public gatbl_Salas()
        {
            sNombre_nm = string.Empty;
            sUbicacion_desc = string.Empty;
            sTelefono_desc = string.Empty;
            sEncargado_nm = string.Empty;
        }

        [Key]
        public int iSala_id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(20, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.", MinimumLength = 1)]
        [Display(Name = "Sala / Aula")]
        public string sNombre_nm { get; set; }

        [StringLength(200, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.", MinimumLength = 3)]
        [Display(Name = "Ubicación")]
        public string sUbicacion_desc { get; set; }

        [StringLength(50, ErrorMessage = "el campo {0} debe tener entre {2} y {1} caracteres", MinimumLength = 3)]
        [Display(Name = "Teléfono")]
        public string sTelefono_desc { get; set; }

        [StringLength(50, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.", MinimumLength = 3)]
        [Display(Name = "Encargado")]
        public string sEncargado_nm { get; set; }


        public List<gatbl_Salas> Paginar(int indiceDePagina, int tamanoDePagina, out int cantidadDePaginas)
        {
            List<gatbl_Salas> salas = new List<gatbl_Salas>();
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["EgresadossConnection"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Sp_Paginar", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    System.Object[] args = new System.Object[3];
                    args[0] = indiceDePagina;
                    args[1] = tamanoDePagina;
                    args[2] = 0;

                    SqlCommandBuilder.DeriveParameters(cmd);

                    int limite = cmd.Parameters.Count;
                    for (int i = 1; i < limite; i++)
                    {
                        SqlParameter p = (SqlParameter)cmd.Parameters[i];
                        if (i <= args.Length)
                            p.Value = args[i - 1];
                        else
                            p.Value = null;
                    }

                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr != null)
                    {
                        while (dr.Read())
                        {
                            gatbl_Salas sala = new gatbl_Salas();
                            if (dr["iSala_id"] != DBNull.Value) sala.iSala_id = (int)dr["iSala_id"];
                            if (dr["sEncargado_nm"] != DBNull.Value) sala.sEncargado_nm = (String)dr["sEncargado_nm"];
                            if (dr["sNombre_nm"] != DBNull.Value) sala.sNombre_nm = (String)dr["sNombre_nm"];
                            if (dr["sTelefono_desc"] != DBNull.Value) sala.sTelefono_desc = (String)dr["sTelefono_desc"];
                            if (dr["sUbicacion_desc"] != DBNull.Value) sala.sUbicacion_desc = (String)dr["sUbicacion_desc"];
                            if (dr["iConcurrencia_id"] != DBNull.Value) sala.iConcurrencia_id = (int)dr["iConcurrencia_id"];
                            if (dr["iEliminado_fl"] != DBNull.Value) sala.iEliminado_fl = (int)dr["iEliminado_fl"];
                            if (dr["iEstado_fl"] != DBNull.Value) sala.iEstado_fl = (bool)dr["iEstado_fl"];
                            if (dr["sCreado_by"] != DBNull.Value) sala.sCreado_by = (String)dr["sCreado_by"];
                            salas.Add(sala);
                        }
                    }
                    dr.Close();
                    cantidadDePaginas = (int)cmd.Parameters["@cantidadDePaginas"].Value;
                }
            }
            return salas;
        }
    }
}