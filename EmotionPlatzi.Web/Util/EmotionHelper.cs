using EmotionPlatzi.Web.Models;
using Microsoft.ProjectOxford.Common.Contract;
using Microsoft.ProjectOxford.Emotion;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace EmotionPlatzi.Web.Util
{
    public class EmotionHelper
    {
        public EmotionServiceClient emoClient;

        public EmotionHelper(string key)
        {
            emoClient = new EmotionServiceClient(key);
        }

        // cuando un metodo es Async debe tener Task
        public async Task<EmoPicture> DetectAndExtractFacesAsync(Stream imageStream)
        {
            Emotion[] emotions = await emoClient.RecognizeAsync(imageStream);

            var emoPicture = new EmoPicture();
            emoPicture.Faces = ExtractFaces(emotions, emoPicture);
            return emoPicture;
        }

        private ObservableCollection<EmoFace> ExtractFaces(Emotion[] emotions, EmoPicture emoPicture)
        {
            // ObservableCollection emite notificaciones cada vez que cambiamos sus miembros asi entity framework reconoceria los cambios
            var listaFaces = new ObservableCollection<EmoFace>();

            foreach (var emotion in emotions)
            {
                var emoFace = new EmoFace()
                {
                    X = emotion.FaceRectangle.Left,
                    Y = emotion.FaceRectangle.Top,
                    Width = emotion.FaceRectangle.Width,
                    Height = emotion.FaceRectangle.Height,
                    Picture = emoPicture,

                };

                emoFace.Emotions = ProcessEmotion(emotion.Scores, emoFace);
                listaFaces.Add(emoFace);
            }
            return listaFaces;
        }

        private ObservableCollection<EmoEmotion> ProcessEmotion(EmotionScores scores, EmoFace emoFace)
        {
            var emotionList = new ObservableCollection<EmoEmotion>();

            // Reflection 
            // permite aaceder a los metodos propios de los componentes en tiempo de ejecucion
            // (que sean publicos o propios de la instancia)
            // traer todas las propiedades
            var properties = scores.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // linq para hacer query en una lista y filtrar tofas las propiedades float
            // var filterProperties = from p in properties where p.PropertyType == typeof(float) select p;

            var filterProperties = properties.Where(p => p.PropertyType == typeof(float));

            var emoType = EmoEmotionEnum.Undertermined;
            foreach (var prop in filterProperties)
            {
                // asignar el valor en un enum
                // convierte el parametro en una opcion del enumerador ENUM -> retorna booleano


                Enum.TryParse<EmoEmotionEnum>(prop.Name, out emoType);
                var emoEmotion = new EmoEmotion()
                {
                    Score = (float)prop.GetValue(scores),
                    EmotionType = emoType,
                    Face = emoFace
                };

                emotionList.Add(emoEmotion);
            }

            return emotionList;
            
        }
    }
}