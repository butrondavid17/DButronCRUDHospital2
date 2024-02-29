using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class HospitalController : Controller
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            Dictionary<string, object> objeto = BL.Hospital.GetAll();
            bool resultado = (bool)objeto["Resultado"];
            if (resultado)
            {
                ML.Hospital hospital = new ML.Hospital();
                hospital = (ML.Hospital)objeto["Hospital"];
                return View(hospital);
            }
            else
            {
                string excepcion = (string)objeto["Excepcion"];
                ViewBag.Mensaje = $"Ocurrio un error {excepcion}";
                return View(excepcion);
            }
        }
        [HttpGet]
        public IActionResult Form(int? IdHospital)
        {
            ML.Hospital hospital = new ML.Hospital();
            if (IdHospital != null)
            {
                Dictionary<string, object> objetoHospital = BL.Hospital.GetById(IdHospital.Value);
                bool resultadoHospital = (bool)objetoHospital["Resultado"];
                if (resultadoHospital)
                {
                    hospital = (ML.Hospital)objetoHospital["Hospital"];
                    Dictionary<string, object> objetoEspecialidad = BL.Especialidad.GetAll();
                    ML.Especialidad especialidad = (ML.Especialidad)objetoEspecialidad["Especialidad"];
                    hospital.Especialidad.Especialidades = especialidad.Especialidades;
                    return View(hospital);
                }
                else
                {
                    string excepcion = (string)objetoHospital["Excepcion"];
                    ViewBag.Mensaje = "Ocurrio un error " + excepcion;
                    return PartialView("Modal");
                }
            }
            else
            {
                hospital.Especialidad = new ML.Especialidad();
                Dictionary<string, object> resultEspecialidad = BL.Especialidad.GetAll();
                ML.Especialidad especialidades = (ML.Especialidad)resultEspecialidad["Especialidad"];
                hospital.Especialidad.Especialidades = especialidades.Especialidades;
                return View(hospital);
            }
        }
        [HttpPost]
        public IActionResult Form(ML.Hospital hospital)
        {
            if (hospital.IdHospital > 0)
            {
                Dictionary<string, object> objeto = BL.Hospital.Update(hospital);
                bool resultado = (bool)objeto["Resultado"];
                if (resultado)
                {
                    ViewBag.Mensaje = "El Hospital ha sido actualizado";
                    return PartialView("Modal");
                }
                else
                {
                    string excepcion = (string)objeto["Excepcion"];
                    ViewBag.Mensaje = $"Ocurrio un error {excepcion}";
                    return PartialView(excepcion);
                }
            }
            else
            {
                Dictionary<string, object> objeto = BL.Hospital.Add(hospital);
                bool resultado = (bool)objeto["Resultado"];
                if (resultado)
                {
                    ViewBag.Mensaje = "El hospital ha sido agregado";
                    return PartialView("Modal");
                }
                else
                {
                    string excepcion = (string)objeto["Excepcion"];
                    ViewBag.Mensaje = $"Ocurrio un error {excepcion}";
                    return PartialView("Modal");
                }
            }
        }
        [HttpGet]
        public IActionResult Delete(int IdHospital)
        {
            Dictionary<string, object> objeto = BL.Hospital.Delete(IdHospital);
            bool resultado = (bool)objeto["Resultado"];
            if (resultado)
            {
                ViewBag.Mensaje = "El hospital se ha eliminado";
                return PartialView("Modal");
            }
            else
            {
                string excepcion = (string)objeto["Excepcion"];
                ViewBag.Mensaje = "A ocurrido en error";
                return PartialView("Modal");
            }
        }
    }
}
