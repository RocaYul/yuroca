﻿using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace yuroca.test.Helpers
{
    public class MockCloudTableTarea : CloudTable
    {
        public MockCloudTableTarea(Uri tableAddress) : base(tableAddress)
        {
        }

        public MockCloudTableTarea(Uri tableAbsoluteUri, StorageCredentials credentials) : base(tableAbsoluteUri, credentials)
        {
        }

        public MockCloudTableTarea(StorageUri tableAddress, StorageCredentials credentials) : base(tableAddress, credentials)
        {
        }
        
        public override async Task<TableResult> ExecuteAsync(TableOperation operation)
        {
            return await Task.FromResult(new TableResult
            {
                HttpStatusCode = 200,
                Result = TestFactory.GetTareaEntity()
            });
        }
    }
}
