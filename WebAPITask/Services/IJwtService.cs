﻿using WebAPITask.Models;

public interface IJwtService
{
    string GenerateToken(User user);
}