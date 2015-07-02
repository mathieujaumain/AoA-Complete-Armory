using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using DM.Armory.Model;

namespace DM.Armory.ViewModel
{
    public class BuildingViewModel
    {
        private AoABuilding _building;

        private List<AoAUnitViewModel> _units = new List<AoAUnitViewModel>();

        public BuildingViewModel(AoABuilding building)
        {
            // TODO: Complete member initialization
            this._building = building;
            foreach (AoAUnit unit in _building.BuildableUnits)
                _units.Add(new AoAUnitViewModel(unit));
        }

        public string Name { get { return _building.Name; } }
        public string Description { get { return _building.Description; } }
        public Bitmap Icon { get { return _building.Icon; } }
        public List<AoAUnitViewModel> Units { get { return _units; } }
    }
}
