using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Threading.Tasks;
using yuroca.funtions.Entities;

namespace yuroca.funtions.Funtions
{
    public static class ScheduledFunction
    {
        [FunctionName("ScheduledFunction")]
        public static async Task Run(
        [TimerTrigger("0 */2 * * * *")] TimerInfo myTimer,
        [Table("tarea", Connection = "AzureWebJobsStorage")] CloudTable tareaTable,
        ILogger log)
        {
            log.LogInformation($"Deleted completed function executed at: {DateTime.Now}");

            string filter = TableQuery.GenerateFilterConditionForBool("IsComplete", QueryComparisons.Equal, true);
            TableQuery<TareaEntity> query = new TableQuery<TareaEntity>().Where(filter);
            TableQuerySegment<TareaEntity> completedTareas = await tareaTable.ExecuteQuerySegmentedAsync(query, null);
            int deleted = 0;
            foreach (TareaEntity completedTarea in completedTareas)
            {
                await tareaTable.ExecuteAsync(TableOperation.Delete(completedTarea));
                deleted++;
            }

            log.LogInformation($"Deleted {deleted} items at: {DateTime.Now}");
        }
    }
}
