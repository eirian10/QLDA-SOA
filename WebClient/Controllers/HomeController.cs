using System.Diagnostics;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using WebClient.Models;

namespace WebClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        private const string RegistrationApi = "http://localhost:5003/api/registration";
        private const string StudentApi = "http://localhost:5001/api/student";
        private const string TopicApi = "http://localhost:5002/api/topic";

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<IActionResult> Index()
        {
            var registrations = new List<RegistrationViewModel>();
            try
            {
                var regList = await _httpClient.GetFromJsonAsync<List<RegistrationViewModel>>(RegistrationApi);
                if (regList != null) registrations = regList;

                var students = await _httpClient.GetFromJsonAsync<List<StudentViewModel>>(StudentApi);
                var topics = await _httpClient.GetFromJsonAsync<List<TopicViewModel>>(TopicApi);

                ViewBag.Students = students ?? new List<StudentViewModel>();
                ViewBag.Topics = topics ?? new List<TopicViewModel>();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi kết nối tới các dịch vụ: " + ex.Message;
            }

            return View(registrations);
        }

        [HttpPost]
        public async Task<IActionResult> Register(string studentId, string topicId)
        {
            var model = new { StudentId = studentId, TopicId = topicId };
            var response = await _httpClient.PostAsJsonAsync(RegistrationApi, model);

            if (!response.IsSuccessStatusCode)
            {
                var errorMsg = await response.Content.ReadAsStringAsync();
                TempData["Error"] = errorMsg;
            }
            else
            {
                TempData["Success"] = "Đăng ký đề tài thành công";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            await _httpClient.DeleteAsync($"{RegistrationApi}/{id}");
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}