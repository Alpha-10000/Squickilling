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
        // Client Object
        static NetClient netClient;

        // Clients list of characters
        static List<Perso> GameStateList;

        // Create timer that tells client, when to send update
        static System.Timers.Timer update;
        public Client()
        {
            // Ask for IP
            Console.WriteLine("Enter IP To Connect");

            // Read Ip to string
            string hostip = Console.ReadLine();

            // Create new instance of configs. Parameter is "application Id". It has to be same on client and server.
            NetPeerConfiguration Config = new NetPeerConfiguration("game");

            // Create new client, with previously created configs
            netClient = new NetClient(Config);

            // Create new outgoing message
            NetOutgoingMessage outmsg = netClient.CreateMessage();


            //LoginPacket lp = new LoginPacket("Katu");

            // Start client
            netClient.Start();

            // Write byte ( first byte informs server about the message type ) ( This way we know, what kind of variables to read )
            outmsg.Write((byte)PacketTypes.LOGIN);

            // Write String "Name" . Not used, but just showing how to do it
            outmsg.Write("MyName");

            // Connect client, to ip previously requested from user 
            netClient.Connect(hostip, 14242, outmsg);


            Console.WriteLine("Client Started");

            // Create the list of characters
            GameStateList = new List<Perso>();

            // Set timer to tick every 50ms
            update = new System.Timers.Timer(50);

            // When time has elapsed ( 50ms in this case ), call "update_Elapsed" function
            update.Elapsed += new System.Timers.ElapsedEventHandler(update_Elapsed);

            // Funtion that waits for connection approval info from server
            WaitForStartingInfo();

            // Start the timer
            update.Start();
        }
        public void Update()
        {
            // Just loop this like madman
            GetInputAndSendItToServer();
        }



        /// <summary>
        /// Every 50ms this is fired
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void update_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // Check if server sent new messages
            CheckServerMessages();

            // Draw the world
            DrawGameState();
        }




        // Before main looping starts, we loop here and wait for approval message
        private static void WaitForStartingInfo()
        {
            // When this is set to true, we are approved and ready to go
            bool CanStart = false;

            // New incomgin message
            NetIncomingMessage inc;

            // Loop untill we are approved
            while (!CanStart)
            {

                // If new messages arrived
                if ((inc = netClient.ReadMessage()) != null)
                {
                    // Switch based on the message types
                    switch (inc.MessageType)
                    {

                        // All manually sent messages are type of "Data"
                        case NetIncomingMessageType.Data:

                            // Read the first byte
                            // This way we can separate packets from each others
                            if (inc.ReadByte() == (byte)PacketTypes.WORLDSTATE)
                            {
                                // Worldstate packet structure
                                //
                                // int = count of players
                                // character obj * count



                                //Console.WriteLine("WorldState Update");

                                // Empty the gamestatelist
                                // new data is coming, so everything we knew on last frame, does not count here
                                // Even if client would manipulate this list ( hack ), it wont matter, becouse server handles the real list
                                GameStateList.Clear();

                                // Declare count
                                int count = 0;

                                // Read int
                                count = inc.ReadInt32();

                                // Iterate all players
                                for (int i = 0; i < count; i++)
                                {

                                    // Create new character to hold the data
                                    Perso p = new Perso();

                                    // Read all properties ( Server writes characters all props, so now we can read em here. Easy )
                                    inc.ReadAllProperties(p);

                                    // Add it to list
                                    GameStateList.Add(p);
                                }

                                // When all players are added to list, start the game
                                CanStart = true;
                            }
                            break;

                        default:
                            // Should not happen and if happens, don't care
                            Console.WriteLine(inc.ReadString() + " Strange message");
                            break;
                    }
                }
            }
        }


        /// <summary>
        /// Check for new incoming messages from server
        /// </summary>
        private static void CheckServerMessages()
        {
            // Create new incoming message holder
            NetIncomingMessage inc;

            // While theres new messages
            //
            // THIS is exactly the same as in WaitForStartingInfo() function
            // Check if its Data message
            // If its WorldState, read all the characters to list
            while ((inc = netClient.ReadMessage()) != null)
            {
                if (inc.MessageType == NetIncomingMessageType.Data)
                {
                    if (inc.ReadByte() == (byte)PacketTypes.WORLDSTATE)
                    {
                        Console.WriteLine("World State uppaus");
                        GameStateList.Clear();
                        int jii = 0;
                        jii = inc.ReadInt32();
                        for (int i = 0; i < jii; i++)
                        {
                            Perso p = new Perso();
                            inc.ReadAllProperties(p);
                            GameStateList.Add(p);
                        }
                    }
                }
            }
        }


        // Get input from player and send it to server
        private static void GetInputAndSendItToServer()
        {

            // Enum object
            MoveDirection MoveDir = new MoveDirection();

            // Default movement is none
            MoveDir = MoveDirection.NONE;

            // Readkey ( NOTE: This normally stops the code flow. Thats why we have timer running, that gets updates)
            // ( Timers run in different threads, so that can be run, even thou we sit here and wait for input )
            ConsoleKeyInfo kinfo = Console.ReadKey();

            // This is wsad controlling system
            if (kinfo.KeyChar == 'w')
                MoveDir = MoveDirection.UP;
            if (kinfo.KeyChar == 's')
                MoveDir = MoveDirection.DOWN;
            if (kinfo.KeyChar == 'a')
                MoveDir = MoveDirection.LEFT;
            if (kinfo.KeyChar == 'd')
                MoveDir = MoveDirection.RIGHT;

            if (kinfo.KeyChar == 'q')
            {

                // Disconnect and give the reason
                netClient.Disconnect("bye bye");

            }

            // If button was pressed and it was some of those movement keys
            if (MoveDir != MoveDirection.NONE)
            {
                // Create new message
                NetOutgoingMessage outmsg = netClient.CreateMessage();

                // Write byte = Set "MOVE" as packet type
                outmsg.Write((byte)PacketTypes.MOVE);

                // Write byte = move direction
                outmsg.Write((byte)MoveDir);

                // Send it to server
                netClient.SendMessage(outmsg, NetDeliveryMethod.ReliableOrdered);

                // Reset movedir
                MoveDir = MoveDirection.NONE;
            }

        }

        // Move direction enumerator
        enum MoveDirection
        {
            UP,
            DOWN,
            LEFT,
            RIGHT,
            NONE
        }

        private static void DrawGameState()
        {
            Console.Clear();
            Console.WriteLine("Coucou, je suis connecté");


            Console.WriteLine("Move with: WASD");
            Console.WriteLine("Connections status: " + (NetConnectionStatus)netClient.ServerConnection.Status);
            // Draw each player to their positions
            foreach (Perso p in GameStateList)
            {
                // Sets manually cursor position in console
                Console.SetCursorPosition((int)p.positionPerso.X, (int)p.positionPerso.Y);

                // Write char that marks player
                Console.Write("@");
            }
        }
    }
}
