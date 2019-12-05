using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using cursomvcapi.Models.WS;
using cursomvcapi.Models;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Text;
using System.Drawing;
using System.IO;
using System.Web.Http.Cors;

namespace cursomvcapi.Controllers
{
    [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
    public class ContenidoController : BaseController
    {


        //este metodo todo los contenidos de las tablas COnteniso
        [HttpGet]
        public Reply GetAll()
        {
            Reply oR = new Reply();
            oR.result = 0;

            try
            {
                using (cursomvcapiEntities db = new cursomvcapiEntities())
                {
                    List<ListAnimalsViewModels> lst = List(db);
                    oR.data = lst;
                    oR.result = 1;
                }

            }
            catch (Exception exp)
            {

                oR.message = "ocurrio un error en el servidor";
            }

            return oR;

        }

        //este metodo todo los contenidos de las tablas COnteniso
        [HttpGet]
        public Reply GetByid([FromUri] int id)
        {
            Reply oR = new Reply();
            oR.result = 0;

            try
            {
                using (cursomvcapiEntities db = new cursomvcapiEntities())
                {

                    List<ListAnimalsViewModels> lst = (from a in db.Contenido
                                                      where a.id == id
                                                      select new ListAnimalsViewModels
                                                      {
                                                          id = a.id,
                                                          titulo = a.titulo,
                                                          subtitulo = a.subtitulo,
                                                          detalle = a.detalle,
                                                          categoria = a.categoria,
                                                          tags = a.tags,
                                                          picture = a.picture,
                                                          name_foto = a.name_picture

                                                      }).ToList();



                    oR.data = lst;
                    oR.result = 1;
                }

            }
            catch (Exception exp)
            {

                oR.message = "ocurrio un error en el servidor";
            }

            return oR;

        }

        //trae todas las imagenes que este relacionadas a un Id de la tabla Contenido
        [HttpGet]
        public Reply GetGallery([FromUri] GalleryViewModel model)
        {
            Reply oR = new Reply();
            oR.result = 0;

            try
            {
                using (cursomvcapiEntities db = new cursomvcapiEntities())
                {

                    List<ListGalleryViewModel> lst = (from a in db.Storage
                                                      where a.id_identificador==model.id
                                                      select new ListGalleryViewModel
                                                      {
                                                          id=a.id_galeria,
                                                          foto = a.foto,
                                                          name=a.foto_name

                                                      }).ToList();


                    
                    oR.data = lst;
                    oR.result = 1;
                }

            }
            catch (Exception exp)
            {

                oR.message = "ocurrio un error en el servidor";
            }

            return oR;

        }


        //añadir contenido a la Tabla Contenido por body 
        [HttpPost]
        public Reply Add([FromBody]AnimalViewModel model)
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
                    Contenido oContent = new Contenido();
                    oContent.titulo = model.titulo;
                    oContent.subtitulo = model.subtitulo;
                    oContent.detalle = model.detalle;
                    oContent.categoria = model.categoria;
                    oContent.tags = model.tags;
                    oContent.picture = null;
                    db.Contenido.Add(oContent);
                    db.SaveChanges();

                    //List<ListAnimalsViewModels> lst = List(db);
                    oR.result = 1;
                    oR.data = oContent.id;
                    oR.message = "contenido cargado";

                }
 

            }
            catch (Exception exp)
            {

                oR.message = "ocurrio un error en el servidor";
            }

            return oR;
        }

        //añadir imagen principal al articulo recibe un id por Url 
        [HttpPost]
        public IHttpActionResult AddImg([FromUri] GalleryViewModel model )
        {
            string foldercrate = HttpContext.Current.Server.MapPath("~/images/");
            if (!Directory.Exists(foldercrate))
            {
                Directory.CreateDirectory(foldercrate);
            }

            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var fileupload = HttpContext.Current.Request.Files["Imgpathsave"];
                if (fileupload != null)
                {
                    var saveimages = Path.Combine(HttpContext.Current.Server.MapPath("~/images/"), fileupload.FileName);
                    fileupload.SaveAs(saveimages);

                    cursomvcapiEntities cm = new cursomvcapiEntities();

                    var query = (from a in cm.Contenido
                                 where a.id == model.id
                                 select a).FirstOrDefault();
                    query.picture = saveimages.ToString();
                    query.name_picture = fileupload.FileName.ToString();
                    cm.SaveChanges();
                }

            }
            return Ok();
        }

        // añade imagenes a una tabla Storage y recibe un ID de contenido para relacionar despues
        [HttpPost]
        public IHttpActionResult AddGallery([FromUri] GalleryViewModel model)
        {
            string foldercrate = HttpContext.Current.Server.MapPath("~/images/");
            if (!Directory.Exists(foldercrate))
            {
                Directory.CreateDirectory(foldercrate);
            }

            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var fileupload = HttpContext.Current.Request.Files["Imgpathsave"];
                if (fileupload != null)
                {
                    var saveimages = Path.Combine(HttpContext.Current.Server.MapPath("~/images/"), fileupload.FileName);
                    fileupload.SaveAs(saveimages);

                    cursomvcapiEntities cm = new cursomvcapiEntities();
                    cm.Storage.Add(new Storage
                    {   
                        foto = saveimages.ToString(),
                        foto_name=fileupload.FileName.ToString(),
                        id_identificador=model.id
                    });
                    cm.SaveChanges();
                    
                }

            }
            return Ok();
        }

        //edita el contenido de un elemento de la Tabla COntenido recibiendo por Id 
        [HttpPost]
        public Reply Edit([FromBody]AnimalViewModel model)
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
                    Contenido oContent = db.Contenido.Find(model.id);
                    oContent.titulo = model.titulo;
                    oContent.subtitulo = model.subtitulo;
                    oContent.detalle = model.detalle;
                    oContent.categoria = model.categoria;
                    oContent.tags = model.tags;
      

                    db.Entry(oContent).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    List<ListAnimalsViewModels> lst = List(db);
                    oR.result = 1;
                    oR.data = lst;
                    oR.message = "Contenido modificado";
                }
            }
            catch (Exception exp)
            {

                oR.message = "ocurrio un error en el servidor";
            }

            return oR;
        }

        //eliminar contenido tabla Contenido por el Id del elemento 
        [HttpPost]
        public Reply Delete([FromBody]GalleryViewModel model)
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

                    var delete = (from a in db.Contenido
                                  where a.id == model.id
                                  select a).FirstOrDefault();
                    db.Contenido.Remove(delete);
                    db.SaveChanges();

                    string nombre2 = delete.name_picture.ToString();
                    var fullPath2 = System.Web.Hosting.HostingEnvironment.MapPath("~/images/" + nombre2);
                    if (System.IO.File.Exists(fullPath2))
                    {
                        System.IO.File.Delete(fullPath2);
                    }

                    /*
                    Animal oAnimal = db.Animal.Find(model.id);
                    oAnimal.titulo = model.titulo;
                    oAnimal.subtitulo = model.subtitulo;
                    oAnimal.detalle = model.detalle;
                    oAnimal.categoria = model.categoria;
                    oAnimal.tags = model.tags;

                    db.Entry(oAnimal).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    */
                    var deleteg = (from a in db.Storage
                                  where a.id_identificador == model.id
                                  select a).Take(10).ToList();
                    foreach (var item in deleteg)
                    {
                        db.Storage.Remove(item);
                        db.SaveChanges();

                        string nombre = item.foto_name.ToString();
                        var fullPath = System.Web.Hosting.HostingEnvironment.MapPath("~/images/" + nombre);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }




                    List<ListAnimalsViewModels> lst = List(db);
                    oR.result = 1;
                    oR.data = lst;
                    oR.message = "eliminado con exito";
                }
            }
            catch (Exception exp)
            {

                oR.message = "ocurrio un error en el servidor";
            }

            return oR;
        }

        //eliminar imagen por Id de la BD Storage y del proyecto en general
        [HttpPost]
        public Reply DeleteFromGallery([FromUri] GalleryViewModel model)
        {
            Reply oR = new Reply();
            oR.result = 0;


            try
            {
                using (cursomvcapiEntities db = new cursomvcapiEntities())
                {
                    var delete = (from a in db.Storage
                                    where a.id_galeria == model.id
                                    select a).FirstOrDefault();
          
                    db.Storage.Remove(delete);
                    db.SaveChanges();

                    string nombre = delete.foto_name.ToString();
                    var fullPath = System.Web.Hosting.HostingEnvironment.MapPath("~/images/" + nombre);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }

                    List<ListGalleryViewModel> lst = Lista(db);
                    oR.result = 1;
                    oR.data = lst;
                    oR.message = "Imagen eliminada con exito";

                }
                

            }
            catch (Exception)
            {

                oR.message = "ocurrio un error en el servidor";
            }



            return oR;

        }




        #region Helpers
        private bool Validate(AnimalViewModel model)
        {
            if (model.titulo == null || model.titulo == "")
            {
                error = "el titulo es obligatorio";
                return false;
            }
            if (model.categoria == null )
            {
                error = "la categoria es obligatoria";
                return false;
            }
            if (model.tags == "" || model.tags == null)
            {
                error = "los tags son obligatorios";
                return false;
            }
            if (model.titulo == null && model.categoria == null && model.tags == null)
            {
                error = "todos los campos son obligatorios";
                return false;
            }
            return true;

        }
        private List<ListAnimalsViewModels> List(cursomvcapiEntities db)
        {
            
               List<ListAnimalsViewModels> lst = (from d in db.Contenido
                                                  select new ListAnimalsViewModels
                                                  {
                                                      id = d.id,
                                                      titulo = d.titulo,
                                                      subtitulo = d.subtitulo,
                                                      detalle = d.detalle,
                                                      categoria = d.categoria,
                                                      tags = d.tags,
                                                      picture = d.picture,
                                                      name_foto=d.name_picture


                                                  }).ToList();

            

            //List<ListAnimalsViewModels> lst = (from a in db.Contenido
            //                                   join c in db.StorageImg
            //                                   on a.id equals c.id_identificador
            //                                   select new ListAnimalsViewModels
            //                                   {
            //                                       id = a.id,
            //                                       titulo = a.titulo,
            //                                       subtitulo = a.subtitulo,
            //                                       detalle = a.detalle,
            //                                       categoria = a.categoria,
            //                                       tags = a.tags,
            //                                       picture = a.picture,
            //                                       foto = c.foto

            //                                   }).ToList();


            return lst;

        }

        private List<ListGalleryViewModel> Lista(cursomvcapiEntities db)
        {

            List<ListGalleryViewModel> lst = (from a in db.Storage
                                               select new ListGalleryViewModel
                                               {
                                                   id=a.id_galeria,
                                                   foto = a.foto

                                               }).ToList();


            return lst;
        }

        //funcion que elimina la imagen en el proyecto 



        #endregion


    }
}
