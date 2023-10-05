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
            //var passportOfficeNames = new List<string>
            //{
            //    "Kannur", "Kozhikode", "Malappuram", "Thrissur", "Wayanad",
            //    "Ernakulam", "Palakkad", "Kollam", "Trivandrum"
            //};
            var passportOfficeNames = passportRepository.getOfficeNames();

            // Pass the list to the view
            ViewBag.PassportOfficeNames = new SelectList(passportOfficeNames);
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
                            TempData["PassportMessage"] = "<script>alert('Passport Data saved.')</script>";
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
            var passportOfficeNames = passportRepository.getOfficeNames();
            ViewBag.PassportOfficeNames = new SelectList(passportOfficeNames);
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
                    TempData["PassportEdit"] = "Passport Data Edited Succesfully";
                    return RedirectToAction("UserList", new { email = passport.Email });
                }
                else
                {
                    TempData["PassportEditFail"] = "Passport Data Update failed";
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
            if (passportRepository.DeleteUser(id, email))
            {
                TempData["PassportDelete"] = "<script>alert('Passport Data Deleted Succesfully.')</script>";
                return RedirectToAction("UserList", new { email = User.Identity.Name });
            }
            else
            {
                TempData["PassportDeleteError"] = "<script>alert('Passport Data Deletion Error.')</script>";
                return RedirectToAction("UserList", new { email = User.Identity.Name });
            }
        }


        /// <summary>
        /// Setting for adding passport office names
        /// </summary>
        /// <returns></returns>
        public ActionResult Settings()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Settings(string PassportOfficeName)
        {
            if(passportRepository.addOfficeName(PassportOfficeName))
            {
                TempData["PassofficeNameSucc"] = "Added successfully";
                return View();
            }
            else
            {
                TempData["PassofficeNameFail"] = "Failed to add";
            }

            return View();
        }

    }
}