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
        private List<AoAResearchViewModel> _res = new List<AoAResearchViewModel>();

        public BuildingViewModel(AoABuilding building)
        {
            // TODO: Complete member initialization
            this._building = building;
            foreach (AoAUnit unit in _building.BuildableUnits)
                _units.Add(new AoAUnitViewModel(unit));

            foreach (AoAResearch research in _building.Researches)
                _res.Add(new AoAResearchViewModel(research));
        }

        public string Name { get { return _building.Name; } }
        public string Description { get { return _building.Description; } }
        public Bitmap Icon { get { return _building.Icon; } }
        public List<AoAUnitViewModel> Units { get { return _units; } }
        public List<AoAResearchViewModel> Researches { get { return _res; } }
        public int CashCost { get { return _building.CashCost; } }
        public int AluminiumCost { get { return _building.AluminiumCost; } }
        public int ElectricityCost { get { return _building.ElectricityCost; } }
        public int RareEarthCost { get { return _building.RareEarthCost; } }
    }
}
