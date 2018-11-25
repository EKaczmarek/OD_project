using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OD_project_console
{
    public static class Conversion
    {
        private static byte[] asciiCodes;

        // Dlugosc skrótu bloku 32B
        private static int hash_len = 32;

        // Dlugosc bloku 8B
        private static int block_len = 8;

        // message
        private static string message;

        private static string dict_for_hash;

        private static byte[] hash_bytes;

        private static string result_count = "";
        private static string result = "";
        private static int hash_len_var = 5;

        public static void set_message(string _message)
        {
            message = _message;
        }
        private static void fulfill()
        {
            byte[] temp = Encoding.UTF8.GetBytes(message);

            for (int i = 0; i < message.Length; i++)
            {
                asciiCodes[i] = temp[i];
            }

            //dopelnienie wg ISO 7816-4
            asciiCodes[message.Length] = 0x80;
            for (int j = message.Length + 1; j < asciiCodes.Length - 1; j++)
            {
                asciiCodes[j] = 0x00;
            }
        }
        private static void xor()
        {
            // każdy blok XOR 
            string bajt = "";
            while (bajt.Length / 8 < hash_len - 1) {
                for (int j = 0; j < asciiCodes.Length - 2; j += 1)
                {
                    //Console.WriteLine("asciiCodes[j] ", asciiCodes[j], "asciiCodes[j+2] ", asciiCodes[j+2]);

                    for (int k = 0; k < 8; k++)
                    {
                        bool test = GetBit(asciiCodes[j], k) ^ (GetBit(asciiCodes[j + 1], k));
                        if (test) bajt += 1;
                        else bajt += 0;
                        if ((bajt.Length) / 8 == hash_len - 1)
                            break;
                    }
                    if (bajt.Length / 8 == hash_len - 1)
                        break;
                }
            }
            //na wysciu powinny byc 32 B
            hash_bytes = getBitwiseByteArray(bajt);
            List<byte> byteArray = hash_bytes.ToList(); //array is the byte array
            hash_bytes = byteArray.ToArray();

            //testowanie dla róznych dlugosci hashy i blokow
            //xor slownik dict_for_hash ^ hash_bytes
            string hash_res = "";
            bool temp;
            for (int i = 1; i < hash_len + 1; i++)
            {
                for (int j = 1; j < 9; j++)
                {
                    if (dict_for_hash[i%j] == '0')
                        temp = false;
                    else temp = true;
                    bool test = GetBit(hash_bytes[i%31], j-1) ^ temp;

                    if (test) hash_res += "1";
                    else hash_res += "0";
                }
            }
            //na wysciu powinny byc hash_len dlugosc B
            hash_bytes = getBitwiseByteArray(hash_res);
            List<byte> byteArray2 = hash_bytes.ToList(); //array is the byte array
            hash_bytes = byteArray2.ToArray();


            //xor hash_bytes ^ numer pod jakim są w tablicy
            string test_hash = "";
            for(int i = 1; i < hash_len + 1; i++)
            {
                for (int j = 1; j < 9; j++)
                {
                    bool test = GetBit(hash_bytes[i-1], j) ^ GetBit((byte)(i-1), j);
                    if (test) test_hash += "1";
                    else test_hash += "0";
                }
            }

            hash_bytes = getBitwiseByteArray(test_hash);
            List<byte> byteArray3 = hash_bytes.ToList(); //array is the byte array
            hash_bytes = byteArray3.ToArray();

            //Console.WriteLine("\nWynikowe bajty:");
            string result_hash = "";


            
            for (int j = 1; j < hash_len + 1; j++)
            {
                int a = (hash_bytes[j - 1] % (hash_bytes.Length - 1)) % (dict_for_hash.Length - 1);
                    //Console.Write(a + ", ");
                    result_hash += dict_for_hash[a];
            }
            result = result_hash.Substring(0, hash_len);

           
            //Console.WriteLine("Wynikowy hash: " + result_hash);
            //Console.WriteLine("Długość hasha: " + result_hash.Length);


        }

        private static void xor_dictionary_hasbytes() {

        }
        
        private static byte[] getBitwiseByteArray(string input)
        {
            List<byte> byteList = new List<byte>();
            for (int i = input.Length - 1; i >= 0; i -= 8)
            {
                string byteString = "";
                for (int j = 0; (i - j) >= 0 && j < 8; j++)
                {
                    byteString = input[i - j] + byteString;
                }
                if (byteString != "")
                {
                    byteList.Add(Convert.ToByte(byteString, 2));
                }
            }
            return byteList.ToArray();
        }

        public static bool GetBit(this byte b, int bitNumber)
        {
            return (b & (1 << bitNumber)) != 0;
        }


        public static void chars_for_hash()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            //NUMBERS
            for (int i = 48; i < 58; i++)
            {
                char symbol = (char)i;
                //Console.WriteLine("{0} -> {1}", i, symbol);
                dict_for_hash += symbol;
            }
            //LETTERS
            //for (int i = 97; i < 123; i++)
            ////for (int i = 97; i < 123; i++)
            //{
            //    char symbol = (char)i;
            //    //Console.WriteLine("{0} -> {1}", i, symbol);
            //    dict_for_hash += symbol;
            //}
            //Console.WriteLine("Rozmiar tablicy: " + dict_for_hash.Length.ToString());
        }
        public static string count(int hash_len_res)
        {
            // pobranie alfabetu do hasha
            chars_for_hash();
          
            asciiCodes = new byte[((message.Length / block_len) + 1) * block_len];
            hash_bytes = new byte[32];

            fulfill();
            xor();

            string res = "";
            for (int i = 0; i < hash_len_res; i++)
            {
                res += result[(i * hash_len_res)];
            }
            return res;
        }

    }
}
