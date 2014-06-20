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
    enum PacketTypes
    {
        LOGIN,
        MAP,
        WORLDSTATE
    }
    class Server
    {
        NetPeerConfiguration config;
        NetServer server;
        NetIncomingMessage inc;
        NetConnection senderConnection;

        public Server()
        {
            config = new NetPeerConfiguration("squickilling");
            config.Port = 14248;
            config.MaximumConnections = 200;
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            server = new NetServer(config);
            server.Start();
            Console.WriteLine("Serveur créé");
        }

        public void Update(ref MapMulti map)
        {
            while ((inc = server.ReadMessage()) != null /*&& inc.MessageType != NetIncomingMessageType.DebugMessage*/)
            {
                switch (inc.MessageType)
                {
                    case NetIncomingMessageType.ConnectionApproval:
                        if (inc.ReadByte() == (byte)PacketTypes.LOGIN)
                        {
                            Console.WriteLine("On se connecte");
                            senderConnection = inc.SenderConnection;
                            inc.SenderConnection.Approve();
                            var outmsg = server.CreateMessage();
                            outmsg.Write((byte)PacketTypes.MAP);
                            //foreach (Perso p in map.persos)
                            //    outmsg.WriteAllProperties(p);
                            server.SendMessage(outmsg, senderConnection, NetDeliveryMethod.ReliableUnordered);
                            //map.persos.Add(new Perso(new Vector2(0, 200), CharacType.player));
                            Console.WriteLine("Connecté maggle");
                        }
                        break;
                    case NetIncomingMessageType.Data:
                        var type = inc.ReadByte();
                        if (type == (byte)PacketTypes.MAP)
                        {
                            var outmsg = server.CreateMessage();
                            foreach (Perso p in map.persos)
                                outmsg.WriteAllProperties(p);
                        }
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        Console.WriteLine("osefps");
                        break;
                }

            }
        }
    }
}