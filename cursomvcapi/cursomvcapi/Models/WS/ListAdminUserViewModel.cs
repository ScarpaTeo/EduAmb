﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cursomvcapi.Models.WS
{
    public class ListAdminUserViewModel
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public int? rol { get; set; }
    }
}