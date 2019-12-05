using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cursomvcapi.Models.WS
{
    public class AccesNewUserModel : SecurityViewModel
    {
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string ubicacion { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string foto { get; set; }
        public int? rol { get; set; }
        public string name_foto { get; set; }
    }
}