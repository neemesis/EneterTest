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
    public class TalkFlow {
        public TalkFlow() {
            var talk = new Talk();

            IRpcFactory factory = new RpcFactory();
            IRpcService<ITalk> service = factory.CreateSingleInstanceService<ITalk>(talk);

            IMessagingSystemFactory messaging = new TcpMessagingSystemFactory();
            IDuplexInputChannel inputChannel = messaging.CreateDuplexInputChannel("tcp://127.0.0.1:8045/");
            service.AttachDuplexInputChannel(inputChannel);

            Console.WriteLine("Service started. Press ENTER to stop.");
            Console.ReadLine();

            service.DetachDuplexInputChannel();
        }
    }
}
