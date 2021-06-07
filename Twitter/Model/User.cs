using System;

namespace Twitter.Model
{
    public class User
    {
        public string Username { get; set; }
        
        public string Password { get; set; }

        public string Env { get; set; }

        public string Token { get; set; }

        public DateTime? TokenExpires { get; set; }
    }
}
