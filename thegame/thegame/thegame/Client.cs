using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Lidgren.Network;

namespace thegame
{

    class Client
    {
        NetPeerConfiguration config;
        NetClient client;
        NetIncomingMessage inc;
        NetOutgoingMessage outmsg;
        public Client()
        {
            config = new NetPeerConfiguration("squickilling");
            client = new NetClient(config);
            client.Start();
            outmsg = client.CreateMessage();
            outmsg.Write((byte)PacketTypes.LOGIN);
            string ip = "127.0.0.1";
            client.Connect(ip, 14248, outmsg);
            Console.WriteLine("Client créé");
        }

        public void Update(ref MapMulti map)
        {
            while ((inc = client.ReadMessage()) != null)
            {
                switch (inc.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        var type = inc.ReadByte();
                        if (type == (byte)PacketTypes.MAP)
                        {
                            var outmsg = client.CreateMessage();
                            foreach (Perso p in map.persos)
                                outmsg.WriteAllProperties(p);
                        }
                        Console.WriteLine("un excellent truc");
                        break;
                }
            }
        }
    }
}
