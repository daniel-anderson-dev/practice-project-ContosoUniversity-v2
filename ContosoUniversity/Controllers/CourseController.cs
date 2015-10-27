using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using System.Data.Entity.Infrastructure;


namespace ContosoUniversity.Controllers
{

	public class CourseControll : Controller
	{
		
		private SchoolContext db = new SchoolContext();

		public ActionResult Index(int? SelectedDepartment)
		{
			var departments = db.Departments.OrderBy(department => department.Name).ToList();
			ViewBag.SelectedDepartment = new SelectList(departments, "DepartmentID", "Name", SelectedDepartment)
			int departmentID = SelectedDepartment.GetValueOrDefault();

			IQueryable<Course> courses = db.Courses
				.Where(c => !SelectedDepartment.HasValue || c.DepartmentID == departmentID)
				.OrderBy(d => d.CourseID)
				.Include(d => d.Department);

			// Probably doesn't have any significance.
			var sql = courses.ToString();

			return View(courses.ToList());
		}


		public ActionResult Details(int? id)
		{
			if (id == null)
			{ return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

			Course course = db.Courses.Find(id);
			if (course == null)
			{ return HttpNotFound(); }

			return View(course);
		}


		public ActionResult Create()
		{
			PopulateDepartmentsDropDownList();
			return View();
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "CourseID,Title,Credits,DepartmentID")] Course course)
		{
			try
			{
				if (ModelState.IsValid)
				{
					db.Courses.Add(course);
					db.SaveChanges();
					return RedirectToAction("Index");
				}
			}
			catch (RetryLimitExceededException)
			{ ModelState.AddModelError("", "Failed."); }

			PopulateDepartmentsDropDownList(course.DepartmentID);
			return View(course);
		}


		public ActionResult Edit(int? id)
		{
			if (id == null)
			{ return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

			var course = db.Courses.Find(id);

			if (course == null)
			{
				return HttpNotFound();
			}

			PopulateDepartmentsDropDownList(course.DepartmentID);
			return View(course);
		}


		[HttpPost, ActionName("Edit")]
		[ValidateAntiForgeryToken]
		public ActionResult EditPost(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var coursetoUdpate = db.Courses.Find(id);

			if (TryUpdateModel(coursetoUdpate, "", new string[] { "Title", "Credits", "DepartmentID" }))
			{
				try
				{
					db.SaveChanges();
					return RedirectToAction("Index");
				}
				catch (RetryLimitExceededException)
				{
					ModelState.AddModelError("", "Failed.");
				}
			}

			PopulateDepartmentsDropDownList(coursetoUdpate.DepartmentID);
			return View(coursetoUdpate);
		}


		private void PopulateDepartmentsDropDownList(object selectedDepartment = null)
		{
			var departmentsQuery = from d in db.Departments
									  orderby d.Name
									  select d;
			ViewBag.DepartmentID = new SelectList(departmentsQuery, "DepartmentID", "Name", selectedDepartment);
		}


		public ActionResult Delete(int? id)
		{
			if (id == null)
			{ return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

			var course = db.Courses.Find(id);
			if (course == null)
			{ return HttpNotFound(); }

			return View(course);

		}


		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			var course = db.Courses.Find(id);
			db.Courses.Remove(course);
			db.SaveChanges();
			return RedirectToAction("Index");
		}
		

		public ActionResult UpdateCourseCredits()
		{
			return View();
		}

		[HttpPost]
		public ActionResult UpdateCourseCredits(int? multiplier)
		{
			if (multiplier != null)
			{
				ViewBag.RowsAffected = db.Database.ExecuteSqlCommand("Update Course SET Credits = Credits * {0}", multiplier);
			}
			return View();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}

	}

}
