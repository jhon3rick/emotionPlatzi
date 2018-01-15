using EmotionPlatzi.Web.Models;
using EmotionPlatzi.Web.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EmotionPlatzi.Web.Controllers
{
    public class EmoUploaderController : Controller
    {
        string key;
        string serverFolderPath;
        EmotionHelper emotionHelper;

        EmotionPlatziWebContext db = new EmotionPlatziWebContext();

        public EmoUploaderController()
        {


            // configuracion de variables de entorno, file Web.config -> appSettings
            key = ConfigurationManager.AppSettings["EMOTION_KEY"];
            serverFolderPath = ConfigurationManager.AppSettings["UPLOAD_DIR"];

            emotionHelper = new EmotionHelper(key);
        }

        // GET: EmoUploader
        public ActionResult Index()
        {
            return View();
        }

        // POST: EmoUploader
        // async por que hay un llamado a un metodo wait
        [HttpPost]
        public async Task<ActionResult> Index(HttpPostedFileBase file)
        {

            // if(file != null && file.ContentLength> 0)
            if (file?.ContentLength> 0)
            {
                // file.FileName -> nombre del archivo
                // Guid -> randomico
                var pictureName = Guid.NewGuid().ToString();

                // return extencion file
                pictureName += Path.GetExtension(file.FileName);

                // pasear ruta dinamica a la del servidor
                var route = Server.MapPath(serverFolderPath);
                route = route + "/" + pictureName;

                // save file in server -> path in system
                file.SaveAs(route);

                // como es un metodo asyncrono debe tener await
                var emoPicture = await emotionHelper.DetectAndExtractFacesAsync(file.InputStream);
                emoPicture.Nombre = file.FileName;

                // path relativo desde la web
                emoPicture.Path = serverFolderPath + "/" + pictureName;

                db.EmoPictures.Add(emoPicture);
                await db.SaveChangesAsync();

                // redireccione a accion Details en controlador EmoPicture
                // variables a enviar a details
                return RedirectToAction("Details", "EmoPictures", new { Id = emoPicture.Id });
            }
            return View();
        }
    }
}