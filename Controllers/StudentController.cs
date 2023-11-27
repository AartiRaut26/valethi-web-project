using Microsoft.AspNetCore.Mvc;
using NewStudentAttendanceAPI.Web.Models;
using NewStudentAttendanceAPI.Web.Models.Dto;

namespace NewStudentAttendanceAPI.Web.Controllers
{
    public class StudentController : Controller
    {
        private readonly StudentServices _studentService;
        private readonly ILogger<ClassController> _logger;  // Add this line
        private readonly IHttpClientFactory _httpClientFactory;


        public IActionResult Index()
        {
            return View();
        }
        public StudentController(StudentServices studentService, ILogger<ClassController> logger, IHttpClientFactory httpClientFactory)
        {
            _studentService = studentService;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        //Method to get the student list 
        public async Task<IActionResult> StudentList()
        {
            var students = await _studentService.GetAllStudentsAsync();
            return View(students);
        }

        //Method to show the AddStudent.cshtml view 
        [HttpGet]
        public IActionResult AddStudent()
        {
            return View(new NewStudViewModel());
        }

        //Method to create the new student 

        [HttpPost]
        public async Task<IActionResult> AddStudent(NewStudViewModel student)
        {
            if (ModelState.IsValid)
            {
                // Call your service to create a new class
                var result = await _studentService.CreateStudentAsync(student);

                if (result.IsSuccess)
                {

                    TempData["SuccessMessage"] = "Data added successfully.";


                    // Redirect to the class list or any other page after creating the class

                    return RedirectToAction("StudentList");

                   

                }

                // If there was an error creating the class, add an error to the model state
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
            }

            // If the model state is not valid or there was an error, return to the create view with validation errors
            return View(student);
        }

        //------------------------------------------------------------------------------------------------

        //Method to get the Student data to Edit 
        [HttpGet]
        public async Task<IActionResult> EditStudent(int id)
        {
            // Retrieve student details from your database or any other source
            // For example, assuming you have a method in your database service
            // that retrieves student details by ID.
            var studentDetails = await _studentService.GetStudentByIdAsync(id);

            if (studentDetails == null)
            {
                return NotFound(); // Student not found
            }

            // Map your database model to the view model
            var model = new Student
            {
                StudentId = studentDetails.StudentId,
                StudentName = studentDetails.StudentName
                // Add other properties as needed
            };

            return View(model);
        }

        //Method to post the edited student data 

        [HttpPost]
        public async Task<IActionResult> EditStudent(Student model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Call your service to update the student
                    var result = await _studentService.UpdateStudentAsync(model);

                    if (result.IsSuccess)
                    {

                        // Set success message in TempData
                        TempData["SuccessMessage"] = "Data Updated Successfully";

                        // Redirect to the students list or any other page after updating the student
                        return RedirectToAction("StudentList");
                    }

                    // If there was an error updating the student, add an error to the model state
                    ModelState.AddModelError(string.Empty, result.ErrorMessage);
                }
                catch (Exception ex)
                {
                    // Log the exception for debugging purposes
                    _logger.LogError($"Error updating student: {ex}");

                    ModelState.AddModelError(string.Empty, "An error occurred while updating the student. Please try again.");
                }
            }

            // If the model state is not valid or there was an error, return to the edit view with validation errors
            return View(model);
        }

        //-------------------------------------------------------------------------------------------------

        //Method to search student by id
        public async Task<IActionResult> SearchStudent(int studentId)
        {
            // Call the API to get the student details by ID asynchronously
            var student = await _studentService.GetStudentByIdAsync(studentId);

            if (student == null)
            {
                // If the student is not found, you may handle it as needed (e.g., show a message).
                return View("StudentList", new List<Student>());
            }

            // Redirect to the "StudentDetails" action with the student details
            return View("StudentDetails", student);
        }

       //Method to  get the student data to delete by student id 

        [HttpGet]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            // Retrieve student details from your database or any other source
            // For example, assuming you have a method in your database service
            // that retrieves student details by ID.
            var studentDetails = await _studentService.GetStudentByIdAsync(id);

            if (studentDetails == null)
            {
                return NotFound(); // Student not found
            }

            // Map your database model to the view model
            var model = new Student
            {
                StudentId = studentDetails.StudentId,
                StudentName = studentDetails.StudentName
                // Add other properties as needed
            };

            return View(model);
        }

        //Method to delete the student data 

        [HttpPost, ActionName("DeleteStudent")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _studentService.DeleteStudentAsync(id);

                if (result.IsSuccess)
                {

                    // Set success message in TempData
                    TempData["SuccessMessage"] = "Data Deleted Successfully";

                    return RedirectToAction("StudentList");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.ErrorMessage);
                    return RedirectToAction("StudentList"); // Redirect to a suitable action
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                _logger.LogError($"Error deleting student: {ex}");

                ModelState.AddModelError(string.Empty, "An error occurred while deleting the student. Please try again.");
                return RedirectToAction("StudentList"); // Redirect to a suitable action
            }
        }

        //Method to get the EditPartialStudent.cshtml view 

        [HttpGet]
        public async Task<IActionResult> EditPartialStudent(int id)
        {
            
            var studentData = await _studentService.GetStudentByIdAsync(id);

            
            return View(studentData);
        }

        //Method to send Edit the student data partially 

        [HttpPost]
        public async Task<IActionResult> EditPartialStudent(int id, List<OperationDto> operations)
        {
            try
            {
                // Call the API to patch the student
                var result = await _studentService.PatchStudentAsync(id, operations);

                // Check if the patch operation was successful
                if (result.IsSuccess)
                {

                    TempData["SuccessMessage"] = "Data Patch Successfully";

                    // Redirect to the student list or any other page after patching the student
                    return RedirectToAction("StudentList");
                }
                else
                {
                    // If there was an error patching the student, add an error to the model state
                    ModelState.AddModelError(string.Empty, result.ErrorMessage);

                    // Redirect to a suitable action
                    return RedirectToAction("StudentList");
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                _logger.LogError($"Error patching student: {ex}");

                ModelState.AddModelError(string.Empty, "An error occurred while patching the student. Please try again.");

                // Redirect to a suitable action
                return RedirectToAction("StudentList");
            }
        }


    }
}
