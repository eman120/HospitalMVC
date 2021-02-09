using Hospital.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.Controllers
{
    public class PatientsController : Controller
    {
        private Context context;
        private readonly IWebHostEnvironment webHostEnvironment;
       
        public PatientsController(Context ctx, IWebHostEnvironment webHost)
        {
            context = ctx;
            webHostEnvironment = webHost;
        }

        [HttpGet]
        public ActionResult Index(string searchText, int pageSize, int PageNumber, string orderType)
        {
            if (orderType == "asc")
            {
                ViewBag.AscendingOrder = true;
                return View(context.Patients.OrderBy(e => e.FullName));
            }
            else if (orderType == "desc")
            {
                ViewBag.DescendingOrder = true;
                return View(context.Patients.OrderByDescending(e => e.FullName));
            }

            //pagination
            if (pageSize > 0 && PageNumber > 0)
            {
                ViewBag.CurrentPageSize = pageSize;
                ViewBag.CurrentPageNumber = PageNumber;
                return View(context.Patients.Skip(pageSize * (PageNumber - 1)).Take(pageSize));
            }

            //Search
            if (string.IsNullOrEmpty(searchText))
            {
                return View(context.Patients.Take(5));
            }
            else
            {
                ViewBag.CurrentSearch = searchText.Trim();
                return View(context.Patients.Where(p => p.FullName.Contains(searchText.Trim())));
            }
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            Patient pat = context.Patients.Include(d => d.Department).SingleOrDefault(p => p.Id == id);

            if (pat == null)
            {
                return NotFound();
            }
            return View(pat);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.All_Department = new SelectList(context.Departments, "Id", "Description");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Patient pat)
        {
            if (ModelState.IsValid)
            {
                string wwwrootPath = webHostEnvironment.WebRootPath;

                Guid imageGuid = Guid.NewGuid();

                string extension = Path.GetExtension(pat.NationalNumberCardFile.FileName);

                string imageFullName = imageGuid + extension;
                pat.NationalNumberCard = imageFullName;


                string imagePath = wwwrootPath + "/images/" + imageFullName;

                using (FileStream fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    pat.NationalNumberCardFile.CopyTo(fileStream);
                }

                //Prescription Image
                long fileLength = pat.PrescriptionImageFile.Length;
                var PrescriptionImageStream = pat.PrescriptionImageFile.OpenReadStream();
                byte[] PrescriptionImageBytes = new byte[fileLength];
                PrescriptionImageStream.Read(PrescriptionImageBytes, 0, Convert.ToInt32(pat.PrescriptionImageFile.Length));

                pat.PrescriptionImageName = Convert.ToBase64String(PrescriptionImageBytes);

                pat.CreationDate = DateTime.Now;
                context.Patients.Add(pat);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewBag.All_Department = new SelectList(context.Departments, "Id", "Description");
            Patient pat = context.Patients.FirstOrDefault(p => p.Id == id);

            if (pat == null)
            {
                return NotFound();
            }
            return View(pat);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Patient pat)
        {
            if (id != pat.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                string wwwrootPath = webHostEnvironment.WebRootPath;

                Guid imageGuid = Guid.NewGuid();

                string extension = Path.GetExtension(pat.NationalNumberCardFile.FileName);

                string imageFullName = imageGuid + extension;
                pat.NationalNumberCard = imageFullName;


                string imagePath = wwwrootPath + "/images/" + imageFullName;

                using (FileStream fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    pat.NationalNumberCardFile.CopyTo(fileStream);
                }

                //Prescription Image
                long fileLength = pat.PrescriptionImageFile.Length;
                var PrescriptionImageStream = pat.PrescriptionImageFile.OpenReadStream();
                byte[] PrescriptionImageBytes = new byte[fileLength];
                PrescriptionImageStream.Read(PrescriptionImageBytes, 0, Convert.ToInt32(pat.PrescriptionImageFile.Length));

                pat.PrescriptionImageName = Convert.ToBase64String(PrescriptionImageBytes);

                pat.UpdateDate = DateTime.Now;
                context.Patients.Update(pat);
                context.SaveChanges();

                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            Patient pat = context.Patients.FirstOrDefault(e => e.Id == id);

            if (pat == null)
            {
                return NotFound();
            }
            return View(pat);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConirmed(int id)
        {
            Patient pat = context.Patients.FirstOrDefault(e => e.Id == id);
            if (id != pat.Id)
            {
                return BadRequest();
            }

            context.Patients.Remove(pat);
            context.SaveChanges();

            string wwwrootPath = webHostEnvironment.WebRootPath;
            string imagePath = wwwrootPath + "/images/" + pat.NationalNumberCard;

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            return RedirectToAction("Index");
        }
    }
}
