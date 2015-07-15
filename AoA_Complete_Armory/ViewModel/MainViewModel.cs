using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DM.Armory.Model;
using DM.Armory.BL;

namespace DM.Armory.ViewModel
{
    public class MainViewModel
    {
        private InitializationController Init;
        private List<BuildingViewModel> _cartelBuildings = new List<BuildingViewModel>();
        private List<BuildingViewModel> _usBuildings = new List<BuildingViewModel>();
        private List<BuildingViewModel> _chimBuildings = new List<BuildingViewModel>();

        public MainViewModel()
        {
            Init = new InitializationController();
            if (Init.LoadData())
            {
                foreach (AoABuilding b in Init.UsBuildings)
                {
                    BuildingViewModel buioding = new BuildingViewModel(b);
                    _usBuildings.Add(buioding);
                }
                Init.UsBuildings.Clear();



                //Cartel
                foreach (AoABuilding b in Init.CartelBuildings)
                {
                    BuildingViewModel buioding = new BuildingViewModel(b);
                    _cartelBuildings.Add(buioding);
                }
                Init.CartelBuildings.Clear();



                //Chimera
                foreach (AoABuilding b in Init.ChimeraBuildings)
                {
                    BuildingViewModel buioding = new BuildingViewModel(b);
                    _chimBuildings.Add(buioding);
                }
                Init.ChimeraBuildings.Clear();


            }
        }

    }
}
