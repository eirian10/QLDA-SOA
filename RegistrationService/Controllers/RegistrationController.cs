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

        // Endpoint trả về danh sách mã đề tài đã có người đăng ký
        [HttpGet("registered-topic-ids")]
        public async Task<ActionResult<IEnumerable<string>>> GetRegisteredTopicIds()
        {
            var registeredTopicIds = await _context.Registrations
                .Select(r => r.TopicId)
                .Distinct()
                .ToListAsync();

            return Ok(registeredTopicIds);
        }

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
            bool studentRegistered = await _context.Registrations.AnyAsync(r => r.StudentId == reg.StudentId);
            if (studentRegistered)
            {
                return BadRequest("Student has already registered for a topic.");
            }

            // 4. Kiểm tra xem đề tài này đã có người khác đăng ký chưa
            bool topicRegistered = await _context.Registrations.AnyAsync(r => r.TopicId == reg.TopicId);
            if (topicRegistered)
            {
                return BadRequest("This topic has already been registered by another student.");
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