using System;

namespace DO_AN
{
    internal class SecurityHelper
    {
        private const string Upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string Lower = "abcdefghijklmnopqrstuvwxyz";
        private const string Number = "0123456789";
        private const string Special = "!@#$^*()_";

        private static Random rnd = new Random();

        public static string GeneratePassword(string role)
        {
            int length;

            switch (role)
            {
                case "Admin":
                    length = rnd.Next(14, 21);
                    break;
                case "Manager":
                    length = rnd.Next(10, 16);
                    break;
                default:
                    length = rnd.Next(8, 12);
                    break;
            }

            string all = Upper + Lower + Number + Special;

            string password =
                Upper[rnd.Next(Upper.Length)].ToString() +
                Lower[rnd.Next(Lower.Length)].ToString() +
                Number[rnd.Next(Number.Length)].ToString() +
                Special[rnd.Next(Special.Length)].ToString();

            for (int i = password.Length; i < length; i++)
            {
                password += all[rnd.Next(all.Length)];
            }

            return password;
        }
    }
}