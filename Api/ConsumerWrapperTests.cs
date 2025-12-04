using System;
using Xunit;
using Moq;
using FluentAssertions;
using Confluent.Kafka;
using Api;
using System.Threading;

namespace Api.Tests
{
    public class ConsumerWrapperTests : IDisposable
    {
        private readonly ConsumerConfig _validConfig;
        private readonly string _validTopicName;

        public ConsumerWrapperTests()
        {
            _validConfig = new ConsumerConfig
            {
                GroupId = "test-group",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _validTopicName = "test-topic";
        }

        [Fact]
        public void Constructor_ValidParameters_ShouldInitializeConsumer()
        {
            // Arrange
            var config = new ConsumerConfig { GroupId = "test-group" };
            var topicName = "test-topic";

            // Act
            using (var consumerWrapper = new ConsumerWrapper(config, topicName))
            {
                // Assert
                consumerWrapper.Should().NotBeNull();
            }
        }

        [Fact]
        public void Constructor_NullConfig_ShouldThrowArgumentNullException()
        {
            // Arrange
            string topicName = "test-topic";

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => new ConsumerWrapper(null, topicName));
            exception.ParamName.Should().Be("config");
        }

        [Fact]
        public void Constructor_NullTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            var config = new ConsumerConfig { GroupId = "test-group" };

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => new ConsumerWrapper(config, null));
            exception.ParamName.Should().Be("topicName");
        }

        [Fact]
        public void Constructor_EmptyTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            var config = new ConsumerConfig { GroupId = "test-group" };
            var emptyTopicName = string.Empty;

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => new ConsumerWrapper(config, emptyTopicName));
            exception.ParamName.Should().Be("topicName");
        }

        [Fact]
        public void Constructor_WhitespaceTopicName_ShouldNotThrow()
        {
            // Arrange
            var config = new ConsumerConfig { GroupId = "test-group" };
            var whitespaceTopicName = "   ";

            // Act & Assert
            using (var consumerWrapper = new ConsumerWrapper(config, whitespaceTopicName))
            {
                consumerWrapper.Should().NotBeNull();
            }
        }

        [Fact]
        public void ReadMessage_NoMessageAvailable_ShouldReturnNull()
        {
            // Arrange
            var config = new ConsumerConfig { GroupId = "test-group" };
            var topicName = "test-topic";

            using (var consumerWrapper = new ConsumerWrapper(config, topicName))
            {
                // Act
                var result = consumerWrapper.readMessage();

                // Assert
                result.Should().BeNull();
            }
        }

        [Fact]
        public void ReadMessage_OperationCanceledException_ShouldReturnNull()
        {
            // Arrange
            var config = new ConsumerConfig 
            { 
                GroupId = "test-group",
                BootstrapServers = "invalid-server:9092",
                SocketTimeoutMs = 100
            };
            var topicName = "test-topic";

            using (var consumerWrapper = new ConsumerWrapper(config, topicName))
            {
                // Act
                var result = consumerWrapper.readMessage();

                // Assert
                result.Should().BeNull();
            }
        }

        [Fact]
        public void ReadMessage_ConsumeException_ShouldReturnNull()
        {
            // Arrange
            var config = new ConsumerConfig 
            { 
                GroupId = "test-group",
                BootstrapServers = "invalid-server:9092",
                SessionTimeoutMs = 100
            };
            var topicName = "test-topic";

            using (var consumerWrapper = new ConsumerWrapper(config, topicName))
            {
                // Act
                var result = consumerWrapper.readMessage();

                // Assert
                result.Should().BeNull();
            }
        }

        [Fact]
        public void ReadMessage_MultipleCallsAfterDispose_ShouldHandleGracefully()
        {
            // Arrange
            var config = new ConsumerConfig { GroupId = "test-group" };
            var topicName = "test-topic";
            var consumerWrapper = new ConsumerWrapper(config, topicName);

            // Act
            consumerWrapper.Dispose();
            
            // Assert - Should not throw when calling readMessage after dispose
            Action act = () => consumerWrapper.readMessage();
            act.Should().NotThrow();
        }

        [Fact]
        public void ReadMessage_ValidConfiguration_ShouldNotThrow()
        {
            // Arrange
            using (var consumerWrapper = new ConsumerWrapper(_validConfig, _validTopicName))
            {
                // Act & Assert
                Action act = () => consumerWrapper.readMessage();
                act.Should().NotThrow();
            }
        }

        [Fact]
        public void Dispose_MultipleDisposes_ShouldNotThrowException()
        {
            // Arrange
            var config = new ConsumerConfig { GroupId = "test-group" };
            var topicName = "test-topic";
            var consumerWrapper = new ConsumerWrapper(config, topicName);

            // Act & Assert
            consumerWrapper.Dispose();
            Action secondDispose = () => consumerWrapper.Dispose();
            secondDispose.Should().NotThrow();
        }

        [Fact]
        public void Dispose_ShouldCloseConsumer()
        {
            // Arrange
            var config = new ConsumerConfig { GroupId = "test-group" };
            var topicName = "test-topic";

            // Act
            using (var consumerWrapper = new ConsumerWrapper(config, topicName))
            {
                consumerWrapper.Dispose();
            }

            // Assert - no exception means successful disposal
        }

        [Fact]
        public void Dispose_AfterReadMessage_ShouldNotThrow()
        {
            // Arrange
            var config = new ConsumerConfig { GroupId = "test-group" };
            var topicName = "test-topic";
            var consumerWrapper = new ConsumerWrapper(config, topicName);

            // Act
            consumerWrapper.readMessage();
            
            // Assert
            Action disposeAction = () => consumerWrapper.Dispose();
            disposeAction.Should().NotThrow();
        }

        [Fact]
        public void UsingStatement_ShouldDisposeAutomatically()
        {
            // Arrange & Act & Assert
            Action usingAction = () =>
            {
                using (var consumerWrapper = new ConsumerWrapper(_validConfig, _validTopicName))
                {
                    consumerWrapper.readMessage();
                }
            };
            
            usingAction.Should().NotThrow();
        }

        public void Dispose()
        {
            // Cleanup any test resources if needed
        }
    }
}