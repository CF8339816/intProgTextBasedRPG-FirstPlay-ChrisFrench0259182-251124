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
        static int p1_x_pos = 1;
        static int p1_y_pos = 1;
        static (int, int) p1_min_max_x = (1, 29);
        //static int LimitValueX(int value, int min, int max)
        //{
        //    // Ensures 'value' is at least 'min', then ensures the result is at most 'max'.
        //    return Math.Max(1, Math.Min(1, 29));
        //}
        //static int p1_min_max_x = LimitValueX(1, 1, 29);
        //static int LimitValueY(int value, int min, int max)
        //{
        //    // Ensures 'value' is at least 'min', then ensures the result is at most 'max'.
        //    return Math.Max(1, Math.Min(1, 32));
        //}
        //static int p1_min_max_y = LimitValueX(1, 1, 32);

        static (int, int) p1_min_max_y = (1, 32);
       

       
        static int enemy_x_pos = 17;
        static int enemy_y_pos = 20;
        static (int, int) enemy_min_max_x = (1, 29);
        static (int, int) enemy_min_max_y = (1, 32);
       

        static string filepath = "maps.txt";


        //static int p1_KillScore = 0;


        static (int, int) healthPrize;

        static int treasure_x_pos = 6;
        static int treasure_y_pos = 19;
        static (int, int) treasure_min_max_x = (1, 29);
        static (int, int) treasure_min_max_y = (1, 32);
        
        static ConsoleColor[] spriteColors = { ConsoleColor.Cyan, ConsoleColor.Red,ConsoleColor.Magenta };

        static bool healthTreasure = true;
        static bool EnemySpawn = true;


       // static int turn = -1;

        static char[] allKeybindings = (new char[] { 'W', 'A', 'S', 'D' });

       


        static void Main(string[] args)
        {

            Console.CursorVisible = false;

            //DrawMap();
            ////mapLegend(); 
          
  
            while (isPlaying)
            {
             
                //Console.Clear();
                ProcessInput();
                GameUpdate();
                DrawMap();
                     
            }


     
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
                            case 'b': // Base
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                break;

                        }

                        Console.Write(mapChar);
                    }

                    Console.WriteLine();
                }


                Console.ResetColor();
                mapLegend();


            }

            catch (FileNotFoundException)
            {
                Console.WriteLine($"Error: The file '{filepath}' was not found.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            Console.SetCursorPosition(p1_x_pos, p1_y_pos);
            Console.ForegroundColor = spriteColors[0];
            Console.Write("&");

            //Console.SetCursorPosition(enemy_x_pos, enemy_y_pos);
            //Console.ForegroundColor = spriteColors[1];
            //Console.Write("#");

            //if (healthTreasure)
            //{

            //    healthTreasure = false;
            //    treasure_x_pos = healthPackSpawn.Next(1, 29);
            //    treasure_y_pos = healthPackSpawn.Next(1, 32);
            //    healthPrize = (treasure_x_pos, treasure_y_pos);
            //    Console.ForegroundColor = spriteColors[2];
            //    Console.Write("$");
            //}





        }


        //m2

        static void mapLegend()
        {
            //Console.Clear();
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

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("b    ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Base");

            Console.ResetColor();
        }

        //m3

        static void ProcessInput()
        {
            // if this isn't here, input will block the game before drawing for the first time
           //if (turn == -1) return;

            // reset input
            p1_x_input = 0;
            p1_y_input = 0;


            char[] allowedKeysThisTurn; // different keys allowed on p1 vs. p2 turn

            //// choose which keybindings to use
            //if (turn % 2 == 0)
            {
            allowedKeysThisTurn = allKeybindings;
            }
            //else
            //{
            //    allowedKeysThisTurn = (allKeybindings);
            //}
            // get the current player's input
            ConsoleKey input = ConsoleKey.NoName;
            //while (allowedKeysThisTurn.Contains(((char)input)))
            //{
                input = Console.ReadKey(true).Key;
            //}

            // check all input keys
            if (input == ConsoleKey.A) p1_x_input = -1;
            if (input == ConsoleKey.D) p1_x_input = 1;
            if (input == ConsoleKey.W) p1_y_input = -1;
            if (input == ConsoleKey.S) p1_y_input = 1;


        }
        //m4
        static void GameUpdate()
        {
           
            //return Math.Max(min, Math.Min(value, max));
            // update players' positions based on input
            p1_x_pos += p1_x_input;
          //  p1_x_pos = p1_min_max_x;

            p1_y_pos += p1_y_input;
          //  p1_x_pos = p1_min_max_y;
            //p1_y_pos = p1_y_pos.Clamp(p1_min_max_y);
            // p1_y_pos = Math.Clamp(p1_min_max_y);
        }
    }
}

