using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage.Queue;
using Moq;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TwitterStats.ServiceLibrary.Services;
using TwitterStats.ServiceLibrary.Services.QueueServices;

namespace TwitterStats.ServiceLibrary.Tests.Services.QueueServices
{
    [TestClass]
    public class QueueServiceTests
    {
        private MockRepository mockRepository;
        private Mock<ICloudStorageAccountHandler> _cloudStorageAccountHandler;

        private string _message;

        [TestInitialize]
        public void TestInitialize()
        {
            

            this.mockRepository = new MockRepository(MockBehavior.Loose);
            this._cloudStorageAccountHandler = this.mockRepository.Create<ICloudStorageAccountHandler>();
            this._cloudStorageAccountHandler.SetupAllProperties();
            this._cloudStorageAccountHandler.Object.Id = 123;
            
            _message = "some message result";
            CloudQueueMessage cloudQueueMessage = new CloudQueueMessage(_message);

            this._cloudStorageAccountHandler.Setup(o => o.AddMessageAsync(It.IsAny<CloudQueueMessage>())).Returns(Task.FromResult(true));
            this._cloudStorageAccountHandler.Setup(o => o.GetMessageAsync()).Returns(Task.FromResult(cloudQueueMessage));
            this._cloudStorageAccountHandler.Setup(o => o.DeleteMessageAsync(cloudQueueMessage)).Returns(Task.FromResult(true));
            
        }

        private QueueService CreateService()
        {
            return new QueueService(this._cloudStorageAccountHandler.Object);
        }

        private QueueService CreateService2()
        {
            return new QueueService(this._cloudStorageAccountHandler.Object);
        }

        [TestMethod]
        public async Task SendMessageOntoQueue_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService2();
             
            // Act
            var success = await service.SendMessageOntoQueue(_message);

            // Assert
            Assert.IsTrue(success);

            this.mockRepository.Verify();
        }

        [TestMethod]
        public async Task DequeueMessage_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();

            // Act
            var result = await service.DequeueMessage();

            // Assert
            Assert.AreEqual(result.AsString, _message);
            this.mockRepository.Verify();
        }
    }
}
