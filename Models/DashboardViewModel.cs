namespace NewStudentAttendanceAPI.Web.Models
{
    public class DashboardViewModel
    {
        public int TotalClasses { get; set; }
        public int TotalStudents { get; set; }
        public int ClassesInSession { get; set; }
        public int AverageClassSize { get; set; }
        public int StudentsEnrolled { get; set; }
        public int AverageAge { get; set; }
    }
}
