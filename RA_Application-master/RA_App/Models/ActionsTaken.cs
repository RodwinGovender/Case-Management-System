using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RA_App.Models
{
    public class ActionsTaken
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Actions ID")]
        public int Action_ID { get; set; }

        [Required(ErrorMessage = "Action type is required")]
        [DisplayName("Action Name")]
        public string Action_Name { get; set; }
    }
}