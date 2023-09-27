using PassportGenerator.Models;
using PassportGenerator.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PassportGenerator.Controllers
{
    public class GeneratorController : Controller
    {
        GeneratorRepository generatorRepository = new GeneratorRepository();
        /// <summary>
        /// Showing list of users to Generate passport
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            var data = generatorRepository.GetPassportDetails();
            List<Generator> list = new List<Generator>();
            //only list data to generate if it is approved
            foreach (Generator i in data)
            {
                if(i.StatusName == "Approved")
                {
                    list.Add(i);
                }
            }
            return View(list);
        }

        /// <summary>
        /// Show passport after Generating passport
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GeneratePassport(int id) 
        {
            var data = generatorRepository.GetPassportDetails().Find(a => a.RegistrationId == id);
            var list = data;
            //only geneate if approved 
            if (data.StatusName == "Approved")
            {
                if (generatorRepository.GeneratePassport(data))
                {
                    return View(list);
                }
                else
                {
                    return RedirectToAction("List");
                }
            }
            return View(list);
        }
    }
}