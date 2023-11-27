using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NewStudentAttendanceAPI.Web.Models
{

    //Attendance Model  to handle all the Attendance, students, classes data 
    public class Attendance
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int AttendanceId { get; set; }
        public int StudentId { get; set; }
        public int ClassId { get; set; }
        public DateTime Time { get; set; }
        public Student Student { get; set; }
        public Class Class { get; set; }
    }
}
