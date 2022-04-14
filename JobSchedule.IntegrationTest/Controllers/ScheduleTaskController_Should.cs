
using JobSchedule.Shared.Common;
using JobSchedule.Shared.Model;
using Maersk.StarterKit.IntegrationTests;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;


namespace JobSchedule.IntegrationTests.Controllers
{

    /// <summary>
    /// class ScheduleJobController_Should 
    /// </summary>
    /// <seealso cref="Xunit.IClassFixture&lt;Maersk.StarterKit.IntegrationTests.SetupServer&gt;" />
    public class ScheduleTaskController_Should : IClassFixture<SetupServer>
    {
        /// <summary>
        /// The test output helper
        /// </summary>
        private readonly ITestOutputHelper _testOutputHelper;
        /// <summary>
        /// The server
        /// </summary>
        private readonly SetupServer _server;

        /// <summary>
        /// All job get relative URI
        /// </summary>
        private const string _allTaskGetRelativeUri = "api/ScheduledTask";
        /// <summary>
        /// The specific job by identifier get relative URI
        /// </summary>
        private const string _specificTaskByIdGetRelativeUri = "api/ScheduledTask/{0}";
        /// <summary>
        /// The add schedule job post relative URI
        /// </summary>
        private const string _processFileTaskRelativeUri = "api/ScheduledTask/ProcessFile";
        /// <summary>
        /// The specific task by identifier delete relativet URI
        /// </summary>
        private const string _specificTaskByIdDeleteRelativetUri = "api/ScheduledTask/{0}";
        /// <summary>
        /// The non existing tes get relativet URI
        /// </summary>
        private const string _nonExistingTesGetRelativetUri = "api/nonexisting";

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleTaskController_Should"/> class.
        /// </summary>
        /// <param name="testOutputHelper">The test output helper.</param>
        /// <param name="setupServer">The setup server.</param>
        public ScheduleTaskController_Should(ITestOutputHelper testOutputHelper,
            SetupServer setupServer)
        {
            _testOutputHelper = testOutputHelper;
            _server = setupServer;
        }

        /// <summary>
        /// Schedules the job controller return 200 if ok.
        /// </summary>
        [Order(1)]
        [Fact]
        public async Task ScheduleTaskController_Return_200_If_OK()
        {
            var result = await _server.Client.GetAsync(_allTaskGetRelativeUri);
            var body = await result.Content.ReadAsStringAsync();
            AssertExtension.AssertStatusCode(HttpStatusCode.OK, result.StatusCode, body, _testOutputHelper);
        }

        /// <summary>
        /// Schedules the job controller return 404 if not found.
        /// </summary>
        [Order(2)]
        [Fact]
        public async Task ScheduleTaskController_Return_404_If_NotFound()
        {
            var result = await _server.Client.GetAsync(_nonExistingTesGetRelativetUri);
            var body = await result.Content.ReadAsStringAsync();
            AssertExtension.AssertStatusCode(HttpStatusCode.NotFound, result.StatusCode, body, _testOutputHelper);
        }

        /// <summary>
        /// Schedules the job controller add job return successful.
        /// </summary>
        [Order(3)]
        [Fact]
        public async Task ScheduleTaskController_AddTask_Return_Successful()
        {
            var file = CreateTestFormFile("PhoneNumeber.txt", "+4912345678901\n004912345678901");
            byte[] data1;
            using (var br = new BinaryReader(file.OpenReadStream()))
                data1 = br.ReadBytes((int)file.OpenReadStream().Length);
            ByteArrayContent bytes1 = new ByteArrayContent(data1);
            MultipartFormDataContent multiContent = new MultipartFormDataContent();
            multiContent.Add(bytes1, "file", file.FileName);
            var response = _server.Client.PostAsync(_processFileTaskRelativeUri, multiContent).Result;
            var actualResponse = await response.Content.ReadAsStringAsync();
            actualResponse = actualResponse.Trim().Trim('"');
            Guid responseGuidId = Guid.Parse(actualResponse);
            Assert.True(Guid.TryParse(actualResponse, out responseGuidId));
        }
        private IFormFile CreateTestFormFile(string fileName, string content)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(content);

            return new FormFile(
                baseStream: new MemoryStream(bytes),
                baseStreamOffset: 0,
                length: bytes.Length,
                name: "Data",
                fileName: fileName
            );
        }
    }
}