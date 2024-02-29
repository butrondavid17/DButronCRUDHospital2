using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Especialidad
    {
        public static Dictionary<string, object> GetAll()
        {
            ML.Especialidad especialidad = new ML.Especialidad();
            string excepcion = "";
            Dictionary<string, object> diccionario = new Dictionary<string, object> { { "Especialidad", especialidad }, { "Excepcion", excepcion }, { "Resultado", false } };
            try
            {
                using (DL.DbutronHospitalContext context = new DL.DbutronHospitalContext())
                {
                    var listaEspecialidades = (from tablaEspecialidad in context.Especialidads
                                               select new
                                               {
                                                   IdEspecialidad = tablaEspecialidad.IdEspecialidad,
                                                   Nombre = tablaEspecialidad.Nombre
                                               }).ToList();
                    if (listaEspecialidades != null)
                    {
                        especialidad.Especialidades = new List<object>();
                        foreach (var registro in listaEspecialidades)
                        {
                            ML.Especialidad especialidad1 = new ML.Especialidad();
                            especialidad1.IdEspecialidad = registro.IdEspecialidad;
                            especialidad1.Nombre = registro.Nombre;
                            especialidad.Especialidades.Add(especialidad1);
                        }
                        diccionario["Resultado"] = true;
                        diccionario["Especialidad"] = especialidad;
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
