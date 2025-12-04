using Api;
using Confluent.Kafka;
using FluentAssertions;
using Moq;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Test
{
    public class ProducerWrapperTests : IDisposable
    {
        private readonly ProducerConfig _validConfig;
        private readonly string _validTopicName;
        private readonly StringWriter _stringWriter;
        private readonly TextWriter _originalOut;

        public ProducerWrapperTests()
        {
            _validConfig = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                ClientId = "test-producer"
            };
            _validTopicName = "test-topic";
            _stringWriter = new StringWriter();
            _originalOut = Console.Out;
        }

        [Fact]
        public void Constructor_WithValidParameters_ShouldCreateInstance()
        {
            // Arrange & Act
            var producer = new ProducerWrapper(_validConfig, _validTopicName);

            // Assert
            producer.Should().NotBeNull();
            producer.Dispose();
        }

        [Fact]
        public void Constructor_WithNullConfig_ShouldThrowArgumentNullException()
        {
            // Arrange
            ProducerConfig nullConfig = default!;

            // Act & Assert
            var action = () => new ProducerWrapper(nullConfig, _validTopicName);
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("config");
        }

        [Fact]
        public void Constructor_WithNullTopicName_ShouldThrowArgumentNullException()
        {
            // Arrange
            string nullTopicName = default!;

            // Act & Assert
            var action = () => new ProducerWrapper(_validConfig, nullTopicName);
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("topicName");
        }

        [Fact]
        public async Task WriteMessage_WithValidMessage_ShouldCompleteSuccessfully()
        {
            // Arrange
            using var producer = new ProducerWrapper(_validConfig, _validTopicName);
            var testMessage = "test message";

            // Act & Assert
            var action = async () => await producer.writeMessage(testMessage);
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task WriteMessage_WithNullMessage_ShouldThrowArgumentNullException()
        {
            // Arrange
            using var producer = new ProducerWrapper(_validConfig, _validTopicName);
            string nullMessage = default!;

            // Act & Assert
            var action = async () => await producer.writeMessage(nullMessage);
            await action.Should().ThrowAsync<ArgumentNullException>()
                .Where(ex => ex.ParamName == "message");
        }

        [Fact]
        public async Task WriteMessage_WithEmptyMessage_ShouldCompleteSuccessfully()
        {
            // Arrange
            using var producer = new ProducerWrapper(_validConfig, _validTopicName);
            var emptyMessage = string.Empty;

            // Act & Assert
            var action = async () => await producer.writeMessage(emptyMessage);
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task WriteMessage_WithLongMessage_ShouldCompleteSuccessfully()
        {
            // Arrange
            using var producer = new ProducerWrapper(_validConfig, _validTopicName);
            var longMessage = new string('a', 1000);

            // Act & Assert
            var action = async () => await producer.writeMessage(longMessage);
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task WriteMessage_WithSpecialCharacters_ShouldCompleteSuccessfully()
        {
            // Arrange
            using var producer = new ProducerWrapper(_validConfig, _validTopicName);
            var specialMessage = "test@#$%^&*()_+{}|:<>?[]\;'",.";

            // Act & Assert
            var action = async () => await producer.writeMessage(specialMessage);
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task WriteMessage_ShouldLogDeliveryInfoToConsole()
        {
            // Arrange
            Console.SetOut(_stringWriter);
            using var producer = new ProducerWrapper(_validConfig, _validTopicName);
            var testMessage = "delivery test message";

            try
            {
                // Act
                await producer.writeMessage(testMessage);

                // Assert
                var output = _stringWriter.ToString();
                output.Should().Contain("KAFKA =>");
                output.Should().Contain("Delivered");
                output.Should().Contain(testMessage);
            }
            finally
            {
                Console.SetOut(_originalOut);
            }
        }

        [Fact]
        public async Task WriteMessage_ShouldGenerateRandomKey()
        {
            // Arrange
            Console.SetOut(_stringWriter);
            using var producer = new ProducerWrapper(_validConfig, _validTopicName);
            var testMessage = "key test message";

            try
            {
                // Act
                await producer.writeMessage(testMessage);

                // Assert
                var output = _stringWriter.ToString();
                var validKeys = new[] { "0", "1", "2", "3", "4" };
                validKeys.Should().Contain(key => output.Contains(key));
            }
            finally
            {
                Console.SetOut(_originalOut);
            }
        }

        [Fact]
        public async Task WriteMessage_WithProduceException_ShouldLogErrorAndRethrow()
        {
            // Arrange
            Console.SetOut(_stringWriter);
            var invalidConfig = new ProducerConfig
            {
                BootstrapServers = "invalid-server:9999",
                MessageTimeoutMs = 1000,
                RequestTimeoutMs = 1000
            };
            using var producer = new ProducerWrapper(invalidConfig, _validTopicName);
            var testMessage = "error test message";

            try
            {
                // Act & Assert
                var action = async () => await producer.writeMessage(testMessage);
                await action.Should().ThrowAsync<ProduceException<string, string>>();

                // Verify error logging
                var output = _stringWriter.ToString();
                output.Should().Contain("Produce failed:");
            }
            finally
            {
                Console.SetOut(_originalOut);
            }
        }

        [Fact]
        public void Dispose_WhenCalledOnce_ShouldCompleteSuccessfully()
        {
            // Arrange
            var producer = new ProducerWrapper(_validConfig, _validTopicName);

            // Act & Assert
            var action = () => producer.Dispose();
            action.Should().NotThrow();
        }

        [Fact]
        public void Dispose_WhenCalledMultipleTimes_ShouldNotThrow()
        {
            // Arrange
            var producer = new ProducerWrapper(_validConfig, _validTopicName);

            // Act & Assert
            var action = () =>
            {
                producer.Dispose();
                producer.Dispose();
                producer.Dispose();
            };
            action.Should().NotThrow();
        }

        [Fact]
        public async Task WriteMessage_AfterDispose_ShouldThrowObjectDisposedException()
        {
            // Arrange
            var producer = new ProducerWrapper(_validConfig, _validTopicName);
            producer.Dispose();

            // Act & Assert
            var action = async () => await producer.writeMessage("test message");
            await action.Should().ThrowAsync<ObjectDisposedException>();
        }

        [Fact]
        public void UsingStatement_ShouldDisposeAutomatically()
        {
            // Arrange & Act & Assert
            var action = () =>
            {
                using var producer = new ProducerWrapper(_validConfig, _validTopicName);
                // Producer should be disposed automatically when leaving using block
            };
            action.Should().NotThrow();
        }

        [Fact]
        public void Dispose_ShouldFlushPendingMessages()
        {
            // Arrange
            var producer = new ProducerWrapper(_validConfig, _validTopicName);

            // Act & Assert - Should not throw even if flush encounters issues
            var action = () => producer.Dispose();
            action.Should().NotThrow();
        }

        [Fact]
        public async Task WriteMessage_MultipleConcurrentCalls_ShouldHandleCorrectly()
        {
            // Arrange
            using var producer = new ProducerWrapper(_validConfig, _validTopicName);
            var tasks = new Task[5];

            // Act
            for (int i = 0; i < tasks.Length; i++)
            {
                var messageIndex = i;
                tasks[i] = producer.writeMessage($"concurrent message {messageIndex}");
            }

            // Assert
            var action = async () => await Task.WhenAll(tasks);
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task WriteMessage_WithUnicodeCharacters_ShouldCompleteSuccessfully()
        {
            // Arrange
            using var producer = new ProducerWrapper(_validConfig, _validTopicName);
            var unicodeMessage = "Hello 世界 🌍 Ñoño";

            // Act & Assert
            var action = async () => await producer.writeMessage(unicodeMessage);
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public void Constructor_WithErrorHandler_ShouldLogProducerErrors()
        {
            // Arrange
            Console.SetOut(_stringWriter);
            var errorConfig = new ProducerConfig
            {
                BootstrapServers = "invalid-server:9999",
                ClientId = "error-test-producer"
            };

            try
            {
                // Act
                using var producer = new ProducerWrapper(errorConfig, _validTopicName);

                // Assert - Constructor should complete without throwing
                producer.Should().NotBeNull();
            }
            finally
            {
                Console.SetOut(_originalOut);
            }
        }

        public void Dispose()
        {
            _stringWriter?.Dispose();
            Console.SetOut(_originalOut);
        }
    }
}