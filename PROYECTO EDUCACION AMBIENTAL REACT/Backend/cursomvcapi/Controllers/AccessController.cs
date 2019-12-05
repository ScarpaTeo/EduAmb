using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using cursomvcapi.Models;
using cursomvcapi.Models.WS;
using System.Net.Mail;
using System.Text;
using System.Web.Http.Cors;

namespace cursomvcapi.Controllers
{
    [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
    public class AccessController : BaseController
    {
        //muestra todos los referentes , su datos y su foto para el panel del adminsitrador por lo que te pide que le pases un token valido
        [HttpGet]
        public Reply ADMusers([FromBody] AccessViewModel model)
        {
            Reply oR = new Reply();
            oR.result = 0;
            if (!Verify(model.token))
            {
                oR.message = "no autorizado";
                return oR;
            }

            try
            {
                using (cursomvcapiEntities db = new cursomvcapiEntities())
                {
                    List<ListAdminUserViewModel> lst = List(db);
                    oR.data = lst;
                    oR.result = 1;
                    oR.message = "listado de Referentes panel admin";
                }

            }
            catch (Exception exp)
            {

                oR.message = "ocurrio un error en el servidor";
            }

            return oR;
        }
        //muestra los referentes de forma publica sin solicitar token 
        [HttpGet]
        public Reply GetReferentes()
        {
            Reply oR = new Reply();
            oR.result = 0;

            try
            {
                using (cursomvcapiEntities db = new cursomvcapiEntities())
                {
                    List<ListAdminUserViewModel> lst = Lista(db);
                    oR.data = lst;
                    oR.result = 1;
                    oR.message = "listado de Referentes Publico";
                }

            }
            catch (Exception exp)
            {

                oR.message = "ocurrio un error en el servidor";
            }

            return oR;
        }

        //iniciar sesion , cuando un usuario registrado inicia sesion Se carga un token en la BD que sirver para valir los demas controllers
        [HttpPost]
        public Reply Login([FromBody] AccessViewModel model)
        {
            Reply oR = new Reply();
            oR.result = 0;
            try
            {
                using (cursomvcapiEntities db = new cursomvcapiEntities())
                {
                    var lst = db.User.Where(d => d.email == model.email && d.password == model.password );
                    if (lst.Count() > 0)
                    {
                        oR.result = 1;
                        oR.data = Guid.NewGuid().ToString();

                        User oUser = lst.First();
                        oUser.token =(string)oR.data;
                        db.Entry(oUser).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        oR.message = "Datos erroneos";
                    }
                }
            }
            catch (Exception ex)
            {

                
                oR.message = "a ocurrido un error";
            }
            return oR;
        }

        public Reply Logout([FromBody] AccessViewModel model)
        {
            Reply oR = new Reply();
            oR.result = 0;
            try
            {
                using (cursomvcapiEntities db = new cursomvcapiEntities())
                {

                    var lst = from a in db.User
                              where a.token.Contains(model.token)
                              select a;
                    if (lst.Count() > 0)
                    {     
                        User oUser = lst.First();
                        oUser.token = null;
                        db.Entry(oUser).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        oR.result = 1;
                        oR.message = "sesion cerrada con exito";
                    }
                    else
                    {
                        oR.message = "Datos erroneos";
                    }
                }
            }
            catch (Exception ex)
            {


                oR.message = "a ocurrido un error";
            }
            return oR;
        }

        //verifica si es SuperAdmin
        [HttpGet]
        public Reply IsMaster([FromUri] string token)
        {
            Reply oR = new Reply();
            oR.result = 0;

            using (cursomvcapiEntities db = new cursomvcapiEntities())
            {
                var lst = from a in db.User
                            where a.token.Contains(token)
                            where a.rol == 1
                            select a;
                if (lst.Count() > 0)
                {
                    oR.data=true;
                    oR.message = "Administrador Master";
                }
                else
                {
                    oR.data = false;
                    oR.message = "Acceso Restringido";
                }

            }

            return oR;
        }

        //verifica si es Admin
        [HttpGet]
        public Reply IsAdmin([FromUri] string token)
        {
            Reply oR = new Reply();
            oR.result = 0;

            using (cursomvcapiEntities db = new cursomvcapiEntities())
            {
                var lst = from a in db.User
                          where a.token.Contains(token)
                          where a.rol == 2
                          select a;
                if (lst.Count() > 0)
                {
                    oR.data = true;
                    oR.message = "Administrador Master";
                }
                else
                {
                    oR.data = false;
                    oR.message = "Acceso Restringido";
                }

            }

            return oR;
        }

        //Registra un referente nuevo solo si el usuario logeado tiene el rol 1 
        [HttpPost]
        public Reply Register([FromBody] AccesNewUserModel model)
        {
            Reply oR = new Reply();
            oR.result = 0;

            if (!Verify(model.token))
            {               
                oR.message = "no autorizado";
                return oR;
            }
            //validaciones 
            if (!Validate(model))
            {
                oR.message = error;
                return oR;
            }
            try
            {
                using (cursomvcapiEntities db = new cursomvcapiEntities())
                {
                    var lst = from a in db.User
                                       where a.token.Contains(model.token)
                                       where a.rol == 1
                                       select a;
                    if (lst.Count()>0)
                    {
                        //valida que el Email no este ya registrado
                        var customer = db.User.FirstOrDefault(x => x.email == model.email);

                        if (customer == null)
                        {
                            User NewUser = new User();
                            NewUser.nombre = model.nombre;
                            NewUser.apellido = model.apellido;
                            NewUser.ubicacion = model.ubicacion;
                            NewUser.email = model.email;
                            NewUser.password = model.password;
                            NewUser.foto = null;
                            NewUser.rol = 2;
                        
                            db.User.Add(NewUser);
                            db.SaveChanges();
                            oR.result = 1;
                            oR.data = NewUser.id;
                            oR.message = "usuario registrado";

                            //body en Html para los correo de Registro
                            string HtmlString = string.Format(@"

                                <html>
                                    <body>
                                        <div>
                                            <img src ='https://images.pexels.com/photos/34950/pexels-photo.jpg?auto=compress&cs=tinysrgb&dpr=1&w=500' alt ='#'>

                                            <h2> Bienvenido {0} </h2>
    
                                            <br><br>
    
                                            <h4> ahora formas parte del Grupo de Referentes de <strong> Educacion Ambiental Chaco </strong> </h4>
       
                                            <p> ahora podes ingresar con tus datos en la plataforma digial
                                                    <br> <br>
                                                    cuenta de Usuario: <strong> {1} </strong>
             
                                                     <br><br>
                                                     contraseña: <strong> {2} </strong>
                
                                            </p>
                                        </div>
                                    </body>
                                </html> ", model.nombre.ToString(), model.email.ToString(), model.password.ToString());

                          

                            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                            MailMessage mail = new MailMessage();
                            mail.From = new MailAddress("OwnerBuilderIntegrate@gmail.com");
                            mail.To.Add(model.email.ToString());
                            mail.Subject = "Bienvenido Nuevo Referente Ambiental";
                            mail.Body = HtmlString;
                            SmtpServer.Port = 587;
                            SmtpServer.Credentials = new System.Net.NetworkCredential("OwnerBuilderIntegrate@gmail.com", "informatorio");
                            mail.IsBodyHtml = true;
                            mail.Priority = MailPriority.Normal;

                            SmtpServer.EnableSsl = true;
                            SmtpServer.Send(mail);
                            mail.Dispose();

                            
                            
                        }
                        else
                        {
                            oR.result = 0;
                            oR.message = "el Email ya ha sido Registrado";
                        }

                    }
                    else
                    {
                        oR.result = 0;
                        oR.message = "no tienes el permiso para registrar personas";
                    }



               
                }
            }
            catch (Exception ex)
            {


                oR.message = "a ocurrido un error";
            }
            return oR;
        }

        //elimina una referente solo si el usuario logeado pertenece al rol 1  y eso lo verifica por el token
        [HttpPost]
        public Reply Eliminar([FromBody] DeleteViewAccesModel model)
        {
            Reply oR = new Reply();
            oR.result = 0;

            if (!Verify(model.token))
            {
                oR.message = "no autorizado";
                return oR;
            }
            try
            {
                using (cursomvcapiEntities db = new cursomvcapiEntities())
                {
                    var lst = from a in db.User
                              where a.token.Contains(model.token)
                              where a.rol == 1
                              select a;
                    if (lst.Count() > 0)
                    {

                        var delete = (from a in db.User
                                      where a.id == model.id
                                      select a).FirstOrDefault();
                        db.User.Remove(delete);
                        db.SaveChanges();
                        oR.result = 1;
                        oR.message = "usuario eliminado";

                        string nombre = delete.foto.ToString();
                        var fullPath = System.Web.Hosting.HostingEnvironment.MapPath("~/referentes/" + nombre);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }

                    }
                    else
                    {
                        oR.result = 0;
                        oR.message = "no tienes el permiso para eliminar personas";
                    }




                }
            }
            catch (Exception ex)
            {


                oR.message = "a ocurrido un error";
            }
            return oR;
        }
        //añadir foto al registro de un Referente por su Id
        [HttpPost]
        public IHttpActionResult AddImg([FromUri] AccessViewModel model)
        {
            string foldercrate = HttpContext.Current.Server.MapPath("~/referentes/");
            if (!Directory.Exists(foldercrate))
            {
                Directory.CreateDirectory(foldercrate);
            }

            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var fileupload = HttpContext.Current.Request.Files["Imgpathsave"];
                if (fileupload != null)
                {
                    var saveimages = Path.Combine(HttpContext.Current.Server.MapPath("~/referentes/"), fileupload.FileName);
                    fileupload.SaveAs(saveimages);

                    cursomvcapiEntities cm = new cursomvcapiEntities();

                    var query = (from a in cm.User
                                 where a.id == model.id
                                 select a).FirstOrDefault();
                    query.foto = saveimages.ToString();
                    query.name_foto= fileupload.FileName.ToString();
                    cm.SaveChanges();
                }

            }
            return Ok();
        }



        #region
        private bool Validate(AccesNewUserModel model)
        {
            if (model.nombre == null || model.nombre == "")
            {
                error = "el nombre es obligatorio";
                return false;
            }
            if (model.email == null||model.email=="")
            {
                error = "el email es obligatorio";
                return false;
            }
            if (model.password == "" || model.password == null)
            {
                error = "el password es obligatorio";
                return false;
            }
            if (model.nombre == null && model.email == null && model.password == null)
            {
                error = "todos los campos son obligatorios";
                return false;
            }
            return true;

        }
        //muestra la lista de referentes para el panel de administrador
        private List<ListAdminUserViewModel> List(cursomvcapiEntities db)
        {
            List<ListAdminUserViewModel> lst = (from d in db.User
                                               select new ListAdminUserViewModel
                                               {
                                                   id = d.id,
                                                   nombre = d.nombre,
                                                   apellido=d.apellido,
                                                   ubicacion=d.ubicacion,
                                                   email = d.email,
                                                   foto=d.foto,
                                                   rol=d.rol
                                                   
                                               }).ToList();

            return lst;
        }
        //muestra la lista publica de referentes
        private List<ListAdminUserViewModel> Lista(cursomvcapiEntities db)
        {
            List<ListAdminUserViewModel> lst = (from d in db.User
                                                select new ListAdminUserViewModel
                                                {
                                                    nombre = d.nombre,
                                                    apellido = d.apellido,
                                                    ubicacion = d.ubicacion,
                                                    email = d.email,
                                                    foto = d.foto,
                                                    name=d.name_foto
                     
                                                }).ToList();

            return lst;
        }
        
        #endregion
    }
}
