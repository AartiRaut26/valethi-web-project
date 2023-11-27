namespace NewStudentAttendanceAPI.Web.Models.Dto
{

    //Class to handle the jsonpatch operations
    public class JsonPatchDocumentDto
    {
        public List<OperationDto> Operations { get; set; }

    }
}
