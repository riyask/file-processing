using Autofac.Extras.Moq;
using JobSchedule.Data;
using JobSchedule.External;
using JobSchedule.Shared.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
namespace JobSchedule.External.UnitTest
{
    /// <summary>
    /// class FileProcessingChannelTest
    /// </summary>
    public class FileProcessingChannelTest
    {

        /// <summary>
        /// FileProcessings the channel get item succcessful.
        /// </summary>
        [Fact]
        public void FileProcessingChannel_GetItem_Succcessful()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var file = CreateTestFormFile("PhoneNumeber.txt", "+4912345678901\n004912345678901");
                var exptectedJobItem = new JobItem(file);

                mock.Mock<IAppDataStorage>().Setup(x => x.GetItem(exptectedJobItem.Id)).Returns(exptectedJobItem);

                var fileProcessingChannel = mock.Create<FileProcessingChannel>();
                var actualJobItem = fileProcessingChannel.GetJobById(exptectedJobItem.Id);
                mock.Mock<IAppDataStorage>().Verify(x => x.GetItem(exptectedJobItem.Id), Times.Exactly(1));

                Assert.Equal(exptectedJobItem.Id, actualJobItem.Id);
            }
        }

        /// <summary>
        /// FileProcessings the channel get all items succesful.
        /// </summary>
        [Fact]
        public void FileProcessingChannel_GetAllItems_Succesful()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var file = CreateTestFormFile("PhoneNumeber.txt", "+4912345678901\n004912345678901");
                var testJobItem = new JobItem(file);
                List<JobItem> actualJobItems = new List<JobItem>();
                actualJobItems.Add(new JobItem(testJobItem));

                mock.Mock<IAppDataStorage>().Setup(x => x.GetAllItems()).Returns(actualJobItems);
                var fileProcessingChannel = mock.Create<FileProcessingChannel>();
                var expectedJobItems = fileProcessingChannel.GetAllJob();

                mock.Mock<IAppDataStorage>().Verify(x => x.GetAllItems(), Times.Exactly(1));
                Assert.Equal(actualJobItems.Count(), expectedJobItems.Count());

                Assert.Contains(expectedJobItems, a => a.Id == actualJobItems[0].Id);
            }
        }

        [Fact]
        public void FileProcessingChannel_AddItem_Succesful()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var file = CreateTestFormFile("PhoneNumeber.txt", "+4912345678901\n004912345678901");
                var testJobItem = new JobItem(file);
                List<JobItem> actualJobItems = new List<JobItem>();
                actualJobItems.Add(new JobItem(testJobItem));

                mock.Mock<IAppDataStorage>().Setup(x => x.Add(testJobItem));
                var fileProcessingChannel = mock.Create<FileProcessingChannel>();
                
                var expectedAddItemSync = fileProcessingChannel.AddItemAsync(testJobItem, System.Threading.CancellationToken.None);
                var expectedAddItemSyncResult = expectedAddItemSync.Result;
                mock.Mock<IAppDataStorage>().Verify(x => x.Add(testJobItem), Times.Exactly(0));
                Assert.Equal(actualJobItems.Count(), 1);
                Assert.Equal(expectedAddItemSyncResult, true);
            }
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