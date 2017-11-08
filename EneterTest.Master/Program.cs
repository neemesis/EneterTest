using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eneter.Messaging.EndPoints.Rpc;
using Eneter.Messaging.MessagingSystems.MessagingSystemBase;
using Eneter.Messaging.MessagingSystems.TcpMessagingSystem;
using EneterTest.Common;

namespace EneterTest.Master {
    class Program {
        static void Main(string[] args) {
            Console.Title = "Master";

            // Simple RPC
            //new TalkFlow(); 

            // Object sending and receiving
            //var or = new ObjectReceiver();
            //or.StartReceiving();

            //var text = "";

            //while ((text = Console.ReadLine()) != "stop") {
            //    or.SendMessage(text);
            //}

            // Channel Wrapper
            var cw = new ChannelWrapper();
            cw.Start();
        }
    }
}
