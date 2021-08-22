using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using yuroca.common.Models;
using yuroca.common.Responses;
using yuroca.funtions.Entities;

namespace yuroca.funtions.Funtions
{
    public static class TareaApi
    {
        //Create a new Task, this is with post because is a new
        [FunctionName(nameof(createTarea))]
        public static async Task<IActionResult> createTarea(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "tarea")] HttpRequest req,
            [Table("tarea", Connection = "AzureWebJobsStorage")] CloudTable tareaTable,
            ILogger log)
        {
            log.LogInformation("Received a new taks");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Tarea tarea = JsonConvert.DeserializeObject<Tarea>(requestBody);

            if (string.IsNullOrEmpty(tarea?.TaskDescription))
            {
                return new BadRequestObjectResult(new Response
                {
                    IsSuccess = false,
                    Message = "The request  must have a TaskDescription"
                });
            }

            //Entrada en la tabla
            TareaEntity tareaEntity = new TareaEntity
            {
                createdTime = DateTime.UtcNow, //hora de londres
                ETag = "*",
                IsComplete = false,
                PartitionKey = "TAREA",
                RowKey = Guid.NewGuid().ToString(),
                TaskDescription = tarea.TaskDescription
            };

            //Guardar la entidad
            TableOperation addOperation = TableOperation.Insert(tareaEntity);
            await tareaTable.ExecuteAsync(addOperation);

            string message = "New task stored in table";
            log.LogInformation(message);

            return new OkObjectResult(new Response
            {
                IsSuccess = true,
                Message = message,
                Result = tareaEntity
            });
        }

        //Update a Task, this is with put because is update a register  
        [FunctionName(nameof(UpdateTarea))]
        public static async Task<IActionResult> UpdateTarea(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "tarea/{id}")] HttpRequest req,
            [Table("tarea", Connection = "AzureWebJobsStorage")] CloudTable tareaTable,
            string id,
            ILogger log)
        {
            log.LogInformation($"Update for todo:{id}, received");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Tarea tarea = JsonConvert.DeserializeObject<Tarea>(requestBody);

            //Validate tarea id
            TableOperation findOperation = TableOperation.Retrieve<TareaEntity>("TAREA", id);
            TableResult findResult = await tareaTable.ExecuteAsync(findOperation);
            if(findResult.Result == null)
            {
                return new BadRequestObjectResult(new Response
                {
                    IsSuccess = false,
                    Message = "Tarea not found."
                });
            }

            //Update  tarea
            TareaEntity tareaEntity = (TareaEntity)findResult.Result;
            tareaEntity.IsComplete = tarea.IsComplete;
            if (!string.IsNullOrEmpty(tarea.TaskDescription))
            {
                tareaEntity.TaskDescription = tarea.TaskDescription;  
            }

            //Guardar la entidad
            TableOperation addOperation = TableOperation.Replace(tareaEntity);
            await tareaTable.ExecuteAsync(addOperation);

            string message = $"Tarea: {id}, update in table ";
            log.LogInformation(message);

            return new OkObjectResult(new Response
            {
                IsSuccess = true,
                Message = message,
                Result = tareaEntity
            });
        }

         //Recuperate task- get all task
        [FunctionName(nameof(GetAllTarea))]
        public static async Task<IActionResult> GetAllTarea(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "tarea")] HttpRequest req,
            [Table("tarea", Connection = "AzureWebJobsStorage")] CloudTable tareaTable,
            ILogger log)
            //Siempre inyectar request aunque no se necesite
        {
            log.LogInformation("Get all task received");

            TableQuery<TareaEntity> query = new TableQuery<TareaEntity>();
            TableQuerySegment<TareaEntity> tareas = await tareaTable.ExecuteQuerySegmentedAsync(query, null);

            string message = "Retrieved all task";
            log.LogInformation(message);

            return new OkObjectResult(new Response
            {
                IsSuccess = true,
                Message = message,
                Result = tareas
            });
        }

        //Recuperate task- get one element of the task
        [FunctionName(nameof(GetTareaById))]
        public static IActionResult GetTareaById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "tarea/{id}")] HttpRequest req,
            [Table("tarea", "TAREA", "{id}", Connection = "AzureWebJobsStorage")] TareaEntity tareaEntity,
            string id,
            ILogger log)
        //Siempre inyectar request aunque no se necesite
        {
            log.LogInformation($"Get task: {id}, received");

            if (tareaEntity == null)
            {
                return new BadRequestObjectResult(new Response
                {
                    IsSuccess = false,
                    Message = "Tarea not found."
                });
            } 

            string message = $"Task: {tareaEntity.RowKey}, retreived";
            log.LogInformation(message);

            return new OkObjectResult(new Response
            {
                IsSuccess = true,
                Message = message,
                Result = tareaEntity
            });
        }

        [FunctionName(nameof(DeleteTarea))]
        public static async Task<IActionResult> DeleteTarea(
           [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "tarea/{id}")] HttpRequest req,
           [Table("tarea", "TAREA", "{id}", Connection = "AzureWebJobsStorage")] TareaEntity tareaEntity,
           [Table("tarea", Connection = "AzureWebJobsStorage")] CloudTable tareaTable,
           string id,
           ILogger log)
        //Siempre inyectar request aunque no se necesite
        {
            log.LogInformation($"Delete task: {id}, received");

            if (tareaEntity == null)
            {
                return new BadRequestObjectResult(new Response
                {
                    IsSuccess = false,
                    Message = "Tarea not found."
                });
            }

            await tareaTable.ExecuteAsync(TableOperation.Delete(tareaEntity));
            string message = $"Task delete: {tareaEntity.RowKey}, retreived";
            log.LogInformation(message);

            return new OkObjectResult(new Response
            {
                IsSuccess = true,
                Message = message,
                Result = tareaEntity
            });
        }
    }
}
