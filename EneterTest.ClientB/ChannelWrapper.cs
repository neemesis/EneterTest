using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eneter.Messaging.EndPoints.TypedMessages;
using Eneter.Messaging.Infrastructure.ConnectionProvider;
using Eneter.Messaging.MessagingSystems.MessagingSystemBase;
using Eneter.Messaging.MessagingSystems.SynchronousMessagingSystem;
using Eneter.Messaging.MessagingSystems.TcpMessagingSystem;
using Eneter.Messaging.Nodes.ChannelWrapper;
using EneterTest.Common;

namespace EneterTest.ClientB {
    public class ChannelWrapper {
        // Unwrapps messages from the input channel and forwards them
        // to corresponding output channels.
        private IDuplexChannelWrapper myDuplexChannelWrapper;

        private IDuplexTypedMessageSender<TestOutput, TestInput> plusSender;
        private IDuplexTypedMessageSender<TestOutput, TestInput> minusSender;
        private IDuplexTypedMessageSender<TestOutput, TestInput> dotSender;


        public ChannelWrapper() {
            IMessagingSystemFactory anInternalMessaging = new SynchronousMessagingSystemFactory();

            // The service receives messages via one channel (i.e. it listens on one address).
            // The incoming messages are unwrapped on the server side.
            // Therefore the client must use wrapper to send messages via one channel.
            IChannelWrapperFactory aChannelWrapperFactory = new ChannelWrapperFactory();
            myDuplexChannelWrapper = aChannelWrapperFactory.CreateDuplexChannelWrapper();


            // To connect message senders and the wrapper with duplex channels we can use the following helper class.
            IConnectionProviderFactory aConnectionProviderFactory = new ConnectionProviderFactory();
            IConnectionProvider aConnectionProvider = aConnectionProviderFactory.CreateConnectionProvider(anInternalMessaging);


            // Factory to create message senders.
            // Sent messages will be serialized in Xml.
            IDuplexTypedMessagesFactory aCommandsFactory = new DuplexTypedMessagesFactory();

            plusSender = aCommandsFactory.CreateDuplexTypedMessageSender<TestOutput, TestInput>();
            plusSender.ResponseReceived += (s, e) => {
                Console.WriteLine("CW.V+: " + e.ResponseMessage.Value);
            };
            // attach method handling the request
            aConnectionProvider.Connect(myDuplexChannelWrapper, plusSender, "plus"); // attach the input channel to get messages from unwrapper

            minusSender = aCommandsFactory.CreateDuplexTypedMessageSender<TestOutput, TestInput>();
            minusSender.ResponseReceived += (s, e) => {
                Console.WriteLine("CW.V-: " + e.ResponseMessage.Value);
            };
            // attach method handling the request
            aConnectionProvider.Connect(myDuplexChannelWrapper, minusSender, "minus"); // attach the input channel to get messages from unwrapper

            dotSender = aCommandsFactory.CreateDuplexTypedMessageSender<TestOutput, TestInput>();
            dotSender.ResponseReceived += (s, e) => {
                Console.WriteLine("CW.V.: " + e.ResponseMessage.Value);
            };
            // attach method handling the request
            aConnectionProvider.Connect(myDuplexChannelWrapper, dotSender, "dot"); // attach the input channel to get messages from unwrapper

            IMessagingSystemFactory aTcpMessagingSystem = new TcpMessagingSystemFactory();

            // Create output channel to send requests to the service.
            IDuplexOutputChannel anOutputChannel = aTcpMessagingSystem.CreateDuplexOutputChannel("tcp://127.0.0.1:8091/");

            // Attach the output channel to the wrapper - so that we are able to send messages
            // and receive response messages.
            // Note: The service has the coresponding unwrapper.
            myDuplexChannelWrapper.AttachDuplexOutputChannel(anOutputChannel);
        }

        public void Send(string sign, string name, string value) {
            switch (sign) {
                case "+":
                    plusSender.SendRequestMessage(new TestInput { Name = name, Value = value });
                    break;
                case "-":
                    minusSender.SendRequestMessage(new TestInput { Name = name, Value = value });
                    break;
                case ".":
                    dotSender.SendRequestMessage(new TestInput { Name = name, Value = value });
                    break;
            }
        }
    }
}
