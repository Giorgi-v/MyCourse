using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using MyCourse.Models.ValueTypes;
using MyCourse.Models.Enums;

namespace MyCourse.Models.ViewModels
{
    public class LessonViewModel   
    {
        public int Id{get; set;}
        public string Titolo {get; set;}
        public TimeSpan Durata {get; set;}    

        public static LessonViewModel FromDataRow(DataRow dataRow)
        {
            var lessonViewModel = new LessonViewModel
            {
            Titolo = Convert.ToString(dataRow["Title"]), //recupero il titolo dal db, leggendo la colonna Title
            Durata = TimeSpan.Parse(Convert.ToString(dataRow["Duration"])),
            Id = Convert.ToInt32(dataRow["Id"])
            };
            return lessonViewModel;

        }    
    }
}