using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class Student
    {
        [Required]
        [Display(Name = "ID")]
        public string ID { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

        public Student(string id, string firstName, string lastName)
        {
            ID = id;
            FirstName = firstName;
            LastName = lastName; 
        }
    }
}