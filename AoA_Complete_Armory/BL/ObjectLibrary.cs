using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DM.Armory.Model;
using DM.Armory.Model.Skills;

namespace DM.Armory.BL
{
    public class ObjectLibrary
    {
        private List<AoAUnit> _CartelUnits = new List<AoAUnit>();
        private List<AoAUnit> _UsUnits = new List<AoAUnit>();
        private List<AoAUnit> _ChimeraUnits = new List<AoAUnit>();
        private List<AoAUppgrade> _Uppgrades = new List<AoAUppgrade>();
        private List<AoABuilding> _CartelBuildings = new List<AoABuilding>();
        private List<AoABuilding> _UsBuildings = new List<AoABuilding>();
        private List<AoABuilding> _ChimeraBuildings = new List<AoABuilding>();

        public ObjectLibrary(List<AoAGameObject> objects) 
        {
            if (objects != null && objects.Count > 0)
            {
                ClassifyObjects(objects);
            }
        }

        private void ClassifyObjects(List<AoAGameObject> objects)
        {
            foreach (AoAGameObject obj in objects)
            {
                if (obj is AoABuilding)
                {
                    ClassifyBuilding(obj as AoABuilding);
                    break;
                }

                if (obj is AoAUnit)
                {
                    ClassifyUnit(obj as AoAUnit);
                    break;
                }

                if (obj is AoAUppgrade)
                    _Uppgrades.Add(obj as AoAUppgrade);
            }
        }

        public bool ClassifyBuilding(AoABuilding building)
        {
            switch (building.Faction) 
            {
                case FactionEnum.Cartel:
                    _CartelBuildings.Add(building);
                    return true;

                case FactionEnum.Chimera:
                    _ChimeraBuildings.Add(building);
                    return true;

                case FactionEnum.US:
                    _UsBuildings.Add(building);
                    return true;

                default: return false;
            }
        }

        public bool ClassifyUnit(AoAUnit unit)
        {
            switch (unit.Faction)
            {
                case FactionEnum.Cartel:
                    _CartelUnits.Add(unit);
                    return true;

                case FactionEnum.Chimera:
                    _ChimeraUnits.Add(unit);
                    return true;

                case FactionEnum.US:
                    _UsUnits.Add(unit);
                    return true;

                default: return false;
            }
        }


        #region Properties
        public List<AoAUnit> CartelUnits
        {
            get { return _CartelUnits; }
            set { _CartelUnits = value; }
        }

        public List<AoAUnit> UsUnits
        {
            get { return _UsUnits; }
            set { _UsUnits = value; }
        }


        public List<AoAUnit> ChimeraUnits
        {
            get { return _ChimeraUnits; }
            set { _ChimeraUnits = value; }
        }


        public List<AoAUppgrade> Uppgrades
        {
            get { return _Uppgrades; }
            set { _Uppgrades = value; }
        }

        public List<AoABuilding> CartelBuildings
        {
            get { return _CartelBuildings; }
            set { _CartelBuildings = value; }
        }

        public List<AoABuilding> UsBuildings
        {
            get { return _UsBuildings; }
            set { _UsBuildings = value; }
        }

        public List<AoABuilding> ChimeraBuildings
        {
            get { return _ChimeraBuildings; }
            set { _ChimeraBuildings = value; }
        }
        #endregion
    }
}
