using Front.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Security.Claims;
using System.Text.RegularExpressions;



namespace Front.Services
{
    public class CreateUserResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class RegisterService
    {
        private readonly HttpClient _httpClient;

        public RegisterService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new System.Uri("http://localhost:5000/");
        }

        public async Task<CreateUserResult> CreateUser(string email, string username, string password)
        {
            var emailValidationAttribute = new EmailAddressAttribute();
            // Vérification du format de l'e-mail avec une regex
            if (!emailValidationAttribute.IsValid(email))
            {
                // Le format de l'e-mail n'est pas valide
                return new CreateUserResult { IsSuccess = false, ErrorMessage = "Format d'e-mail non valide." };
            }

            UserCreateModel u = new UserCreateModel() { Email = email, Name = username, Password = password };
            var res = await _httpClient.PostAsJsonAsync<UserCreateModel>("api/User/register", u);

            if (res.IsSuccessStatusCode)
            {
                return new CreateUserResult { IsSuccess = true, ErrorMessage = null };
            }
            else
            {
                // Récupérez le message d'erreur du serveur en cas d'échec de la requête HTTP
                var errorMessage = await res.Content.ReadAsStringAsync();

                // Retournez le message d'erreur au front-end
                return new CreateUserResult { IsSuccess = false, ErrorMessage = errorMessage };
            }
        }

        /*
        private bool IsValidEmail(string email)
        {
            string emailPattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";
            Regex regex = new Regex(emailPattern);

            return regex.IsMatch(email);
        }
        */
    }
}










