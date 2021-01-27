using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestFlight.Models;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace TestFlight.Controllers
{
    [Authorize (Roles = "Operator")]
    public class OperatorController : Controller
    {

        private ApplicationDbContext _DBdata;
        public OperatorController()
        {
            _DBdata = new ApplicationDbContext();
        }

        //Главная

        public ActionResult Main()
        {
            return View();
        }

        //ЛИСТ ЗАКАЗОВ

        public ActionResult OrderList(Int32? Id)
        {
            if (Id == 1)
            {
                var OrdersAct = _DBdata.Order.Where(o => o.OrderTime.Year == DateTime.Now.Year)
                                         .Where(o => o.OrderTime.Month == DateTime.Now.Month)
                                         .Where(o => o.OrderTime.Day == DateTime.Now.Day)
                                         .Where(o => o.Done == false)
                                         .ToList();

                return View(OrdersAct);
            }
            else if (Id == 2)
            {
                var Yest = new DateTime();
                Yest = DateTime.Now.AddDays(-1);
                var OrdersYest = _DBdata.Order.Where(o => o.OrderTime.Year == Yest.Year)
                                         .Where(o => o.OrderTime.Month == Yest.Month)
                                         .Where(o => o.OrderTime.Day == Yest.Day)
                                         .ToList();

                return View(OrdersYest);
            }
            else if (Id == 3)
            {
                var OrdersAll = _DBdata.Order.ToList();
                return View(OrdersAll);
            }

            var Orders = _DBdata.Order.Where(o => o.OrderTime.Year == DateTime.Now.Year)
                                   .Where(o => o.OrderTime.Month == DateTime.Now.Month)
                                   .Where(o => o.OrderTime.Day == DateTime.Now.Day)
                                   .ToList();

            return View(Orders);
        }

        public ActionResult OrderDetails(Int32 Id)
        {
            var Order = _DBdata.Order.SingleOrDefault(o => o.Id == Id);
            if (Order == null) return HttpNotFound();

            return View(Order);
        }

        public ActionResult ShowBasket(Int32 Id)
        {
            var Order = _DBdata.Order.Include(o => o.OrderContent)
                                     .SingleOrDefault(o => o.Id == Id);
            if (Order == null) return HttpNotFound();

            return View(Order.OrderContent);
        }

        public ActionResult Done(Int32 Id)
        {
            var OrderMove = _DBdata.Order.SingleOrDefault(o => o.Id == Id);
            if (OrderMove == null) return HttpNotFound();

            OrderMove.Done = true;
            _DBdata.SaveChanges();

            return RedirectToAction("OrderList", "Operator");
        }

        public ActionResult Delete(Int32 Id)
        {
            var OrderToDelete = _DBdata.Order.Include(o => o.OrderContent)
                                            .SingleOrDefault(o => o.Id == Id);
            if (OrderToDelete == null) return HttpNotFound();

            _DBdata.Order.Remove(OrderToDelete);
            _DBdata.SaveChanges();

            return RedirectToAction("OrderList", "Operator");
        }

        //НОВОСТИ

        public ActionResult NewsList()
        {
            var News = _DBdata.News.ToList();
            return View("NewsList", News);
        }

        public ActionResult AddNews()
        {
            var NewUp = new NewsUpload();
            return View("NewsForm", NewUp);
        }

        public ActionResult NewsForm(NewsUpload NewUp)
        {
            if (!ModelState.IsValid)
            {
                return View("NewsForm", NewUp);
            }

            var NewsIn = new News();
            NewsIn.Header = NewUp.News.Header;
            NewsIn.Body = NewUp.News.Body;
            NewsIn.Date = DateTime.Now;

            if (NewUp.File != null)
            {
                string photoName = Path.GetFileName(NewUp.File.FileName);
                NewUp.File.SaveAs(Server.MapPath("/Images/News/" + photoName));
                NewsIn.Photo = photoName;
            }
            _DBdata.News.Add(NewsIn);
            _DBdata.SaveChanges();

            return RedirectToAction("NewsList", "Operator");
        }

        public ActionResult DeleteNews(Int32 id)
        {
            News NewOut = _DBdata.News.SingleOrDefault(n => n.Id == id);
            if (NewOut == null) return HttpNotFound();

            if (System.IO.File.Exists(Server.MapPath("/Images/News/") + NewOut.Photo))
            {
                System.IO.File.Delete(Server.MapPath("/Images/News/") + NewOut.Photo);
            }

            _DBdata.News.Remove(NewOut);
            _DBdata.SaveChanges();

            return RedirectToAction("NewsList", "Operator");
        }

        //ВАКАНСИИ

        public ActionResult VacanciesList()
        {
            var Vacancies = _DBdata.Vacancies.ToList();
            return View(Vacancies);
        }

        public ActionResult AddVacancy()
        {
            var vacancy = new Vacancy();
            return View("VacanciesForm", vacancy);
        }

        public ActionResult VacanciesForm(Vacancy vacancyUp)
        { 
            if (!ModelState.IsValid)
            {
                return View("VacancyForm", vacancyUp);
            }

            var VacancyIn = new Vacancy();

            VacancyIn.Title = vacancyUp.Title;
            VacancyIn.Shedule = vacancyUp.Shedule;
            VacancyIn.Requirements = vacancyUp.Requirements;
            VacancyIn.Salary = vacancyUp.Salary;
            VacancyIn.Date = DateTime.Now;

            _DBdata.Vacancies.Add(VacancyIn);
            _DBdata.SaveChanges();

            return RedirectToAction("VacanciesList", "Operator");
        }

        public ActionResult DeleteVacancy(Int32 id)
        {
            var VacancyOut = _DBdata.Vacancies.SingleOrDefault(v => v.Id == id);
            if (VacancyOut == null) return HttpNotFound();

            _DBdata.Vacancies.Remove(VacancyOut);
            _DBdata.SaveChanges();

            return RedirectToAction("VacanciesList", "Operator");
        }

        //ОТЗЫВЫ
       
        public ActionResult ReviewList()
        {
            var Reviews = _DBdata.Review.ToList();
            return View(Reviews);
        }

        public ActionResult ConfirmReview(Int32 id)
        {
            _DBdata.Review.SingleOrDefault(r => r.Id == id).Valid = true;
            _DBdata.SaveChanges();
            return RedirectToAction("ReviewList", "Operator");
        }

        public ActionResult DeleteReview(Int32 id)
        {
            var ReviewOut = _DBdata.Review.SingleOrDefault(r => r.Id == id);
            if (ReviewOut == null) return HttpNotFound();

            _DBdata.Review.Remove(ReviewOut);
            _DBdata.SaveChanges();

            return RedirectToAction("ReviewList", "Operator");
        }

        protected override void Dispose(bool disposing)
        {
            _DBdata.Dispose();
        }
    }
}