namespace Motoryzacja.Validators
{
    using System;
    using System.Text.RegularExpressions;
    
    public class ConfigValidator
    {
        public bool ValidateCarInput(string carInput)
        {
            // checks if input contains only letters, numbers or spaces
            const string regexPattern = "^[a-zA-Z0-9 ]*$";
            
            if (carInput == string.Empty
                || carInput.Split(" ").Length > 2
                || !Regex.IsMatch(carInput, regexPattern))
            {
                return false;
            }

            return true;
        }

        public bool ValidatePageInput(string pageInput)
        {
            try
            {
                return Int32.Parse(pageInput) >= 0;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        
        public bool ValidatePageInput(int pageInput)
        {
            return pageInput >= 0;
        }
    }
}