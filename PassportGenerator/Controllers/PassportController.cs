using PassportGenerator.Models;
using PassportGenerator.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PassportGenerator.Controllers
{
    public class PassportController : Controller
    {

        PassportRepository passportRepository = new PassportRepository();

        /// <summary>
        /// View
        /// List of users applied fot passport 
        /// data from passportdata table
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            var data = passportRepository.GetUsers();
            return View(data);
        }


        /// <summary>
        /// To Get details of user from  
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public ActionResult UserList(string email)
        {
            var data = passportRepository.GetUsers(email);
            return View(data);
        }

        /// <summary>
        /// View
        /// To create new passport application
        /// in passport data table
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }


        /// <summary>
        /// To create passport application
        /// containing details 
        /// photo and certificates
        /// </summary>
        /// <param name="passport"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult Create(Passport passport)
        {
            if (ModelState.IsValid)
            {
                var registrationId = passportRepository.getRegistrationId(passport.Email);

                //check wheather registration id is already in passport data
                if (passportRepository.checkIdPassportData(registrationId))
                {
                    if (passportRepository.checkRegistrationId(registrationId))
                    {
                        if (passportRepository.InsertUsers(passport))
                        {
                            ViewBag.Message = "User successfully saved";
                            TempData["PassportMessage"] = "Passport Data saved";
                            return RedirectToAction("UserList", new { email = passport.Email });
                        }
                    }
                    else
                    {
                        ViewBag.Message = "User Already applied";
                        return View();
                    }
                }
            }
            return View();
        }


        /// <summary>
        /// Select data to edit 
        /// and show it in view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            var data = passportRepository.GetUsers().Find(passport => passport.Id == id);
            if (data != null)
            {
                return View(data);
            }
            else
            {
                return HttpNotFound();
            }

        }

        /// <summary>
        /// Edit passport application data 
        /// </summary>
        /// <param name="passport"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(Passport passport)
        {
            if (ModelState.IsValid)
            {
                if (passportRepository.UpdateUser(passport))
                {
                    return RedirectToAction("UserList", new { email = passport.Email });
                }
                else
                {
                    return RedirectToAction("UserList", new { email = passport.Email });
                }
            }
            return View();
        }


        /// <summary>
        /// Delete passport application data
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            string email = User.Identity.Name;
            if (passportRepository.DeleteUser(id,email))
            {
                return RedirectToAction("UserList",new { email = User.Identity.Name});
            }
            else
            {
                return RedirectToAction("UserList", new {email = User.Identity.Name});
            }
        }
    }
}