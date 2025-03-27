using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Microsoft.eShopOnDapr.Services.API.Abstraction.Utilities;

public static class StringExtension
{
    public static string IfNullOrEmptyAs(this string str, string defaultValue)
        => string.IsNullOrEmpty(str) ? defaultValue : str;

    public static string IfNullOrWhiteSpaceAs(this string str, string defaultValue)
       => string.IsNullOrWhiteSpace(str) ? defaultValue : str;

    public static string SubstringMax(this string str, int maxLength)
    {
        if (string.IsNullOrEmpty(str))
            return str;

        if (str.Length > maxLength)
            return str.Substring(0, maxLength);
        else
            return str;
    }

    public static string[] SplitByNewLine(this string input)
    {
        if (input == null)
            return [];

        return input.Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries);
    }

    public static string? RemoveNewLine(this string input)
    {
        if (input == null)
            return null;

        return input.Replace("\r", " ").Replace("\n", " ").Trim();
    }

    public static (string str1, string str2) SplitByStringAsTuple(this string str, string sep)
    {
        if (!string.IsNullOrWhiteSpace(str) && str.Contains(sep))
        {
            var splits = str.Split(sep);
            return (splits[0], splits[1]);
        }
        return (string.Empty, string.Empty);
    }

    public static (string str1, string str2) SplitByCharAsTuple(this string str, char sep)
    {
        if (!string.IsNullOrWhiteSpace(str) && str.Contains(sep))
        {
            var splits = str.Split(sep);
            return (splits[0], splits[1]);
        }
        return (string.Empty, string.Empty);
    }

    public static bool IsEqualTo(this string? str1, string? str2, StringComparison option = StringComparison.OrdinalIgnoreCase)
    {
        if (str1 == null) return str2 == null;

        return str1.Equals(str2, option);
    }

    public static string CleanStr(this string? str)
    {
        if (string.IsNullOrWhiteSpace(str)) return string.Empty;

        return str.Replace(" ", "").Replace("\t", "").Replace("\n", "").Replace("\r", "");
    }

    public static bool IsJsonString(this string input)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(input)) return false;

            // 尝试将字符串解析为 JSON 对象或数组
            JsonDocument.Parse(input);
            return true;
        }
        catch (JsonException)
        {
            // 如果解析失败，则说明不是有效的 JSON
            return false;
        }
    }

    #region JsonDocument & String mutual conversion
    public static string JsonDocumentToString(this JsonDocument jsonDocument)
    {
        // 使用 JsonDocument 的根元素来写入 JSON 字符串
        using var stream = new MemoryStream();
        using (var writer = new Utf8JsonWriter(stream))
        {
            jsonDocument.WriteTo(writer);
        }
        return Encoding.UTF8.GetString(stream.ToArray());
    }

    public static JsonDocument StringToJsonDocument(this string jsonString)
    {
        if (string.IsNullOrWhiteSpace(jsonString))
            return JsonDocument.Parse("{}");

        string jsonStr = jsonString.JsonContent();
        // 使用 JsonDocument.Parse 方法将字符串解析为 JsonDocument
        return JsonDocument.Parse(jsonStr);
    }
    #endregion

    public static string JsonContent(this string text)
    {
        var m = Regex.Match(text, @"\{(?:[^{}]|(?<open>\{)|(?<-open>\}))+(?(open)(?!))\}");
        return m.Success ? m.Value : "{}";
    }

    public static T? JsonContent<T>(this string text)
    {
        text = JsonContent(text);

        return JsonSerializer.Deserialize<T>(text, Options);
    }

    public static string JsonArrayContent(this string text)
    {
        var m = Regex.Match(text, @"\[(.|\n|\r)*\]");
        return m.Success ? m.Value : "[]";
    }

    public static T[]? JsonArrayContent<T>(this string text)
    {
        text = JsonArrayContent(text);

        return JsonSerializer.Deserialize<T[]>(text, Options);
    }

    private static JsonSerializerOptions Options => new()
    {
        Encoder = JavaScriptEncoder.Default, // Default support for UTF-8 encoding
        //Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, // Ensure that special characters are encoded correctly (be aware of potential security issues)
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false, // Optional: JSON Format output
        AllowTrailingCommas = true
    };
}