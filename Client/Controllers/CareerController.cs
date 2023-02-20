using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Client.Controllers {
    // [Route("[controller]")]
    public class CareerController : Controller {
        private readonly HttpClient _http = new HttpClient();
        private readonly HttpClientHandler _httpHandler = new HttpClientHandler();
        private readonly string _url = "https://localhost:7120/api/Career/";
        public CareerController() => _httpHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        public async Task<ActionResult<List<Career>>> Index() {
            var response = new List<Career>();
            using (var _api = await _http.GetAsync(_url)) {
                string _response = await _api.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<List<Career>>(_response)!;
            }

            return View(response);
        }
        public async Task<ActionResult<Career>> Details(int id) {
            var response = new Career();
            using (var _api = await _http.GetAsync(_url + id)) {
                string _response = await _api.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<Career>(_response)!;
            }

            return View(response);
        }
        public ActionResult Create() => View();
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<List<Career>>> Create(Career request) {
            if (request == null)
                return RedirectToAction(nameof(Index));

            CareerDto _request = new CareerDto() {
                Name = request.Name,
                Code = request.Code,
                SchoolId = request.SchoolId
            };

            var response = new List<Career>();

            StringContent content = new StringContent(JsonConvert.SerializeObject(_request), Encoding.UTF8, "application/json");
            using (var _api = await _http.PostAsync(_url, content)) {
                string _response = await _api.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<List<Career>>(_response)!;
            }

            if (response == null)
                return RedirectToAction(nameof(Index));

            return RedirectToAction(nameof(Index));
        }
        public async Task<ActionResult<Career>> Edit(int id) {
            var response = new Career();
            using (var _api = await _http.GetAsync(_url + id)) {
                string _response = await _api.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<Career>(_response)!;
            }

            return View(response);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<Career>> Edit(Career request) {
            if (request == null)
                return RedirectToAction(nameof(Index));

            CareerDto _request = new CareerDto() {
                Name = request.Name,
                Code = request.Code,
                SchoolId = request.SchoolId
            };

            var response = new Career();

            StringContent content = new StringContent(JsonConvert.SerializeObject(_request), Encoding.UTF8, "application/json");
            using (var _api = await _http.PutAsync(_url + request.Id, content)) {
                string _response = await _api.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<Career>(_response)!;
            }

            if (response == null)
                return RedirectToAction(nameof(Index));

            return RedirectToAction(nameof(Index));
        }
        public async Task<ActionResult<Career>> Delete(int id) {
            var response = new Career();
            using (var _api = await _http.GetAsync(_url + id)) {
                string _response = await _api.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<Career>(_response)!;
            }

            return View(response);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<Career>> Delete(int id, Career request) {
            var response = new Career();

            using (var _api = await _http.DeleteAsync(_url + id)) {
                string _response = await _api.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<Career>(_response)!;
            }

            if (response == null)
                return RedirectToAction(nameof(Index));

            return RedirectToAction(nameof(Index));
        }
    }
}