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
        public string? ErrorMessage { get; set; }
    }

    public class RegisterService: AuthService
    {
        public RegisterService(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<CreateUserResult> CreateUser(string email, string username, string password)
        {
            var emailValidationAttribute = new EmailAddressAttribute();
          
            if (!emailValidationAttribute.IsValid(email))
            {
                
                return new CreateUserResult { IsSuccess = false, ErrorMessage = "Format d'e-mail non valide." };
            }

            if (string.IsNullOrWhiteSpace(username))
            {
                return new CreateUserResult { IsSuccess = false, ErrorMessage = "Le nom d'utilisateur ne peut pas être vide." };
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return new CreateUserResult { IsSuccess = false, ErrorMessage = "Le mot de passe ne peut pas être vide." };
            }


            UserCreateModel u = new UserCreateModel() { Email = email, Name = username, Password = password };
            var res = await _httpClient.PostAsJsonAsync<UserCreateModel>("api/User/register", u);

            if (res.IsSuccessStatusCode)
            {
                return new CreateUserResult { IsSuccess = true, ErrorMessage = null };
            }
            else
            {
              
                var errorMessage = await res.Content.ReadAsStringAsync();

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










