using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Utilities.Convertors
{
   public class FixedText
    {
        public static string FixeEmail(string email)
        {
            return email.Trim().ToLower();
        }
    }
}
