using System.CommandLine;

namespace Exentials.ReCache.ReCli.Parameters
{
    internal class SlidingOption : Option<TimeSpan?>
    {
        public SlidingOption()
            : base("--sliding", "Sliding time expiration (h:mm:ss,nn)")
        {
            AddAlias("-sx");
            SetDefaultValue(null);
        }
    }
}
