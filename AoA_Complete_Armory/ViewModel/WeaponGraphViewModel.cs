using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using DM.Armory.Model;
using DM.Armory.BL;

namespace DM.Armory.ViewModel
{

    public class WeaponGraphViewModel
    {

        public PlotModel Model { get; private set; }
        private Simulation _Simulation = new Simulation();
        private ICommand _startCommand;
        private ICommand _stopCommand;
        private ICommand _pauseCommand;


        public WeaponGraphViewModel(List<AoAWeaponViewModel> weaponList)
        {

            PrepareModel(weaponList);
            _Simulation.OnUpdate += UpdatePlotModel;
            _startCommand = new StartCommand(_Simulation, weaponList);
            _stopCommand = new StopCommand(_Simulation);
        }

        public void PrepareModel(List<AoAWeaponViewModel> weaponList)
        {

            Model = new PlotModel() { Title = "Rate of fire" };
            LinearAxis xAxis =  new LinearAxis(AxisPosition.Bottom,"Time (seconds)" );
            LinearAxis yAxis = new LinearAxis(AxisPosition.Left, "Shots fired");
            Model.Axes.Add(xAxis);
            Model.Axes.Add(yAxis);

            foreach (AoAWeaponViewModel updatable in weaponList)
            {
                Model.Series.Add(new LineSeries { LineStyle = LineStyle.Solid, Title = updatable.Name, });
            }

        }

        public void UpdatePlotModel(List<IUpdatable> weaponList, double time )
        {
            lock (this.Model.SyncRoot)
            {
                int count = Math.Min(weaponList.Count, Model.Series.Count);
                double maxY = 0;
                for (int i = 0; i < count; i++)
                {
                    LineSeries serie = Model.Series[i] as LineSeries;
                    AoAWeapon weapon = weaponList[i] as AoAWeapon;
                    if (weapon != null)
                    {
                        long fired = weapon.CurrentNbProjectilesFired;
                        if (fired > maxY)
                            maxY = fired;
                        serie.Points.Add(new DataPoint(time, fired));
                    }
                }

                if (time > 0 && maxY > 0)
                {
                    Model.DefaultXAxis.AbsoluteMinimum = 0;
                    Model.DefaultXAxis.AbsoluteMaximum = time;

                    Model.DefaultYAxis.AbsoluteMinimum = 0;
                    Model.DefaultYAxis.AbsoluteMaximum = maxY;
                }

                Model.InvalidatePlot(true);
            }


            
        }

        public ICommand StartCommand { get { return _startCommand; } }
        public ICommand StopCommand { get { return _stopCommand; } }
    }


    public class StartCommand : ICommand
    {
        private Simulation _sim;
        private List<AoAWeaponViewModel> _list;

        public StartCommand(Simulation sim, List<AoAWeaponViewModel> weaponList)
        {
            _sim = sim;
            _list = weaponList;
        }

        public bool CanExecute(object parameter)
        {
            return !_sim.IsRunning;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            List<IUpdatable> ups = new List<IUpdatable>();
            foreach (AoAWeaponViewModel wea in _list)
                ups.Add(wea.Weapon);
            _sim.StartSimulation(ups);
        }
    }

    public class StopCommand : ICommand
    {
        private Simulation _sim;


        public StopCommand(Simulation sim)
        {
            _sim = sim;
        }

        public bool CanExecute(object parameter)
        {
            return _sim.IsRunning;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _sim.StopSimulation();
        }
    }

     
}
