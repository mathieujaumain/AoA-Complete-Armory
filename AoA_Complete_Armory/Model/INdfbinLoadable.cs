using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IrisZoomDataApi.Model.Ndfbin;
using IrisZoomDataApi;

namespace DM.Armory.Model
{
    interface INdfbinLoadable
    {
        bool LoadData(NdfObject dataobject, TradManager dictionary, TradManager dictionary2, EdataManager iconPackage);
    }
}
