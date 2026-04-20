using System;
using System.Collections.Generic;
using System.Text;

namespace Console1
{
    internal class MedUser
    {
        public class User
        {
            public string Login { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }

            public bool IsDoctor { get; set; }
            public bool IsTemporaryPassword { get; set; }
        }


    }
}
