using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
        static bool healthTreasure = true;
        static bool EnemySpawn = true;
        static bool inCombat = true;

        //static Random randomShield = new Random();
       static Random randomHealth = new Random();
        static Random randomDMG = new Random();
        //static Random randomEnDMG = new Random();
        //static Random randomXP = new Random();
        static Random healthPackSpawn = new Random();
        static Random EnemyStartSpawn = new Random();

        static string character;
        static int p1_x_input;
        static int p1_y_input;
        static int p1_x_pos;
        static int p1_y_pos;
        static (int, int) p1_min_max_x = (1, 55);
        static (int, int) p1_min_max_y = (0, 27);
        static int p1_Old_X = p1_x_pos;
        static int p1_Old_Y = p1_y_pos;
        //static string[] Maps = File.ReadAllLines(filepath);

      
        static int output_X= 61;
        static int output_Y= 1;
        static (int, int) output_min_max_x = (60, 85);
        static (int, int) output_min_max_y = (0, 27);
      





        static int enemy_x_pos;
        static int enemy_y_pos;
        static (int, int) enemy_min_max_x = (15, 35);
        static (int, int) enemy_min_max_y = (12, 27);
        static string ehStat;
        static int eHealth = 100;
        static int enemydmg = randomDMG.Next(12, 35);
        static (int, int) enemyLoc = (enemy_x_pos, enemy_y_pos);

        static char mapChar;
        static string filepath = "maps.txt";
        //static char filepath = "maps.txt";

        //static int p1_KillScore = 0;
        //static int health = 100;
        static int score = 0;
        static int dmg = 0;
        static int hurt = 0;

        static (int, int) HealthUp = (treasure_x_pos, treasure_y_pos);

        static int treasure_x_pos;
        static int treasure_y_pos;
        static (int, int) treasure_min_max_x = (9, 45);
        static (int, int) treasure_min_max_y = (7, 20);
        static int healing = randomHealth.Next(15, 75);
        
     


        static ConsoleColor[] spriteColors = { ConsoleColor.Cyan, ConsoleColor.Red, ConsoleColor.Magenta };
        static char[] allKeybindings = (new char[] { 'W', 'A', 'S', 'D' });


        static (int, int) player1_positionPROXY;

        static int health = 100;
        //static int minhealth = 0;
        //static int maxhealth = 100;

        //static int clampedhealth = health < minhealth ? minhealth : (health > maxhealth ? maxhealth : health);
        ////static string healthStatus;
        //static int shield = 100;
        //static int minshield = 0;
        //static int maxshield = 100;

        //static int clampedshield = shield < minshield ? minshield : (shield > maxshield ? maxshield : shield);
        //static int lives = 3;
        //static int minlives = 0;
        //static int maxlives = 99;

        //static int clampedlives = lives < minlives ? minlives : (lives > maxlives ? maxlives : lives);
        //static string Character;
        static int xp = 0;
        static int level = 0;

      
        //static int giveXP;
        //static int regenShield;
        //static int regenHealth;
        //static class RangedInt;








        static void Main(string[] args)
        {
            alias();
            Console.Clear();
            Console.CursorVisible = false;

            //DrawMap();
            ////mapLegend(); 
            //p1_x_pos = p1_min_max_x.Item1;
            //p1_y_pos = p1_min_max_y.Item1;


              DrawMap();
            //mapLegend();
            //hud();

            while (isPlaying)
            {
                // Console.Clear();



                ProcessInput();

                GameUpdate();

                DrawP();
                DrawE();
                DrawH();
                mapLegend();
                hud();
                DeBug();
            }

            
           
           // DeBug();
          
            //while (inCombat)
            //{

            //}


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




        //m2

        static void mapLegend()
        {

            //Console.Clear();
            Console.SetCursorPosition(output_X, output_Y + 10);
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

        static void ProcessInput()
        {


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

            player1_positionPROXY = (p1_x_pos, p1_y_pos);
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

            //Console.Clear();
            Console.SetCursorPosition(output_X, output_Y + 6);
            string HUD =
            "+========= HUD ============+";
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($" {HUD}\n");

                     
            Console.SetCursorPosition(output_X + 2, output_Y + 7);
            
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("Name:");
            Console.SetCursorPosition(output_X + 8, output_Y + 7);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"{character}");

            Console.SetCursorPosition(output_X + 2, output_Y + 8);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("{0,0}{1,9}{2,9}{3,9}{4,11}{5,9}{6,16}", "XP", "Level", "Score", "Life", "Attack", "Hurt", "Enemy Looks");
            Console.SetCursorPosition(output_X + 2, output_Y + 9);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("{0,1}{1,8}{2,8}{3,11}{4,9}{5,10}{6,21}", xp, level, score, health, dmg, hurt, ehStat + "\n");
            Console.ResetColor();



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


            //    Console.ForegroundColor = ConsoleColor.White;
            //    Console.Write("Player 1 position:");
            //    Console.ForegroundColor = spriteColors[0];
            //    Console.WriteLine(player1_positionPROXY);
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
        //static void Draw(player)
        static void DrawP()
        {
            //Console.Clear();
            //DrawMap();
            //Console.SetCursorPosition(p1_Old_X, p1_Old_Y);
            //Console.Write(mapChar);
            Console.SetCursorPosition(p1_x_pos, p1_y_pos);
            Console.ForegroundColor = spriteColors[0];
            Console.Write("&");

            Console.ResetColor();


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


            Console.ResetColor();
        }
        //m11

        //static void healthpack()
        //{
        //    if (player1_positionPROXY == HealthUp) 
                                                           
        //    {
        //        health = health + healing;
        //        healthTreasure = true;
        //    }
        //}

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


        ////m13
        //static void Heal(int hp)
        //{
      
        //    int regenHealth = randomHealth.Next(15, 40);

        //    hp = regenHealth; //randomizes exp
        //    health += hp;

        //    if (health < 100)
        //    {
        //        health = health + hp;
        //    }
        //}
        ////m14
        //static void RegenerateShield(int hp)
        //{
       
        //    int regenShield = randomShield.Next(15, 40);

        //    hp = regenShield; //randomizes exp
        //    shield += hp;

        //    if (shield < 100)
        //    {
        //        shield = shield + hp;
        //    }

        //}

        ////m15
        //static void Revive()
        //{

        //    if (lives > 0)
        //    {
        //        Console.ForegroundColor = ConsoleColor.Magenta;
        //        Console.WriteLine($"You have died, applying resurection.");
        //        Console.ForegroundColor = ConsoleColor.DarkYellow;


        //        shield = 100;
        //        health = 100;
        //        lives--;
        //    }


        //}

        ////m16
        //static void TakeDamage(int damage)
        //{
        //    //Random randomDMG = new Random();
        //    int enemydmg = randomDMG.Next(12, 35);
        //    Console.WriteLine();
        //    Console.Write($"You take ");
        //    Console.ForegroundColor = ConsoleColor.Blue;
        //    Console.Write(enemydmg);
        //    Console.ForegroundColor = ConsoleColor.DarkYellow;
        //    Console.WriteLine(" damage...");

        //    int remainingDamage = enemydmg; //sets up remaining spillover damage
        //                                    // int remainingDamage2 = remainingDamage;   // sets up remaining slillover hp damage foer disreggard
        //                                    // Damage the shield first
        //    if (shield > 0)
        //    {
        //        int damageToShield = Math.Min(shield, remainingDamage);
        //        shield -= damageToShield;
        //        remainingDamage -= damageToShield;
        //    }

        //    if (remainingDamage > 0)

        //        if (remainingDamage > 0)
        //        {
        //            if (health >= 0)
        //            {
        //               
        //                health -= remainingDamage;
        //            }

        //            else if (health <= 0 && lives > 0)
        //            {

        //                lives--;

        //                // Console.ForegroundColor = ConsoleColor.Magenta;
        //                // Console.WriteLine($"You have died, applying resurection.");
        //                // Console.ForegroundColor = ConsoleColor.DarkYellow;
        //                Revive();
        //            }

        //            else
        //            {
        //                Console.ForegroundColor = ConsoleColor.Magenta;
        //                Console.WriteLine($"You have died, Game over. \n\n Your ending stats:.....\n");
        //                Console.ForegroundColor = ConsoleColor.DarkYellow;
        //                Console.ReadKey();
        //                Console.Clear();

        //            }
        //        }
        //}

        ////m17

        //static void IncreaseXP(int exp) // evil witchcraft

        //{
        //    // Random randomXP = new Random();
        //    int giveXP = randomXP.Next(15, 30);

        //    exp = giveXP; //randomizes exp
        //    xp += exp; //modifies xp to be  xp + exp

        //    if (xp >= (level * 100)) // defines level of xp where level will increase
        //    {
        //        level++; //increases level by 1

        //    }
        //}



    }

}

