using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TopicService.Models;

namespace TopicService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicController : ControllerBase
    {
        private readonly TopicDbContext _context;

        public TopicController(TopicDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Topic>>> GetAll()
        {
            return await _context.Topics.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Topic>> GetById(string id)
        {
            var topic = await _context.Topics.FindAsync(id);
            if (topic == null) return NotFound();
            return topic;
        }

        [HttpPost]
        public async Task<ActionResult<Topic>> Create(Topic topic)
        {
            _context.Topics.Add(topic);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = topic.TopicId }, topic);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Topic topic)
        {
            var existing = await _context.Topics.FindAsync(id);
            if (existing == null) return NotFound();

            existing.Title = topic.Title;
            existing.Supervisor = topic.Supervisor;
            existing.Status = topic.Status;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var topic = await _context.Topics.FindAsync(id);
            if (topic == null) return NotFound();

            _context.Topics.Remove(topic);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}