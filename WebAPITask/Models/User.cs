using System;
using System.Collections.Generic;

namespace WebAPITask.Models;

public partial class User
{
    public int n_id { get; set; }
    public string s_name { get; set; } = null!;
    public string s_lastname { get; set; } = null!;
    public string s_username { get; set; } = null!;
    public string s_description { get; set; } = null!;
}

public class LoginDto
{
    public string username { get; set; }
    public string password { get; set; }
}

public class UserDto
{
    public int? n_id { get; set; }
    public string? s_name { get; set; }
    public string? s_lastname { get; set; }
    public string? s_username { get; set; }
    public string? s_description { get; set; }
    public string mensaje { get; set; } = string.Empty;
}

public class UserListDto
{
    public int n_id { get; set; }
    public string s_name { get; set; }
    public string s_lastname { get; set; }
    public string s_username { get; set; }
}

public class LoginResponseDto
{
    public bool success { get; set; }
    public string message { get; set; }
    public string token { get; set; }
    public UserDto user { get; set; }
}