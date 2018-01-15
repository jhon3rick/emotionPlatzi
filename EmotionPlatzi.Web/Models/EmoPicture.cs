using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmotionPlatzi.Web.Models
{
    public class EmoPicture
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        // decoradores
        [Required]
        // [MaxLength(10, ErrorMessage ="La ruta supera el tamaño establecido")]
        public string Path { get; set; }

        // proppiedades de estructura o navegacion
        // no equivalen a un campo pero .NET entity framework la va a usar para la arquitectura de la bd
        // llaves foraneas
        // relacion a una colleccion de EmoFace
        public virtual ObservableCollection<EmoFace> Faces { get; set;  }
    }
}