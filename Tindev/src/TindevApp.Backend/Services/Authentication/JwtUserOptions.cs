using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TindevApp.Backend.Services.Authentication
{
    public class JwtUserOptions
    {
        public string Secret { get; set; }

        public byte[] ByteSecret { get { return Encoding.ASCII.GetBytes(Secret); } }
    }
}
