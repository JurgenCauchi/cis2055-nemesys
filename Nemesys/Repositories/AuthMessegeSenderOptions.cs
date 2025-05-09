﻿using Microsoft.AspNetCore.Mvc;

namespace Nemesys.Repositories
{
    public class AuthMessageSenderOptions
    {
        public string? Host { get; set; }
        public int Port { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? From { get; set; }
    }
}
