using Microsoft.AspNetCore.Mvc;
using WebClient.Models;
using System.Text.Json;
using System.Text;

namespace WebClient.Controllers
{
    public class StudentController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _baseUrl = "http://localhost:5001/api/student";

        public StudentController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _clientFactory.CreateClient();
            var list = new List<StudentViewModel>();

            var response = await client.GetAsync(_baseUrl);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                list = JsonSerializer.Deserialize<List<StudentViewModel>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<StudentViewModel>();
            }

            return View(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create(StudentViewModel model)
        {
            var client = _clientFactory.CreateClient();
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await client.PostAsync(_baseUrl, content);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"{_baseUrl}/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var student = JsonSerializer.Deserialize<StudentViewModel>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(student);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(StudentViewModel model)
        {
            var client = _clientFactory.CreateClient();
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await client.PutAsync($"{_baseUrl}/{model.StudentId}", content);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var client = _clientFactory.CreateClient();
            await client.DeleteAsync($"{_baseUrl}/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}