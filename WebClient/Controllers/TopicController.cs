using Microsoft.AspNetCore.Mvc;
using WebClient.Models;
using System.Text.Json;
using System.Text;

namespace WebClient.Controllers
{
    public class TopicController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _baseUrl = "http://localhost:5002/api/topic";

        public TopicController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _clientFactory.CreateClient();
            var list = new List<TopicViewModel>();

            var response = await client.GetAsync(_baseUrl);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                list = JsonSerializer.Deserialize<List<TopicViewModel>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<TopicViewModel>();
            }

            return View(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TopicViewModel model)
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
                var topic = JsonSerializer.Deserialize<TopicViewModel>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(topic);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TopicViewModel model)
        {
            var client = _clientFactory.CreateClient();
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await client.PutAsync($"{_baseUrl}/{model.TopicId}", content);
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