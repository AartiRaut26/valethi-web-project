using Microsoft.Extensions.Logging;
using NewStudentAttendanceAPI.Web.Models;
using NewStudentAttendanceAPI.Web.Models.Dto;
using System.Net;
using System.Text;
using System.Text.Json;

namespace NewStudentAttendanceAPI.Web
{
    public class StudentServices
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ClassService> _logger;

        public StudentServices(HttpClient httpClient, ILogger<ClassService> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpClient.BaseAddress = new Uri("https://localhost:7169/"); // Set the correct base address
            _logger = logger;

        }

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            var response = await _httpClient.GetAsync("/api/students");

            if (response.IsSuccessStatusCode)
            {
                var students = await response.Content.ReadFromJsonAsync<List<Student>>();
                return students;
            }

            // Handle errors appropriately
            // For example, you can throw an exception or return an empty list
            return new List<Student>();
        }
        /************************/

        // FrontEnd/Services/ApiService.cs
        public async Task<Student> GetStudentByIdAsync(int studentId)
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/students");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Raw JSON response: {json}");

                    try
                    {
                        var students = JsonSerializer.Deserialize<List<Student>>(json, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                        });

                        var student = students.FirstOrDefault(s => s.StudentId == studentId);

                        return student;
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"Error deserializing JSON for students: {ex.Message}");
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
                    Console.WriteLine($"Error fetching students: {response.ReasonPhrase}");
                    return null;
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle exception (e.g., log, return null, or rethrow)
                Console.WriteLine($"Error fetching students: {ex.Message}");
                return null;
            }
        }

        //-----------------------------------------------------------------------------------------------------

        // FrontEnd/Services/ApiService.cs
        public async Task<CustomResult> CreateStudentAsync(NewStudViewModel student)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/students", student);
                response.EnsureSuccessStatusCode();

                var createdStud =  response.Content.ReadFromJsonAsync<Student>();

                return new CustomResult { IsSuccess = true, ErrorMessage = null };
            }
            catch (HttpRequestException ex)
            {
                // Handle the exception appropriately for your application
                Console.WriteLine($"Error calling Minimal API for CreateStudentAsync: {ex.Message}");

                // Return a more specific error message or handle the error
                return new CustomResult { IsSuccess = false, ErrorMessage = $"Failed to create student. Error: {ex.Message}" };
            }
        }

        /*********************************/

        /*********************************/


        // FrontEnd/Services/ApiService.cs
        public async Task<CustomResult> UpdateStudentAsync(Student student)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"/api/students/{student.StudentId}", student);
                response.EnsureSuccessStatusCode();

                // If you need to handle the response content, you can do it here

                return new CustomResult { IsSuccess = true, ErrorMessage = null};
            }
            catch (HttpRequestException ex)
            {
                // Handle exception (e.g., log, return error result, or rethrow)
                Console.WriteLine($"Error updating student: {ex.Message}");
                return new CustomResult { IsSuccess = false, ErrorMessage = $"Failed to update student. Error: {ex.Message}" };
            }
        }




        // FrontEnd/Services/ApiService.cs
        public async Task<CustomResult> DeleteStudentAsync(int studentId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/api/students/{studentId}?studentId={studentId}");
                response.EnsureSuccessStatusCode();

                // If you need to handle the response content, you can do it here

                return new CustomResult { IsSuccess = true, ErrorMessage = null };
            }
            catch (HttpRequestException ex)
            {
                // Handle exception (e.g., log, return error result, or rethrow)
                Console.WriteLine($"Error deleting student: {ex.Message}");
                return new CustomResult { IsSuccess = false, ErrorMessage = $"Failed to delete student. Error: {ex.Message}" };
            }
        }
        public async Task<CustomResult> PatchStudentAsync(int id, List<OperationDto> operations)
        {
            try
            {
                var patchDoc = new JsonPatchDocumentDto { Operations = operations };
                var serializedPatchDoc = JsonSerializer.Serialize(patchDoc);
                var content = new StringContent(serializedPatchDoc, Encoding.UTF8, "application/json");

                var response = await _httpClient.PatchAsync($"/api/students/{id}", content);

                response.EnsureSuccessStatusCode();

                return new CustomResult { IsSuccess = true, ErrorMessage = null };
            }
            catch (HttpRequestException ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Error patching student. StudentId: {id}. Exception: {ex}");

                // Handle exception (e.g., return error result with detailed message or rethrow)
                return new CustomResult
                {
                    IsSuccess = false,
                    ErrorMessage = $"Failed to patch student with ID {id}. Error: {ex.Message}"
                };
            }
        }


    }
}
