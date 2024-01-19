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
        protected CustomAuthenticationStateProvider? _auth;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new System.Uri("http://localhost:5000/");
            _auth = null;
        }

        public void SetAuthenticator(CustomAuthenticationStateProvider auth) { // set auth when null
            if (_auth != null) return;
            _auth = auth;
            auth.AuthenticationStateChanged += UpdateJWToken;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth.JWToken);
        }

        private void UpdateJWToken(Task<AuthenticationState> task)
        {
            if (_auth != null) _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _auth.JWToken);
        }
    }
}


