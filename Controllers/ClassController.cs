using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NewStudentAttendanceAPI.Web.Models;
using NewStudentAttendanceAPI.Web.Models.Dto;
using System.Net.Http;
using static ClassService;

namespace NewStudentAttendanceAPI.Web.Controllers
{

    //Class Controller 

    public class ClassController : Controller
    {
        private readonly ClassService _classService;
        private readonly ILogger<ClassController> _logger;  // Add this line
        private readonly IHttpClientFactory _httpClientFactory;


        public ClassController(ClassService classService, ILogger<ClassController> logger, IHttpClientFactory httpClientFactory)
        {
            _classService = classService;
            _logger = logger;
            _httpClientFactory = httpClientFactory;



        }

        //Method to get the Class List
        public async Task<IActionResult> ClassList()
        {
            var classes = await _classService.GetAllClasses();
            return View(classes); // Assuming GetAllClasses returns List<ClassViewModel>
        }

        //Method to show the CreateClass.cshtml view 

        public IActionResult CreateClass()
        {
            return View(new NewClassViewModel());
        }


        //Method to create the new class  

        [HttpPost]
        public async Task<IActionResult> CreateClass(NewClassViewModel newClass)
        {
            if (ModelState.IsValid)
            {
                // Call your service to create a new class
                var result = await _classService.CreateClassAsync(newClass);

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Data added successfully.";


                    // Redirect to the class list or any other page after creating the class
                    return RedirectToAction("ClassList");

                    // Set a TempData message for the ClassList page
                    TempData["SuccessMessage"] = "Class Data Added Successfully";
                }

                // If there was an error creating the class, add an error to the model state
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
            }

            // If the model state is not valid or there was an error, return to the create view with validation errors
            return View(newClass);
        }
        

        //Method to show the ClassDetails.cshtml page view 
        public async Task<IActionResult> Details(int id)
        {
            var classDetails = await _classService.GetClassById(id);

            if (classDetails == null)
            {
                return NotFound(); // Class not found
            }

            // Wrap the single class in a list and return the first item
            var classList = new List<ClassViewModel> { classDetails };

            return View(classList.First());
        }

        //Method to Edit the Class 

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            // Retrieve class details from your database or any other source
            // For example, assuming you have a method in your database service
            // that retrieves class details by ID.
            var classDetails = await _classService.GetClassById(id);

            if (classDetails == null)
            {
                return NotFound(); // Class not found
            }

            // Map your database model to the view model
            var model = new ClassViewModel
            {
                ClassId = classDetails.ClassId,
                ClassName = classDetails.ClassName
                // Add other properties as needed
            };

            return View(model);
        }

        //Method to Edit the class 

        [HttpPost]
        public async Task<IActionResult> Edit(ClassViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Call your service to update the class
                    var result = await _classService.UpdateClassAsync(model);

                    if (result.IsSuccess)
                    {

                        TempData["SuccessMessage"] = "Data Edited successfully.";

                        // Redirect to the class list or any other page after updating the class
                        return RedirectToAction("ClassList");
                    }

                    // If there was an error updating the class, add an error to the model state
                    ModelState.AddModelError(string.Empty, result.ErrorMessage);
                }
                catch (Exception ex)
                {
                    // Log the exception for debugging purposes
                    _logger.LogError($"Error updating class: {ex}");

                    ModelState.AddModelError(string.Empty, "An error occurred while updating the class. Please try again.");
                }
            }

            // If the model state is not valid or there was an error, return to the edit view with validation errors
            return View(model);
        }

        //Method to get  the class by id 
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var classDetails = await _classService.GetClassById(id);

            if (classDetails == null)
            {
                return NotFound(); // Class not found
            }

            var model = new ClassViewModel
            {
                ClassId = classDetails.ClassId,
                ClassName = classDetails.ClassName
                // Add other properties as needed
            };

            return View(model);
        }
        [HttpPost, ActionName("Delete")]


        //Method to delete the class 
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _classService.DeleteClassAsync(id);

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Data Deleted successfully.";

                    return RedirectToAction("ClassList");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.ErrorMessage);
                    return RedirectToAction("ClassList"); // Redirect to a suitable action
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                _logger.LogError($"Error deleting class: {ex}");

                ModelState.AddModelError(string.Empty, "An error occurred while deleting the class. Please try again.");
                return RedirectToAction("ClassList"); // Redirect to a suitable action
            }
        }
      
        //Method to get the class for partialEdit
        [HttpGet]
        public async Task<IActionResult> EditPartialClass(int id)
        {
            // Assuming you have a ViewModel for the class data you want to edit
            // Replace ClassViewModel with the actual ViewModel you are using
            var classData = await _classService.GetClassById(id);

            // Assuming you have a ViewModel for the class data you want to edit
            // Replace ClassViewModel with the actual ViewModel you are using
            return View(classData);
        }

        //Method to edit the class partially using Jsonpatch 
        [HttpPost]
        public async Task<IActionResult> EditPartialClass(int id, List<OperationDto> operations)
        {
            try
            {
                // Call the API to patch the class
                var result = await _classService.PatchClassAsync(id, operations);

                // Check if the patch operation was successful
                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Data patch successfully.";

                    // Redirect to the class list or any other page after patching the class
                    return RedirectToAction("ClassList");
                }
                else
                {
                    // If there was an error patching the class, add an error to the model state
                    ModelState.AddModelError(string.Empty, result.ErrorMessage);

                    // Redirect to a suitable action
                    return RedirectToAction("ClassList");
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                _logger.LogError($"Error patching class: {ex}");

                ModelState.AddModelError(string.Empty, "An error occurred while patching the class. Please try again.");

                // Redirect to a suitable action
                return RedirectToAction("ClassList");
            }
        }

    }
}
