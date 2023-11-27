namespace NewStudentAttendanceAPI.Web.Models.Dto
{

    //Class to handle jsonpatch operations 
    public class OperationDto
    {
        public string Op { get; set; }
        public string Path { get; set; }
        public string Value { get; set; }
    }
}
