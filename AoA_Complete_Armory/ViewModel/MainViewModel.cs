using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Controls;
using System.Threading.Tasks;
using DM.Armory.View;
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

        public FillContainer FillWithCartel;
        public FillContainer FillWithUS;
        public FillContainer FillWithChimera;

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


                FillWithChimera = new FillContainer(_chimBuildings);
                FillWithCartel = new FillContainer(_cartelBuildings);
                FillWithUS = new FillContainer(_usBuildings);
            }
        }

        public class FillContainer : ICommand
        {
            List<BuildingViewModel> models;


            public FillContainer(List<BuildingViewModel> factionBuildings)
            {
                models = factionBuildings;
            }

            public bool CanExecute(object parameter)
            {
                Panel grid = parameter as Panel;
                if(grid == null) return false;
                return grid.Children.Count <= 0;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {

                Panel grid = parameter as Panel;
                if(grid == null) return;

                foreach (BuildingViewModel model in models)
                {
                    IconView view = new IconView(model);
                    grid.Children.Add(view);
                }
            }
        }

    }
}
