using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BL
{
    public class Hospital
    {
        public static Dictionary<string, object> Add(ML.Hospital hospital)
        {
            string excepcion = "";
            Dictionary<string, object> diccionario = new Dictionary<string, object> { { "Excepcion", excepcion }, { "Resultado", false } };
            try
            {
                using (DL.DbutronHospitalContext context = new DL.DbutronHospitalContext())
                {
                    int filasAfectadas = context.Database.ExecuteSqlRaw($"HospitalAdd '{hospital.Nombre}', '{hospital.Direccion}', '{hospital.AnioConstruccion}', {hospital.Capacidad}, {hospital.Especialidad.IdEspecialidad}");
                    if (filasAfectadas > 0)
                    {
                        diccionario["Resultado"] = true;
                    }
                    else
                    {
                        diccionario["Resultado"] = false;
                    }
                }
            }
            catch (Exception ex)
            {
                diccionario["Resultado"] = false;
                diccionario["Excepcion"] = ex.Message;
            }
            return diccionario;
        }
        public static Dictionary<string, object> Update(ML.Hospital hospital)
        {
            string excepcion = "";
            Dictionary<string, object> diccionario = new Dictionary<string, object> { { "Excepcion", excepcion }, { "Resultado", false } };
            try
            {
                using (SqlConnection context = new SqlConnection(DL.DBConnection.GetConnection()))
                {
                    var query = "UPDATE Hospital SET (Nombre = @Nombre, Direccion = @Direccion, AnioConstruccion = @AnioConstruccion, Capacidad = @Capacidad, IdEspecialidad = @IdEspecialidad) WHERE IdHospital = @IdHospital";
                    SqlParameter[] parametros = new SqlParameter[5];
                    parametros[0] = new SqlParameter("@Nombre", System.Data.SqlDbType.VarChar);
                    parametros[0].Value = hospital.Nombre;
                    parametros[1] = new SqlParameter("@Direccion", System.Data.SqlDbType.VarChar);
                    parametros[1].Value = hospital.Direccion;
                    parametros[2] = new SqlParameter("@AnioConstruccion", System.Data.SqlDbType.DateTime);
                    parametros[2].Value = hospital.AnioConstruccion;
                    parametros[3] = new SqlParameter("@Capacidad", System.Data.SqlDbType.Int);
                    parametros[3].Value = hospital.Capacidad;
                    parametros[4] = new SqlParameter("@IdEspecialidad", System.Data.SqlDbType.Int);
                    parametros[4].Value = hospital.Especialidad.IdEspecialidad;

                    SqlCommand command = new SqlCommand(query, context);
                    command.Parameters.Add(parametros);
                    command.Connection.Open();
                    int filasAfectadas = command.ExecuteNonQuery();
                    if (filasAfectadas > 0)
                    {
                        diccionario["Resultado"] = true;
                    }
                    else
                    {
                        diccionario["Resultado"] = false;
                    }
                }
            }
            catch (Exception ex)
            {
                diccionario["Resultado"] = false;
                diccionario["Excepcion"] = ex.Message;
            }
            return diccionario;
        }
        public static Dictionary<string, object> Delete(int IdHospital)
        {
            ML.Hospital hospital = new ML.Hospital();
            string excepcion = "";
            Dictionary<string, object> diccionario = new Dictionary<string, object> { { "Excepcion", excepcion }, { "Resultado", false } };
            try
            {
                using (DL.DbutronHospitalContext context = new DL.DbutronHospitalContext())
                {
                    int filasAfectadas = context.Database.ExecuteSqlRaw($"HospitalDelete {hospital.IdHospital}");
                    if (filasAfectadas > 0)
                    {
                        diccionario["Resultado"] = true;
                    }
                    else
                    {
                        diccionario["Resultado"] = false;
                    }
                }
            }
            catch (Exception ex)
            {
                diccionario["Resultado"] = false;
                diccionario["Excepcion"] = ex.Message;
            }
            return diccionario;
        }
        public static Dictionary<string, object> GetAll()
        {
            ML.Hospital hospital = new ML.Hospital();
            string excepcion = "";
            Dictionary<string, object> diccionario = new Dictionary<string, object> { { "Hospital", hospital }, { "Excepcion", excepcion }, { "Resultado", false } };
            try
            {
                using (DL.DbutronHospitalContext context = new DL.DbutronHospitalContext())
                {
                    var listaHospitales = (from tablaHospital in context.Hospitals
                                           join tablaEspecialidad in context.Especialidads on tablaHospital.IdEspecialidad equals tablaEspecialidad.IdEspecialidad
                                           select new
                                           {
                                               IdHospital = tablaHospital.IdEspecialidad,
                                               Nombre = tablaHospital.Nombre,
                                               Direccion = tablaHospital.Direccion,
                                               AnioConstruccion = tablaHospital.AnioConstruccion,
                                               Capacidad = tablaHospital.Capacidad,
                                               IdEspecialidad = tablaEspecialidad.IdEspecialidad,
                                               TipoEspecialidad = tablaEspecialidad.Nombre
                                           }).ToList();
                    if (listaHospitales != null)
                    {
                        hospital.Hospitales = new List<object>();
                        foreach (var registro in listaHospitales)
                        {
                            ML.Hospital hospital1 = new ML.Hospital();
                            hospital1.IdHospital = registro.IdHospital;
                            hospital1.Nombre = registro.Nombre;
                            hospital1.Direccion = registro.Direccion;
                            hospital1.AnioConstruccion = registro.AnioConstruccion;
                            hospital1.Capacidad = registro.Capacidad;
                            hospital1.Especialidad = new ML.Especialidad();
                            hospital1.Especialidad.IdEspecialidad = registro.IdEspecialidad;
                            hospital1.Especialidad.Nombre = registro.TipoEspecialidad;
                            hospital.Hospitales.Add(hospital1);
                        }
                        diccionario["Resultado"] = true;
                        diccionario["Hospital"] = hospital;
                    }
                    else
                    {
                        diccionario["Resultado"] = false;
                    }
                }
            }
            catch (Exception ex)
            {
                diccionario["Resultado"] = false;
                diccionario["Excepcion"] = ex.Message;
            }
            return diccionario;
        }
        public static Dictionary<string, object> GetById(int IdHospital)
        {
            ML.Hospital hospital = new ML.Hospital();
            string excepcion = "";
            Dictionary<string, object> diccionario = new Dictionary<string, object> { { "Hospital", hospital }, { "Excepcion", excepcion }, { "Resultado", false } };
            try
            {
                using (DL.DbutronHospitalContext context = new DL.DbutronHospitalContext())
                {
                    var objetoHospital = (from tablaHospital in context.Hospitals
                                          join tablaEspecialidad in context.Especialidads on tablaHospital.IdEspecialidad equals tablaEspecialidad.IdEspecialidad
                                          where tablaHospital.IdEspecialidad == IdHospital
                                          select new
                                          {
                                              IdHospital = tablaHospital.IdEspecialidad,
                                              Nombre = tablaHospital.Nombre,
                                              Direccion = tablaHospital.Direccion,
                                              AnioConstruccion = tablaHospital.AnioConstruccion,
                                              Capacidad = tablaHospital.Capacidad,
                                              IdEspecialidad = tablaEspecialidad.IdEspecialidad,
                                              TipoEspecialidad = tablaEspecialidad.Nombre
                                          }).FirstOrDefault();
                    if (objetoHospital != null)
                    {
                        hospital.IdHospital = objetoHospital.IdHospital;
                        hospital.Nombre = objetoHospital.Nombre;
                        hospital.Direccion = objetoHospital.Direccion;
                        hospital.AnioConstruccion = objetoHospital.AnioConstruccion;
                        hospital.Capacidad = objetoHospital.Capacidad;
                        hospital.Especialidad = new ML.Especialidad();
                        hospital.Especialidad.IdEspecialidad = objetoHospital.IdEspecialidad;
                        hospital.Especialidad.Nombre = objetoHospital.TipoEspecialidad;
                        diccionario["Resultado"] = true;
                        diccionario["Hospital"] = hospital;
                    }
                    else
                    {
                        diccionario["Resultado"] = false;
                    }
                }
            }
            catch (Exception ex)
            {
                diccionario["Resultado"] = false;
                diccionario["Excepcion"] = ex.Message;
            }
            return diccionario;
        }
    }
}