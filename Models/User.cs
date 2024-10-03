using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.NET_HW_09_10.Models
{
    internal class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int CountLoginAttempts { get; set; } = 0;
        public bool IsBannedForLogin { get; set; } = false;
    }
}
