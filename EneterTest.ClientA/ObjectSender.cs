using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eneter.Messaging.EndPoints.TypedMessages;
using Eneter.Messaging.MessagingSystems.MessagingSystemBase;
using Eneter.Messaging.MessagingSystems.TcpMessagingSystem;
using EneterTest.Common;

namespace EneterTest.ClientA {
    internal class ObjectSender {
        public ObjectSender() {
            // Create message sender sending request messages of type Person and receiving responses of type string.
            IDuplexTypedMessagesFactory aTypedMessagesFactory = new DuplexTypedMessagesFactory();
            myMessageSender = aTypedMessagesFactory.CreateDuplexTypedMessageSender<string, Person>();
            myMessageSender.ResponseReceived += OnResponseReceived;

            // Create messaging based on TCP.
            IMessagingSystemFactory aMessagingSystemFactory = new TcpMessagingSystemFactory();
            IDuplexOutputChannel anOutputChannel = aMessagingSystemFactory.CreateDuplexOutputChannel("tcp://127.0.0.1:8094/");

            // Attach output channel and be able to send messages and receive response messages.
            myMessageSender.AttachDuplexOutputChannel(anOutputChannel);
        }

        private void OnResponseReceived2(object sender, TypedResponseReceivedEventArgs<Person> e) {
            Console.WriteLine("2: " + e.ResponseMessage.Name + " | " + e.ResponseMessage.NumberOfItems);
        }

        public void Close() {
            myMessageSender.DetachDuplexOutputChannel();
        }

        public void SendPersonalInfo(string name, int numberOfItems) {
            Person aPerson = new Person { Name = name, NumberOfItems = numberOfItems };

            // Send the type Person.
            myMessageSender.SendRequestMessage(aPerson);
        }

        private void OnResponseReceived(object sender, TypedResponseReceivedEventArgs<string> e) {
            Console.WriteLine(e.ResponseMessage);
        }

        private IDuplexTypedMessageSender<string, Person> myMessageSender;
    }
}
