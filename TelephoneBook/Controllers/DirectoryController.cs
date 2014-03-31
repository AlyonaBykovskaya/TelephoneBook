using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TelephoneBook.Models;


namespace TelephoneBook.Controllers
{
    public class DirectoryController : Controller
    {
        //
        // GET: /Directory/

        private TelephoneDatabaseEntities db = new TelephoneDatabaseEntities();

        public ActionResult ShowData()
        {

            var persons = db.Phones.Include(n => n.Person);

            return View(persons.ToList());


        }
        [HttpGet]
        public ActionResult AddData()
        {
            var p = new Person() {Phones = new List<Phone>()};
            return View(p);
        }
        [HttpPost]
        public ActionResult AddData(Person person)
        {
            
            try
            {

                if (ModelState.IsValid)
                {   
                    var p = new Person();
                    p.Name = person.Name;
                    p.Surname = person.Surname;
                    foreach (var par in Request.Form.AllKeys)
                    {
                    if(par.Contains("Phone"))    
                        p.Phones.Add(new Phone{Telephone = Request.Form[par]});
                    }
                    
                    db.Persons.Add(p);
                   
                    db.SaveChanges();
                    return RedirectToAction("ShowData");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex);
            }

            return View();


        }
        [HttpGet]
        public ActionResult EditData(int? id)
        {

            Phone phone = db.Phones.Single(s => s.Id == id);

            return View(phone);
        }

        [HttpPost]
        public ActionResult EditData(Phone phone)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dbPhone = db.Phones.Where(x => x.Id == phone.Id).FirstOrDefault();
                    if (dbPhone == null) return RedirectToAction("ShowData");
                    dbPhone.Person.Name = phone.Person.Name;
                    dbPhone.Person.Surname = phone.Person.Surname;
                    dbPhone.Person = phone.Person;
                    dbPhone.Telephone = phone.Telephone;

                  

                    db.SaveChanges();
                    return RedirectToAction("ShowData");
                }


            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex);
            }

            return View();
        }

        public ActionResult Delete(int? id)
        {

            Phone phone = db.Phones.Single(s => s.Id == id);

            return View(phone);

        }

        [HttpPost]
        public ActionResult Delete(Phone phone)
        {
            try
            {
                var dbPhone = db.Phones.Where(x => x.Id == phone.Id).FirstOrDefault();
                if (dbPhone == null) return RedirectToAction("ShowData");
                db.Phones.Remove(dbPhone);
                db.SaveChanges();

                return RedirectToAction("ShowData");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex);
                return View();
            }

        }

    }

}
