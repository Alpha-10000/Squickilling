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
        PERSOLEAVE,
        PROJ
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

        private int nb_players = 0;
        private int myindex;
        private bool IKnowwhereIAm = false;
        private string theid;

        public Client(string theid)
        {
            this.theid = theid;

            config = new NetPeerConfiguration("squickilling");
            client = new NetClient(config);
            client.Start();
            outmsg = client.CreateMessage();
            outmsg.Write((byte)PacketTypes.LOGIN);
            outmsg.Write(theid);
            //outmsg.Write("osef");
            string ip = "localhost";//ip server : 37.187.43.213
            client.Connect(ip, 14242, outmsg);
            p = new Perso(new Vector2(0, 200), CharacType.player);
            p.utilisable = true;
            Console.WriteLine("Client créé");

        }

        private void OnTriche(ref MapMulti map)
        {
            for (int i = 0; i < nb_players; i++)
                if (map.persos[i] == null && i != myindex)
                    map.persos[i] = new Perso(new Vector2(0, 200), CharacType.player);

        }

      
        public void Update(ref MapMulti map)
        {

            NetIncomingMessage inc;
            if ((inc = client.ReadMessage()) != null)//Receive message
            {


                byte truc = inc.ReadByte();
                switch (inc.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        if (truc == (byte)PacketTypes.NEWPERSO)
                        {

                            senderConnection = inc.SenderConnection;
                            Console.WriteLine("I received a new perso");
                            int index = inc.ReadInt32();
                            nb_players = inc.ReadInt32();
                            if (!IKnowwhereIAm)
                                myindex = nb_players - 1;


                            Console.WriteLine("There is " + nb_players + " players");
                            if (index == myindex && !IKnowwhereIAm)// if im the one
                            {
                                Console.WriteLine("Me and id : " + myindex);
                                OnTriche(ref map);
                                map.persos[myindex] = p;
                                oldx = map.persos[myindex].animationPerso.Position.X;
                                oldy = map.persos[myindex].animationPerso.Position.Y;
                                oldscore = map.persos[myindex].score;
                                oldhealth = map.persos[myindex].health;
                                oldbonus = map.persos[myindex].nbNuts;
                                indexC = myindex;
                                IKnowwhereIAm = true;
                            }
                            else // other perso
                            {
                                Console.WriteLine("Not me and id  : " + index);
                                OnTriche(ref map);


                            }

                            Console.WriteLine("connecté");
                        }

                       

                        if (truc == (byte)PacketTypes.POSITIONX)
                        {

                            int whichPersoIndex = inc.ReadInt32();
                            float newPosX = inc.ReadFloat();
                            Console.WriteLine(newPosX);
                            map.persos[whichPersoIndex].animationPerso.Position.X = newPosX;

                        }
                        if (truc == (byte)PacketTypes.POSITIONY)
                        {

                            int whichPersoIndex = inc.ReadInt32();
                            float newPosY = inc.ReadFloat();
                            Console.WriteLine(newPosY);
                            map.persos[whichPersoIndex].animationPerso.Position.Y = newPosY;

                        }
                        if (truc == (byte)PacketTypes.SCORE)
                        {

                            int whichPersoIndex = inc.ReadInt32();
                            int newScore = inc.ReadInt32();
                            Console.WriteLine(newScore);
                            map.persos[whichPersoIndex].score = newScore;

                        }
                        if (truc == (byte)PacketTypes.BONUS)
                        {

                            int whichPersoIndex = inc.ReadInt32();
                            int newBonus = inc.ReadInt32();
                            Console.WriteLine(newBonus);
                            map.persos[whichPersoIndex].nbNuts = newBonus;

                        }
                        if (truc == (byte)PacketTypes.HEALTH)
                        {

                            int whichPersoIndex = inc.ReadInt32();
                            int newHealth = inc.ReadInt32();
                            Console.WriteLine(newHealth);
                            map.persos[whichPersoIndex].health = newHealth;
                        }

                        if (truc == (byte)PacketTypes.PROJ)
                        {
                            int whichPersoIndex = inc.ReadInt32();
                            Console.WriteLine("proj");
                            map.persos[whichPersoIndex].UpdateNoix();
                        }

                        //IT HAS TO BE AT THE END
                        if (truc == (byte)PacketTypes.PERSOLEAVE)//this happens only if there is at least 2 players in the game!
                        {

                            int LeaveId = inc.ReadInt32();
                            nb_players--;
                            myindex--;
                            for (int i = LeaveId; i < nb_players; i++)
                            {
                                map.persos[i] = map.persos[i + 1];
                            }

                            map.persos[nb_players] = null;

                        }


                        /*
                    else if (truc == (byte)PacketTypes.SCORE)
                    {
                        senderConnection = inc.SenderConnection;
                        int indexScore = inc.ReadInt32();
                        oldscore = inc.ReadInt32();
                        try
                        {
                            map.persos[indexScore].score = oldscore;
                        }
                        catch
                        {
                            OnTriche(indexScore, ref map);
                        }
                    }
                    else if (truc == (byte)PacketTypes.BONUS)
                    {
                        senderConnection = inc.SenderConnection;
                        int indexBonus = inc.ReadInt32();
                        oldbonus = inc.ReadInt32();
                        try
                        {
                            map.persos[indexBonus].score = oldbonus;
                        }
                        catch
                        {
                            OnTriche(indexBonus, ref map);
                        }
                    }
                    else if (truc == (byte)PacketTypes.HEALTH)
                    {
                        senderConnection = inc.SenderConnection;
                        int indexHealth = inc.ReadInt32();
                        oldhealth = inc.ReadInt32();
                        try
                        {
                            map.persos[indexHealth].score = oldhealth;
                        }
                        catch
                        {
                            OnTriche(indexHealth, ref map);
                        }
                    }
                    */
                        break;

                    default:
                        break;

                }
            }
            if (IKnowwhereIAm)
            {

                if (map.persos[myindex].positionPerso.X != oldx)//If i move I send JUST my position
                {
                    outmsg = client.CreateMessage();
                    outmsg.Write((byte)PacketTypes.POSITIONX);
                    outmsg.Write(myindex);
                    outmsg.Write(map.persos[myindex].positionPerso.X);
                    client.SendMessage(outmsg, senderConnection, NetDeliveryMethod.ReliableOrdered);
                    oldx = map.persos[myindex].positionPerso.X;
                }

                if (map.persos[myindex].positionPerso.Y != oldy)
                {
                    outmsg = client.CreateMessage();
                    outmsg.Write((byte)PacketTypes.POSITIONY);
                    outmsg.Write(myindex);
                    outmsg.Write(map.persos[myindex].positionPerso.Y);
                    client.SendMessage(outmsg, senderConnection, NetDeliveryMethod.ReliableOrdered);
                    oldy = map.persos[myindex].positionPerso.Y;
                }

                if (map.persos[myindex].score != oldscore)
                {
                    outmsg = client.CreateMessage();
                    outmsg.Write((byte)PacketTypes.SCORE);
                    outmsg.Write(myindex);
                    outmsg.Write(map.persos[myindex].score);
                    client.SendMessage(outmsg, senderConnection, NetDeliveryMethod.ReliableOrdered);
                    oldscore = map.persos[myindex].score;
                }

                if (map.persos[myindex].nbNuts != oldbonus)
                {
                    outmsg = client.CreateMessage();
                    outmsg.Write((byte)PacketTypes.BONUS);
                    outmsg.Write(myindex);
                    outmsg.Write(map.persos[myindex].nbNuts);
                    client.SendMessage(outmsg, senderConnection, NetDeliveryMethod.ReliableOrdered);
                    oldbonus = map.persos[myindex].nbNuts;
                }

                if (map.persos[myindex].health != oldhealth)
                {
                    outmsg = client.CreateMessage();
                    outmsg.Write((byte)PacketTypes.HEALTH);
                    outmsg.Write(myindex);
                    outmsg.Write(map.persos[myindex].health);
                    client.SendMessage(outmsg, senderConnection, NetDeliveryMethod.ReliableOrdered);
                    oldhealth = map.persos[myindex].health;
                }
                if (Inputs.isKeyRelease(Keys.Space))
                {
                    outmsg = client.CreateMessage();
                    outmsg.Write((byte)PacketTypes.PROJ);
                    outmsg.Write(myindex);
                    client.SendMessage(outmsg, senderConnection, NetDeliveryMethod.ReliableOrdered);
                }


                /* DEBUG 
                if (Imoved)
                    Console.WriteLine("I moved");
                else
                    Console.WriteLine("I did not moved");
          */







                /*
                if (map.persos[indexC].score != oldscore)
                {
                    outmsg = client.CreateMessage();
                    outmsg.Write((byte)PacketTypes.SCORE);
                    try
                    {
                        outmsg.Write(map.persos[indexC].score);
                    }
                    catch
                    {
                        OnTriche(indexC, ref map);
                    }
                    client.SendMessage(outmsg, senderConnection, NetDeliveryMethod.ReliableOrdered);
                }
                if (map.persos[indexC].nbNuts != oldbonus)
                {
                    outmsg = client.CreateMessage();
                    outmsg.Write((byte)PacketTypes.BONUS);
                    try
                    {
                        outmsg.Write(map.persos[indexC].nbNuts);
                    }
                    catch
                    {
                        OnTriche(indexC, ref map);
                    }
                    client.SendMessage(outmsg, senderConnection, NetDeliveryMethod.ReliableOrdered);
                }
                if (map.persos[indexC].health != oldhealth)
                {
                    outmsg = client.CreateMessage();
                    outmsg.Write((byte)PacketTypes.HEALTH);
                    try
                    {
                        outmsg.Write(map.persos[indexC].health);
                    }
                    catch
                    {
                        OnTriche(indexC, ref map);
                    }
                    client.SendMessage(outmsg, senderConnection, NetDeliveryMethod.ReliableOrdered);
                }
                 */



            }
        }
    }
}

