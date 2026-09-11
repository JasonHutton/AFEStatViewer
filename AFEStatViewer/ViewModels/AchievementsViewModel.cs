using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AFEStatViewer.Models;

namespace AFEStatViewer.ViewModels
{
    public sealed class AchievementsViewModel
    {
        public ObservableCollection<AchievementViewModel> Items { get; }

        public AchievementsViewModel(IEnumerable<AchievementDefinition> defs)
        {
            Items = new ObservableCollection<AchievementViewModel>(defs.Select(def => new AchievementViewModel(def)).OrderBy(achievement => achievement.Name, StringComparer.CurrentCultureIgnoreCase));
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
