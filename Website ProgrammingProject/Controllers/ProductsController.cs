using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Drawing.Imaging;
using System.Web.Mvc;
using Website_ProgrammingProject.Models;
using System.Drawing;

namespace Website_ProgrammingProject.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        public PhoneDBEntities db = new PhoneDBEntities();
        // StorePhoneDbContext db = new StorePhoneDbContext(); 
        public ActionResult Index(string search = "", int PageNo = 1)
        {
            ViewBag.Search = search;
            List<Product> products = db.Products.Where(t => t.ProductName.Contains(search)).ToList();
            int NoOfRecordsPerPage = 5;
            int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(products.Count) / Convert.ToDouble(NoOfRecordsPerPage)));
            int NoOfRecordsToSkip = (PageNo - 1) * NoOfRecordsPerPage;
            ViewBag.PageNo = PageNo;
            ViewBag.NoOfPage = NoOfPages;
            products = products.Skip(NoOfRecordsToSkip).Take(NoOfRecordsPerPage).ToList();
            return View(products);
        }
        public ActionResult Create()
        {

            ViewBag.categories = db.Categories.ToList();
            ViewBag.Brands = db.Brands.ToList();
            return View();

        }
        private string ConvertImageToBase64(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                try
                {
                    byte[] imgBytes;
                    using (BinaryReader reader = new BinaryReader(file.InputStream))
                    {
                        imgBytes = reader.ReadBytes(file.ContentLength);
                    }
                    return Convert.ToBase64String(imgBytes);
                }
                catch (Exception ex)
                {
                    // Xử lý ngoại lệ nếu có lỗi trong quá trình chuyển đổi
                    // Ví dụ: Log lỗi hoặc trả về một giá trị mặc định.
                    return string.Empty;
                }
            }
            return string.Empty;
        }

        [HttpPost]
        public ActionResult Create(Product p)
        {
            if (Request.Files.Count >= 1)
            {
                var file = Request.Files[0];
                p.Image = ConvertImageToBase64(file);
            }

            db.Products.Add(p);
            db.SaveChanges();
            return RedirectToAction("index", "products");

        }


        public ActionResult Edit(long id)
        {

            Product exitstingProduct = db.Products.Where(t => t.ProductId == id).FirstOrDefault();
            ViewBag.Categories = db.Categories.ToList();
            ViewBag.Brands = db.Brands.ToList();
            return View(exitstingProduct);
        }


        [HttpPost]
        public ActionResult Edit(Product p)
        {
            Product existingProduct = db.Products.FirstOrDefault(t => t.ProductId == p.ProductId);
            ViewBag.Categories = db.Categories.ToList();
            ViewBag.Brands = db.Brands.ToList();

            if (existingProduct != null)
            {
                existingProduct.ProductName = p.ProductName;
                existingProduct.Price = p.Price;
                existingProduct.Display = p.Display;
                existingProduct.Chip = p.Chip;
                existingProduct.Ram = p.Ram;
                existingProduct.CategoryId = p.CategoryId;
                existingProduct.BrandId = p.BrandId;

                HttpPostedFileBase image = Request.Files["Image"];
                if (image != null && image.ContentLength > 0)
                {
                    byte[] imageBytes;
                    using (BinaryReader reader = new BinaryReader(image.InputStream))
                    {
                        imageBytes = reader.ReadBytes(image.ContentLength);
                    }

                    string base64ImageRepresentation = Convert.ToBase64String(imageBytes);

                    existingProduct.Image = base64ImageRepresentation;
                }

                db.SaveChanges();
            }

            return RedirectToAction("index", "products", "existingProduct");
        }


        public ActionResult Delete(long id)
        {

            Product exitstingProduct = db.Products.Where(t => t.ProductId == id).FirstOrDefault();
            return View(exitstingProduct);
        }
        [HttpPost]
        public ActionResult Delete(long id, Product p)
        {

            Product exitstingProduct = db.Products.Where(t => t.ProductId == id).FirstOrDefault();
            db.Products.Remove(exitstingProduct);
            db.SaveChanges();
            return RedirectToAction("index", "products");
        }
        public ActionResult Details(long id)
        {
            Product p = db.Products.Where(t => t.ProductId == id).FirstOrDefault();
            return View(p);
        }
    }
}