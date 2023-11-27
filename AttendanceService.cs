using NewStudentAttendanceAPI.Web.Models;
using System.Text.Json;
using System.Text;
using NewStudentAttendanceAPI.Web.Models.Dto;

namespace NewStudentAttendanceAPI.Web
{
	public class AttendanceService
	{
		private readonly HttpClient _httpClient;
		private readonly ILogger<ClassService> _logger;
		public AttendanceService(HttpClient httpClient, ILogger<ClassService> logger)
		{

			_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
			_httpClient.BaseAddress = new Uri("https://localhost:7169/"); // Set the correct base address
			_logger = logger;
		}

		public async Task<List<Attendance>> GetAllAttendances()
		{
			try
			{
				var response = await _httpClient.GetAsync($"/api/attendances");
				response.EnsureSuccessStatusCode();

				return await response.Content.ReadFromJsonAsync<List<Attendance>>();
			}
			catch (HttpRequestException ex)
			{
				// Handle exception (e.g., log, return empty list, or rethrow)
				Console.WriteLine($"Error fetching Attendances: {ex.Message}");
				return new List<Attendance>();
			}
		}

		public async Task<CustomResult> CreateAttendanceAsync(Attendance attendance)
		{
			try
			{
				var response = await _httpClient.PostAsJsonAsync("/api/attendances", attendance);
				response.EnsureSuccessStatusCode();

				// If you need to handle the response content, you can do it here
				// var createdAttendance = await response.Content.ReadFromJsonAsync<Attendance>();

				return new CustomResult { IsSuccess = true, ErrorMessage = null };
			}
			catch (HttpRequestException ex)
			{
				// Handle exception (e.g., log, return error result, or rethrow)
				Console.WriteLine($"Error creating attendance: {ex.Message}");
				return new CustomResult { IsSuccess = false, ErrorMessage = $"Failed to create attendance. Error: {ex.Message}" };
			}
		}

		public async Task<Attendance> GetAttendanceByIdAsync(int attendanceId)
		{
			try
			{
				var response = await _httpClient.GetAsync($"/api/Attendances/{attendanceId}?attendanceId={attendanceId}");
				response.EnsureSuccessStatusCode();

				return await response.Content.ReadFromJsonAsync<Attendance>();
			}
			catch (HttpRequestException ex)
			{
				// Handle exception (e.g., log, return null, or rethrow)
				Console.WriteLine($"Error fetching attendance by ID {attendanceId}: {ex.Message}");
				return null;
			}
		}




	}
}
