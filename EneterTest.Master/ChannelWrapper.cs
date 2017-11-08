using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eneter.Messaging.EndPoints.TypedMessages;
using Eneter.Messaging.Infrastructure.ConnectionProvider;
using Eneter.Messaging.MessagingSystems.MessagingSystemBase;
using Eneter.Messaging.MessagingSystems.TcpMessagingSystem;
using Eneter.Messaging.MessagingSystems.ThreadPoolMessagingSystem;
using Eneter.Messaging.Nodes.ChannelWrapper;
using EneterTest.Common;

namespace EneterTest.Master {
    public class ChannelWrapper {
        // Unwrapps messages from the input channel and forwards them
        // to corresponding output channels.
        private IDuplexChannelUnwrapper myDuplexChannelUnwrapper;

        private IDuplexTypedMessageReceiver<TestOutput, TestInput> plusReceiver;
        private IDuplexTypedMessageReceiver<TestOutput, TestInput> minusReceiver;
        private IDuplexTypedMessageReceiver<TestOutput, TestInput> dotReceiver;

        public ChannelWrapper() {
            // Internal messaging used for messaging between channel unwrapper
            // and typed message receivers.
            // We want that requests do not block each other. So every request will be processed in its own thread.
            IMessagingSystemFactory anInternalMessaging = new ThreadPoolMessagingSystemFactory();

            // All messages are received via one channel. So we must provide "unwrapper" forwarding incoming messages
            // to correct receivers.
            IChannelWrapperFactory aChannelWrapperFactory = new ChannelWrapperFactory();
            myDuplexChannelUnwrapper = aChannelWrapperFactory.CreateDuplexChannelUnwrapper(anInternalMessaging);

            // To connect receivers and the unwrapper with duplex channels we can use the following helper class.
            IConnectionProviderFactory aConnectionProviderFactory = new ConnectionProviderFactory();
            IConnectionProvider aConnectionProvider = aConnectionProviderFactory.CreateConnectionProvider(anInternalMessaging);

            // Factory to create message receivers.
            IDuplexTypedMessagesFactory aMessageReceiverFactory = new DuplexTypedMessagesFactory();

            // Create receiver to sum two numbers.
            plusReceiver = aMessageReceiverFactory.CreateDuplexTypedMessageReceiver<TestOutput, TestInput>();
            plusReceiver.MessageReceived += (s, e) => {
                Console.WriteLine("PLUS: " + e.RequestMessage.Name + " | " + e.RequestMessage.Value);
                plusReceiver.SendResponseMessage(e.ResponseReceiverId, new TestOutput { Value = e.RequestMessage.Name + "+" + e.RequestMessage.Value });
            }; 
            // attach method handling the request
            aConnectionProvider.Attach(plusReceiver, "plus"); // attach the input channel to get messages from unwrapper


            // Create receiver to sum two numbers.
            minusReceiver = aMessageReceiverFactory.CreateDuplexTypedMessageReceiver<TestOutput, TestInput>();
            minusReceiver.MessageReceived += (s, e) => {
                Console.WriteLine("MINUS: " + e.RequestMessage.Name + " | " + e.RequestMessage.Value);
                minusReceiver.SendResponseMessage(e.ResponseReceiverId, new TestOutput { Value = e.RequestMessage.Name + "-" + e.RequestMessage.Value });
            };
            // attach method handling the request
            aConnectionProvider.Attach(minusReceiver, "minus"); // attach the input channel to get messages from unwrapper

            // Create receiver to sum two numbers.
            dotReceiver = aMessageReceiverFactory.CreateDuplexTypedMessageReceiver<TestOutput, TestInput>();
            dotReceiver.MessageReceived += (s, e) => {
                Console.WriteLine("DOT: " + e.RequestMessage.Name + " | " + e.RequestMessage.Value);
                dotReceiver.SendResponseMessage(e.ResponseReceiverId, new TestOutput { Value = e.RequestMessage.Name + "." + e.RequestMessage.Value });
            };
            // attach method handling the request
            aConnectionProvider.Attach(dotReceiver, "dot"); // attach the input channel to get messages from unwrapper
        }

        public void Start() {
            // We use TCP based messaging.
            IMessagingSystemFactory aServiceMessagingSystem = new TcpMessagingSystemFactory();
            IDuplexInputChannel anInputChannel = aServiceMessagingSystem.CreateDuplexInputChannel("tcp://127.0.0.1:8091/");

            // Attach the input channel to the unwrapper and start to listening.
            myDuplexChannelUnwrapper.AttachDuplexInputChannel(anInputChannel);
        }

        public void Stop() {
            // Detach the input channel from the unwrapper and stop listening.
            // Note: It releases listening threads.
            myDuplexChannelUnwrapper.DetachDuplexInputChannel();
        }
    }
}
