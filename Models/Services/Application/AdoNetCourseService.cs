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

        /*
        CourseDetailViewModel GetCourse(int id)
        {

        }
        List<CourseViewModel> Ricerca(string cerca)
        {

        }*/
        
    }
}