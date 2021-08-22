using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using yuroca.common.Models;
using yuroca.funtions.Funtions;
using yuroca.test.Helpers;

namespace yuroca.test.Tests
{
    public class TareaApiTest
    {
        //Se crea este vlaor por el suso reiteartivo del logger
        private readonly ILogger logger = TestFactory.CreateLogger();

        [Fact]
        public async void CreateTarea_Should_Return_200()
        {
            //Arrenge inicio de las pruebas
            MockCloudTableTarea mockTarea = new MockCloudTableTarea(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            Tarea tareaRequest = TestFactory.GetTareaRequest();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(tareaRequest);

            //Act procedimiento
            IActionResult response = await TareaApi.createTarea(request, mockTarea, logger);

            //Asert si la prueba si hace lo que necesita
            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async void UpdateTarea_Should_Return_200()
        {
            //Arrenge inicio de las pruebas
            MockCloudTableTarea mockTarea = new MockCloudTableTarea(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            Tarea tareaRequest = TestFactory.GetTareaRequest();
            Guid tareaId = Guid.NewGuid();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(tareaId, tareaRequest);

            //Act procedimiento
            IActionResult response = await TareaApi.UpdateTarea(request, mockTarea, tareaId.ToString(),logger);

            //Asert si la prueba si hace lo que necesita
            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        //Terminar el get all get one y delete
    }
}
