using Front.Entities;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;


namespace Front.Services
{
    /// <summary>
    /// Base class for all services that need to be authenticated (that requests JWT protected API)
    /// </summary>
    public abstract class AuthService
    {
        protected readonly HttpClient _httpClient;
        protected readonly CustomAuthenticationStateProvider _auth;

        public AuthService(HttpClient httpClient,
                           AuthenticationStateProvider customAuthenticationStateProvider
            )
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new System.Uri("http://localhost:5000/");
            _auth = (CustomAuthenticationStateProvider)customAuthenticationStateProvider;
            _auth.AuthenticationStateChanged += UpdateJWToken;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _auth.JWToken);
        }

        private void UpdateJWToken(Task<AuthenticationState> task)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _auth.JWToken);
        }

        protected async Task<UserData?> GetUserDataAsync()
        {
            return await _auth.GetUserDataAsync();
        }
    }
}


