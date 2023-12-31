﻿using PassportGenerator.Models;
using PassportGenerator.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace PassportGenerator.Controllers
{
    public class LoginController : Controller
    {
        LoginRepository loginRepository = new LoginRepository();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// Login funtion check registration table wheather the username and password is available or not 
        /// redirect funtion not working (used refresh page in ajax) 
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(Login login)
        {
            var data = loginRepository.GetUsers();
            bool flag = false;
            foreach (var user in data)
            {
                if (user.Email == login.Email && user.Password == login.Password)
                {
                    flag = true;
                    TempData["LoginMessage"] = "User login successfull";
                    break;
                }
                //using the BC                     //Here check password                  //database 
                //if (user.Email == login.Email && BCrypt.Net.BCrypt.Verify(login.Password, user.Password))
                //{
                //    flag = true;
                //    TempData["LoginMessage"] = "User login successfull";
                //    break;
                //}
                //string password = login.Password;
                //string hashedEnteredPassword = HashString(password);
                //if (user.Email == login.Email && string.Equals(hashedEnteredPassword, user.Password, StringComparison.OrdinalIgnoreCase))
                //{
                //    flag = true;
                //    TempData["LoginMessage"] = "User login successfull";
                //    break;
                //}
                else
                {
                    TempData["LoginMessage"] = "User not Registered";
                }
            }


            if (flag)
            {
                FormsAuthentication.SetAuthCookie(login.Email, false);

                return RedirectToAction("Index", "Home");
            }
            else
            {

                return RedirectToAction("Create", "Registration");
            }
        }

        /// <summary>
        /// Logout function
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

        public ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgetPassword(Registration registration)
        {
            //if (ModelState.IsValid)
            //{
                var data = loginRepository.GetUsers();
                foreach (var user in data)
                {
                    if (user.Email == registration.Email)
                    {
                        if (loginRepository.ChangePassword(registration))
                        {
                            TempData["ChangePassSucc"] = "Password Changed successfully";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["ChangePassError"] = "Password Change failed";
                            return View();
                        }
                    } 
                }
                TempData["PasswordChangeUserNotExist"] = "User not registered";
                return View();

            //}

            //return View();


        }


        /// <summary>
        /// To hash Password into SHA-256
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        static string HashString(string input)
        {
            // Convert the input string to bytes
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            using (SHA256 sha256 = SHA256.Create())
            {
                // Compute the hash value from the input bytes
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                // Convert the byte array to a hexadecimal string
                string hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                return hashString;
            }
        }

    }
}