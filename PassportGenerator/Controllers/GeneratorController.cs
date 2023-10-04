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
                if(i.StatusName == "Approved" && generatorRepository.checkPassportGenerated(i.RegistrationId))
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
                    TempData["GeneratorSuccess"] = "<script>alert(Passport Generated success)</script>";
                    return View(list);
                }
                else
                {
                    return RedirectToAction("List");
                }
            }
            return View(list);
        }

        /// <summary>
        /// Showing the list of generated passport
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ListOfGeneratedPassport() 
        {
            //Get list of generated passport 
            var data = generatorRepository.GeneratedPassportList();
            //return view 
            return View(data);
        }

        /// <summary>
        /// User to view the details of the 
        /// generated passport and to know
        /// wheather passport generated or not 
        /// </summary>
        /// <returns></returns>
        public ActionResult UserGeneratedPassportList(string email)
        {
            //check if passport is available or not 
            //find registration id using email and check wheather available or not 
            var data = generatorRepository.GeneratedPassportListUser(email);

            //check passport generated ot not 
            // if not show it on the view as Passport not Generated
            //if generated show the details of perticular user
            return View(data);
        }
    }
}