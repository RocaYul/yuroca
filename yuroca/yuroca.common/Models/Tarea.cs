using System;

namespace yuroca.common.Models
{
    internal class Tarea
    {
        public DateTime createdTime { get; set; }
        //Fecha
        public string TaskDescription { get; set; }
        //Descripcion de tarea
        public bool IsComplete { get; set; }
        //Si la tarea esta completada
    }
}
