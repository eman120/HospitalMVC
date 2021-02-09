using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.Models
{
    public class Patient
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "You have provide a valid full name.")] //يجب عليك ادخال اسم صحيح
        [MinLength(11, ErrorMessage = "Full name mustn't be less than 11 characters.")]
        [MaxLength(50, ErrorMessage = "Full name mustn't exceed 50 characters.")]
        [DisplayName("Name")]
        public string FullName { get; set; }


        [Required(ErrorMessage = "You have provide a valid Age.")]
        [Range(1, 100, ErrorMessage = "Age must be between 1 and 100.")]
        [DisplayName("Age")]
        public int Age { get; set; }


        [Required(ErrorMessage = "You have provide a valid Diagnosis.")]
        [DisplayName("Diagnosis")]
        public String Diagnosis { get; set; }


        [Required(ErrorMessage = "You have provide a valid Treatment.")]
        [DisplayName("Treatment")]
        public String Treatment { get; set; }


        [DisplayName("Notes")]
        public String Notes { get; set; }

        [DisplayName("department")]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }


        [DisplayName("Birth Date")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }


        public DateTime ComingDate { get; set; }


        [DataType(DataType.Time)]
        public DateTime ComingTime { get; set; }


        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }

        [Required]
        public double Reciept { get; set; }

        [DisplayName("National Number")]
        public string NationalNumberCard { get; set; }

        [NotMapped]
        public IFormFile NationalNumberCardFile { get; set; }

        [DisplayName("Prescription Image")]
        public string PrescriptionImageName { get; set; }

        [NotMapped]
        public IFormFile PrescriptionImageFile { get; set; }
    }
}
