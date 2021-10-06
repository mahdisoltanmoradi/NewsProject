using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Utilities.Convertors
{
    public class NameGenerator
    {
        public static string GeneratorUniqCode()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
