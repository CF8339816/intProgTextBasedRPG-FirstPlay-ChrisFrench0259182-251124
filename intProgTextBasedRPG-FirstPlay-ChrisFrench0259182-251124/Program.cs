using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace intProgTextBasedRPG_FirstPlay_ChrisFrench0259182_251124
{
    internal class Program
    {
        static bool isPlaying = true;

        static Random healthPackSpawn = new Random();
        static Random EnemyStartSpawn = new Random();
        static string character;
        static int p1_x_input;
        static int p1_y_input;
        static int p1_x_pos = 1;
        static int p1_y_pos = 1;
        static (int, int) p1_min_max_x = (0, 55);
        static (int, int) p1_min_max_y = (0, 32);
       

       
        static int enemy_x_pos = 17;
        static int enemy_y_pos = 20;
        static (int, int) enemy_min_max_x = (1, 55);
        static (int, int) enemy_min_max_y = (1, 32);
       

        static string filepath = "maps.txt";


        //static int p1_KillScore = 0;
        static int health = 100;
        static int score = 0;
        static int dmg = 0;
        static int hurt = 0;

        static (int, int) healthPrize;

        static int treasure_x_pos = 6;
        static int treasure_y_pos = 19;
        static (int, int) treasure_min_max_x = (1, 55);
        static (int, int) treasure_min_max_y = (1, 32);
        
        static ConsoleColor[] spriteColors = { ConsoleColor.Cyan, ConsoleColor.Red,ConsoleColor.Magenta };

        static bool healthTreasure = true;
        static bool EnemySpawn = true;


       // static int turn = -1;

        static char[] allKeybindings = (new char[] { 'W', 'A', 'S', 'D' });
        
        
        static (int, int) player1_positionPROXY = (p1_x_pos, p1_y_pos);



        static void Main(string[] args)
        {
            alias();
            Console.Clear();
            Console.CursorVisible = false;

            //DrawMap();
            ////mapLegend(); 
            p1_x_pos = p1_min_max_x.Item1;
            p1_y_pos = p1_min_max_y.Item1;

         
           
           

            mapLegend();
            hud();
            while (isPlaying)
            {
             
                
               // Console.Clear();

                ProcessInput();
                GameUpdate();
                DrawMap();


                DeBug();  

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
            //    treasure_x_pos = healthPackSpawn.Next(p1_min_max_x.Item1, p1_min_max_x.Item2 + 1);
            //    treasure_y_pos = healthPackSpawn.Next(p1_min_max_y.Item1, p1_min_max_y.Item2 + 1);
            //    healthPrize = (treasure_x_pos, treasure_y_pos);
            //    Console.SetCursorPosition(treasure_x_pos, treasure_y_pos);
            //    Console.ForegroundColor = spriteColors[2];
            //    Console.Write("$");
            //}

    
          //  Console.SetCursorPosition(0, p1_min_max_y.Item2 + 10); 
            Console.ResetColor();
           

            Console.ResetColor();
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



            ConsoleKey input = Console.ReadKey(true).Key;
           

            // check all input keys
            if (input == ConsoleKey.A) p1_x_input = -1;
            if (input == ConsoleKey.D) p1_x_input = 1;
            if (input == ConsoleKey.W) p1_y_input = -1;
            if (input == ConsoleKey.S) p1_y_input = 1;

            if (input == ConsoleKey.Q) isPlaying = false; //Quit the 'is playing' loop
        }
        //m4
        static void GameUpdate()
        {

            p1_x_pos += p1_x_input;
            p1_y_pos += p1_y_input;

            p1_x_pos = Math.Max(p1_min_max_x.Item1, Math.Min(p1_x_pos, p1_min_max_x.Item2));

            p1_y_pos = Math.Max(p1_min_max_y.Item1, Math.Min(p1_y_pos, p1_min_max_y.Item2));

          
        }

        //m5

        static void alias()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("What is your character's name");
            Console.ForegroundColor = ConsoleColor.Blue;
            character = Console.ReadLine();
        }
         
        

        static void hud()
            {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("Name:");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"{character}");

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("{0,0}{1,8}{2,12}{3,9}", "Score", "Life", "Attack", "Hurt");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("{0,2}{1,10}{2,10}{3,10}", score, health, dmg, hurt + "\n");
            Console.ResetColor();



        }



        static void DeBug()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("debug block\n");
                            

            //checking the new proxy for position compliance C.F.
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Player 1 position:");
            Console.ForegroundColor = spriteColors[0];
            Console.WriteLine(player1_positionPROXY);


        }






    }

}

