using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.Armory.ViewModel
{
    public class FactionViewModel
    {

        public List<BuildingViewModel> Buildings { get; private set; }

        public FactionViewModel(List<BuildingViewModel> buildings)
        {
            Buildings = buildings;
        }
    }
}
