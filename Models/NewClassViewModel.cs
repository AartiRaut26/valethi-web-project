using System.ComponentModel.DataAnnotations;

namespace NewStudentAttendanceAPI.Web.Models
{
    public class NewClassViewModel
    {
       
            [Required(ErrorMessage = "Class Name is required")]
            public string ClassName { get; set; }
      
    }
}
