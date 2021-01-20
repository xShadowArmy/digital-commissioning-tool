using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ProjectComponents.Abstraction;

namespace ProjectComponents.FileIntegration
{
    public class ContainerHandler
    {
        private ContainerWriter Writer { get; set; }
        private ContainerReader Reader { get; set; }

        public ContainerHandler()
        {
            Writer = new ContainerWriter( new XmlDocument( ) );
            Reader = new ContainerReader( new XmlDocument( ) );
        }

        public void SaveFile( InternalProjectContainer container )
        {
            Writer.WriteFile( container );
        }

        public InternalProjectContainer LoadFile()
        {
            return Reader.ReadFile( );
        }
    }
}
