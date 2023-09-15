using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Website_ProgrammingProject.Models;

namespace Website_ProgrammingProject.Controllers
{
    public class BrandsController : Controller
    {
        // GET: Brands
        public ActionResult Index(string search="",int PageNo = 1)
        {
            PhoneDBEntities db = new PhoneDBEntities();
            ViewBag.Search = search;
            List<Brand> brands = db.Brands.Where(t => t.BrandName.Contains(search)).ToList();
            int NoOfRecordsPerPage = 5;
            int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(brands.Count) / Convert.ToDouble(NoOfRecordsPerPage)));
            int NoOfRecordsToSkip = (PageNo - 1) * NoOfRecordsPerPage;
            ViewBag.PageNo = PageNo;
            ViewBag.NoOfPage = NoOfPages;
            brands = brands.Skip(NoOfRecordsToSkip).Take(NoOfRecordsPerPage).ToList();
            return View(brands);
        }
    }
}