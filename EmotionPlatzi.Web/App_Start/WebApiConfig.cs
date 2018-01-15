using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace EmotionPlatzi.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // correccion bucle sql
            // Newtonsoft.Json.ReferenceLoopHandling
            //config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            // preserva las relaciones de los bucles en objetos
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
            config.Formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;

        }
    }
}
