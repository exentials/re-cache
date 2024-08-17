using System.CommandLine;

namespace Exentials.ReCache.ReCli.Parameters;

internal class AbsoluteOption : Option<DateTime?>
{
    public AbsoluteOption()
        : base("--absolute", "Absolute date time expiration")
    {
        AddAlias("-ax");
        SetDefaultValue(null);
    }
}
