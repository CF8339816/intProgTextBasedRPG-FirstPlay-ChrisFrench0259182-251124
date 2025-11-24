using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace intProgTextBasedRPG_FirstPlay_ChrisFrench0259182_251124
{
    //coded by Chris French
    //using minimal references, i did have to look up some functions as i had no
    //frame of refernce for some of the processes such as setting the enemy to
    //move in real time using a timer since the game is not turn based..
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
        static int p1dmg;
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
                // runs each time Health= is called to verify min max ranges
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
        static int eHealth = 75;
        static int minEhealth = 0;
        static int maxEhealth = 75;
        #region//"clamping" ehealth
        static int ehealth
        {
            get { return eHealth; }
            set
            {
                // runs each time eHealth= is called to verify min max ranges
                if (value < minEhealth)
                    eHealth = minEhealth;
                else if (value > maxEhealth)
                    eHealth = maxEhealth;
                else
                    eHealth = value;
            }
        }
        #endregion
        static int enemydmg;
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

        static double lastEnemyMoveTime;


        static void Main(string[] args)
        {
            #region//request maxamizing console
            Directory.GetCurrentDirectory();
            Console.WriteLine("Please maximize the window before proceeding to avoid any issues, \n then press any key to continue... ");
            Console.ReadKey();
            Console.Clear();
            #endregion

            int enemyMoveSpeedMs = 250; 
            DateTime lastEnemyMoveTime = DateTime.Now;

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
                inCombat = false;
                CanMoveTo(mapXs, mapYs);
               DrawE();

                if ((DateTime.Now - lastEnemyMoveTime).TotalMilliseconds >= enemyMoveSpeedMs)
                {
                    if (!inCombat)
                    {
                        EraseEnemy();
                        MoveEnemy();
                        lastEnemyMoveTime = DateTime.Now; // Reset the timer
                    }
                }
                ProcessInput();
                GameUpdate();
                ErasePlayer();
                DrawP();
                ChkWinCond();

                // MoveEnemy();
               
                DrawH();
                hud();
                DeBug();

                damageDealt();
                damageTaken();

            }
            //#region //tried a while incombat. broke the game
            //while (inCombat)
            //{

            //    ProcessInput();
            //    ChkWinCond();
            //    GameUpdate();
            //    ErasePlayer();
            //    DrawP();
            //    DrawE();
            //    DrawH();
            //    hud();
            //    DeBug();

            //    //    TakeDamage();
            //    damageDealt();
            //damageTaken();
            //}
            //#endregion


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
            
            Console.Write(mapTile);
        }
        //m3
        static void mapLegend()
        {
            Console.SetCursorPosition(output_X, output_Y + 11);
            string MapLegend =
            "+========= Map Legend ============+";
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($" {MapLegend}\n");

            Console.SetCursorPosition(output_X + 2, output_Y + 12);
            
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("g = Grass  ");
            
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("w = Water  ");
           
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("m = Mountain  ");
            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("t = Trees  ");
           
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("b = Base  ");
           
            Console.ResetColor();
        }
        //m4
        static bool CanMoveTo(int tarMapX, int tarMapY)
        {
            if (tarMapY >= 0 && tarMapY < Maps.Length && tarMapX >= 0 && tarMapX < Maps[tarMapY].Length)
            {
                char tarTile = Maps[tarMapY][tarMapX];

                switch (tarTile)
                {
                    case 'm': //defines un traversable
                    case 'w':
                   
                        return false;

                    case 'g':  //verifies traversable
                    case 't':
                    case 'b':
                    case 'P':
                        return true;

                    default:
                        return true; 
                }
            }
           return false;
        }
        //m5
        static void ProcessInput()
        {
            p1_x_input = 0;
            p1_y_input = 0;

            ConsoleKey input = Console.ReadKey(true).Key;

            if (input == ConsoleKey.A) p1_x_input = -1;
            if (input == ConsoleKey.D) p1_x_input = 1;
            if (input == ConsoleKey.W) p1_y_input = -1;
            if (input == ConsoleKey.S) p1_y_input = 1;

            if (input == ConsoleKey.Q) isPlaying = false; //Quit the 'is playing' loop

            int nextX = p1_x_pos + p1_x_input;
            int nextY = p1_y_pos + p1_y_input;

            if (CanMoveTo(nextX, nextY))
            {
                p1_Old_X = p1_x_pos;
                p1_Old_Y = p1_y_pos;
            }
            else
            {
                p1_x_input = 0;
                p1_y_input = 0;
            }

        }
        //m6
        static void GameUpdate()
        {

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
            DrawEnemyAtCurrentPos();
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

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"{character}");
            Console.SetCursorPosition(output_X + 2, output_Y + 10);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($" feels: ");
            Console.SetCursorPosition(output_X + 10, output_Y + 10);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write($"                                                       ");
            Console.SetCursorPosition(output_X + 10, output_Y + 10);
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
            Console.Write("Health Pickup:");

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
                Console.SetCursorPosition(enemy_x_pos, enemy_y_pos);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write('#');
                Console.SetCursorPosition(0, 0);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write('+');
                Console.SetCursorPosition(0, 1);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write('|');
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
            enemy_Old_X = enemy_x_pos;
            enemy_Old_Y = enemy_y_pos;

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
                bool clearESpawn = false;
            
                while (!clearESpawn)
                {
                    enemy_x_pos = EnemyStartSpawn.Next(enemy_min_max_x.Item1, enemy_min_max_x.Item2 + 1);
                    enemy_y_pos = EnemyStartSpawn.Next(enemy_min_max_y.Item1, enemy_min_max_y.Item2 + 1);

                    if (CanMoveTo(enemy_x_pos, enemy_y_pos))
                    {
                        
                        if (enemy_x_pos != p1_x_pos || enemy_y_pos != p1_y_pos) //checks for player
                        {
                            clearESpawn = true;
                        }
                    }
                }
                enemyLoc = (enemy_x_pos, enemy_y_pos);

                Console.SetCursorPosition(enemy_x_pos, enemy_y_pos);

                Console.ForegroundColor = spriteColors[1];

                Console.Write("#");
                eHealth = 75;
                Console.ResetColor();

                EnemySpawn = false;
            }
            Console.ResetColor();
        }
        //m14
        static void DrawH()
        {
            if (healthTreasure)
            {
                bool clearHSpawn = false;

                while (!clearHSpawn)
                {
                    treasure_x_pos = healthPackSpawn.Next(treasure_min_max_x.Item1, treasure_min_max_x.Item2 + 1);
                    treasure_y_pos = healthPackSpawn.Next(treasure_min_max_y.Item1, treasure_min_max_y.Item2 + 1);

                    if (CanMoveTo(treasure_x_pos, treasure_y_pos))
                    {
                       
                        if (treasure_x_pos != p1_x_pos || treasure_y_pos != p1_y_pos) //checks for player
                        {
                            clearHSpawn = true;
                        }
                    }
                }

                HealthUp = (treasure_x_pos, treasure_y_pos);

                Console.SetCursorPosition(treasure_x_pos, treasure_y_pos);

                Console.ForegroundColor = spriteColors[2];

                Console.Write("$");
                
                Console.ResetColor();

                healthTreasure = false;

            }
            Console.ResetColor();
        }
        //m15a
        static void MoveEnemy()
        {
            int posDifX = p1_x_pos - enemy_x_pos; //checks the horizontal dist 
            int posDifY = p1_y_pos - enemy_y_pos; // checks the vert dist 

            int moveX = 0;
            int moveY = 0;

            if (Math.Abs(posDifX) > Math.Abs(posDifY))
            {
               moveX = Math.Sign(posDifX); // prioritize horizontal returns 1, -1, or 0
            }
            else
            {
               moveY = Math.Sign(posDifY); //  prioritize verticle returns 1, -1, or 0
            }
            int nextEnemyX = enemy_x_pos + moveX;
            int nextEnemyY = enemy_y_pos + moveY;

            if (CanMoveTo(nextEnemyX, nextEnemyY))
            {
                EraseEnemy();
                enemy_x_pos = nextEnemyX;
                enemy_y_pos = nextEnemyY;
                enemyLoc = (enemy_x_pos, enemy_y_pos); 
            }
            else
            {
                moveX = 0;
                moveY = 0;

                if (Math.Abs(posDifY) > Math.Abs(posDifX)) // Try Y first this time
                {
                    moveY = Math.Sign(posDifY);
                }
                else
                {
                    moveX = Math.Sign(posDifX);
                }

                nextEnemyX = enemy_x_pos + moveX;
                nextEnemyY = enemy_y_pos + moveY;

                if (CanMoveTo(nextEnemyX, nextEnemyY))
                {
                    EraseEnemy();
                    enemy_x_pos = nextEnemyX;
                    enemy_y_pos = nextEnemyY;
                    enemyLoc = (enemy_x_pos, enemy_y_pos);
                }
            }
        }
        //m15b
        static void DrawEnemyAtCurrentPos()
        {
            if (!inCombat) 
            {
                Console.SetCursorPosition(enemy_x_pos, enemy_y_pos);
                Console.ForegroundColor = spriteColors[1]; 
                Console.Write("#");
                Console.ResetColor();
            }
        }
               
        //m16
        static void EnemyHealth()
        {
          
            if (eHealth <= 75) ehStat = "Looks Healthy";
            if (eHealth <= 50) ehStat = "looks Hurt";
            if (eHealth <= 25) ehStat = "looks Bloodied ";
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
                if (Health < 100)
                
                {
                    health = health + hp;// adjusts capped health variable to output to main Health variable
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
                //inCombat = true;
            {
                p1dmg = randomDMG.Next(17, 45);
                enemydmg = randomDMG.Next(12, 30);

                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(output_X + 2, output_Y + 21);
                Console.Write("Player Takes ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(output_X + 22, output_Y + 21);
                Console.WriteLine(enemydmg);
                hurt = enemydmg;
                health = Health - enemydmg;

                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(output_X + 2, output_Y + 22);
                Console.Write("Enemy takes: ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(output_X + 22, output_Y + 22);
                Console.WriteLine(p1dmg);
                myDmg = p1dmg;
                ehealth = eHealth - p1dmg;

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
            if (ehealth == 0)
            {
                //inCombat = false;
                kills += 1;
                IncreaseXP(xp);
                EraseEnemy();
                DrawE();
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
                inCombat = false;
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
            int mapY = p1_y_pos;
            int mapX = p1_x_pos;

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

