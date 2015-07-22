using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;
using OxyPlot.Series;
using DM.Armory.Model;
using DM.Armory.BL;

namespace DM.Armory.ViewModel
{

    public class WeaponGraphViewModel
    {

        public PlotModel Model { get; private set; }
        private Simulation _Simulation = new Simulation();

        public WeaponGraphViewModel(List<AoAWeaponViewModel> weaponList)
        {
            PrepareModel(weaponList);
        }

        public void PrepareModel(List<AoAWeaponViewModel> weaponList)
        {

            Model = new PlotModel() { Title = "Rate of fire" };
            Model.DefaultXAxis.Title = "Time (seconds)";
            Model.DefaultYAxis.Title = "Shots fired";

            foreach (AoAWeaponViewModel updatable in weaponList)
            {
                Model.Series.Add(new LineSeries { LineStyle = LineStyle.Solid, Title = updatable.Name, });
            }

        }

        public void Start(List<AoAWeaponViewModel> weaponList)
        {
            _Simulation.OnUpdate += UpdatePlotModel;

            List<IUpdatable> ups = new List<IUpdatable>();
            foreach(AoAWeaponViewModel wea in weaponList)
                ups.Add(wea.)
            _Simulation.StartSimulation(weaponList);
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
                    long fired = weaponList[i].Weapon.CurrentNbProjectilesFired;
                    if (fired > maxY)
                        maxY = fired;
                    serie.Points.Add(new DataPoint(time, fired));
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

        

    }


     
}
