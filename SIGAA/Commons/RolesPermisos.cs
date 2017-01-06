using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGAA.Commons
{
    public enum RolesPermisos
    { 
        #region EGRESADOS
        EGRE_perfil_puedeVerIndice = 1,
        EGRE_perfil_puedeCrearNuevo =2,
        EGRE_perfil_puedeEliminar = 3,
        EGRE_perfil_puedeEditar = 4,
        EGRE_perfil_puedeVerDetalle = 5,

        EGRE_recepcionTFGdelAlumno_puedeVerIndice = 6,
        EGRE_recepcionTFGdelAlumno_puedeCrearNuevo = 7,
        EGRE_recepcionTFGdelAlumno_puedeEliminar = 8,
        EGRE_recepcionTFGdelAlumno_puedeEditar = 9,
        EGRE_recepcionTFGdelAlumno_puedeVerDetalle = 10,

        EGRE_entregaTFGalTribunal_puedeVerIndice = 11,
        EGRE_entregaTFGalTribunal_puedeCrearNuevo = 12,
        EGRE_entregaTFGalTribunal_puedeEliminar = 13,
        EGRE_entregaTFGalTribunal_puedeEditar = 14,
        EGRE_entregaTFGalTribunal_puedeVerDetalle = 15,

        EGRE_recepcionTFGdelTribunal_puedeVerIndice = 16,
        EGRE_recepcionTFGdelTribunal_puedeCrearNuevo = 17,
        EGRE_recepcionTFGdelTribunal_puedeEliminar = 18,
        EGRE_recepcionTFGdelTribunal_puedeEditar = 19,
        EGRE_recepcionTFGdelTribunal_puedeVerDetalle = 20,

        EGRE_entregaTFGalAlumno_puedeVerIndice = 21,
        EGRE_entregaTFGalAlumno_puedeCrearNuevo = 22,
        EGRE_entregaTFGalAlumno_puedeEliminar = 23,
        EGRE_entregaTFGalAlumno_puedeEditar = 24,
        EGRE_entregaTFGalAlumno_puedeVerDetalle = 25,

        EGRE_accederAseguimientoDeEjemplaresTFG = 26,

        EGRE_comunicacionAlAlumno_puedeVerIndice = 27,
        EGRE_comunicacionAlAlumno_puedeCrearNuevo = 28,
        EGRE_comunicacionAlAlumno_puedeEliminar = 29,
        EGRE_comunicacionAlAlumno_puedeEditar = 30,
        EGRE_comunicacionAlAlumno_puedeVerDetalle = 31,
        EGRE_comunicacionAlAlumno_puedeImprimir = 32,

        EGRE_comunicacionInterna_puedeVerIndice = 33,
        EGRE_comunicacionInterna_puedeCrearNuevo = 34,
        EGRE_comunicacionInterna_puedeEliminar = 35,
        EGRE_comunicacionInterna_puedeEditar = 36,
        EGRE_comunicacionInterna_puedeVerDetalle = 37,
        EGRE_comunicacionInterna_puedeImprimir = 38,

        EGRE_comunicacionExternaColProfesional_puedeVerIndice = 39,
        EGRE_comunicacionExternaColProfesional_puedeCrearNuevo = 40,
        EGRE_comunicacionExternaColProfesional_puedeEliminar = 41,
        EGRE_comunicacionExternaColProfesional_puedeEditar = 42,
        EGRE_comunicacionExternaColProfesional_puedeVerDetalle = 43,
        EGRE_comunicacionExternaColProfesional_puedeImprimir = 44,

        EGRE_comunicacionExternaUnivPublica_puedeVerIndice = 45,
        EGRE_comunicacionExternaUnivPublica_puedeCrearNuevo = 46,
        EGRE_comunicacionExternaUnivPublica_puedeEliminar = 47,
        EGRE_comunicacionExternaUnivPublica_puedeEditar = 48,
        EGRE_comunicacionExternaUnivPublica_puedeVerDetalle = 49,
        EGRE_comunicacionExternaUnivPublica_puedeImprimir = 50,

        EGRE_actaDeSorteo_puedeVerIndice = 51,
        EGRE_actaDeSorteo_puedeCrearNuevo = 52,
        EGRE_actaDeSorteo_puedeEliminar = 53,
        EGRE_actaDeSorteo_puedeEditar = 54,
        EGRE_actaDeSorteo_puedeVerDetalle = 55,
        EGRE_actaDeSorteo_puedeImprimir = 56,

        EGRE_actaDeDefensaFinal_puedeVerIndice = 57,
        EGRE_actaDeDefensaFinal_puedeCrearNuevo = 58,
        EGRE_actaDeDefensaFinal_puedeEliminar = 59,
        EGRE_actaDeDefensaFinal_puedeEditar = 60,
        EGRE_actaDeDefensaFinal_puedeVerDetalle = 61,
        EGRE_actaDeDefensaFinal_puedeImprimir = 62,
        EGRE_actaDeDefensaFinal_puedeVerActaDigitalizada = 63,

        EGRE_tribunalTutor_puedeVerIndice = 64,
        EGRE_tribunalTutor_puedeCrearNuevo = 65,
        EGRE_tribunalTutor_puedeEliminar = 66,
        EGRE_tribunalTutor_puedeEditar = 67,
        EGRE_tribunalTutor_puedeVerDetalle = 68,

        EGRE_presidenteColProfesional_puedeVerIndice = 69,
        EGRE_presidenteColProfesional_puedeCrearNuevo = 70,
        EGRE_presidenteColProfesional_puedeEliminar = 71,
        EGRE_presidenteColProfesional_puedeEditar = 72,
        EGRE_presidenteColProfesional_puedeVerDetalle = 73,
         
        EGRE_rectorUniversidaPublica_puedeVerIndice = 74,
        EGRE_rectorUniversidaPublica_puedeCrearNuevo = 75,
        EGRE_rectorUniversidaPublica_puedeEliminar = 76,
        EGRE_rectorUniversidaPublica_puedeEditar = 77,
        EGRE_rectorUniversidaPublica_puedeVerDetalle = 78,

        EGRE_profesionTutor_puedeVerIndice = 79,
        EGRE_profesionTutor_puedeCrearNuevo = 80,
        EGRE_profesionTutor_puedeEliminar = 81,
        EGRE_profesionTutor_puedeEditar = 82,
        EGRE_profesionTutor_puedeVerDetalle = 83,

        EGRE_colegioProfesional_puedeVerIndice = 84,
        EGRE_colegioProfesional_puedeCrearNuevo = 85,
        EGRE_colegioProfesional_puedeEliminar = 86,
        EGRE_colegioProfesional_puedeEditar = 87,
        EGRE_colegioProfesional_puedeVerDetalle = 88,
         
        EGRE_areaAdministrativa_puedeVerIndice = 89,
        EGRE_areaAdministrativa_puedeCrearNuevo = 90,
        EGRE_areaAdministrativa_puedeEliminar = 91,
        EGRE_areaAdministrativa_puedeEditar = 92,
        EGRE_areaAdministrativa_puedeVerDetalle = 93,

        EGRE_aulaSala_puedeVerIndice = 94,
        EGRE_aulaSala_puedeCrearNuevo = 95,
        EGRE_aulaSala_puedeEliminar = 96,
        EGRE_aulaSala_puedeEditar = 97,
        EGRE_aulaSala_puedeVerDetalle = 98,
        #endregion
        #region SEGURIDAD
        SEGU_home_puedeVerIndex = 99,
        SEGU_login_puedeAutenticarse = 100,
        SEGU_login_puedeCerrarSession = 101,

        SEGU_permiso_puedeVerIndice = 102,
        SEGU_permiso_puedeCrearNuevo = 103,
        SEGU_permiso_puedeEliminar = 104,
        SEGU_permiso_puedeEditar = 105,
        SEGU_permiso_puedeVerDetalle = 106,

        SEGU_permisoDenegadoPorRol_puedeVerIndice = 107,
        SEGU_permisoDenegadoPorRol_puedeCrearNuevo = 108,
        SEGU_permisoDenegadoPorRol_puedeEliminar = 109,
        SEGU_permisoDenegadoPorRol_puedeEditar = 110,
        SEGU_permisoDenegadoPorRol_puedeVerDetalle = 111,

        SEGU_rol_puedeVerIndice = 112,
        SEGU_rol_puedeCrearNuevo = 113,
        SEGU_rol_puedeEliminar = 114,
        SEGU_rol_puedeEditar = 115,
        SEGU_rol_puedeVerDetalle = 116,

        SEGU_usuario_puedeVerIndice = 117,
        SEGU_usuario_puedeCrearNuevo = 118,        
        SEGU_usuario_puedeEditar = 119,
        SEGU_usuario_puedeVerDetalle = 120,
        SEGU_usuario_puedeCambiarContrasena = 121,

        SEGU_HistorialCambioRolDeUsuario_puedeVerIndice = 204,

        #endregion
        #region O&M
        OYM_plantillas_puedeVerIndice = 122,
        OYM_plantillas_puedeCrearNuevo = 123,
        OYM_plantillas_puedeEliminar = 124,
        OYM_plantillas_puedeEditar = 125,
        OYM_plantillas_puedeVerDetalle = 126,

        OYM_documentos_puedeVerIndice = 127,
        OYM_documentos_puedeCrearNuevo = 128,
        OYM_documentos_puedeEliminar = 129,
        OYM_documentos_puedeEditar = 130,
        OYM_documentos_puedeVerDetalle = 131,
        OYM_documentos_puedeAsignarProceso = 132,
        OYM_documentos_puedeDescargarDocumento = 133,

        OYM_formularios_puedeVerIndice = 134,
        OYM_formularios_puedeCrearNuevo = 135,
        OYM_formularios_puedeEliminar = 136,
        OYM_formularios_puedeEditar = 137,
        OYM_formularios_puedeVerDetalle = 138,
        OYM_formularios_puedeAsignarProceso = 139,
        OYM_formularios_puedeDescargarDocumento = 140,

        OYM_procesoDocumentos_puedeVerIndice = 141,
        OYM_procesoDocumentos_puedeCrearNuevo = 142,
        OYM_procesoDocumentos_puedeEliminar = 143,
        OYM_procesoDocumentos_puedeEditar = 144,
        OYM_procesoDocumentos_puedeVerDetalle = 145,

        OYM_procesoFormularios_puedeVerIndice = 146,
        OYM_procesoFormularios_puedeCrearNuevo = 147,
        OYM_procesoFormularios_puedeEliminar = 148,
        OYM_procesoFormularios_puedeEditar = 149,
        OYM_procesoFormularios_puedeVerDetalle = 150,

        OYM_seguimientoDocumentos_puedeVerIndice = 151,
        OYM_seguimientoDocumentos_puedeEditar = 152,
        OYM_seguimientoDocumentos_puedeEditarResponderAprobado = 153,
        OYM_seguimientoDocumentos_puedeEditarResponderRechazado = 154,

        OYM_seguimientoFormularios_puedeVerIndice = 155,
        OYM_seguimientoFormularios_puedeEditar = 156,
        OYM_seguimientoFormularios_puedeEditarResponderAprobado = 157,
        OYM_seguimientoFormularios_puedeEditarResponderRechazado = 158,

        OYM_documentosPublicados_puedeVerIndice = 159,
        OYM_documentosPublicados_puedeVerDetalle = 160,

        OYM_formulariosPublicados_puedeVerIndice = 161,
        OYM_formulariosPublicados_puedeVerDetalle = 162,

        OYM_directorioDeArchivos_puedeVerIndice = 163,
        OYM_directorioDeArchivos_puedeCrearNuevo = 164,
        OYM_directorioDeArchivos_puedeEditar = 165,
        OYM_directorioDeArchivos_puedeEliminar = 166,
        OYM_directorioDeArchivos_puedeVerDetalle = 167,

        OYM_tipoDeFormasDeDocumento_puedeVerIndice = 168,
        OYM_tipoDeFormasDeDocumento_puedeCrearNuevo = 169,
        OYM_tipoDeFormasDeDocumento_puedeEditar = 170,
        OYM_tipoDeFormasDeDocumento_puedeEliminar = 171,
        OYM_tipoDeFormasDeDocumento_puedeVerDetalle = 172,

        OYM_formaDeDocumento_puedeVerIndice = 173,
        OYM_formaDeDocumento_puedeCrearNuevo = 174,
        OYM_formaDeDocumento_puedeEditar = 175,
        OYM_formaDeDocumento_puedeEliminar = 176,
        OYM_formaDeDocumento_puedeVerDetalle = 177,

        OYM_tipoDeDocumento_puedeVerIndice = 178,
        OYM_tipoDeDocumento_puedeCrearNuevo = 179,
        OYM_tipoDeDocumento_puedeEditar = 180,
        OYM_tipoDeDocumento_puedeEliminar = 181,
        OYM_tipoDeDocumento_puedeVerDetalle = 182,

        OYM_tipoDeProcesos_puedeVerIndice = 183,
        OYM_tipoDeProcesos_puedeCrearNuevo = 184,
        OYM_tipoDeProcesos_puedeEditar = 185,
        OYM_tipoDeProcesos_puedeEliminar = 186,
        OYM_tipoDeProcesos_puedeVerDetalle = 187,

        OYM_categoriaDeParcipante_puedeVerIndice = 188,
        OYM_categoriaDeParcipante_puedeCrearNuevo = 189,
        OYM_categoriaDeParcipante_puedeEditar = 190,
        OYM_categoriaDeParcipante_puedeEliminar = 191,
        OYM_categoriaDeParcipante_puedeVerDetalle = 192,

        OYM_procesos_puedeVerIndice = 193,
        OYM_procesos_puedeCrearNuevo = 194,
        OYM_procesos_puedeEditar = 195,
        OYM_procesos_puedeEliminar = 196,
        OYM_procesos_puedeVerDetalle = 197,

        OYM_reportes_documentosPorTipoDeProceso = 198,
        OYM_reportes_formulariosPorTipoDeProcesos = 199,
        OYM_reportes_promedioDeRespuestasPorProcesosDeDocumentos = 200,
        OYM_reportes_promedioDeRespuestasPorProcesosDeFormularios = 201,
        OYM_reportes_DetalleDeRespuestasPorProcesosDeDocumentos = 202,
        OYM_reportes_DetalleDeRespuestasPorProcesosDeFormularios = 203,

        #endregion
        #region CONVALIDACIONES
        CONV_Solicitud_Nuevo = 205,
        CONV_Solicitud_Buscar = 206,
        CONV_Solicitud_Editar = 207,
        CONV_Solicitud_Certificado = 208,
        CONV_Solicitud_Ver = 209,
        CONV_Solicitud_Eliminar = 210,

        CONV_Postulante_Listado = 211,
        CONV_Postulante_Nuevo = 212,
        CONV_Postulante_Editar = 213,
        CONV_Postulante_Ver = 214,
        CONV_Postulante_Eliminar = 215,

        CONV_PreConvalidacion_Pendientes = 216,
        CONV_PreConvalidacion_Realizados = 217,

        CONV_Analisis_Pendientes = 218,
        CONV_Analisis_Realizados = 219,

        CONV_Academico_ListadoUniversidades = 220,
        CONV_Academico_EditarUniversidad = 221,
        CONV_Academico_VerUniversidad = 222,
        CONV_Academico_EliminarUniversidad = 223,

        CONV_Academico_ListadoFacultades = 224,
        CONV_Academico_EditarFacultad = 225,
        CONV_Academico_VerFacultad = 226,
        CONV_Academico_EliminarFacultad = 227,

        CONV_Academico_ListadoCarreras = 228,
        CONV_Academico_EditarCarrera = 229,
        CONV_Academico_VerCarrera = 230,
        CONV_Academico_EliminarCarrera = 231,

        CONV_Academico_ListadoProgramasAnaliticos = 232,
        CONV_Academico_EditarProgramasAnalitico = 233,
        CONV_Academico_DuplicarProgramasAnalitico = 234,
        CONV_Academico_VerProgramasAnalitico = 235,
        CONV_Academico_EliminarProgramasAnalitico = 236,



        CONV_Parametros_ListadoDepartamentos = 237,
        CONV_Parametros_EditarDepartamento = 238,
        CONV_Parametros_VerDepartamento = 239,
        CONV_Parametros_EliminarDepartamento = 240,

        CONV_Parametros_ListadoTipoDocumentoPersonal = 241,
        CONV_Parametros_EditarTipoDocumentoPersonal = 242,
        CONV_Parametros_VerTipoDocumentoPersonal = 243,
        CONV_Parametros_EliminarTipoDocumentoPersonal = 244,

        CONV_Parametros_ListadoNacionalidades = 245,
        CONV_Parametros_EditarNacionalidad = 246,
        CONV_Parametros_VerNacionalidad = 247,
        CONV_Parametros_EliminarNacionalidad = 248,

        CONV_Parametros_ListadoTipoDocumentoSolicitud = 249,
        CONV_Parametros_EditarTipoDocumentoSolicitud = 250,
        CONV_Parametros_VerTipoDocumentoSolicitud = 251,
        CONV_Parametros_EliminarTipoDocumentoSolicitud = 252,

        CONV_Parametros_ListadoOrigenOtraUniversidad = 253,
        CONV_Parametros_EditarOrigenOtraUniversidad = 254,
        CONV_Parametros_VerOrigenOtraUniversidad = 255,
        CONV_Parametros_EliminarOrigenOtraUniversidad = 256,

        CONV_Parametros_ListadoTipoPresentacionDocumento = 257,
        CONV_Parametros_EditarTipoPresentacionDocumento = 258,
        CONV_Parametros_VerTipoPresentacionDocumento = 259,
        CONV_Parametros_EliminarTipoPresentacionDocumento = 260,

        CONV_Parametros_ListadoUnidadNegocio = 261,
        CONV_Parametros_EditarUnidadNegocio = 262,
        CONV_Parametros_VerUnidadNegocio = 263,
        CONV_Parametros_EliminarUnidadNegocio = 264,

        CONV_Parametros_ListadoNivelProgramaAnalitico = 265,
        CONV_Parametros_EditarNivelProgramaAnalitico = 266,
        CONV_Parametros_VerNivelProgramaAnalitico = 267,
        CONV_Parametros_EliminarNivelProgramaAnalitico = 268,

        CONV_Parametros_ListadoPensumAcademico = 269,
        CONV_Parametros_EditarPensumAcademico = 270,
        CONV_Parametros_VerPensumAcademico = 271,
        CONV_Parametros_EliminarPensumAcademico = 272,

        CONV_Parametros_ListadoTipoCargaHoraria = 273,
        CONV_Parametros_EditarTipoCargaHoraria = 274,
        CONV_Parametros_VerTipoCargaHoraria = 275,
        CONV_Parametros_EliminarTipoCargaHoraria = 276,
        #endregion
    }
}