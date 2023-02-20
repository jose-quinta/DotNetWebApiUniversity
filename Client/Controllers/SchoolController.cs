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
    public class SchoolController : Controller {
        private readonly HttpClient _http = new HttpClient();
        private readonly HttpClientHandler _httpHandler = new HttpClientHandler();
        private readonly string _url = "https://localhost:7120/api/School/";
        public SchoolController() => _httpHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        public async Task<ActionResult<List<School>>> Index() {
            var response = new List<School>();
            using (var _api = await _http.GetAsync(_url)) {
                string _response = await _api.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<List<School>>(_response)!;
            }

            return View(response);
        }
        public async Task<ActionResult> SchoolList() {
            var response = new List<School>();
            using (var _api = await _http.GetAsync(_url)) {
                string _response = await _api.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<List<School>>(_response)!;
            }

            return Json(new SelectList(response, "Id", "Name"));
        }
        public async Task<ActionResult<School>> Details(int id) {
            var response = new School();
            using (var _api = await _http.GetAsync(_url + id)) {
                string _response = await _api.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<School>(_response)!;
            }

            return View(response);
        }
        public ActionResult Create() => View();
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<List<School>>> Create(School request) {
            if (request == null)
                return RedirectToAction(nameof(Index));

            SchoolDto _request = new SchoolDto() {
                Name = request.Name,
                Code = request.Code,
                FacultyId = request.FacultyId
            };

            var response = new List<School>();

            StringContent content = new StringContent(JsonConvert.SerializeObject(_request), Encoding.UTF8, "application/json");
            using (var _api = await _http.PostAsync(_url, content)) {
                string _response = await _api.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<List<School>>(_response)!;
            }

            if (response == null)
                return View();

            return RedirectToAction(nameof(Index));
        }
        public async Task<ActionResult<School>> Edit(int id) {
            var response = new School();
            using (var _api = await _http.GetAsync(_url + id)) {
                string _response = await _api.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<School>(_response)!;
            }

            return View(response);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<School>> Edit(School request) {
            if (request == null)
                return RedirectToAction(nameof(Index));

            SchoolDto _request = new SchoolDto() {
                Name = request.Name,
                Code = request.Code,
                FacultyId = request.FacultyId
            };

            var response = new School();

            StringContent content = new StringContent(JsonConvert.SerializeObject(_request), Encoding.UTF8, "application/json");
            using (var _api = await _http.PutAsync(_url + request.Id, content))
            {
                string _response = await _api.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<School>(_response)!;
            }

            if (response == null)
                return View();

            return RedirectToAction(nameof(Index));
        }
        public async Task<ActionResult<School>> Delete(int id) {
            var response = new School();
            using (var _api = await _http.GetAsync(_url + id)) {
                string _response = await _api.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<School>(_response)!;
            }

            return View(response);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<School>> Delete(int id, School request) {
            var response = new School();

            using (var _api = await _http.DeleteAsync(_url + id)) {
                string _response = await _api.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<School>(_response)!;
            }

            if (response == null)
                return View();

            return RedirectToAction(nameof(Index));
        }
    }
}