using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace yuroca.funtions.Entities
{
    public class TareaEntity : TableEntity
    {
        //Tiene los mismos campo de models tarea
        public DateTime createdTime { get; set; }
        //Fecha
        public string TaskDescription { get; set; }
        //Descripcion de tarea
        public bool IsComplete { get; set; }
        //Si la tarea esta completada
    }
}
