using System.CommandLine;

namespace Exentials.ReCache.ReCli.Parameters
{
    internal class ValueArgument
        : Argument<string>
    {
        public ValueArgument()
            : base("value", "Value to cache")
        {
        }
    }
}
