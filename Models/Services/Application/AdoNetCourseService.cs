using System;
using System.Collections.Generic;
using System.Linq;
using MyCourse.Models.Services.Infrastractures;
using MyCourse.Models.ViewModels;
using System.Data;

namespace MyCourse.Models.Services.Application
{
    //servizio applicativo che ha il compito di recuperare dati dal database
    //tramite l'utilizzo del servizio infrastrutturale IDatabaseAccessor
    public class AdoNetCourseService : ICourseService
    {
        private readonly IDatabaseAccessor db; //proprietà che deve essere iniettata nel servizio applicativo
        public AdoNetCourseService(IDatabaseAccessor db) //dependency injection
        {
            this.db = db;
        }
        public List<CourseViewModel> GetCourses()
        {
            string query = "SELECT Id, Title,ImagePath, Author,Rating,FullPrice_Amount, FullPrice_Currency, CurrentPrice_Amount, CurrentPrice_Currency  FROM Courses";
            DataSet dataSet = db.Query(query);
            var dataTable = dataSet.Tables[0]; //recupera la prima tabella del dataSet
            var courseList = new List<CourseViewModel>(); //crea la lista di corsi da passare alla View

            //per ogni riga della dataTable, deve creare un oggetto di tipo CourseView>Model
            foreach(DataRow courseRow in dataTable.Rows)
            {
                CourseViewModel course = CourseViewModel.FromDataRow(courseRow); //metodo in CourseViewModel.cs
                courseList.Add(course);
            }
            return courseList;

        }

        public CourseDetailViewModel GetCourse(int id)
        {
            string query = "SELECT Id, Title, Description, ImagePath, Author, Rating, FullPrice_Amount, FullPrice_Currency, CurrentPrice_Amount, CurrentPrice_Currency FROM Courses WHERE Id=" + id +
            "; SELECT Id, Title, Description, Duration FROM Lessons WHERE CourseId=" + id; //due query
            DataSet dataSet = db.Query(query);
            var courseTable = dataSet.Tables[0]; //recupera la prima tabella del dataSet: i dati del corso
            
            //dato che la prima query mi ritorna esattamente una riga, controllo se effettivamente la tabella contiene una sola riga
            //tramite Rows.Count
            if (courseTable.Rows.Count != 1) {
                throw new InvalidOperationException($"Did not return exactly 1 row for Course {id}");
            }
            //recupero i dati della prima riga della tabella -> quindi i dati del corso con id recuperato
            var courseRow = courseTable.Rows[0];
            //creo l'oggetto ViewModel popolandolo con i dati recuperati da db
            var courseDetailViewModel = CourseDetailViewModel.FromDataRow(courseRow);

            var lessonTable = dataSet.Tables[1]; //recupera la seconda tabella del dataSet: i dati delle lezioni del corso (risultato della seconda query)
            //per ogni riga (lezione) presente nella tabella crea una lezione (oggetto LessonViewModel)
            //popolandola con i dati letti da db
            foreach(DataRow lessonRow in lessonTable.Rows)
            {
                LessonViewModel lessonViewModel = LessonViewModel.FromDataRow(lessonRow); //metodo in CourseViewModel.cs
                //aggiungi la lezione alla lista di lezioni del corso
                courseDetailViewModel.Lezioni.Add(lessonViewModel);
            }

            return courseDetailViewModel;
        }

        /*
        List<CourseViewModel> Ricerca(string cerca)
        {

        }*/
        
    }
}