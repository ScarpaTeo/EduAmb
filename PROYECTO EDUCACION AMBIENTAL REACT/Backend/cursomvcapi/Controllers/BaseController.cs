using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using cursomvcapi.Models;
using System.Web.Http.Cors;


namespace cursomvcapi.Controllers
{
    [EnableCors(origins: "http://localhost:3000", headers:"*",methods:"*")]
    public class BaseController : ApiController
    {
        public string error = "token invalido";
        public bool Verify(string token)
        {
            using(cursomvcapiEntities db = new cursomvcapiEntities())
            {
                if (db.User.Where(d => d.token == token).Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
