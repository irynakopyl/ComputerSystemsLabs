using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace base64_encoding
{
    class Program
    {
      
        static Dictionary<char, float> frequency;
        static int lengthOfFile = 0;
        static void Main(string[] args)
        {
            const string folder = @"D:\University\ComputerSystems\";
            string filePath = folder + GetOpenedFileName(folder);
            string info = GetAllStr(filePath);
            Console.WriteLine($"Ur string is:\n{info}");
            string result = ToBase64Custom(Encoding.Default.GetBytes(info));
            WriteEncryptedFile(result, filePath);
            Console.WriteLine($"Ur base64-string is:\n{result}");
            FillChars();
            GetProbability(filePath+ "encrypted.txt");
            Console.WriteLine($"Ur info is - {CountEntropy() * lengthOfFile / 8}  bytes");
        }
        public static void FillChars()
        {
            frequency = new Dictionary<char, float>();
            for (int i = 0; i < base64Table.Length; i++)
            {
                frequency.Add(base64Table[i], 0);
            }
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
                foreach (char item in base64Table)
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
            double H = 0;
            foreach (char item in base64Table)
            {
                if (frequency[item] == 0)
                    continue;
                H += frequency[item] * Math.Log(frequency[item], 2);
            }
            return -H;
        }
    
        static readonly char[] base64Table = {'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O',
                                                       'P','Q','R','S','T','U','V','W','X','Y','Z','a','b','c','d',
                                                       'e','f','g','h','i','j','k','l','m','n','o','p','q','r','s',
                                                       't','u','v','w','x','y','z','0','1','2','3','4','5','6','7',
                                                       '8','9','+','/','=' };

        public static string ToBase64Custom(byte[] data)
        {
            var arrayOfBinaryStrings = data.Select(x => Convert.ToString(x, 2).PadLeft(8, '0'));
            var count = arrayOfBinaryStrings.Count();
            var append = count % 3 == 1 ? "==" : count % 3 == 2 ? "=" : "";

            var allBytes = string.Join("", arrayOfBinaryStrings);
            var countOfBytes = allBytes.Count();
            var remOfDivision = countOfBytes % 6;
            var newList = Enumerable.Range(0, countOfBytes / 6).Select(x => allBytes.Substring(x * 6, 6)).ToList();

            if (remOfDivision != 0)
            {
                newList.Add(allBytes.Substring(countOfBytes / 6 * 6, remOfDivision).PadRight(6, '0'));
            }

            return (string.Join("", newList.Select(x => base64Table[Convert.ToByte(x, 2)])) + append);
        }
        static string GetOpenedFileName(string path)//to find if filename exist
        {
            string name = "";
            do
            {
                Console.WriteLine("\nEnter the filename:");
                name = Console.ReadLine();
            } while (!File.Exists(path + name + ".txt"));
            return name + ".txt";
        }
        static string GetAllStr(string path)
        {
            string text;
            StreamReader dataIn = null;
            try
            {
                dataIn = new StreamReader(path);
                text = dataIn.ReadToEnd(); // put all the text in one string (we know that it is rather small)
                
            }
            finally
            {
                if (dataIn != null) dataIn.Close();
            }
            return text;
        }
        static void WriteEncryptedFile(string text, string path)//puts the encrypted info into file
        {
            StreamWriter dataOut = null;
            try
            {
               dataOut = new StreamWriter(String.Concat(String.Format(path,".txt", ""),"encrypted.txt"));
               dataOut.Write(text);
                  
            }
            finally
            {
                if (dataOut != null) dataOut.Close();

            }
        }

    }
}
