using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;


namespace Lab1_ComputerSystems
{
     class Program
    {
        static List<char> alphabet = new List<char>() { 'а', 'б', 'в', 'г', 'д', 'е', 'є', 'ж', 'з', 'и', 'і', 'ї', 'й', 'к', 'л',
            'м', 'н', 'о', 'п', 'р', 'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ь', 'ю', 'я' };
        static Dictionary<char, float> frequency;
        static int lengthOfFile = 0;
        static void Main(string[] args)
        {
            const string folder = @"D:\University\ComputerSystems\";
            string filePath = folder + GetOpenedFileName(folder);
            FillChars();
            GetProbability(filePath);
            foreach(char item in alphabet)
            {
                Console.WriteLine($"{item} - {frequency[item]}");
            }
            Console.WriteLine($"Ur amount of letters is - {lengthOfFile}");
            Console.WriteLine($"Ur entropy is - {CountEntropy()}");
            Console.WriteLine($"Ur info is - {CountEntropy()*lengthOfFile/8}  bytes");
        }
        public static void FillChars()
        {
            frequency = new Dictionary<char, float>();
            for (int i = 0; i < alphabet.Count; i++)
            {
                frequency.Add(alphabet[i], 0);
            }
        }
        static string GetOpenedFileName(string path)//для зчитування існуючого файлу
        {
            string name = "";
            do
            {
                Console.WriteLine("\nВведіть назву файлу для зчитування:");
                name = Console.ReadLine();
            } while (!File.Exists(path + name + ".txt"));
            return name + ".txt";
        }
        static void GetProbability(string path)
        {
            string text;
            StreamReader dataIn = null;
            try
            {
                dataIn = new StreamReader(path);
                int comp;
                text = dataIn.ReadToEnd(); // put all the text in one string (we know that it is rather small)
                lengthOfFile = text.Count(c => char.IsLetter(c)); ;
                comp = text.Count(c => char.IsLetter(c));
                for (int i = 0; i < text.Length; i++)
                {
                    if (frequency.ContainsKey(Char.ToLower(text[i])))
                    {
                        frequency[Char.ToLower(text[i])] += 1;
                    }
                }
                foreach (char item in alphabet)
                {
                    frequency[item] /= comp;
                }
            }
            finally
            {
                if (dataIn != null) dataIn.Close();
            }
        }
        static double CountEntropy()
        {
            double H=0;
            foreach (char item in alphabet)
            {
                if (frequency[item] == 0)
                    continue;
                H += frequency[item] * Math.Log(frequency[item], 2);
            }
            return -H;
        }

    }
}
