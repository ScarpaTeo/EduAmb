using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cursomvcapi.Models.WS
{
    public class GalleryViewModel : SecurityViewModel
    {
        public int id { get; set; }
        public string picture { get; set; }
    }
}