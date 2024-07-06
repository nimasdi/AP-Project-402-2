using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Project_s_classes
{
    public class ValidateStrings
    {
        public ValidateStrings()
        {
            
        }

        public bool ValidateName(string input)
        {
            Regex regex = new Regex(@"^[a-zA-Z\s]+$");
            if (!regex.IsMatch(input))
            {
                return false;
            }

            string[] words = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (words[0].Length < 3 || words[0].Length > 32)
            {
                return false;
            }

            return true;
        }

        public bool ValidateEmail(string input)
        {
            string[] parts = input.Split('@');

            if (parts.Length != 2)
                return false;

            string local = parts[0];
            string domain = parts[1];

            string[] localwords = local.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (localwords[0].Length < 3 || localwords[0].Length > 32)
                return false;

            Regex regex = new Regex(@"^[a-zA-Z\s]+$");
            //if (!regex.IsMatch(local))
            //return false;

            string[] domainParts = domain.Split('.');
            if (domainParts.Length != 2)
                return false;

            string back = domainParts[0];
            string front = domainParts[1];

            string[] frontWords = front.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (frontWords[0].Length < 2 || frontWords[0].Length > 3)
                return false;

            if (!regex.IsMatch(front))
                return false;

            return true;
        }

        public bool ValidateMobileNumber(string input)
        {
            if (!input.StartsWith("09"))
                return false;

            Regex regex = new Regex("^09[0-9]{9}$");

            return regex.IsMatch(input);
        }

        public bool ValidateUsername(string input)
        {
            Regex regex = new Regex("^[a-zA-Z0-9]+$");
            if (!regex.IsMatch(input))
                return false;

            string[] words = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (words[0].Length < 3)
                return false;

            return true;
        }

        public bool ValidatePassword(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            if (input.Length < 8 || input.Length > 32)
                return false;

            if (!Regex.IsMatch(input, @"[A-Z]"))
                return false;

            if (!Regex.IsMatch(input, @"[a-z]"))
                return false;

            if (!Regex.IsMatch(input, @"\d"))
                return false;
            return true;
        }
    }
}
