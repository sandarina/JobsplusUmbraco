using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace JobsplusUmbraco
{
    public class JobsplusHelpers
    {
        /// <summary>
        /// Odstraní dakritiku z předaného řetězce.
        /// </summary>
        /// <param name="text">Řetězec</param>
        /// <returns></returns>
        public static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        /// <summary>
        /// Získá HTML reprezentaci zprávy z Exception.
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <returns></returns>
        public static string GetMsgFromException(Exception ex)
        {
            var innterMsgText = ex.InnerException != null ? "<br />" + ex.InnerException.Message : "";
            return "<h3>" + ex.Message + "</h3><br /><p>" + ex.StackTrace + innterMsgText + "</p>";
        }

        public static string GetValueToEmail(object value)
        {
            if (value == null) return "-";

            var type = value.GetType();

            // prázdný řetězec
            if (type == typeof(string) && string.IsNullOrWhiteSpace((string)value)) return "-";

            // binární
            if (type == typeof(bool)) return (bool)value ? "Ano" : "Ne";

            // datum
            if (type == typeof(DateTime)) return ((DateTime)value).ToShortDateString();

            return value.ToString().Replace("\n", "<br />");
        }
    }
}

namespace System
{
    public static class StringExtension
    {
        public static MvcHtmlString ToMvcHtmlString(this string value)
        {
            return MvcHtmlString.Create(value);
        }
    }
}