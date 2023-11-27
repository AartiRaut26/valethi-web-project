namespace NewStudentAttendanceAPI.Web.Models
{

    //Model to handle Success, errors
    public class CustomResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }
}
