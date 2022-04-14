using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using JobSchedule.API;
using System;
using System.Net.Http;

namespace Maersk.StarterKit.IntegrationTests
{
    /// <summary>
    /// class SetupServer
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class SetupServer : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetupServer"/> class.
        /// </summary>
        public SetupServer()
        {

            HostServer = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseTestServer();
                    webBuilder.UseStartup<Startup>();
                }).Build();
            HostServer.Start();
            Client = HostServer.GetTestClient();
        }

        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <value>
        /// The client.
        /// </value>
        public HttpClient Client { get; }

        /// <summary>
        /// Gets the host server.
        /// </summary>
        /// <value>
        /// The host server.
        /// </value>
        public IHost HostServer { get; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Client?.Dispose();
            HostServer.StopAsync().Wait();
        }
    }
}
