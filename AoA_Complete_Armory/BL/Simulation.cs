using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using DM.Armory.Model;
using DM.Armory.ViewModel;

namespace DM.Armory.BL
{

        public delegate void UpdatableDelegate(List<IUpdatable> list, double elapsed);

        public class Simulation
        {
            private List<IUpdatable> _Updatables = new List<IUpdatable>();
            private bool _IsRunning = false;
            private double _TotalElapsedTime = 0;

            public event UpdatableDelegate OnUpdate = delegate { };
            public event EventHandler SimulationStarted = delegate { };
            public event EventHandler SimulationFinished = delegate { };

            public Simulation()
            {
                Status = SimulationStatus.Configuration;
            }

            private static object _Lock = new object();

            public void StartSimulation(List<IUpdatable> list)
            {
                if (list.Count > 0)
                {
                    Status = SimulationStatus.Ongoing;
                    TotalElapsedTime = 0;
                    IsRunning = true;
                    new Thread(() => UpdateLoop(list)).Start();
                }
            }

            private void UpdateLoop(List<IUpdatable> list)
            {
                DateTime startCycle = DateTime.Now;
                DateTime endCycle = DateTime.Now;
                double elapsedTimeSinceLastUpdate = 0;
                double totalElapsed = 0;
                SimulationStarted(this, null);
                while (IsRunning)
                {
                    endCycle = DateTime.Now;
                    elapsedTimeSinceLastUpdate = (endCycle - startCycle).TotalSeconds;
                    startCycle = endCycle;
                    totalElapsed += elapsedTimeSinceLastUpdate;
                    foreach (IUpdatable updatable in list)
                        updatable.Update(elapsedTimeSinceLastUpdate);

                    OnUpdate(list, totalElapsed);
                    Thread.Sleep(50);
                }
                SimulationFinished(this, null);
                Status = SimulationStatus.Finished;
            }

            public void StopSimulation()
            {
                IsRunning = false;
                foreach (IUpdatable updatable in Updatables)
                {
                    updatable.Reset();
                }
            }

            /// <summary>
            /// In seconds.
            /// </summary>
            public double TotalElapsedTime
            {
                get { lock (_Lock) return _TotalElapsedTime; }
                set { lock (_Lock) _TotalElapsedTime = value; }
            }

            public bool IsRunning
            {
                get { lock (_Lock) return _IsRunning; }
                set { lock (_Lock) _IsRunning = value; }
            }

            public List<IUpdatable> Updatables
            {
                get { lock (_Lock) return _Updatables; }
                set { lock (_Lock) _Updatables = value; }
            }


            public SimulationStatus Status { get; set; }
        }

        public enum SimulationStatus
        {
            Configuration, Ongoing, Finished
        }
    
}
