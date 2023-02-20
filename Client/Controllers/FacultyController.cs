using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Client.Controllers {
    // [Route("[controller]")]
    public class FacultyController : Controller {
        private readonly HttpClient _http = new HttpClient();
        private readonly HttpClientHandler _httpHandler = new HttpClientHandler();
        private readonly string _url = "https://localhost:7120/api/Faculty/";
        public FacultyController() => _httpHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        public async Task<ActionResult<List<Faculty>>> Index() {
            var response = new List<Faculty>();
            using (var _api = await _http.GetAsync(_url)) {
                string _response = await _api.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<List<Faculty>>(_response)!;
            }

            return View(response);
        }
        public async Task<ActionResult> FacultyList() {
            var response = new List<Faculty>();
            using (var _api = await _http.GetAsync(_url)) {
                string _response = await _api.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<List<Faculty>>(_response)!;
            }

            return Json(new SelectList(response, "Id", "Name"));
        }
        public async Task<ActionResult<Faculty>> Details(int? id, string type) {
            if (id == null)
                return PartialView($"_Faculty{type}");

            var response = new Faculty();
            using (var _api = await _http.GetAsync(_url + id)) {
                string _response = await _api.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<Faculty>(_response)!;
            }

            if (response == null)
                return RedirectToAction(nameof(Index));

            return PartialView($"_Faculty{type}", response);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<List<Faculty>>> Create(Faculty request) {
            if (request == null)
                return RedirectToAction(nameof(Index));

            FacultyDto _request = new FacultyDto() {
                Name = request.Name,
                Code = request.Code
            };

            var response = new List<Faculty>();

            StringContent content = new StringContent(JsonConvert.SerializeObject(_request), Encoding.UTF8, "application/json");
            using (var _api = await _http.PostAsync(_url, content)) {
                string _response = await _api.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<List<Faculty>>(_response)!;
            }

            if (response == null)
                return RedirectToAction(nameof(Index));

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<Faculty>> Update(Faculty request) {
            if (request ==  null)
                return RedirectToAction(nameof(Index));

            FacultyDto _request = new FacultyDto() {
                Name = request.Name,
                Code = request.Code
            };

            var response = new Faculty();

            StringContent content = new StringContent(JsonConvert.SerializeObject(_request), Encoding.UTF8, "application/json");
            using (var _api = await _http.PutAsync(_url + request.Id, content)) {
                string _response = await _api.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<Faculty>(_response)!;
            }

            if (response == null)
                return RedirectToAction(nameof(Index));

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<Faculty>> Delete(int id) {
            var response = new Faculty();

            using(var _api = await _http.DeleteAsync(_url + id)) {
                string _response = await _api.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<Faculty>(_response)!;
            }

            if (response == null)
                return RedirectToAction(nameof(Index));

            return RedirectToAction(nameof(Index));
        }
    }
}