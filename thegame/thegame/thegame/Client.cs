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

        int oldscore = 0;
        int oldhealth = 20;
        int oldbonus = 0;
        float oldx = 0;
        float oldy = 200;
        int indexC = 0;
        NetConnection senderConnection;

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


                byte truc = inc.ReadByte();
                Console.WriteLine(Convert.ToString(truc));
                switch (inc.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        if (truc == (byte)PacketTypes.NEWPERSO)
                        {
                            senderConnection = inc.SenderConnection;
                            Console.WriteLine("newperso");
                            int index = inc.ReadInt32();

                            if (!map.persos.Contains(p))
                            {
                                map.persos[index] = p;
                                oldx = map.persos[index].positionPerso.X;
                                oldy = map.persos[index].positionPerso.Y;
                                oldscore = map.persos[index].score;
                                oldhealth = map.persos[index].health;
                                oldbonus = map.persos[index].nbNuts;
                                indexC = index;
                            }
                            else
                            {
                                map.persos[index] = new Perso(new Vector2(0, 200), CharacType.player);
                                oldx = map.persos[index].positionPerso.X;
                                oldy = map.persos[index].positionPerso.Y;
                                oldscore = map.persos[index].score;
                                oldhealth = map.persos[index].health;
                                oldbonus = map.persos[index].nbNuts;
                            }

                            Console.WriteLine("connecté");
                        }

                        if (truc == (byte)PacketTypes.POSITIONX)
                        {
                            senderConnection = inc.SenderConnection;
                            Console.WriteLine("posx");
                            int indexPos = inc.ReadInt32();
                            oldx = inc.ReadFloat();
                            map.persos[indexPos].positionPerso.X = oldx;
                        }
                        else if (truc == (byte)PacketTypes.POSITIONY)
                        {
                            senderConnection = inc.SenderConnection;
                            int indexPos = inc.ReadInt32();
                            oldy = inc.ReadFloat();
                            map.persos[indexPos].positionPerso.Y = oldy;
                        }
                        else if (truc == (byte)PacketTypes.SCORE)
                        {
                            senderConnection = inc.SenderConnection;
                            int indexScore = inc.ReadInt32();
                            oldscore = inc.ReadInt32();
                            map.persos[indexScore].score = oldscore;
                        }
                        else if (truc == (byte)PacketTypes.BONUS)
                        {
                            senderConnection = inc.SenderConnection;
                            int indexBonus = inc.ReadInt32();
                            oldbonus = inc.ReadInt32();
                            map.persos[indexBonus].score = oldbonus;
                        }
                        else if (truc == (byte)PacketTypes.HEALTH)
                        {
                            senderConnection = inc.SenderConnection;
                            int indexHealth = inc.ReadInt32();
                            oldhealth = inc.ReadInt32();
                            map.persos[indexHealth].score = oldhealth;
                        }

                        break;

                    default:
                        break;
                }
                if (p != null)
                {
                    if (senderConnection != null)
                    {
                        if (map.persos[indexC].positionPerso.X == oldx)
                        {
                            outmsg = client.CreateMessage();
                            outmsg.Write((byte)PacketTypes.POSITIONX);
                            outmsg.Write(map.persos[indexC].positionPerso.X);
                            client.SendMessage(outmsg, senderConnection, NetDeliveryMethod.ReliableOrdered);
                        }
                        if (map.persos[indexC].positionPerso.Y == oldy)
                        {
                            outmsg = client.CreateMessage();
                            outmsg.Write((byte)PacketTypes.POSITIONY);
                            outmsg.Write(map.persos[indexC].positionPerso.Y);
                            client.SendMessage(outmsg, senderConnection, NetDeliveryMethod.ReliableOrdered);
                            
                        }
                        if (map.persos[indexC].score == oldscore)
                        {
                            outmsg = client.CreateMessage();
                            outmsg.Write((byte)PacketTypes.SCORE);
                            outmsg.Write(map.persos[indexC].score);
                            client.SendMessage(outmsg, senderConnection, NetDeliveryMethod.ReliableOrdered);
                        }
                        if (map.persos[indexC].nbNuts == oldbonus)
                        {
                            outmsg = client.CreateMessage();
                            outmsg.Write((byte)PacketTypes.BONUS);
                            outmsg.Write(map.persos[indexC].nbNuts);
                            client.SendMessage(outmsg, senderConnection, NetDeliveryMethod.ReliableOrdered);
                        }
                        if (map.persos[indexC].health != oldhealth)
                        {
                            outmsg = client.CreateMessage();
                            outmsg.Write((byte)PacketTypes.HEALTH);
                            outmsg.Write(map.persos[indexC].health);
                            client.SendMessage(outmsg, senderConnection, NetDeliveryMethod.ReliableOrdered);
                        }
                    }
                    
                }
            }
        }
    }
}

