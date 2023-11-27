using Microsoft.Extensions.Logging;
using NewStudentAttendanceAPI.Web.Models;
using NewStudentAttendanceAPI.Web.Models.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

public class ClassService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ClassService> _logger;


    public ClassService(HttpClient httpClient, ILogger<ClassService> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _httpClient.BaseAddress = new Uri("https://localhost:7169/"); // Set the correct base address
        _logger = logger;

    }

    public async Task<List<ClassViewModel>> GetAllClasses()
    {
        try
        {
            var response = await _httpClient.GetAsync("/api/classes");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<ClassViewModel>>();
        }
        catch (HttpRequestException ex)
        {
            // Handle exception (e.g., log, return empty list, or rethrow)
            Console.WriteLine($"Error fetching classes: {ex.Message}");
            return new List<ClassViewModel>();
        }
    }

    public async Task<ClassViewModel> GetClassById(int classId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/classes?id={classId}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Raw JSON response: {json}");

                try
                {
                    var classViewModelList = JsonSerializer.Deserialize<List<ClassViewModel>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    });

                    // Assuming the API returns a list, you might want to find the item with the matching ID
                    var classViewModel = classViewModelList.FirstOrDefault(c => c.ClassId == classId);

                    if (classViewModel != null)
                    {
                        return classViewModel;
                    }
                    else
                    {
                        Console.WriteLine($"Error: Class with ID {classId} not found in the returned list");
                        return null;
                    }
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"Error deserializing JSON for class ID {classId}: {ex.Message}");
                    return null;
                }
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                // Handle 404 Not Found
                return null;
            }
            else
            {
                // Handle other status codes
                Console.WriteLine($"Error fetching class by ID {classId}: {response.ReasonPhrase}");
                return null;
            }
        }
        catch (HttpRequestException ex)
        {
            // Handle exception (e.g., log, return null, or rethrow)
            Console.WriteLine($"Error fetching class by ID {classId}: {ex.Message}");
            return null;
        }
    }
    public async Task<CustomResult> CreateClassAsync(NewClassViewModel newClass)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/classes", newClass);
            response.EnsureSuccessStatusCode();

            // Read the created class data from the response
            var createdClass = await response.Content.ReadFromJsonAsync<ClassViewModel>();

            return new CustomResult { IsSuccess = true, ErrorMessage = null };
        }
        catch (HttpRequestException ex)
        {
            // Handle exception (e.g., log, return error result, or rethrow)
            Console.WriteLine($"Error creating class: {ex.Message}");
            return new CustomResult { IsSuccess = false, ErrorMessage = $"Failed to create class. Error: {ex.Message}" };
        }
    }

    public async Task<CustomResult> UpdateClassAsync(ClassViewModel classModel)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync("/api/classes/" + classModel.ClassId, classModel);
            response.EnsureSuccessStatusCode();

            // If you need to handle the response content, you can do it here
            // var updatedClass = await response.Content.ReadFromJsonAsync<ClassViewModel>();

            return new CustomResult { IsSuccess = true, ErrorMessage = null };
        }
        catch (HttpRequestException ex)
        {
            // Handle exception (e.g., log, return error result, or rethrow)
            Console.WriteLine($"Error updating class: {ex.Message}");
            return new CustomResult { IsSuccess = false, ErrorMessage = $"Failed to update class. Error: {ex.Message}" };
        }
    }

    public async Task<CustomResult> DeleteClassAsync(int classId)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"/api/classes/{classId}?classId={classId}");
            response.EnsureSuccessStatusCode();

            return new CustomResult { IsSuccess = true, ErrorMessage = null };
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error deleting class: {ex.Message}");
            return new CustomResult { IsSuccess = false, ErrorMessage = $"Failed to delete class. Error: {ex.Message}" };
        }
    }



    /*********************************************/


    /*public async Task<CustomResult> PatchClassAsync(int id, JsonPatchDocumentDto patchDoc)
        {
            try
            {
                var response = await _httpClient.PatchAsJsonAsync($"/api/classes/{id}", patchDoc);
                response.EnsureSuccessStatusCode();

                return new CustomResult { IsSuccess = true, ErrorMessage = null };
            }
            catch (HttpRequestException ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Error calling Minimal API for PatchClassAsync: {ex.Message}");

                // Return a more specific error message or handle the error appropriately for your application
                return new CustomResult { IsSuccess = false, ErrorMessage = $"Failed to patch class. Error: {ex.Message}" };
            }
        }
   }*/


    public async Task<CustomResult> PatchClassAsync(int id, List<OperationDto> operations)
    {
        try
        {
            var patchDoc = new JsonPatchDocumentDto { Operations = operations };
            var serializedPatchDoc = JsonSerializer.Serialize(patchDoc);
            var content = new StringContent(serializedPatchDoc, Encoding.UTF8, "application/json");

            var response = await _httpClient.PatchAsync($"/api/classes/{id}", content);

            response.EnsureSuccessStatusCode();

            return new CustomResult { IsSuccess = true, ErrorMessage = null };
        }
        catch (HttpRequestException ex)
        {
            // Log the exception for debugging purposes
            Console.WriteLine($"Error patching class. ClassId: {id}. Exception: {ex}");

            // Handle exception (e.g., return error result with detailed message or rethrow)
            return new CustomResult
            {
                IsSuccess = false,
                ErrorMessage = $"Failed to patch class with ID {id}. Error: {ex.Message}"
            };
        }
    }

}
public class ClassViewModel
{
    public int ClassId { get; set; }

    [Required(ErrorMessage = "Class Name is required")]
    public string ClassName { get; set; }

    // Add other properties with validation attributes as needed
}


