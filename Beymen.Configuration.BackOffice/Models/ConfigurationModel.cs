using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Beymen.Configuration.BackOffice.Models
{
    public class ConfigurationModel
    {

        public int Id { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Please enter Name")]
        public string Name { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Please enter Type")]
        public string Type { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Please enter Value")]
        public string Value { get; set; }
        public bool IsActive { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Please enter Application Name")]
        public string ApplicationName { get; set; }
    }
}
