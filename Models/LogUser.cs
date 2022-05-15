using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;



namespace petstagram.Models
{
    public class LogUser
    {
        [Required]
        [EmailAddress]
        public string LogEmail {get;set;}
        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string LogPassword {get;set;}
    }
}