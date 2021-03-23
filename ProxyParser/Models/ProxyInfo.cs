using System;
using System.Collections.Generic;
using System.Text;

namespace ProxyParser.Models
{
    public class ProxyInfo
    {
        public string Ip { get; set; }
        public int Port { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public DateTime? LastCheck { get; set; } = null;
        public int? LastSpeed { get; set; } = null;
        public int? LastPing { get; set; } = null;
    }
}
