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




        static string filepath = "maps.txt";




        static void Main(string[] args)
        {


            DrawMap();







            Console.ReadKey();
        }

        static void DrawMap()
        {
            try
            {

                string[] Maps = File.ReadAllLines(filepath);

                foreach (string map in Maps)
                {
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


    }
}

