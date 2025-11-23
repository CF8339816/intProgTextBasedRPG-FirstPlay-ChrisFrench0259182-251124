using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace intProgTextBasedRPG_FirstPlay_ChrisFrench0259182_251124
{
    //code ed by Chris French
    //using minimal references, i did have to look up some functions as i had no frame of refernce for some of the processes.
    internal class Program
    {
        #region//bools
        static bool isPlaying = true;
        static bool healthTreasure = true;
        static bool EnemySpawn = true;
        static bool inCombat = true;
        #endregion

        #region//randoms
        static Random randomHealth = new Random();
        static Random randomDMG = new Random();
        static Random randomEnDMG = new Random();
        static Random randomXP = new Random();
        static Random healthPackSpawn = new Random();
        static Random EnemyStartSpawn = new Random();
        #endregion

        #region //player
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
        static string player;
        static (int, int) player1_positionPROXY;
        static (int, int) player1_positionOLD;
        private static int Health = 100;
        static int minhealth = 0;
        static int maxhealth = 100;
        #region//"clamping" health
        static int health
        {
            get { return Health; }
            set
            {
                // This logic runs every time we do 'Health = X;'
                if (value < minhealth)
                    Health = minhealth;
                else if (value > maxhealth)
                    Health = maxhealth;
                else
                    Health = value;
            }
        }
        #endregion
        static int hp;
        #endregion

        #region // output field
        static int output_X = 61;
        static int output_Y = 1;
        static (int, int) output_min_max_x = (60, 100);
        static (int, int) output_min_max_y = (1, 29);
        #endregion

        #region//enemy
        static string enemy;
        static int enemy_x_pos;
        static int enemy_y_pos;
        static int enemy_Old_X;
        static int enemy_Old_Y;
        static (int, int) enemy_min_max_x = (15, 35);
        static (int, int) enemy_min_max_y = (12, 29);
        static string ehStat;
        static int eHealth = 100;
        static int enemydmg = randomDMG.Next(12, 35);
        static (int, int) enemyLoc = (enemy_x_pos, enemy_y_pos);
        #endregion

        #region//map
        static char mapTile;
        static string filepath = "maps.txt";
        static string[] Maps;
        static string map;
        static int mapXs;
        static int mapYs;
        #endregion

        #region //hud
        static int kills = 0;
        static int score = 0;
      
        static int hurt = 0;
      
        static int myDmg = 0;
        #endregion

        #region //healing pickups
        static (int, int) HealthUp = (treasure_x_pos, treasure_y_pos);
        static int treasure_x_pos;
        static int treasure_y_pos;
        static (int, int) treasure_min_max_x = (9, 45);
        static (int, int) treasure_min_max_y = (7, 20);
        static int healing = randomHealth.Next(15, 75);
        #endregion

        static ConsoleColor[] spriteColors = { ConsoleColor.Cyan, ConsoleColor.Red, ConsoleColor.Magenta };
        static char[] allKeybindings = (new char[] { 'W', 'A', 'S', 'D' });

        #region// xp and level 
        static int xp = 0;
        static int level = 0;
        static int giveXP = randomXP.Next(15, 30);
      
        #endregion




        static void Main(string[] args)
        {
            #region//request maxamizing console
            Directory.GetCurrentDirectory();
            Console.WriteLine("Please maximize the window before proceeding to avoid any issues, \n then press any key to continue... ");
            Console.ReadKey();
            Console.Clear();
            #endregion

            #region // get name  of player
            Console.SetCursorPosition(0, 0);
            alias();
            Console.Clear();
            Console.CursorVisible = false;
            #endregion


            DrawMap();
            Console.WriteLine("press any key to start game...\nPress 'Q' to exit...\nUse W,A,S,D to move around the map...\nGet to the base to be safe...\nPick up '$' supply caches to heal and repair...\nFight enemies '#' by manouvering to them or try to avoid them... ");

            mapLegend();

            while (isPlaying)
            {
                ProcessInput();
                GameUpdate();
                ErasePlayer();
                DrawP();
                ChkWinCond();
                EraseEnemy();
                //MoveEnemy();
                DrawE();
                DrawH();
                hud();
                DeBug();
                TakeDamage();
                damageDealt();
                damageTaken();

                #region //tried a while incombat. broke the game
                //while (inCombat)
                //{
                //    //ProcessInput();

                //    //GameUpdate();

                //    //TakeDamage();
                //}
                #endregion
            }


        }
        //m1
        static void DrawMap()
        {
            Directory.GetCurrentDirectory();
            try
            {
                 Maps = File.ReadAllLines(filepath);

                foreach (string map in Maps)
                {
                    //foreach (char mapChar in map)
                    foreach (char mapTile in map)
                    {
                        // switch (mapChar)
                        switch (mapTile)
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
                        Console.Write(mapTile);
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
        static void MapChar()
        {
            switch (mapTile)
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
            Console.Write(mapTile);
        }
        //m3
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
        //m4
        static bool CanMoveTo(int x, int y)
        {
            int mapYs = y - 1;
            int mapXs = x - 1;

            if (Maps == null || mapYs < 0 || mapYs >= Maps.Length || mapXs < 0 || mapXs >= Maps[mapYs].Length)
            {
                return false;
            }

            char mapTile = Maps[mapYs][mapXs];

            if (mapTile == 'm' || mapTile == 'w')
            {
                return false;
            }

            #region//tried to adjust the processing time for the keystrokes to provide illusion ofdifferent terraine speeds  
            //if (mapTile == 't' )
            //{

            //    return true;
            //    Thread.Sleep(2000);

            //}


            //if (mapTile == 'g' )
            //{

            //    return true;
            //    Thread.Sleep(1000);
            //}
            #endregion

            return true;
        }
        //m5
        static void ProcessInput()
        {
            // player1_positionOLD = player1_positionPROXY;

            CanMoveTo(mapXs, mapYs);

            p1_Old_X = p1_x_pos;
            p1_Old_Y = p1_y_pos;

            p1_x_input = 0;
            p1_y_input = 0;

            ConsoleKey input = Console.ReadKey(true).Key;

            if (input == ConsoleKey.A) p1_x_input = -1;
            if (input == ConsoleKey.D) p1_x_input = 1;
            if (input == ConsoleKey.W) p1_y_input = -1;
            if (input == ConsoleKey.S) p1_y_input = 1;

            if (input == ConsoleKey.Q) isPlaying = false; //Quit the 'is playing' loop
        }
        //m6
        static void GameUpdate()
        {
            CanMoveTo(mapXs, mapYs);

            p1_x_pos += p1_x_input;
            p1_y_pos += p1_y_input;

            p1_x_pos = Math.Max(p1_min_max_x.Item1, Math.Min(p1_x_pos, p1_min_max_x.Item2));
            p1_y_pos = Math.Max(p1_min_max_y.Item1, Math.Min(p1_y_pos, p1_min_max_y.Item2));

            player1_positionPROXY = (p1_x_pos, p1_y_pos);

            if (EnemySpawn && !inCombat)
            {
                MoveEnemy();
            }

            if (player1_positionPROXY == HealthUp)
            {
                Heal();
                healthTreasure = true;
            }

            if (player1_positionPROXY == enemyLoc)
            {
                inCombat = true;

                if (eHealth <= 0)
                {
                    inCombat = false;
                    EnemySpawn = true;
                }
            }
            ChkWinCond();
        }
        //m7
        static void alias()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("What is your character's name");
            Console.ForegroundColor = ConsoleColor.Blue;
            character = Console.ReadLine();
        }
        //m8
        static void hud()
        {
            PlayerHealth();

            // Console.Clear();
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
            Console.WriteLine("{0,0}{1,9}{2,9}{3,9}{4,11}{5,9}{6,9}", "XP", "Level", "Kills", "Heals", "Life", "Attack", "Hurt");
            Console.SetCursorPosition(output_X + 2, output_Y + 8);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("                                                                                                           ");
            Console.SetCursorPosition(output_X + 2, output_Y + 8);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("{0,1}{1,8}{2,8}{3,11}{4,9}{5,10}{6,10}", xp, level, kills, score, health, myDmg, hurt + "\n");
            Console.ResetColor();

            Console.SetCursorPosition(output_X + 2, output_Y + 9);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"{character} feels: ");
            Console.SetCursorPosition(output_X + 16, output_Y + 9);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine($"                                                       ");
            Console.SetCursorPosition(output_X + 16, output_Y + 9);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"{phStat}");


            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(output_X, output_Y + 14);
            string healing =
"+========= Healing output ============+";
            Console.WriteLine($" {healing}\n");


            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(output_X, output_Y + 20);
            string Kapow =
"+========= Damage output ============+";
            Console.WriteLine($" {Kapow}\n");
            //Console.ReadKey(true);
        }
        //m9
        static void DeBug()
        {
            //Console.Clear();
            Console.SetCursorPosition(output_X, output_Y);
            string DebugBox =

"+========= debug block ============+";
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($" {DebugBox}\n");

            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(output_X + 2, output_Y + 1);
            Console.Write("Player 1 Pickup:");

            Console.SetCursorPosition(output_X + 22, output_Y + 1);
            Console.WriteLine(HealthUp);

            Console.SetCursorPosition(output_X + 2, output_Y + 2);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Player 1 position:");

            Console.SetCursorPosition(output_X + 22, output_Y + 2);
            Console.WriteLine(player1_positionPROXY);

            Console.SetCursorPosition(output_X + 2, output_Y + 3);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Enemy position:");

            Console.SetCursorPosition(output_X + 22, output_Y + 3);
            Console.WriteLine(enemyLoc);

            Console.SetCursorPosition(output_X + 2, output_Y + 4);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("enemy health:");
            Console.SetCursorPosition(output_X + 22, output_Y + 4);
            Console.WriteLine("              ");
            Console.SetCursorPosition(output_X + 22, output_Y + 4);
            Console.WriteLine(eHealth);

            Console.SetCursorPosition(output_X + 26, output_Y + 4);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("player health:");

            Console.SetCursorPosition(output_X + 46, output_Y + 4);
            Console.WriteLine("                   ");
            Console.SetCursorPosition(output_X + 46, output_Y + 4);
            Console.WriteLine(Health);
        }
        //m10
        static void ErasePlayer()
        {
            int mapX = p1_Old_X;
            int mapY = p1_Old_Y;

            if (Maps != null && mapY >= 0 && mapY < Maps.Length && mapX >= 0 && mapX < Maps[mapY].Length)
            {
                char mapTile = Maps[mapY][mapX];

                Console.SetCursorPosition(p1_Old_X, p1_Old_Y);

                switch (mapTile)
                {
                    case 'g': Console.ForegroundColor = ConsoleColor.DarkYellow; break;
                    case 'w': Console.ForegroundColor = ConsoleColor.Blue; break;
                    case 'm': Console.ForegroundColor = ConsoleColor.Gray; break;
                    case 't': Console.ForegroundColor = ConsoleColor.Green; break;
                    case 'b': Console.ForegroundColor = ConsoleColor.DarkGray; break;

                }

                Console.Write(mapTile);
                Console.ResetColor();
            }
            Console.SetCursorPosition(p1_x_pos, p1_y_pos);
        }
        //m11
        static void DrawP()
        {
         
            Console.SetCursorPosition(p1_x_pos, p1_y_pos);

            Console.ForegroundColor = spriteColors[0];
            Console.Write("&");

            Console.ResetColor();

        }
        //m12
        static void EraseEnemy()
        {
            int mapX = enemy_Old_X;
            int mapY = enemy_Old_Y;

            if (Maps != null && mapY >= 0 && mapY < Maps.Length && mapX >= 0 && mapX < Maps[mapY].Length)
            {
                char mapTile = Maps[mapY][mapX];

                Console.SetCursorPosition(enemy_Old_X, enemy_Old_Y);

                switch (mapTile)
                {
                    case 'g': Console.ForegroundColor = ConsoleColor.DarkYellow; break;
                    case 'w': Console.ForegroundColor = ConsoleColor.Blue; break;
                    case 'm': Console.ForegroundColor = ConsoleColor.Gray; break;
                    case 't': Console.ForegroundColor = ConsoleColor.Green; break;
                    case 'b': Console.ForegroundColor = ConsoleColor.DarkGray; break;

                }

                Console.Write(mapTile);
                Console.ResetColor();
            }
            Console.SetCursorPosition(enemy_x_pos, enemy_y_pos);
        }
        //m13
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
                eHealth = 100;

            }
            Console.ResetColor();
        }
        //m14
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
        //m15
        static void MoveEnemy()
        {
            Thread.Sleep(1000);
            EraseEnemy();

            int eMove_x = enemy_x_pos;
            int eMove_y = enemy_y_pos;

            int deltaX = p1_x_pos - enemy_x_pos;  //determins which axis hasthe largestgap to prioritize movement
            int deltaY = p1_y_pos - enemy_y_pos;

            bool moved = false;

            if (Math.Abs(deltaX) > Math.Abs(deltaY))
            {
                if (deltaX > 0) eMove_x++;
                else if (deltaX < 0) eMove_x--;

                //if (CanMoveTo(eMove_x, enemy_y_pos))
                //{
                //    enemy_Old_X = enemy_x_pos;

                //    enemy_x_pos = eMove_x;
                //    moved = true;
                //}
            }

            if (!moved)
            {
                eMove_x = enemy_x_pos;
                eMove_y = enemy_y_pos;

                if (deltaY > 0) eMove_y++;
                else if (deltaY < 0) eMove_y--;

                //if (CanMoveTo(enemy_x_pos, eMove_y))
                //{
                //    enemy_Old_Y = enemy_y_pos;

                //    enemy_y_pos = eMove_y;
                //    moved = true;
                //}
            }
            Console.SetCursorPosition(enemy_x_pos, enemy_y_pos);
            Console.ForegroundColor = spriteColors[1];
            Console.Write("#");

            //if (enemy_x_pos == p1_x_pos && enemy_y_pos == p1_y_pos)
            //{
            //    inCombat = true;

            //}
        }
        //m16
        static void EnemyHealth()
        {
            if (eHealth == 100) ehStat = "Looks Healthy";
            if (eHealth <= 75) ehStat = "looks Hurt";
            if (eHealth <= 50) ehStat = "looks Bloodied ";
            if (eHealth <= 25) ehStat = "Looks Injured ";
            if (eHealth <= 10) ehStat = "Looks Mortally wounded ";
            if (eHealth <= 0) ehStat = " Um...Sleeping... ";
        }
        //m17
        static void PlayerHealth()
        {
            if (health == 100) phStat = "Looks Healthy";
            if (health <= 75) phStat = "looks Hurt";
            if (health <= 50) phStat = "looks Bloodied ";
            if (health <= 25) phStat = "Looks Injured ";
            if (health <= 10) phStat = "Looks Mortally wounded ";
            if (health <= 0) phStat = " Um...Sleeping... ";
        }
        //m18
        static void Heal()
        {
            if (player1_positionPROXY == HealthUp)
            {
                int regenHealth = randomHealth.Next(15, 75);
                hp = regenHealth;
                score += 1;
                if (health < 100)
                {
                    health = health + hp;
                }
            }
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.SetCursorPosition(output_X + 2, output_Y + 15);
            Console.Write(" you picked up a potion soaked Bandage you \n");

            Console.SetCursorPosition(output_X + 2, output_Y + 16);
            Console.Write("are healed for: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(hp);
        }
        //m19
        static void TakeDamage()
        {
            EnemyHealth();
            Console.ForegroundColor = ConsoleColor.Red;

            if (player1_positionPROXY == enemyLoc)
            {
                
               

                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(output_X + 2, output_Y + 21);
                Console.Write("Player Takes ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(output_X + 22, output_Y + 21);
                Console.WriteLine(enemydmg);
                hurt = enemydmg;
                health = health - enemydmg;

                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(output_X + 2, output_Y + 22);
                Console.Write("Enemy takes: ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(output_X + 22, output_Y + 22);
                Console.WriteLine(p1dmg);
                myDmg = p1dmg;
                eHealth = eHealth - p1dmg;

                Console.SetCursorPosition(output_X + 2, output_Y + 23);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("Enemy Looks: ");
                Console.SetCursorPosition(output_X + 16, output_Y + 23);
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine($"                                                       ");
                Console.SetCursorPosition(output_X + 16, output_Y + 23);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($" {ehStat}");
            }
          
        }
        //m20
        static void damageDealt()
        {
            if (eHealth <= 0)
            {
                kills += 1;
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
            }
            else
            {
                Console.SetCursorPosition(output_X + 2, output_Y + 25);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("                                                                ");
                Console.SetCursorPosition(output_X + 2, output_Y + 26);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("                      ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"                   ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("                           ");
                TakeDamage();
            }
        }
        //m21        
        static void damageTaken()
        {
            if (health >= 0)
            {
                TakeDamage();
            }

            if (health <= 0)
            {
                GameOver();
                Console.SetCursorPosition(output_X + 2, output_Y + 30);
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"You have died, Game over. ");
                Console.SetCursorPosition(output_X + 2, output_Y + 31);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Your XP = {xp}, your level is {level}... ");
                Console.SetCursorPosition(output_X + 2, output_Y + 32);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"You have 'Sleep-ed' {kills} enemies, and have used {score} potion soaked bandages");
                Console.SetCursorPosition(output_X + 2, output_Y + 34);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"Press 'Q' to exit. ");

                //inCombat = false;
            }
        }
        //m22
        static void IncreaseXP(int exp) 
        {



            exp = giveXP;
            xp += exp;

            if (xp >= (level * 100))
            { 
                level++; 

            }
        }
        //m23
        static void ChkWinCond()
        {
            int mapY = p1_y_pos - 1;
            int mapX = p1_x_pos - 1;

            if (Maps != null && mapY >= 0 && mapY < Maps.Length && mapX >= 0 && mapX < Maps[mapY].Length)
            {
                char mapTile = Maps[mapY][mapX];
                if (mapTile == 'b')
                {
                    WnrWnrChknDnnr();
                }
            }
        }
        //m24
        static void WnrWnrChknDnnr()
        {
            GameOver();

            Console.SetCursorPosition(output_X + 2, output_Y + 30);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"You made it back to base, you won!! ");
            Console.SetCursorPosition(output_X + 2, output_Y + 31);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Your XP = {xp}, your level is {level}... ");
            Console.SetCursorPosition(output_X + 2, output_Y + 32);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"You have 'Sleep-ed' {kills} enemies, and have used {score} potion soaked bandages");
            Console.SetCursorPosition(output_X + 2, output_Y + 34);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"Press 'Q' to exit. ");
        }
        //m25 
        static void GameOver()
        {
            Console.SetCursorPosition(output_X, output_Y + 28);
            string GO =
"+========= Game Over ============+";
            Console.WriteLine($" {GO}\n");
        }
    }
}

