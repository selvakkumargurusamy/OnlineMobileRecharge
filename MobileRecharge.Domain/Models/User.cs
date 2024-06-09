﻿using System.Collections;

namespace MobileRecharge.Domain.Models;

public class User
{

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }    
   
    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public string Email { get; set; } = null!;

    public bool IsVerified { get; set; }

    [InverseProperty(nameof(Beneficiarie.User))]
    public virtual ICollection<Beneficiarie> Beneficiaries { get; set; } = new List<Beneficiarie>();
}