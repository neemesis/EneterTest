using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eneter.Messaging.EndPoints.TypedMessages;
using Eneter.Messaging.MessagingSystems.MessagingSystemBase;
using Eneter.Messaging.MessagingSystems.TcpMessagingSystem;
using EneterTest.Common;

namespace EneterTest.Master {

    internal class ObjectReceiver {
        public ObjectReceiver() {
            // Create duplex typed message receiver.
            // It receives request messages of type Person and responses string.
            IDuplexTypedMessagesFactory aDuplexTypedMessagesFactory = new DuplexTypedMessagesFactory();
            myMessageReceiver = aDuplexTypedMessagesFactory.CreateDuplexTypedMessageReceiver<string, Person>();
            myMessageReceiver.MessageReceived += OnMessageReceived;
        }

        public void StartReceiving() {
            // Create messaging based on TCP.
            IMessagingSystemFactory aMessagingSystemFactory = new TcpMessagingSystemFactory();
            IDuplexInputChannel anDuplexInputChannel = aMessagingSystemFactory.CreateDuplexInputChannel("tcp://127.0.0.1:8094/");

            // Attach the input channel and be able to receive messages and send back responses.
            myMessageReceiver.AttachDuplexInputChannel(anDuplexInputChannel);
        }

        public void StopReceiving() {
            myMessageReceiver.DetachDuplexInputChannel();
        }

        private void OnMessageReceived(object sender, TypedRequestReceivedEventArgs<Person> e) {
            Console.WriteLine("1.Name: " + e.RequestMessage.Name);
            Console.WriteLine("1.Number of items: " + e.RequestMessage.NumberOfItems.ToString());
            Console.WriteLine();

            if (e.RequestMessage.Name != "bye") {
                myMessageReceiver.SendResponseMessage(e.ResponseReceiverId, "Thanks for your message.");
            }
        }

        public void SendMessage(string msg) {
            myMessageReceiver.SendResponseMessage("*", msg);
        }

        // Duplex typed message receicer that receives type Person and responses type string.
        private IDuplexTypedMessageReceiver<string, Person> myMessageReceiver;
    }
}
