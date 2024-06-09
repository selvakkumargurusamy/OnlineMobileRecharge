﻿namespace MobileRecharge.Domain.Dtos;

public class UserDto
{
    public int Id { get; set; }    

    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public string Email { get; set; } = null!;

    public bool IsVerified { get; set; }
}
