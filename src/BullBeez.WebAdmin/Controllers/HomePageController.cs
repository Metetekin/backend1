﻿using BullBeez.WebAdmin.Classed;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BullBeez.WebAdmin.Controllers
{
    [CustomAuthorizeAttribute]
    public class HomePageController : Controller
    {
        // GET: HomePage
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Users");
        }
    }
}