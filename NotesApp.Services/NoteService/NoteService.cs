using Microsoft.AspNetCore.Http;
using NotesApp.Models;
using NotesApp.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NotesApp.Services.NoteService
{
    public class NoteService : INoteService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        public NoteService(IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public Task AddNote(Note note)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Note>> GetAllNotes()
        {
            var httpClient = MakeHttpClient();

            var httpResponse = await httpClient.GetAsync("allnotes");

            if (httpResponse.IsSuccessStatusCode)
            {
                
            }
            else
            {
                
            }

            return new List<Note>();
        }

        public async Task<string> GetFromApi()
        {
            var httpClient = _httpClientFactory.CreateClient("Notes");
            var token = _httpContextAccessor.HttpContext.Request.Cookies[Constants.XAccessToken];
            if (!string.IsNullOrWhiteSpace(token))
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", $"bearer {token}");
            }
            
            var httpResponse = await httpClient.GetAsync("");
            string? result = "";

            if (httpResponse.IsSuccessStatusCode)
            {
                result = await httpResponse.Content.ReadAsStringAsync();
            } else
            {
                result = "please log in to see your notes";
            }

            return result;
        }

        public Task RemoveNote(Note note)
        {
            throw new NotImplementedException();
        }

        public Task UpdateNote(Note note)
        {
            throw new NotImplementedException();
        }

        private HttpClient MakeHttpClient()
        {
            var httpClient = _httpClientFactory.CreateClient("Notes");
            var token = _httpContextAccessor.HttpContext.Request.Cookies[Constants.XAccessToken];
            if (!string.IsNullOrWhiteSpace(token))
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", $"bearer {token}");
            }

            return httpClient;
        }
    }
}
