using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmotionPlatzi.Web.Models
{
    public class EmoEmotion
    {
        public int Id { get; set; }
        public float Score { get; set; }
        public int EmoFaceId { get; set; }

        // ctrl + . -> generar nuevo tipo -> enum en new file
        public EmoEmotionEnum EmotionType { get; set; }

        // Relaciones 
        public virtual EmoFace Face { get; set; }
    }
}