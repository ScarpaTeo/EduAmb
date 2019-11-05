using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cursomvcapi.Models.WS
{
    public class AccessViewModel : SecurityViewModel
    {
        public int id { get; set; }
        public string email { get; set; }
        public string password { get; set; }

    }
}