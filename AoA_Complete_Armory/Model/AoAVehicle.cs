﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IrisZoomDataApi.Model.Ndfbin;

namespace DM.Armory.Model
{
    public class AoAVehicle:IUpdatable, INdfbinLoadable
    {
        public void Update(double timeElapsed)
        {
            throw new NotImplementedException();
        }



        public bool LoadData(NdfObject dataobject, IrisZoomDataApi.TradManager dictionary, IrisZoomDataApi.EdataManager iconPackage)
        {
            throw new NotImplementedException();
        }
    }
}
