using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrationService.Models;

namespace RegistrationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly RegistrationDbContext _context;
        private readonly HttpClient _httpClient;

        // Cấu hình URL port của 2 service còn lại
        private const string StudentServiceUrl = "http://localhost:5001/api/student";
        private const string TopicServiceUrl = "http://localhost:5002/api/topic";

        public RegistrationController(RegistrationDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Registration>>> GetAll()
        {
            return await _context.Registrations.ToListAsync();
        }

        // Đăng ký đề tài: Kiểm tra hợp lệ qua REST API trước khi lưu
        [HttpPost]
        public async Task<IActionResult> Register(Registration reg)
        {
            // 1. Kiểm tra sinh viên có tồn tại bên StudentService không
            var studentRes = await _httpClient.GetAsync($"{StudentServiceUrl}/{reg.StudentId}");
            if (!studentRes.IsSuccessStatusCode)
            {
                return BadRequest($"Student with ID '{reg.StudentId}' does not exist.");
            }

            // 2. Kiểm tra đề tài có tồn tại bên TopicService không
            var topicRes = await _httpClient.GetAsync($"{TopicServiceUrl}/{reg.TopicId}");
            if (!topicRes.IsSuccessStatusCode)
            {
                return BadRequest($"Topic with ID '{reg.TopicId}' does not exist.");
            }

            // 3. Kiểm tra xem sinh viên này đã đăng ký đề tài nào chưa
            bool alreadyRegistered = await _context.Registrations.AnyAsync(r => r.StudentId == reg.StudentId);
            if (alreadyRegistered)
            {
                return BadRequest("Student has already registered for a topic.");
            }

            reg.RegisteredDate = DateTime.Now;
            _context.Registrations.Add(reg);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Registration successful!", data = reg });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelRegistration(int id)
        {
            var reg = await _context.Registrations.FindAsync(id);
            if (reg == null) return NotFound();

            _context.Registrations.Remove(reg);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}