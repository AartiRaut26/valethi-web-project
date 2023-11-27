using Microsoft.AspNetCore.Mvc;
using NewStudentAttendanceAPI.Web.Models;
using NewStudentAttendanceAPI.Web.Models.Dto;

namespace NewStudentAttendanceAPI.Web.Controllers
{
    //Attendance Controller Class 
    public class AttendanceController : Controller
    {
        
        private readonly AttendanceService _attendanceService;
        private readonly ILogger<ClassController> _logger;  // Add this line
        private readonly IHttpClientFactory _httpClientFactory;


        public AttendanceController(AttendanceService attendanceService, ILogger<ClassController> logger, IHttpClientFactory httpClientFactory)
        {
            _attendanceService = attendanceService;
            _logger = logger;
            _httpClientFactory = httpClientFactory;

        }

        //Method to show all the Attendance
        public async Task<IActionResult> Attendances()
        {
            try
            {
                var attendances = await _attendanceService.GetAllAttendances();
                return View(attendances);
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log, display an error message, or rethrow)
                Console.WriteLine($"Error in Index action: {ex.Message}");
                return View(new List<Attendance>()); // or return an error view
            }
        }

        //Method to show the AddAttendance.cshtml page view 

        [HttpGet]
        public IActionResult AddAttendance()
        {
            return View();
        }

        //Method to Create/Add Attendance  

        [HttpPost]
        public async Task<IActionResult> AddAttendance(Attendance attendance)
        {
            try
            {
                var result = await _attendanceService.CreateAttendanceAsync(attendance);

                if (result.IsSuccess)
                {

                    TempData["SuccessMessage"] = "Data added successfully.";



                    // Redirect to the list of attendances or show a success message
                    return RedirectToAction("Attendances");
                }

                // If there was an error creating the attendance, add an error to the model state
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Error creating attendance: {ex}");

                // Add an error to the model state
                ModelState.AddModelError(string.Empty, "An error occurred while creating the attendance. Please try again.");
            }

            // If the model state is not valid or there was an error, return to the create view with validation errors
            return View(attendance);
        }


        [HttpPost]

        //Method to Get Attendance  

        [HttpGet]
        public async Task<IActionResult> SearchAttendance(int id)
        {
            try
            {
                var attendance = await _attendanceService.GetAttendanceByIdAsync(id);

                if (attendance != null)
                {
                    return View(attendance);
                }
                else
                {
                    return View("AttendanceNotFound"); // Create a view for this case
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Error fetching attendance: {ex}");

                // Handle the error, maybe return an error view
                return View("Error"); // Create an error view
            }
        }
        
    }
}
