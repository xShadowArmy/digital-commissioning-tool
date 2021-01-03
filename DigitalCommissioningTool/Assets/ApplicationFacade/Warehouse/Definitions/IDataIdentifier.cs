using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationFacade.Warehouse
{
    public interface IDataIdentifier
    {
        void SetID( long id );
        long GetID();
    }
}
