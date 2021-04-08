using System;
using System.Collections.Generic;
using System.Text;

namespace ProxyParser.Models
{
    public class ProxyInfo
    {
        public int Id { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }
        public ProxyType Type { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public DateTime? LastCheck { get; set; } = null;
        public int? LastSpeed { get; set; } = null;
        public int? LastPing { get; set; } = null;
    }

    public class ProxyType
    {
        public bool HTTP { get; set; } = false;
        public bool HTTPS { get; set; } = false;
        public bool SOCKS4 { get; set; } = false;
        public bool SOCKS5 { get; set; } = false;
    }
}
