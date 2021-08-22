using System;

namespace yuroca.common.Models
{
    public class Tarea
    {
        public DateTime createdTime { get; set; }
        //Fecha
        public string TaskDescription { get; set; }
        //Descripcion de tarea
        public bool IsComplete { get; set; }
        //Si la tarea esta completada
    }
}
