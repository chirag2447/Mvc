using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc.Models
{
    public class StudentModel
    {
        public int? c_id {get;set;}
        public string? c_name {get;set;}
        public DateTime? c_dob {get;set;}
        public string? c_gender {get;set;}
        public string? c_address {get;set;}
        public string[]? c_language {get;set;}
        public int c_course {get;set;}
        public string? c_phone {get; set;}
        public string? c_profile {get;set;}
        
        public IFormFile? Photo {get;set;}
        public string c_course_name{get;set;}

    }
}