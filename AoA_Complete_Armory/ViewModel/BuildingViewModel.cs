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
        private List<AoATurretViewModel> _turrets = new List<AoATurretViewModel>();

        public BuildingViewModel(AoABuilding building)
        {
            // TODO: Complete member initialization
            this._building = building;
            foreach (AoAUnit unit in _building.BuildableUnits)
                _units.Add(new AoAUnitViewModel(unit));

            foreach (AoAResearch research in _building.Researches)
                _res.Add(new AoAResearchViewModel(research));

            foreach (AoATurret turret in _building.Turrets)
                _turrets.Add(new AoATurretViewModel(turret));
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
        public float ViewRange { get { return _building.ViewRange; } }
        public bool Stealth { get { return _building.IsStealthy; } }
        public float Health { get { return _building.Health; } }
        public float Armor { get { return _building.Armor; } }

        public List<AoATurretViewModel> Turrets { get { return _turrets; } }
    }
}
