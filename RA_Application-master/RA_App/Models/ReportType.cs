using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RA_App.Models
{
    public class ReportType
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Type of Report ID")]
        public int Report_ID { get; set; }

        [Required(ErrorMessage = "Category required")]
        [DisplayName("Category Type")]
        public string Category_Type { get; set; }

        [Required(ErrorMessage = "Incident/Illness is required")]
        [DisplayName("Incident/Illness")]
        public string Name { get; set; }

    }
}