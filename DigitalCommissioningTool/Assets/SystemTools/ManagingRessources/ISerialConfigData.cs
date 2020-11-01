using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemTools.ManagingRessources
{
    public interface ISerialConfigData
    {
        void Serialize( SerialConfigData storage );
        void Restore( SerialConfigData storage );
    }
}
