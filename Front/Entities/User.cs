﻿namespace Front.Entities
{
    public class UserDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Token { get; set; }
    }

    public class UserLogin
    {
        public required string Name { get; set; }
        public required string Pass { get; set; }
    }
}

public class UserCreateModel
{
    public required string Password { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
}
