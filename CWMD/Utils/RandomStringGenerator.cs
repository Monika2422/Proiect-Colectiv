using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CWMD.Utils
{
    public class RandomStringGenerator
    {
        Random rand = new Random();

        public const string Alphabet = "abcdefghijklmnopqrstuvwyxzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public string GenerateRandomString(int size)
        {
            char[] chars = new char[size];
            for (int i = 0; i < size; i++)
            {
                chars[i] = Alphabet[rand.Next(Alphabet.Length)];
            }
            return new string(chars);
        }
    }
}