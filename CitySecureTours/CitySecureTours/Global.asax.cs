﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security; //Add
using CitySecureTours.Data; // Add

namespace CityToursMVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            if (FormsAuthentication.CookiesSupported == true)
            {
                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    try
                    {
                        //let us take out the username now                 
                        string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                        string role = string.Empty;


                        using (ToursContainer1 entities = new ToursContainer1())
                        {
                            UserAccount user = entities.UserAccounts.SingleOrDefault(u => u.Username == username);


                            role = user.Role.Name;
                        }
                        //let us extract the roles from our own custom cookie 




                        //Let us set the Pricipal with our user specific details 
                        HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(
                          new System.Security.Principal.GenericIdentity(username, "Forms"), role.Split(';'));
                    }
                    catch (Exception)
                    {
                        //somehting went wrong 
                    }
                }
            }
        }

    }
}
