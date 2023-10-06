using PassportGenerator.Models;
using PassportGenerator.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PassportGenerator.Controllers
{
    public class PoliceController : Controller
    {
        PoliceRepository policeRepository = new PoliceRepository();

        /// <summary>
        /// To show the list of passport applications
        /// User - police
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {

            var data = policeRepository.GetPassportApplications();
            List<Police> polices = new List<Police>();
            foreach (var application in data)
            {
                if(!policeRepository.CheckPoliceApproval(application.RegistrationId))
                {
                    polices.Add(application);
                }
            }
            //only display data which is approved by admin 
            //dont display data which is already approved by police 
            return View(polices);
        }

        
        /// <summary>
        /// To approve the person by police 
        /// User - police 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Approve(int id) 
        { 
            //Check if admin approved
            //if admin approved then 
            //change polic status to approve
            if(policeRepository.PoliceStatusApprove(id))
            {
                TempData["PoliceApprove"] = "Police Approved";
                return RedirectToAction("List");
            }
            TempData["PoliceApproveError"] = "Police Approve Error";
            return RedirectToAction("List");
        }
    }
}