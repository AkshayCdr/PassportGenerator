using PassportGenerator.Models;
using PassportGenerator.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PassportGenerator.Controllers
{
    public class StatusController : Controller
    {

        StatusRepository statusRepository = new StatusRepository();

        public ActionResult List()
        {
            return View();
        }

        
        public ActionResult DetailsofUser(string email)
        {
            var data = statusRepository.GetDetails(email);
            return View(data);
        }

        public ActionResult Details()
        {
            var data = statusRepository.GetDetils();
            return View(data);
        }

        //public ActionResult Edit(int id)
        //{
        //    var data = statusRepository.GetDetils().Find(a => a.RegistrationId == id);
        //    return View(data);
        //}

        //[HttpPost]
        //public ActionResult Edit(Status status)
        //{
        //    if (statusRepository.EditDetils(status))
        //    {
        //        RedirectToAction("Details", "Status");
        //    }
        //    return View();
        //}

        /// <summary>
        /// Set the status of 
        /// passport application to approve 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ChangeStatus(int id)
        {
            if (statusRepository.changeStatusToApproved(id))
            {
                return RedirectToAction("Details");
            }
            return RedirectToAction("Details");
        }

        /// <summary>
        /// set the status of 
        /// passport application to pending
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ChangeToPending(int id)
        {
            if (statusRepository.removeApproval(id))
            {
                return RedirectToAction("Details");
            }
            return RedirectToAction("Details");
        }
    }
}