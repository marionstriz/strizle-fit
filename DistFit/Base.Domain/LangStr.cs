using System.Diagnostics;

namespace Base.Domain;

public class LangStr : Dictionary<string, string>
{
    public const string DefaultCulture = "en-US";
    public static readonly List<string> SupportedCultures = new()
    {
        DefaultCulture,
        "et-EE"
    };
    
    public LangStr() {}
    
    public LangStr(string value) : this(value, Thread.CurrentThread.CurrentUICulture.Name) { }
    
    public LangStr(string value, string culture)
    {
        if (!SupportedCultures.Contains(culture))
        {
            throw new ArgumentException("Culture not supported.");
        }
        this[culture] = value;
        if (culture != DefaultCulture)
        {
            this[DefaultCulture] = value;
        }
    }

    public string? Translate(string? culture = null)
    {
        if (Count == 0) return null;

        culture = culture?.Trim() ?? Thread.CurrentThread.CurrentUICulture.Name;

        if (ContainsKey(culture))
        {
            return this[culture];
        }
        
        var neutralCulture = culture.Split("-")[0];
        if (ContainsKey(neutralCulture))
        {
            return this[neutralCulture];
        }

        return ContainsKey(DefaultCulture) ? this[DefaultCulture] : null;
    }

    public void SetTranslation(string value)
    {
        this[Thread.CurrentThread.CurrentUICulture.Name] = value;
    }
    
    public void SetTranslation(string value, string culture)
    {
        this[culture] = value;

        if (!ContainsKey(DefaultCulture))
        {
            this[DefaultCulture] = value;
        }
    }

    public static string SupportedCultureOrDefault(string? culture)
    {
        return IsCultureSupported(culture) ? culture! : DefaultCulture;
    }

    public static bool IsCultureSupported(string? culture)
    {
        return culture != null && SupportedCultures.Contains(culture);
    }

    public override string ToString()
    {
        return Translate() ?? "???";
    }

    // string xxx = new LangStr("zzz");
    public static implicit operator string(LangStr? langStr) => langStr?.ToString() ?? "null";
    
    // LangStr langStr = "xxx";
    public static implicit operator LangStr(string value) => new(value);
}