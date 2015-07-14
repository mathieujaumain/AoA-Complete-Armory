using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DM.Armory.Model;
using System.Drawing;

namespace DM.Armory.ViewModel
{
    public class AoAResearchViewModel
    {
        private AoAResearch _res;

        public string Name { get { return _res.Name; } }
        public string Description { get { return _res.Description; } }
        public int CashCost { get { return _res.CashCost; } }
        public int AluminiumCost { get { return _res.AluminiumCost; } }
        public int ElectricityCost { get { return _res.ElectricityCost; } }
        public int RareEarthCost { get { return _res.RareEarthCost; } }
        public Bitmap Icon { get { return _res.Icon; } }

        public AoAResearchViewModel(AoAResearch research)
        {
            _res = research;
        }


    }

}

