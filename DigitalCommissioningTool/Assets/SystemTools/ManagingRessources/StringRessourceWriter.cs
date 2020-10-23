using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using SystemTools.Logging;
using System.Globalization;

namespace SystemTools.ManagingRessources
{
    internal class StringRessourceWriter
    {
        private XmlDocument Doc = null;

        public StringRessourceWriter( string path, CultureInfo info )
        {
            LogManager.WriteInfo( "Initialisierung des StringRessourceWriters", "StringRessourceWriter", "StringRessourceWriter" );

            Doc = new XmlDocument( );

            try
            {
                Doc.Load( path );
            }

            catch ( Exception e )
            {
                LogManager.WriteError( "StringRessource Datei konnte nicht geoffnet werden! Pfad: " + path + " Fehler: " + e.Message, "StringRessourceWriter", "StringRessourceWriter" );
            }
        }

        internal void WriterStringRessources()
        {

        }
    }
}
