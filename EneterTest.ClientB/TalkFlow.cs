using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eneter.Messaging.EndPoints.Rpc;
using Eneter.Messaging.MessagingSystems.MessagingSystemBase;
using Eneter.Messaging.MessagingSystems.TcpMessagingSystem;
using EneterTest.Common;

namespace EneterTest.ClientB {
    public class TalkFlow {
        public TalkFlow() {
            IRpcFactory factory = new RpcFactory();
            var client = factory.CreateClient<ITalk>();

            TcpMessagingSystemFactory messaging = new TcpMessagingSystemFactory();
            IDuplexOutputChannel anOutputChannel = messaging.CreateDuplexOutputChannel("tcp://127.0.0.1:8045/");
            client.AttachDuplexOutputChannel(anOutputChannel);

            string sentence = "";

            while ((sentence = Console.ReadLine()) != "stop") {
                client.Proxy.Talk("Client B", sentence);
            }
        }
    }
}
