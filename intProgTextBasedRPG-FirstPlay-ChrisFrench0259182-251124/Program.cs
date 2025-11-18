using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.MemoryMappedFiles;

namespace intProgTextBasedRPG_FirstPlay_ChrisFrench0259182_251124
{
    internal class Program
    {
        static bool isPlaying = true;

        static Random healthPackSpawn = new Random();
        static Random EnemyStartSpawn = new Random();
        static int p1_x_input;
        static int p1_y_input;
        static int enemy_x_input;
        static int enemy_y_input;

        static (int, int) p1_min_max_x = (1, 29);
        static (int, int) p1_min_max_y = (1, 32);
        static (int, int) p2_min_max_x = (1, 29);
        static (int, int) p2_min_max_y = (1, 32);



        static string filepath = "maps.txt";
        static int p1_KillScore = 0;


        static bool healthTreasure = true;
        static bool EnemySpawn = true;


        static int turn = -1;

        static char[] allKeybindings = (new char[] { 'W', 'A', 'S', 'D' });
        static ConsoleColor[] playerColors = { ConsoleColor.Cyan};
        static ConsoleColor[] enemyColors = { ConsoleColor.Red };
        static ConsoleColor[] treasureColors = { ConsoleColor.Magenta };

        static void Main(string[] args)
        {

            

            DrawMap();
            mapLegend();

            while (isPlaying)





            Console.ReadKey();
        }
        //m1
        static void DrawMap()
        {
            Directory.GetCurrentDirectory();
            try
            {

                string[] Maps = File.ReadAllLines(filepath);

                foreach (string map in Maps)
                {

                    foreach (char mapChar in map)
                    {

                       switch (mapChar)
                        
                        {
                           
                            case 'g': // Grass
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                break;
                            
                            case 'w': // Water
                                Console.ForegroundColor = ConsoleColor.Blue;
                                break;
                            
                            case 'm': // Mountain
                                Console.ForegroundColor = ConsoleColor.Gray;
                                break;
                           
                            case 't': // Trees
                                Console.ForegroundColor = ConsoleColor.Green;
                                break;


                        }
                        
                        Console.Write(mapChar);
                    }
                    
                    Console.WriteLine();
                }

               
                Console.ResetColor();
            

                
            }

            catch (FileNotFoundException)
            {
                Console.WriteLine($"Error: The file '{filepath}' was not found.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }




            }
        //m2
        
        static void mapLegend()
        {

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("g    ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Grass");

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("w    ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Water");

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("m    ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Mountain");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("t    ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Trees");

            Console.ResetColor();
        }

        //m3

        static void ProcessInput()
        {
            // if this isn't here, input will block the game before drawing for the first time
            if (turn == -1) return;

            // reset input
            p1_x_input = 0;
            p1_y_input = 0;
            enemy_x_input = 0;
           enemy_y_input = 0;

            char[] allowedKeysThisTurn; // different keys allowed on p1 vs. p2 turn

            // choose which keybindings to use
            if (turn % 2 == 0)
            {
                allowedKeysThisTurn = allKeybindings;
            }
            else
            {
                allowedKeysThisTurn = (allKeybindings);
            }
                // get the current player's input
                ConsoleKey input = ConsoleKey.NoName;
            while (allowedKeysThisTurn.Contains(((char)input)))
            {
                input = Console.ReadKey(true).Key;
            }

            // check all input keys
            if (input == ConsoleKey.A) p1_x_input = -1;
            if (input == ConsoleKey.D) p1_x_input = 1;
            if (input == ConsoleKey.W) p1_y_input = -1;
            if (input == ConsoleKey.S) p1_y_input = 1;

           
        }


    }
}

