using System.ComponentModel.DataAnnotations;

namespace NewStudentAttendanceAPI.Web.Models
{

    //Student Model 
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        public string StudentName { get; set; }
/*        public List<Attendance> Attendances { get; set; }
*/    }
}
