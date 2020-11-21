using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ProjectComponents.Abstraction;

namespace ProjectComponents.FileIntegration
{
    public class WarehouseHandler
    {
        private WarehouseReader Reader;
        private WarehouseWriter Writer;

        public WarehouseHandler()
        {
            Reader = new WarehouseReader( new XmlDocument() );
            Writer = new WarehouseWriter( new XmlDocument() );
        }

        public void SaveFile( InternalProjectWarehouse warehouse)
        {
            Writer.WriteFile( warehouse );
        }

        public InternalProjectWarehouse LoadFile()
        {
            return Reader.ReadFile( );
        }
    }
}
