using System.ComponentModel.DataAnnotations;

namespace NewStudentAttendanceAPI.Web.Models
{

    //Class Model 
    public class Class
    {
        [Key]
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public List<Attendance> Attendances { get; set; }
    }
}
