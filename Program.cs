using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OD_project_console
{
    class Program
    {
        public static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        static void Main(string[] args)
        {
            string text;
            int words_ammout = 1000;
            int words_lenght = 20;

            List<string> words = new List<string>();
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            while (words.Count != words_ammout)
            {
                text = RandomString(words_lenght, true);
                if (words.Count == 0)
                {
                    words.Add(text);
                }
                else
                {
                    if (!words.Contains(text))
                        words.Add(text);
                }
            }


            foreach (string item in words) { 

                Conversion.set_message(item);
                string result = Conversion.count();
                dictionary.Add(item, result);
            }

            int hash_lenght = dictionary[words[0]].Length;
            //foreach (KeyValuePair<string, string> item in dictionary)
            //{
            //    Console.WriteLine("Key: {0}, Value: {1}", item.Key, item.Value);
            //}


            int kolizje = 0;
            var lookup = dictionary.ToLookup(x => x.Value).Where(x => x.Count() > 1);
            foreach (var item in lookup)
            {
                var keys = item.Aggregate("", (s, v) => s + ", " + v);
                var message = "te same wartości maja: " + item.Key + ":" + keys;
                Console.WriteLine(message);
                kolizje++;
            }
            Console.WriteLine("\nDlugosc stringow wejsciowych: " + words_lenght);            
            Console.WriteLine("Ilość stringów wejściowych: " + words_ammout);
            Console.WriteLine("Długość hashy: " + hash_lenght);
            Console.WriteLine("Ilość kolizji: " + kolizje);

            
            Console.ReadKey();

        }
    }
}
