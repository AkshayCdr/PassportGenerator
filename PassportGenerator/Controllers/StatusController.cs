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

        //public ActionResult List()
        //{
        //    return View();
        //}

        /// <summary>
        /// To Track the Status of the Passport
        /// Application
        /// User module
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public ActionResult DetailsofUser(string email)
        {
            var data = statusRepository.GetDetails(email);
            return View(data);
        }

        /// <summary>
        /// To view and Set the status 
        /// Admin module - to approve
        /// </summary>
        /// <returns></returns>
        public ActionResult Details()
        {
            var data = statusRepository.GetDetils();
            return View(data);
        }

       

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