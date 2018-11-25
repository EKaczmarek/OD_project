using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OD_project_console
{
    public static class RandomString
    {
        private static Random random = new Random();
        public static void SetSeed(int seed)
        { random = new Random(seed); }

        public static string Next(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnoprstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
    class Program
    {
        ////////////////// DANE //////////////////////////
        private static int words_ammount = 2000;
        private static int words_lenght = 20;
        private static int hash_len_res = 5;
        private static int seed = 10;


        static void Main(string[] args)
        {
            List<string> words = new List<string>();
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            // USTAWIENIE ZIARNA
            RandomString.SetSeed(Convert.ToInt32(seed));

            // GENEROWANIE STRNGÓW WEJSCIOWYCH
            while (words.Count != words_ammount)
            {
                var text = RandomString.Next(words_lenght);
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
            //LISTA POJEDYCZNYCH HASHY TO ZLICZENIA 
            List<string> hashes = new List<string>();

            // ROZPOCZĘCIE ODLICZANIA CZASU
            var watch = System.Diagnostics.Stopwatch.StartNew();

            // GENEROWANIE HASHY DLA KAŻDEGO STRINGA 
            foreach (string item in words) {

                Conversion.set_message(item);
                string result = Conversion.count(hash_len_res);                
                dictionary.Add(item, result);

                hashes.Add(result);
            }
            // ZAKOŃCZENIE ODLICZANIA CZASU
            watch.Stop();

            //////////////////WYNIKI//////////////////////////
            ///
            // SŁOWNIK dictionary string wejściowy - hash
            int kolizje = hashes.GroupBy(x => x)
                        .Where(group => group.Count() > 1)
                        .Select(group => group.Key).Count();

            var elapsedMs = watch.ElapsedMilliseconds;

            Console.WriteLine("\nDlugosc stringow wejsciowych: " + words_lenght);
            Console.WriteLine("Ilość stringów wejściowych: " + words_ammount);
            Console.WriteLine("Długość hashy: " + hash_len_res);
            Console.WriteLine("Ilość kolizji: " + kolizje);

            Console.WriteLine("Czas generowanie hashy: " + elapsedMs + " ms");


            Console.ReadKey();
            #region zapis_do_pliku
            try
            { 
                //Pass the filepath and filename to the StreamWriter Constructor
                StreamWriter sw = new StreamWriter("Test.txt");
                
                foreach (KeyValuePair<string, string> item in dictionary)
                {
                    //Console.WriteLine("Key: {0}, Value: {1}", item.Key, item.Value);
                    sw.WriteLine(item.Key + "\t" + item.Value);
                }

                sw.WriteLine("Czas generowania: " + elapsedMs + " ms");                    

                sw.WriteLine("Ilość kolizji: " + kolizje);

                sw.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                
            }
            #endregion
        }
    }
}
