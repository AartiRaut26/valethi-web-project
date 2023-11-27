namespace NewStudentAttendanceAPI.Web.Models
{
    public class PatchViewModel
    {
        public int Id { get; set; }
        public string Op { get; set; }
        public string Path { get; set; }
        public string Value { get; set; }
    }
}
