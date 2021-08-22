using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using yuroca.common.Models;
using yuroca.funtions.Entities;

namespace yuroca.test.Helpers
{
    //Class to moquear
    public class TestFactory
    {
        public static TareaEntity GetTareaEntity()
        {
            return new TareaEntity
            {
                ETag = "*",
                PartitionKey = "TODO",
                RowKey = Guid.NewGuid().ToString(),
                createdTime = DateTime.UtcNow,
                IsComplete = false,
                TaskDescription = "Task: destroy the World."
            };
        }

        //Mapear los metodos de crear
        public static DefaultHttpRequest CreateHttpRequest(Guid tareaId, Tarea tareaRequest)
        {
            string request = JsonConvert.SerializeObject(tareaRequest);
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Body = GenerateStreamFromString(request),
                Path = $"/{tareaId}"
            };
        }

        //Mapear los metodos de update
        public static DefaultHttpRequest CreateHttpRequest(Guid tareaId)
        {
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Path = $"/{tareaId}"
            };
        }

        //Mapear los metodos de Eliminar
        public static DefaultHttpRequest CreateHttpRequest(Tarea tareaRequest)
        {
            string request = JsonConvert.SerializeObject(tareaRequest);
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Body = GenerateStreamFromString(request)
            };
        }

        //Mapear el metodo getAll
        public static DefaultHttpRequest CreateHttpRequest()
        {
            return new DefaultHttpRequest(new DefaultHttpContext());
        }

        public static Tarea GetTareaRequest()
        {
            return new Tarea
            {
                createdTime = DateTime.UtcNow,
                IsComplete = false,
                TaskDescription = "Try the save of world"
            };
        }
        public static Stream GenerateStreamFromString(string stringToConvert)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(stringToConvert);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        //ESTA SE DEBE DE MODIFICAR DEPENDIENDO DE CADA PROYECTO
        public static ILogger CreateLogger(LoggerTypes type = LoggerTypes.Null)
        {
            ILogger logger;
            if (type == LoggerTypes.List)
            {
                logger = new ListLogger();
            }
            else
            {
                logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");
            }
            return logger;
        }
    }
}
