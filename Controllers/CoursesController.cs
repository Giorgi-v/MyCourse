using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyCourse.Models.Services.Application;
using MyCourse.Models.ViewModels;

namespace MyCourse.Controllers
{
    public class CoursesController : Controller
    {
        public CoursesController()
        {
        }


        //metodo per recuperare la lista di tutti i corsi
        public IActionResult Index() 
        {
            var courseService = new CourseService(); //invocazione del servizio
            List<CourseViewModel> courses = courseService.GetCourses();
            ViewData["Title"] = "Elenco dei corsi";
            return View(courses); //ritorna la lista di tutti i corsi
        }

        //metodo che deve recuperare le info dello specifico corso avente un certo id
        public IActionResult Detail(int id)
        {
            var courseService = new CourseService(); //invocazione del servizio
            CourseDetailViewModel viewModel = courseService.GetCourse(id);
            ViewData["Title"] = viewModel.Titolo;
            return View(viewModel);
            //return Content($"Sono Detail, ho ricevuto l'id {id}");
            //return Redirect("https://www.amazon.it/");
        }

        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}