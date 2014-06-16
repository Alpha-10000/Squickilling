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
using Microsoft.Xna.Framework.Net;
using System.Net;
using System.Net.Sockets;

namespace thegame
{
    enum PacketTypes
    {
        LOGIN,
        MOVE,
        WORLDSTATE
    }
    enum MoveDirection
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
        NONE
    }
    class Server
    {
        // Server object
        static NetServer netServer;
        // Configuration object
        static NetPeerConfiguration Config;

        // Object that can be used to store and read messages
        NetIncomingMessage inc;
        // Create list of "Characters" ( defined later in code ). This list holds the world state. Character positions
        List<Perso> GameWorldState;
        // Check time
        DateTime time;

        TimeSpan timetopass;
        public Server()
        {
            IPHostEntry host;
            string localIP = "?";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                }
            }
            Console.WriteLine(localIP);

            //TODO
            // Envoyer au serveur l'adresse locale de l'hôte de la partie.
            // Partie créée.
            // Le serveur la redistribue à tous ceux qui rejoignent la partie.
            // Ensuite c'est utilisé par les clients pour se connecter au serveur.
            // La partie est ensuite lancée.
            // Fonctionne uniquement sur le même réseau

            // Create new instance of configs. Parameter is "application Id". It has to be same on client and server.
            Config = new NetPeerConfiguration("game");

            // Set server port
            Config.Port = 14242;

            // Max client amount
            Config.MaximumConnections = 4;

            // Enable New messagetype. Explained later
            Config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            // Create new server based on the configs just defined
            netServer = new NetServer(Config);

            // Start it
            netServer.Start();

            // Eh..
            Console.WriteLine("Server Started");

            GameWorldState = new List<Perso>();


            time = DateTime.Now;

            // Create timespan of 30ms
            timetopass = new TimeSpan(0, 0, 0, 0, 30);

            // Write to con..
            Console.WriteLine("Waiting for new connections and updateing world state to current ones");
        }

        public void Update()
        {
            if ((inc = netServer.ReadMessage()) != null)
            {
                // Theres few different types of messages.
                switch (inc.MessageType)
                {
                    // If incoming message is Request for connection approval
                    // This is the very first packet/message that is sent from client
                    case NetIncomingMessageType.ConnectionApproval:

                        // Read the first byte of the packet
                        // ( Enums can be casted to bytes, so it be used to make bytes human readable )
                        if (inc.ReadByte() == (byte)PacketTypes.LOGIN)
                        {
                            Console.WriteLine("Incoming LOGIN");

                            // Approve clients connection ( Its sort of agreenment. "You can be my client and i will host you" )
                            inc.SenderConnection.Approve();

                            // Add new character to the game.
                            // It adds new player to the list and stores name, ( that was sent from the client )
                            // Random x, y and stores client IP+Port
                            //Character(inc.ReadString(), r.Next(1, 40), r.Next(1, 20), inc.SenderConnection)
                            GameWorldState.Add(new Perso(new Vector2(20, 300), CharacType.player, inc.SenderConnection));

                            // Create message, that can be written and sent
                            NetOutgoingMessage outmsg = netServer.CreateMessage();

                            // first we write byte
                            outmsg.Write((byte)PacketTypes.WORLDSTATE);

                            // then int
                            outmsg.Write(GameWorldState.Count);

                            // iterate trought every character ingame
                            foreach (Perso ch in GameWorldState)
                            {
                                // This is handy method
                                // It writes all the properties of object to the packet
                                outmsg.WriteAllProperties(ch);
                            }

                            // Now, packet contains:
                            // Byte = packet type
                            // Int = how many players there is in game
                            // character object * how many players is in game

                            // Send message/packet to all connections, in reliably order, channel 0
                            // Reliably means, that each packet arrives in same order they were sent. Its slower than unreliable, but easyest to understand
                            netServer.SendMessage(outmsg, inc.SenderConnection, NetDeliveryMethod.ReliableOrdered, 0);

                            // Debug
                            Console.WriteLine("Approved new connection and updated the world status");
                        }

                        break;
                    // Data type is all messages manually sent from client
                    // ( Approval is automated process )
                    case NetIncomingMessageType.Data:

                        // Read first byte
                        if (inc.ReadByte() == (byte)PacketTypes.MOVE)
                        {
                            // Check who sent the message
                            // This way we know, what character belongs to message sender
                            foreach (Perso p in GameWorldState)
                            {
                                // If stored connection ( check approved message. We stored ip+port there, to character obj )
                                // Find the correct character
                                if (p.Connection != inc.SenderConnection)
                                    continue;

                                // Read next byte
                                byte b = inc.ReadByte();

                                // Handle movement. This byte should correspond to some direction
                                if ((byte)MoveDirection.UP == b)
                                    p.positionPerso.Y--;
                                if ((byte)MoveDirection.DOWN == b)
                                    p.positionPerso.Y++;
                                if ((byte)MoveDirection.LEFT == b)
                                    p.positionPerso.X--; ;
                                if ((byte)MoveDirection.RIGHT == b)
                                    p.positionPerso.X++;

                                // Create new message
                                NetOutgoingMessage outmsg = netServer.CreateMessage();

                                // Write byte, that is type of world state
                                outmsg.Write((byte)PacketTypes.WORLDSTATE);

                                // Write int, "how many players in game?"
                                outmsg.Write(GameWorldState.Count);

                                // Iterate throught all the players in game
                                foreach (Perso p2 in GameWorldState)
                                {
                                    // Write all the properties of object to message
                                    outmsg.WriteAllProperties(p2);
                                }

                                // Message contains
                                // Byte = PacketType
                                // Int = Player count
                                // Character obj * Player count

                                // Send messsage to clients ( All connections, in reliable order, channel 0)
                                netServer.SendMessage(outmsg, netServer.Connections, NetDeliveryMethod.ReliableOrdered, 0);
                                break;
                            }

                        }
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        // In case status changed
                        // It can be one of these
                        // NetConnectionStatus.Connected;
                        // NetConnectionStatus.Connecting;
                        // NetConnectionStatus.Disconnected;
                        // NetConnectionStatus.Disconnecting;
                        // NetConnectionStatus.None;

                        // NOTE: Disconnecting and Disconnected are not instant unless client is shutdown with disconnect()
                        Console.WriteLine(inc.SenderConnection.ToString() + " status changed. " + (NetConnectionStatus)inc.SenderConnection.Status);
                        if (inc.SenderConnection.Status == NetConnectionStatus.Disconnected || inc.SenderConnection.Status == NetConnectionStatus.Disconnecting)
                        {
                            // Find disconnected character and remove it
                            foreach (Perso p in GameWorldState)
                            {
                                if (p.Connection == inc.SenderConnection)
                                {
                                    GameWorldState.Remove(p);
                                    break;
                                }
                            }
                        }
                        break;
                    default:
                        // As i statet previously, theres few other kind of messages also, but i dont cover those in this example
                        // Uncommenting next line, informs you, when ever some other kind of message is received
                        //Console.WriteLine("Not Important Message");
                        break;
                }
            } // If New messages

            // if 30ms has passed
            if ((time + timetopass) < DateTime.Now)
            {
                // If there is even 1 client
                if (netServer.ConnectionsCount != 0)
                {
                    // Create new message
                    NetOutgoingMessage outmsg = netServer.CreateMessage();

                    // Write byte
                    outmsg.Write((byte)PacketTypes.WORLDSTATE);

                    // Write Int
                    outmsg.Write(GameWorldState.Count);

                    // Iterate throught all the players in game
                    foreach (Perso p2 in GameWorldState)
                    {

                        // Write all properties of character, to the message
                        outmsg.WriteAllProperties(p2);
                    }

                    // Message contains
                    // byte = Type
                    // Int = Player count
                    // Character obj * Player count

                    // Send messsage to clients ( All connections, in reliable order, channel 0)
                    netServer.SendMessage(outmsg, netServer.Connections, NetDeliveryMethod.ReliableOrdered, 0);
                }
                // Update current time
                time = DateTime.Now;
            }

            // While loops run as fast as your computer lets. While(true) can lock your computer up. Even 1ms sleep, lets other programs have piece of your CPU time
            //System.Threading.Thread.Sleep(1);
        }
    }
}