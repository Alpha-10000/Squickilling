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
using System.Threading;
using Lidgren.Network;

namespace thegame
{
    enum PacketTypes
    {
        LOGIN,
        NEWPERSO,
        POSITIONX,
        POSITIONY,
        SCORE,
        BONUS,
        HEALTH,
        IA
    }

    class Client
    {
        NetPeerConfiguration config;
        NetClient client;
        NetOutgoingMessage outmsg;
        Perso p;
        public Client()
        {
            config = new NetPeerConfiguration("squickilling");
            client = new NetClient(config);
            client.Start();
            outmsg = client.CreateMessage();
            outmsg.Write((byte)PacketTypes.LOGIN);
            //outmsg.Write("osef");
            string ip = "127.0.0.1";
            client.Connect(ip, 14242, outmsg);
            p = new Perso(new Vector2(0, 200), CharacType.player);
            p.utilisable = true;
            Console.WriteLine("Client créé");
        }

        public void Update(ref MapMulti map)
        {
            NetIncomingMessage inc;
            if ((inc = client.ReadMessage()) != null)
            {
                //Console.WriteLine(inc.ReadString());
                switch (inc.MessageType)
                {
                    case NetIncomingMessageType.Data:

                        NetConnection senderConnection;
                        //Thread.Sleep(1000);
                        if (inc.ReadByte() == (byte)PacketTypes.NEWPERSO)
                        {
                            senderConnection = inc.SenderConnection;
                            Console.WriteLine("newperso");
                            int index = inc.ReadInt32();
                            if (!map.persos.Contains(p))
                                map.persos[index] = p;
                            else
                                map.persos[index] = new Perso(new Vector2(0, 200), CharacType.player);

                            outmsg = client.CreateMessage();
                            outmsg.Write((byte)PacketTypes.POSITIONX);
                            outmsg.Write(map.persos[map.persos.IndexOf(p)].positionPerso.X);
                            client.SendMessage(outmsg, senderConnection, NetDeliveryMethod.ReliableOrdered);

                            outmsg = client.CreateMessage();
                            outmsg.Write((byte)PacketTypes.POSITIONY);
                            outmsg.Write(map.persos[map.persos.IndexOf(p)].positionPerso.Y);
                            client.SendMessage(outmsg, senderConnection, NetDeliveryMethod.ReliableOrdered);

                            outmsg = client.CreateMessage();
                            outmsg.Write((byte)PacketTypes.SCORE);
                            outmsg.Write(map.persos[map.persos.IndexOf(p)].score);
                            client.SendMessage(outmsg, senderConnection, NetDeliveryMethod.ReliableOrdered);

                            outmsg = client.CreateMessage();
                            outmsg.Write((byte)PacketTypes.BONUS);
                            outmsg.Write(map.persos[map.persos.IndexOf(p)].nbNuts);
                            client.SendMessage(outmsg, senderConnection, NetDeliveryMethod.ReliableOrdered);

                            outmsg = client.CreateMessage();
                            outmsg.Write((byte)PacketTypes.HEALTH);
                            outmsg.Write(map.persos[map.persos.IndexOf(p)].health);
                            client.SendMessage(outmsg, senderConnection, NetDeliveryMethod.ReliableOrdered);
                            Console.WriteLine("connecté");

                        }

                        else if (inc.ReadByte() == (byte)PacketTypes.POSITIONX)
                        {
                            senderConnection = inc.SenderConnection;
                            Console.WriteLine("posx");
                            int indexPos = inc.ReadInt32();
                            float pos = inc.ReadFloat();
                            map.persos[indexPos].positionPerso.X = pos;
                            outmsg = client.CreateMessage();
                            outmsg.Write((byte)PacketTypes.POSITIONX);
                            //outmsg.Write(map.persos.IndexOf(p));
                            outmsg.Write(map.persos[map.persos.IndexOf(p)].positionPerso.X);
                            client.SendMessage(outmsg, senderConnection, NetDeliveryMethod.ReliableOrdered);
                        }
                        else if (inc.ReadByte() == (byte)PacketTypes.POSITIONY)
                        {
                            senderConnection = inc.SenderConnection;
                            map.persos[inc.ReadInt32()].positionPerso.Y = inc.ReadFloat();
                            outmsg = client.CreateMessage();
                            outmsg.Write((byte)PacketTypes.POSITIONY);
                            outmsg.Write(map.persos[map.persos.IndexOf(p)].positionPerso.Y);
                            client.SendMessage(outmsg, senderConnection, NetDeliveryMethod.ReliableOrdered);
                        }
                        else if (inc.ReadByte() == (byte)PacketTypes.SCORE)
                        {
                            senderConnection = inc.SenderConnection;
                            map.persos[inc.ReadInt32()].score = inc.ReadInt32();
                            outmsg = client.CreateMessage();
                            outmsg.Write((byte)PacketTypes.SCORE);
                            outmsg.Write(map.persos[map.persos.IndexOf(p)].score);
                            client.SendMessage(outmsg, senderConnection, NetDeliveryMethod.ReliableOrdered);
                        }
                        else if (inc.ReadByte() == (byte)PacketTypes.BONUS)
                        {
                            senderConnection = inc.SenderConnection;
                            map.persos[inc.ReadInt32()].score = inc.ReadInt32();
                            outmsg = client.CreateMessage();
                            outmsg.Write((byte)PacketTypes.BONUS);
                            outmsg.Write(map.persos[map.persos.IndexOf(p)].nbNuts);
                            client.SendMessage(outmsg, senderConnection, NetDeliveryMethod.ReliableOrdered);
                        }
                        else if (inc.ReadByte() == (byte)PacketTypes.HEALTH)
                        {
                            senderConnection = inc.SenderConnection;
                            map.persos[inc.ReadInt32()].score = inc.ReadInt32();
                            outmsg = client.CreateMessage();
                            outmsg.Write((byte)PacketTypes.HEALTH);
                            outmsg.Write(map.persos[map.persos.IndexOf(p)].health);
                            client.SendMessage(outmsg, senderConnection, NetDeliveryMethod.ReliableOrdered);
                        }

                        break;

                    default:
                        break;
                }
            }
        }
    }
}

