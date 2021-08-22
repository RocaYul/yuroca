using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using yuroca.funtions.Funtions;
using yuroca.test.Helpers;

namespace yuroca.test.Tests
{
     public class ScheduledFunctionTest
    {
        [Fact]
        public void ScheduledFunction_Should_Log_Message()
        {
            //Arrage
            MockCloudTableTarea mockTarea = new MockCloudTableTarea(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            ListLogger logger = (ListLogger)TestFactory.CreateLogger(LoggerTypes.List);

            //Act
            ScheduledFunction.Run(null, mockTarea, logger);
            string message = logger.Logs[0];

            //Asert
            Assert.Contains("Deleted completed", message);
        }
    }
}
