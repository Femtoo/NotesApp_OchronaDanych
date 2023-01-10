using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using NotesApp.Models;
using NotesApp.Models.ViewModels;
using NotesApp.Utility;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NotesApp.Services.NoteService
{
    public class NoteService : INoteService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IPasswordHasher<NoteDTO> _passwordHasher;
        public NoteService(IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory, IPasswordHasher<NoteDTO> passwordHasher)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
            _passwordHasher = passwordHasher;
        }

        public async Task AddNote(NoteDTO note)
        {
            //szyfrowanie
            note.Content = Encrypt(note.Content, note.PasswordHash);
            var hashed = _passwordHasher.HashPassword(new NoteDTO { Id = note.Id }, note.PasswordHash);
            note.PasswordHash = hashed;

            var httpClient = MakeHttpClient();

            var httpResponse = await httpClient.PostAsJsonAsync("addnote", note);

        }

        public async Task<NoteDTO?> GetNote(PasswordVM password)
        {
            NoteDTO? note = new NoteDTO();
            var hash = await GetHash(password.NoteId);

            var result = _passwordHasher.VerifyHashedPassword(new NoteDTO { Id = 0 }, hash, password.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return null;
            }

            var httpClient = MakeHttpClient();

            var httpResponse = await httpClient.PostAsJsonAsync("getnote", password.NoteId);

            if (httpResponse.IsSuccessStatusCode)
            {
                note = await httpResponse.Content.ReadFromJsonAsync<NoteDTO>();
                if(note == null)
                {
                    return null;
                }
                note.Content = Decrypt(note.Content, password.Password);
                return note;
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<NoteDTO>> GetAllNotes()
        {
            var httpClient = MakeHttpClient();

            var httpResponse = await httpClient.GetAsync("allnotes");

            List<NoteDTO>? notes = new List<NoteDTO>();

            if (httpResponse.IsSuccessStatusCode)
            {
                notes = await httpResponse.Content.ReadFromJsonAsync<List<NoteDTO>>();
            }
            else
            {
                
            }

            return notes;
        }

        public async Task<bool> RemoveNote(int id)
        {
            var httpClient = MakeHttpClient();

            var httpResponse = await httpClient.PostAsJsonAsync("deletenote", id);

            if (httpResponse.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task UpdateNote(NoteDTO note)
        {
            //szyfrowanie
            note.Content = Encrypt(note.Content, note.PasswordHash);
            note.PasswordHash = "---";

            var httpClient = MakeHttpClient();

            var httpResponse = await httpClient.PostAsJsonAsync("updatenote", note);
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

        private string Encrypt(string plain, string pass)
        {
            byte[] key = new Rfc2898DeriveBytes(pass, new byte[8], 1000).GetBytes(16);
            // Get bytes of plaintext string
            byte[] plainBytes = Encoding.UTF8.GetBytes(plain);

            // Get parameter sizes
            int nonceSize = AesGcm.NonceByteSizes.MaxSize;
            int tagSize = AesGcm.TagByteSizes.MaxSize;
            int cipherSize = plainBytes.Length;

            // We write everything into one big array for easier encoding
            int encryptedDataLength = 4 + nonceSize + 4 + tagSize + cipherSize;
            Span<byte> encryptedData = encryptedDataLength < 1024
                                     ? stackalloc byte[encryptedDataLength]
                                     : new byte[encryptedDataLength].AsSpan();

            // Copy parameters
            BinaryPrimitives.WriteInt32LittleEndian(encryptedData.Slice(0, 4), nonceSize);
            BinaryPrimitives.WriteInt32LittleEndian(encryptedData.Slice(4 + nonceSize, 4), tagSize);
            var nonce = encryptedData.Slice(4, nonceSize);
            var tag = encryptedData.Slice(4 + nonceSize + 4, tagSize);
            var cipherBytes = encryptedData.Slice(4 + nonceSize + 4 + tagSize, cipherSize);

            // Generate secure nonce
            RandomNumberGenerator.Fill(nonce);

            // Encrypt
            using var aes = new AesGcm(key);
            aes.Encrypt(nonce, plainBytes.AsSpan(), cipherBytes, tag);

            // Encode for transmission
            return Convert.ToBase64String(encryptedData);
        }

        private string Decrypt(string cipher, string pass)
        {
            byte[] key = new Rfc2898DeriveBytes(pass, new byte[8], 1000).GetBytes(16);
            // Decode
            Span<byte> encryptedData = Convert.FromBase64String(cipher).AsSpan();

            // Extract parameter sizes
            int nonceSize = BinaryPrimitives.ReadInt32LittleEndian(encryptedData.Slice(0, 4));
            int tagSize = BinaryPrimitives.ReadInt32LittleEndian(encryptedData.Slice(4 + nonceSize, 4));
            int cipherSize = encryptedData.Length - 4 - nonceSize - 4 - tagSize;

            // Extract parameters
            var nonce = encryptedData.Slice(4, nonceSize);
            var tag = encryptedData.Slice(4 + nonceSize + 4, tagSize);
            var cipherBytes = encryptedData.Slice(4 + nonceSize + 4 + tagSize, cipherSize);

            // Decrypt
            Span<byte> plainBytes = cipherSize < 1024
                                  ? stackalloc byte[cipherSize]
                                  : new byte[cipherSize];
            using var aes = new AesGcm(key);
            aes.Decrypt(nonce, cipherBytes, tag, plainBytes);

            // Convert plain bytes back into string
            return Encoding.UTF8.GetString(plainBytes);
        }

        private async Task<string?> GetHash(int id)
        {
            string? result = string.Empty;
            var httpClient = MakeHttpClient();

            var httpResponse = await httpClient.PostAsJsonAsync("getnotehash", id);

            if (httpResponse.IsSuccessStatusCode)
            {
                result = await httpResponse.Content.ReadAsStringAsync();
                if (result == null)
                {
                    return null;
                }
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
