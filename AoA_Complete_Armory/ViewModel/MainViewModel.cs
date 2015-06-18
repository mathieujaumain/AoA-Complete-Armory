using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DM.Armory.BL;

namespace DM.Armory.ViewModel
{
    public class MainViewModel
    {
        private ObjectLibrary _Library;
        public ObjectLibrary Library { get { return _Library; } }
        

        public MainViewModel()
        {
            InitializationController init = new InitializationController();
            _Library = new ObjectLibrary(init.LoadData()); //...
        }

        public MainViewModel(long version)
        {
            InitializationController init = new InitializationController();
            _Library = new ObjectLibrary(init.LoadData(version)); //...
        }
    }
}