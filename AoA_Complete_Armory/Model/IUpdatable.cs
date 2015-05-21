using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.Armory.Model
{

    public interface IUpdatable
    {
        void Update(double timeElapsed);
    }
    
}
