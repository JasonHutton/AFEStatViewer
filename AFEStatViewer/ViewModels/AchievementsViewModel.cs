using AFEStatViewer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AFEStatViewer.ViewModels
{
    public sealed class AchievementsViewModel
    {
        public ObservableCollection<AchievementViewModel> Items { get; }

        public AchievementsViewModel(IEnumerable<AchievementDefinition> defs)
        {
            Items = new ObservableCollection<AchievementViewModel>(defs.Select(d => new AchievementViewModel(d)).OrderBy(a => a.Name, StringComparer.CurrentCultureIgnoreCase));
        }

        public void Apply(AchievementProgress progress)
        {
            foreach (var vm in Items)
            {
                vm.Value = progress.GetValue(vm.Definition);
            }
        }
    }
}
