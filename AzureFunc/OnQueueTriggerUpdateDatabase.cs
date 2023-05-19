using System;
using AzureFunc.Data;
using AzureFunctions.AzureFunc.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AzureFunctions.AzureFunc
{
    public class OnQueueTriggerUpdateDatabase
    {
        private readonly AzureFuncDbContext _dbContext;
        public OnQueueTriggerUpdateDatabase(AzureFuncDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [FunctionName("OnQueueTriggerUpdateDatabase")]
        public void Run([QueueTrigger("SalesRequestInBound", Connection = "AzureWebJobsStorage")]SalesRequest myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            myQueueItem.Status = "Submitted";
            _dbContext.SalesRequests.Add(myQueueItem);
            _dbContext.SaveChanges();
        }
    }
}
