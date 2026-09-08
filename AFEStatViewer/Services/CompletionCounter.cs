using AFEStatViewer.Models;
using System.Collections.Generic;
using System.Linq;

namespace AFEStatViewer.Services
{
    public static class CompletionCounter
    {
        public static CompletionSummary Count(IEnumerable<bool> items)
        {
            var itemList = items as IReadOnlyCollection<bool> ?? items.ToList();

            return new CompletionSummary(
                itemList.Count(item => item),
                itemList.Count
            );
        }
    }
}