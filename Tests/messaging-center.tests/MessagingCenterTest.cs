using FluentAssertions;
using messaging_center.Impl;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace messaging_center.tests
{
    public class MessagingCenterTest
    {
        private readonly MessagingCenter _messagingCenter;
        public MessagingCenterTest()
        {
            _messagingCenter = new MessagingCenter(new Mock<ILogger>().Object);
        }

        [Fact]
        public void GivenSubscriber_WhenSend_ThenReceivedEvent()
        {
            bool eventReceived = false;

            _messagingCenter.Subscribe<MessagingCenterTest>(this, "message", sender => eventReceived = true);
            _messagingCenter.Send(this, "message");

            eventReceived.Should().Be(true);
        }

        [Fact]
        public void GivenSubscriber_WhenUnsubscribe_ThenNoReceivedEvent()
        {
            bool eventReceived = false;

            _messagingCenter.Subscribe<MessagingCenterTest>(this, "message", sender => eventReceived = true);
            _messagingCenter.Unsubscribe<MessagingCenterTest>(this, "message");
            _messagingCenter.Send(this, "message");

            eventReceived.Should().Be(false);
        }

        [Fact]
        public void GivenSubscriberWithArgs_WhenUnsubscribe_ThenNoReceivedEvent()
        {
            bool eventReceived = false;

            _messagingCenter.Subscribe<MessagingCenterTest, string>(this, "message", (sender, args) => eventReceived = true);
            _messagingCenter.Unsubscribe<MessagingCenterTest, string>(this, "message");
            _messagingCenter.Send(this, "message");

            eventReceived.Should().Be(false);
        }

        [Fact]
        public void GivenNullSubscriber_WhenSubscribe_ThenThrowException()
        {
            var act = () => _messagingCenter.Subscribe<MessagingCenterTest>(null, "message", sender => { });
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void GivenNullSender_WhenSend_ThenThrowException()
        {
            var act = () => _messagingCenter.Send<MessagingCenterTest>(null, "message");
            act.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData("message", "message", true)]
        [InlineData("subscriber", "sender", false)]
        [InlineData("", "", true)]
        public void GivenMessage_WhenSend_ThenEventReceivedIsExpectedResult(string subscriberMessage, string senderMessage, bool expectedResult)
        {
            bool eventReceived = false;

            _messagingCenter.Subscribe<MessagingCenterTest>(this, subscriberMessage, sender => eventReceived = true);
            _messagingCenter.Send(this, senderMessage);

            eventReceived.Should().Be(expectedResult);
        }

        [Fact]
        public void GivenAsyncCallback_WhenSend_ThenEventReceivedAndProcessed()
        {
            bool eventReceived = false;

            _messagingCenter.Subscribe<MessagingCenterTest>(this, "message", async sender => 
            {
                await Task.Delay(1000);
                eventReceived = true;
            });

            _messagingCenter.Send(this, "message");
            eventReceived.Should().Be(true);
        }
    }
}