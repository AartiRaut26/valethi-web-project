using System.ComponentModel.DataAnnotations;

namespace NewStudentAttendanceAPI.Web.Models
{
    public class NewStudViewModel
    {

        [Required(ErrorMessage = "Student Name is required")]
        public string StudentName { get; set; }

    }
}
