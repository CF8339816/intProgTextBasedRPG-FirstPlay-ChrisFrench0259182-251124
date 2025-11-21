using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace intProgTextBasedRPG_FirstPlay_ChrisFrench0259182_251124
{
    internal class Program
    {
        
        static bool isPlaying = true;
        static bool healthTreasure = true;
        static bool EnemySpawn = true;
        static bool inCombat = true;
       


       static Random randomHealth = new Random();
        static Random randomDMG = new Random();
       static Random randomEnDMG = new Random();
       static Random randomXP = new Random();
        static Random healthPackSpawn = new Random();
        static Random EnemyStartSpawn = new Random();

        static string character;
        static int p1_x_input;
        static int p1_y_input;
        static int p1_x_pos;
        static int p1_y_pos;
        static (int, int) p1_min_max_x = (1, 55);
        static (int, int) p1_min_max_y = (1, 29);
        static int p1_Old_X;
        static int p1_Old_Y;
        static string phStat;
        static int p1dmg = randomDMG.Next(12, 35);
      

        static int output_X= 61;
        static int output_Y= 1;
        static (int, int) output_min_max_x = (60, 100);
        static (int, int) output_min_max_y = (1, 29);
      
        
        static int enemy_x_pos;
        static int enemy_y_pos;
        static (int, int) enemy_min_max_x = (15, 35);
        static (int, int) enemy_min_max_y = (12, 29);
        static string ehStat;
        static int eHealth = 100;
        static int enemydmg = randomDMG.Next(12, 35);
        static (int, int) enemyLoc = (enemy_x_pos, enemy_y_pos);

        static char mapChar;
        static string filepath = "maps.txt";
       
        static int score = 0;
        static int dmg = 0;
        static int hurt = 0;
        static int dmgE = 0;

        static (int, int) HealthUp = (treasure_x_pos, treasure_y_pos);

        static int treasure_x_pos;
        static int treasure_y_pos;
        static (int, int) treasure_min_max_x = (9, 45);
        static (int, int) treasure_min_max_y = (7, 20);
        static int healing = randomHealth.Next(15, 75);
        
     


        static ConsoleColor[] spriteColors = { ConsoleColor.Cyan, ConsoleColor.Red, ConsoleColor.Magenta };
        static char[] allKeybindings = (new char[] { 'W', 'A', 'S', 'D' });


        static (int, int) player1_positionPROXY;
        static (int, int) player1_positionOLD;

        static int health = 100;
        static int minhealth = 0;
        static int maxhealth = 100;
        static int hp;

       static int clampedhealth = health < minhealth ? minhealth : (health > maxhealth ? maxhealth : health);



        static int xp = 0;
        static int level = 0;

      
        static int giveXP;


        static string[] Maps;
        static string map ;
        static int mapXs;
        static int mapYs;


        static void Main(string[] args)
        {

            Directory.GetCurrentDirectory();
            Console.WriteLine("Please maximize the window before proceeding to avoid any issues, \n then press any key to continue... ");

            Console.ReadKey();
            Console.Clear();


            Console.SetCursorPosition(0, 0);
            alias();
            Console.Clear();
            Console.CursorVisible = false;

            

              DrawMap();
            Console.WriteLine("press any key to start game...\nPress 'Q' to exit...\nUse W,A,S,D to move around the map...\nGet to the base to be safe...\nPick up '$' supply caches to heal and repair...\nFight enemies '#' by manouvering to them or try to avoid them... ");

            mapLegend();
            //mapLegend();
            //hud();

            while (isPlaying)
            {
                // Console.Clear();

              

                ProcessInput();

                GameUpdate();
                ErasePlayer();
                DrawP();
                DrawE();
                DrawH();

                hud();
                DeBug();

                // while (inCombat)
                //{
                //    ProcessInput();

                //    GameUpdate();

                //    //TakeDamage();
                //}

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
                            #region // colouring the individual tiles
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

                            case '|': //Border
                                Console.ForegroundColor = ConsoleColor.Yellow;

                                break;

                            case '-': // Border
                                Console.ForegroundColor = ConsoleColor.Yellow;

                                break;

                            case '+': // Border
                                Console.ForegroundColor = ConsoleColor.Yellow;

                                break;
                                #endregion
                        }
                        //Console.SetCursorPosition(0, 0);
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

        static void maptile()
        {
           
                switch (mapChar)

                {
                    #region // colouring the individual tiles
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

                    case '|': //Border
                        Console.ForegroundColor = ConsoleColor.Yellow;

                        break;

                    case '-': // Border
                        Console.ForegroundColor = ConsoleColor.Yellow;

                        break;

                    case '+': // Border
                        Console.ForegroundColor = ConsoleColor.Yellow;

                        break;

                     #endregion
            }
                //Console.SetCursorPosition(0, 0);
                Console.Write(mapChar);

            
        }

        //m2

        static void mapLegend()
        {

            //Console.Clear();
            Console.SetCursorPosition(output_X, output_Y + 11);
            string MapLegend =
            "+========= Map Legend ============+";
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($" {MapLegend}\n");


            Console.SetCursorPosition(output_X + 2, output_Y + 12);
            //Console.Clear();

            //Console.SetCursorPosition(output_X + 2, output_Y + 12);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("g = Grass  ");
            //Console.ForegroundColor = ConsoleColor.DarkYellow;
            //Console.Write("= Grass    ");

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("w = Water  ");
            //Console.ForegroundColor = ConsoleColor.DarkYellow;
            //Console.Write("= Water    ");

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("m = Mountain  ");
            //Console.ForegroundColor = ConsoleColor.DarkYellow;
            //Console.Write("= Mountain    ");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("t = Trees  ");
            //Console.ForegroundColor = ConsoleColor.DarkYellow;
            //Console.Write("= Trees    ");

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("b = Base  ");
            //Console.ForegroundColor = ConsoleColor.DarkYellow;
            //Console.WriteLine("= Base    ");

            Console.ResetColor();
        }

        //m3

        //static bool CanMoveTo(int x, int y)
        //{
            
        //    int mapYs = y - 1;
        //    int mapXs = x - 1;

          
        //    if (Maps == null || mapYs < 0 || mapYs >= Maps.Length || mapXs < 0 || mapXs >= Maps[mapYs].Length)
        //    {
        //        return false;
        //    }

          
        //    char tile = Maps[mapYs][mapXs];

          
        //    if (tile == 'm' || tile == 'w' )
        //    {
                
        //        return false;
        //    }

           
        //    return true;
        //}

        static void ProcessInput()
        {
            // player1_positionOLD = player1_positionPROXY;
         
            //  CanMoveTo(mapXs, mapYs);
            
            p1_Old_X = p1_x_pos;
            p1_Old_Y = p1_y_pos;

            p1_x_input = 0;
            p1_y_input = 0;

            ConsoleKey input = Console.ReadKey(true).Key;

            // check all input keys
            if (input == ConsoleKey.A ) p1_x_input = -1;
            if (input == ConsoleKey.D) p1_x_input = 1;
            if (input == ConsoleKey.W) p1_y_input = -1;
            if (input == ConsoleKey.S) p1_y_input = 1;

            if (input == ConsoleKey.Q) isPlaying = false; //Quit the 'is playing' loop

        }
        //m4
        static void GameUpdate()
        {
         //   CanMoveTo(mapXs, mapYs);

            p1_x_pos += p1_x_input;
            p1_y_pos += p1_y_input;

            p1_x_pos = Math.Max(p1_min_max_x.Item1, Math.Min(p1_x_pos, p1_min_max_x.Item2));
            p1_y_pos = Math.Max(p1_min_max_y.Item1, Math.Min(p1_y_pos, p1_min_max_y.Item2));

            player1_positionPROXY = (p1_x_pos, p1_y_pos);

            if (player1_positionPROXY == HealthUp) 
            {
                Heal();
                score++;
                healthTreasure = true;
            }

            if (player1_positionPROXY == enemyLoc) 
            {
               // inCombat = true;
               TakeDamage();    
                score++;
                EnemySpawn = true;
            }
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
        //m6
        static void hud()
        {
            PlayerHealth();
          
            //Console.Clear();
            Console.SetCursorPosition(output_X, output_Y + 5);
            string HUD =
            "+========= HUD ============+";
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($" {HUD}\n");
            
            Console.SetCursorPosition(output_X + 2, output_Y + 6);
            
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("Name:");
            Console.SetCursorPosition(output_X + 8, output_Y + 6);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"{character}");

            Console.SetCursorPosition(output_X + 2, output_Y + 7);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("{0,0}{1,9}{2,9}{3,9}{4,11}{5,9}", "XP", "Level", "Score", "Life", "Attack", "Hurt");
            Console.SetCursorPosition(output_X + 2, output_Y + 8);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("{0,1}{1,8}{2,8}{3,11}{4,9}{5,10}", xp, level, score, health, dmg, hurt+ "\n");
            Console.ResetColor();

            Console.SetCursorPosition(output_X + 2, output_Y + 9);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"{character} feels: ");
            Console.SetCursorPosition(output_X + 16, output_Y + 9);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"{phStat}");
        }
        //m7
        static void DeBug()
        {
            //Console.Clear();
            Console.SetCursorPosition(output_X, output_Y);
            string DebugBox=

"+========= debug block ============+";
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($" {DebugBox}\n");

            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(output_X +2 , output_Y +1);
            Console.Write("Player 1 Pickup:");
            
            Console.SetCursorPosition(output_X + 22, output_Y+1);
            Console.WriteLine(HealthUp);

            Console.SetCursorPosition(output_X +2, output_Y +2);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Player 1 position:");
           
            Console.SetCursorPosition(output_X + 22, output_Y +2 );
            Console.WriteLine(player1_positionPROXY);

            Console.SetCursorPosition(output_X +2, output_Y + 3);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Enemy position:");

            Console.SetCursorPosition(output_X + 22, output_Y + 3);
            Console.WriteLine(enemyLoc);
        }
        //m8
        static void ErasePlayer()
        {
            int mapX = p1_Old_X;
            int mapY = p1_Old_Y;

            if (Maps != null && mapY >= 0 && mapY < Maps.Length && mapX >= 0 && mapX < Maps[mapY].Length)
            {
                char originalMapChar = Maps[mapY][mapX];

                Console.SetCursorPosition(p1_Old_X, p1_Old_Y);

                switch (originalMapChar)
                {
                    case 'g': Console.ForegroundColor = ConsoleColor.DarkYellow; break;
                    case 'w': Console.ForegroundColor = ConsoleColor.Blue; break;
                    case 'm': Console.ForegroundColor = ConsoleColor.Gray; break;
                    case 't': Console.ForegroundColor = ConsoleColor.Green; break;
                    case 'b': Console.ForegroundColor = ConsoleColor.DarkGray; break;
                    case '|': Console.ForegroundColor = ConsoleColor.Yellow; break;
                    case '-': Console.ForegroundColor = ConsoleColor.Yellow; break;
                    case '+': Console.ForegroundColor = ConsoleColor.Yellow; break;
                }

                Console.Write(originalMapChar);
                Console.ResetColor();
            }
            Console.SetCursorPosition(p1_x_pos, p1_y_pos);
        }
        static void DrawP()
        {
           Console.SetCursorPosition(p1_x_pos, p1_y_pos);
          
            Console.ForegroundColor = spriteColors[0];
            Console.Write("&");

            Console.ResetColor();

        }

        //m9

        //static void DrawE(enemy)
        static void DrawE()
        {
            if (EnemySpawn)
            {
                EnemySpawn = false;
                enemy_x_pos = EnemyStartSpawn.Next(enemy_min_max_x.Item1, enemy_min_max_x.Item2 + 1);
                enemy_y_pos = EnemyStartSpawn.Next(enemy_min_max_y.Item1, enemy_min_max_y.Item2 + 1);
                enemyLoc = (enemy_x_pos, enemy_y_pos);
                Console.SetCursorPosition(enemy_x_pos, enemy_y_pos);
                Console.ForegroundColor = spriteColors[1];
                Console.Write("#");
            }
            Console.ResetColor();
        }
        //m10
        static void DrawH()
        {
            if (healthTreasure)
            {
                healthTreasure = false;
                treasure_x_pos = healthPackSpawn.Next(treasure_min_max_x.Item1, treasure_min_max_x.Item2 + 1);
                treasure_y_pos = healthPackSpawn.Next(treasure_min_max_y.Item1, treasure_min_max_y.Item2 + 1);
                HealthUp = (treasure_x_pos, treasure_y_pos);
                Console.SetCursorPosition(treasure_x_pos, treasure_y_pos);
                Console.ForegroundColor = spriteColors[2];
                Console.Write("$");
            }
            Console.ResetColor();
        }

        //m12
        static void EnemyHealth()
        {
            if (eHealth == 100)
            {
                ehStat = "Looks Healthy";
            }
            else if (eHealth <= 75)
            {
                ehStat = "looks Hurt";
            }
            else if (eHealth <= 50)
            {
                ehStat = "looks Bloodied ";
            }
            else if (eHealth <= 25)
            {
                ehStat = "Looks Injured ";
            }
            else if (eHealth <= 10)
            {
                ehStat = "Looks Mortally wounded ";
            }
            else if (eHealth <= 0)
            {
                ehStat = " Um...Sleeping... ";
            }
        }
        static void PlayerHealth()
        {
            if (health == 100)
            {
                phStat = "Looks Healthy";
            }
            else if (health <= 75)
            {
                phStat = "looks Hurt";
            }
            else if (health <= 50)
            {
                phStat = "looks Bloodied ";
            }
            else if (health <= 25)
            {
                phStat = "Looks Injured ";
            }
            else if (health <= 10)
            {
                phStat = "Looks Mortally wounded ";
            }
            else if (health <= 0)
            {
                phStat = " Um...Sleeping... ";
            }
        }
        ////m13
        static void Heal()
        {
           health = 50;  // value for testing

            if (player1_positionPROXY == HealthUp)
            {
                int regenHealth = randomHealth.Next(15, 75);

                hp = regenHealth; //randomizes exp
               // health += hp;

                if (health < 100)
                {
                    health = health + hp;
                }
                else 
                {
                    health = 100;
                }
            }
            Console.ForegroundColor = ConsoleColor.Green;
               
            Console.SetCursorPosition(output_X, output_Y + 14);
            string healing =
"+========= Healing output ============+";
            Console.WriteLine($" {healing}\n");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.SetCursorPosition(output_X + 2, output_Y + 15);
            Console.Write(" you picked up a potion soaked Bandage you \n");

            Console.SetCursorPosition(output_X + 2, output_Y + 16);
            Console.Write("are healed for: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write( hp);
        }
        ////m14
        static void TakeDamage()
        {
            EnemyHealth();
            Console.ForegroundColor = ConsoleColor.Red;

            if (player1_positionPROXY == enemyLoc)
                {
                    Random randomEnDMG = new Random();
                    int enemydmg = randomEnDMG.Next(12, 35);

                    Random randomDMG = new Random();
                    int dmg = randomDMG.Next(12, 35);

                    Console.SetCursorPosition(output_X, output_Y + 20);
                    string Kapow =
        "+========= Damage output ============+";
                    Console.WriteLine($" {Kapow}\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.SetCursorPosition(output_X + 2, output_Y + 21);
                    Console.Write("Player Takes ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.SetCursorPosition(output_X + 22, output_Y + 21);
                    Console.WriteLine(enemydmg);
                    
                    health= health-enemydmg;
                    
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.SetCursorPosition(output_X + 2, output_Y + 22);
                    Console.Write("Enemy takes: ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.SetCursorPosition(output_X + 22, output_Y + 22);
                    Console.WriteLine(dmg);
                    
                    eHealth = eHealth-dmg;

                    Console.SetCursorPosition(output_X + 2, output_Y + 23);
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write("Enemy Looks: ");
                    Console.SetCursorPosition(output_X + 16, output_Y + 23);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($" {ehStat}");
                }

            if (eHealth >= 0)
                IncreaseXP(xp);
                Console.SetCursorPosition(output_X + 2, output_Y + 25);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("You made enemy go night night...better not disturb them ");
                Console.SetCursorPosition(output_X + 2, output_Y + 26);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("You gain ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"{xp} ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("XP ");
                       
            //inCombat = false;

            if (health >= 0)
            {
                TakeDamage();
            }

            else if (health <= 0)
            {

                Console.SetCursorPosition(output_X + 2, output_Y + 26);
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"You have died, Game over. ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.ReadKey();
                Console.Clear();
                inCombat = false;
            }
        }
        ////m17
        static void IncreaseXP(int exp) // evil witchcraft
        {
            // Random randomXP = new Random();
            int giveXP = randomXP.Next(15, 30);

            exp = giveXP; //randomizes exp
            xp += exp; //modifies xp to be  xp + exp

            if (xp >= (level * 100)) // defines level of xp where level will increase
            {
                level++; //increases level by 1

            }
        }
    }
}

