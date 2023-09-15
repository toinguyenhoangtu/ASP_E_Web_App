using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website_ProgrammingProject.Models;

namespace Website_ProgrammingProject.Controllers
{
    public class CategoriesController : Controller
    {
        // GET: Categories
        public ActionResult Index()
        {
            PhoneDBEntities db =new PhoneDBEntities();
            List<Category> Category = db.Categories.ToList();
            return View(Category);
        }
    }
}