using System.Net;
using Xunit;
using Xunit.Abstractions;

namespace Maersk.StarterKit.IntegrationTests
{
    /// <summary>
    /// class AssertExtension
    /// </summary>
    public class AssertExtension
    {
        /// <summary>
        /// Asserts the status code.
        /// </summary>
        /// <param name="expected">The expected.</param>
        /// <param name="actual">The actual.</param>
        /// <param name="body">The body.</param>
        /// <param name="testOutputHelper">The test output helper.</param>
        public static void AssertStatusCode(HttpStatusCode expected, HttpStatusCode actual, string body, ITestOutputHelper testOutputHelper)
        {
            try
            {
                Assert.Equal(expected, actual);
            }
            catch
            {
                testOutputHelper.WriteLine(body);
                throw;
            }
        }

        /// <summary>
        /// Asserts the response.
        /// </summary>
        /// <param name="expected">The expected.</param>
        /// <param name="actual">The actual.</param>
        /// <param name="testOutputHelper">The test output helper.</param>
        public static void AssertResponse(object expected, object actual, ITestOutputHelper testOutputHelper)
        {
            try
            {
                Assert.Equal(expected, actual);
            }
            catch
            {
                testOutputHelper.WriteLine(actual.ToString());
                throw;
            }
        }

    }
}
