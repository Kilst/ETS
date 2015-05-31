using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//..
using System.Security.Cryptography;

namespace ETS.logic
{
    // Modified my code based on the code example I was given
    public static class Hasher
    {
        public static string EncodeFull(string salt, string input)
        {
            // Get salt
            //string salt = SetSalt();


            // Create new hasher object
            SHA256Managed sha = new SHA256Managed();
            // Covert input + salt to byte array
            byte[] dataBytes = System.Text.Encoding.UTF8.GetBytes(input + salt);
            // Hash the byte array
            byte[] hashedDataBytes = sha.ComputeHash(dataBytes);
            // Convert to string
            string output = Convert.ToBase64String(hashedDataBytes);
            return output;
        }

        public static string Encode(string input)
        {
            byte[] encode = System.Text.Encoding.UTF8.GetBytes(input);
            return System.Convert.ToBase64String(encode);
        }
        public static string Decode(string input)
        {
            byte[] deCode = System.Convert.FromBase64String(input);
            return System.Text.Encoding.UTF8.GetString(deCode);
        }
        public static string SetSalt()
        {
            // Create randomBytes object with a max of 24 bytes
            byte[] randomBytes = new byte[24];

            // Use RNGCrypto to create a random set of bytes
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);
            // Return generated Salt value (needs to be ASCII or the database replaces chars with ?????)
            return System.Text.Encoding.ASCII.GetString(randomBytes);
        }
    }
}
