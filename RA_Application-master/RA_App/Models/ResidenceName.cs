using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RA_App.Models
{
    public class ResidenceName
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Residence ID")]
        public int Res_ID { get; set; }

        [Required(ErrorMessage = "Residence Name is required")]
        [DisplayName("Residence Name")]
        public string Res_Name { get; set; }

        
        
    }
}