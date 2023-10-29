using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UpdraftShop
{
    public static class StringExtensions
    {
        public static string ReplaceEscapeCharacter(this string text)
        {
            return text
                .Replace("\\n", "\n")
                .Replace("&lt;", "<")
                .Replace("&gt;", ">");
        }
    }
}