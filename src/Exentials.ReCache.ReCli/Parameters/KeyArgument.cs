using System.CommandLine;

namespace Exentials.ReCache.ReCli.Parameters
{
    internal class KeyArgument : Argument<string>
    {
        public KeyArgument()
            : base("key", "The dictionary key")
        {
        }
    }
}
