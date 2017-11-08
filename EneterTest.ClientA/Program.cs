using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eneter.Messaging.EndPoints.Rpc;
using Eneter.Messaging.MessagingSystems.MessagingSystemBase;
using Eneter.Messaging.MessagingSystems.TcpMessagingSystem;
using EneterTest.Common;

namespace EneterTest.ClientA {
    class Program {
        static void Main(string[] args) {
            Console.Title = "Client A";

            // Simple RPC
            //new TalkFlow(); 

            // Object sending and receiving
            //var os = new ObjectSender();

            //var text = "";

            //while ((text = Console.ReadLine()) != "stop") {
            //    if (text.StartsWith("2")) {
            //        os.SendPersonal(text, int.Parse(Console.ReadLine()));
            //    } else 
            //        os.SendPersonalInfo(text, int.Parse(Console.ReadLine()));
            //}


            // Channel Wrapper
            var cw = new ChannelWrapper();

            var text = "";
            while ((text = Console.ReadLine()) != "stop") {
                cw.Send(text, Console.ReadLine(), Console.ReadLine());
            }
        }
    }
}
