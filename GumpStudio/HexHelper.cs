using System;

namespace GumpStudio
{
    public class HexHelper
    {
        private const string Numbers = "0123456789ABCDEF";

        public static int HexToDec(string value)
        {
            // Convert to uppercase for consistent comparison
            value = value.ToUpper();

            // Check if the string is a valid hex value with "0X" prefix
            // and between 3-6 characters long (0X1 to 0XFFFF)
            if (value.Length <= 2 || value.Length > 6 || !value.StartsWith("0X"))
                return 0;

            int result = 0;
            int currentPosition = value.Length;

            while (currentPosition >= 3)
            {
                // Get single character at current position
                string currentChar = value[currentPosition - 1].ToString();

                // Find position in hex character set (0-15)
                int hexValue = Numbers.IndexOf(currentChar);

                if (hexValue == -1)
                    return 0;  // Invalid hex character found

                // Calculate hex power value and add to result
                result += (int)(Math.Pow(16, value.Length - currentPosition) * hexValue);
                currentPosition--;
            }

            return result;
        }
    }
}