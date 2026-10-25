using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace PublicCMS.Utils;

public static class Slugifier
{
    public static string Slugify(this string text, string pattern = @"[^a-z0-9\s-]")
    {
        // Based on: https://stackoverflow.com/questions/249087/how-do-i-remove-diacritics-accents-from-a-string-in-net/249126#249126
        // And: https://stackoverflow.com/questions/2920744/url-slugify-algorithm-in-c/14538799#14538799
        StringBuilder sb = new();
        foreach (var c in text.Normalize(NormalizationForm.FormD))
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            {
                sb.Append(c);
            }
        }

        var slug = sb.ToString().ToLowerInvariant();
        slug = Regex.Replace(slug, pattern, "");
        slug = Regex.Replace(slug, @"[\s-]+", "-").Trim('-');

        return slug;
    }
}
