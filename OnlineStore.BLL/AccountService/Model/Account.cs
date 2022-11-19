﻿using OnlineStore.Domain.Models;

namespace OnlineStore.BLL.AccountService.Model
{
    public class Account 
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}