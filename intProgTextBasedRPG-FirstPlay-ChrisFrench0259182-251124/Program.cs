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
       
           
         







        static string filepath = "C:\\introGameUnityProjectsCFrench\\intProgTextBasedRPG-FirstPlay-ChrisFrench0259182-251124\\intProgTextBasedRPG-FirstPlay-ChrisFrench0259182-251124\\maps.txt";

      

        static void Main(string[] args)
        {


            DrawMap();
            mapLegend();







            Console.ReadKey();
        }
        //m1
        static void DrawMap()
        {
            try
            {

                string[] Maps = File.ReadAllLines(filepath);

                foreach (string map in Maps)
                {


                    
                    switch (map)
                    {

                        case "g": // Grass
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            break;
                        case "w": // Water
                            Console.ForegroundColor = ConsoleColor.Blue;
                            break;
                        case "m": // Mountain
                            Console.ForegroundColor = ConsoleColor.Gray;
                            break;
                        case "t": // Trees
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;


                    }
                  




                    Console.WriteLine(map);


                }
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


       
        //m3

        static void mapLegend()
        {

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("g");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Grass");

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("w");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Water");

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("m");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Mountain");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("t");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Trees");

            Console.ResetColor();
        }

    }
}

