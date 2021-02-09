using Hospital.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.Views.Departments
{
    public class DepartmentsController : Controller
    {
        private readonly Context context;
        public DepartmentsController(Context cont)
        {
            context = cont;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(context.Departments);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            Department dept = context.Departments.Include(d => d.Patients).FirstOrDefault(d => d.Id == id);
            if (dept == null)
            {
                return NotFound();
            }
            return View(dept);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]//To prevent CSRF(cross site request forgery) Attacks
        public ActionResult Create(Department dept)
        {
            if (ModelState.IsValid)
            {
                context.Departments.Add(dept);
                context.SaveChanges();

                return RedirectToAction("Index"); //return to index again
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Department dept = context.Departments.FirstOrDefault(e => e.Id == id);
            if (dept == null)
            {
                return NotFound();
            }
            return View(dept);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            Department dept = context.Departments.FirstOrDefault(d => d.Id == id);
            if (dept == null)
            {
                return NotFound();
            }
            return View(dept);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Department dept = context.Departments.FirstOrDefault(d => d.Id == id);
            if (dept == null)
            {
                return BadRequest();
            }
            context.Departments.Remove(dept);
            context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
