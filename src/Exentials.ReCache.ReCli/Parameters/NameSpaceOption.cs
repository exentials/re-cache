using System.CommandLine;

namespace Exentials.ReCache.ReCli.Parameters
{
    internal sealed class NameSpaceOption : Option<string>
    {
        public NameSpaceOption()
            : base("--namespace", "Namespace")
        {
            AddAlias("-n");
        }
    }
}
