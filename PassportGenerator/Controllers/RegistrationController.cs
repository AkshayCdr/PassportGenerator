using Org.BouncyCastle.Cms;
using PassportGenerator.Models;
using PassportGenerator.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PassportGenerator.Controllers
{
    public class RegistrationController : Controller
    {
        RegistrationRepository registrationRepository = new RegistrationRepository();


        /// <summary>
        /// View
        /// To show data of registered users
        /// from Registration table
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles ="Admin")]
        public ActionResult List()
        {
            var data = registrationRepository.GetUsers();
            return View(data);
        }

        /// <summary>
        /// Create view 
        /// To create new Registration user
        /// view to create new user
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Insert user details in database
        /// inset to registration table
        /// </summary>
        /// <param name="registration"></param>
        /// <returns></returns>
        //[Authorize(Roles ="Admin")]
        [HttpPost]
        public ActionResult Create(Registration registration)
        {
            if (ModelState.IsValid)
            {
                if (registrationRepository.InsertUser(registration))
                {
                    TempData["InsertMsg"] = "<script>alert('User saved successfully.')</script>";
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    TempData["InsertErrorMsg"] = "<script>alert('Error')</script>";
                }
            }
            return View(registration);
        }


        /// <summary>
        /// Edit view
        /// To select data to edit and 
        /// show data in the view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Edit(int id)
        {
            var data = registrationRepository.GetUsers().Find(a => a.Id == id);
            if (data != null)
            {
                data.Dob = data.Dob.Date;
                return View(data);
            }
            else
            {
                return HttpNotFound();
            }
        }

        /// <summary>
        /// Save the edited data in the 
        /// registration table 
        /// </summary>
        /// <param name="registration"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult Edit(Registration registration)
        {
            if (ModelState.IsValid)
            {
                if (registrationRepository.UpdateUser(registration))
                {
                    TempData["UpdateMsg"] = "<script>alert('User updated successfully.')</script>";
                    //return RedirectToAction("Details");
                    return RedirectToAction("Details", new { email = registration.Email });

                }
                else
                {
                    TempData["UpdateErrorMsg"] = "<script>alert('Error')</script>";
                }
            }
           
            return View(registration);
        }

        /// <summary>
        /// Delete the specific user data from the database 
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Delete(int id)
        {
            int result = registrationRepository.DeleteUser(id);
            if (result > 0)
            {
                TempData["DeleteMsg"] = "<script>alert('User Deleted sucessfully.....')</script>";
                return RedirectToAction("List");
            }
            else
            {
                TempData["DeleteErrorMsg"] = "<script>alert('Error Deleting')</script>";

            }
            return View();
        }

        /// <summary>
        /// To show profile of the user
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public ActionResult Details(string email)
        {
            int id = registrationRepository.findRegistrationId(email);
            var data = registrationRepository.GetUsers().Find(a => a.Id == id);
            return View(data);
        }

        /// <summary>
        /// To add admin previlage to the user 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public ActionResult AddRole(string email)
        {
            //find id from email
            int id = registrationRepository.GetUserIdFromEmail(email);

            //check wheather user id is already present in the code and retrive the user role
            UserRole userrole = new UserRole();
            userrole = registrationRepository.CheckUserIdUserRole(id);
            if(userrole == null)
            {
                userrole = new UserRole
                {
                    UserId = id,
                    Role = "Admin"
                };
                registrationRepository.InsertIntoRole(userrole);
                return RedirectToAction("RoleView");
            }
            else
            {
                return RedirectToAction("RoleView");
            }         
        }

        /// <summary>
        /// To remove the admin previlage of the user
        /// delete the role from the user role table
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public ActionResult RemoveRole(string email)
        {
            int id = registrationRepository.GetUserIdFromEmail(email);
            UserRole userrole = new UserRole();
            userrole = registrationRepository.CheckUserIdUserRole(id);
            if(userrole == null)
            {
                return RedirectToAction("RoleView");
            }
            else
            {
                registrationRepository.removeRole(userrole);
                return RedirectToAction("RoleView");
            }
        }


        /// <summary>
        /// To view the Registration table with the UsersRole
        /// Left join the registration table with userrole table 
        /// </summary>
        /// <returns></returns>
        public ActionResult RoleView()
        {
            var data = registrationRepository.GetUsersAndRoles();
            return View(data);
        }  
    }
}