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
    /// <summary>
    /// Ermöglicht das Speichern von StringRessourcen.
    /// </summary>
    internal class StringRessourceWriter
    {
        /// <summary>
        /// Repräsentiert die StringRessource Xml Datei.
        /// </summary>
        private XmlDocument Doc { get; set; }

        /// <summary>
        /// Informationen zur aktuellen Systemsprache.
        /// </summary>
        private CultureInfo LangInfo { get; set; }

        /// <summary>
        /// Der Pfad an dem die StringRessourcen liegen.
        /// </summary>
        private string Path { get; set; }

        /// <summary>
        /// Initialisiert den StringRessourceWriter.
        /// </summary>
        /// <param name="path">Pfad an dem die StringRessourcen liegen.</param>
        /// <param name="info">Informationen über die Systemsprache.</param>
        /// <param name="doc">Die StringRessource Datei.</param>
        internal StringRessourceWriter( string path, XmlDocument doc, CultureInfo info )
        {
            LogManager.WriteInfo( "Initialisierung des StringRessourceWriter", "StringRessourceWriter", "StringRessourceWriter" );

            Path     = path;
            LangInfo = info;
            Doc      = doc;
        }
        
        /// <summary>
        /// Speichert eine StringRessource in den Puffer.
        /// </summary>
        /// <param name="name">Der Schlüssel der StringRessource.</param>
        /// <param name="content">Der String.</param>
        /// <param name="overwrite">Gibt an ob die Ressource überschrieben werden soll.</param>
        /// <param name="stringRessources">Der Datenpuffer.</param>
        /// <returns>Gibt true zurück wenn Erfolgreich.</returns>
        public bool StoreString( string name, string content, bool overwrite, List<StringRessourceReader.StringRessourceData> stringRessources )
        {
            StringRessourceReader.StringRessourceData newData;

            foreach ( StringRessourceReader.StringRessourceData data in stringRessources )
            {
                if ( data.Name.Equals( name ) )
                {
                    if ( !overwrite )
                    {
                        return false;
                    }

                    newData = data;

                    stringRessources.Remove( data );

                    newData.Value = content;

                    stringRessources.Add( newData );

                    return true;
                }
            }

            newData = new StringRessourceReader.StringRessourceData
            {
                Name  = name,
                Value = content,
                ID    = GetNextID( stringRessources )
            };

            stringRessources.Add( newData );

            return true;
        }
        
        /// <summary>
        /// Schreibt den Puffer in die Datei.
        /// </summary>
        /// <param name="stringRessources">Der Datenpuffer.</param>
        internal void WriteRessourceFile( List<StringRessourceReader.StringRessourceData> stringRessources )
        {
            try
            {
                string xmlns = "https://github.com/xShadowArmy/digital-commissioning-tool/tree/main/DigitalCommissioningTool/Output/Ressources/Strings";

                if ( File.Exists( Path ) )
                {
                    File.Delete( Path );
                }

                using ( StreamWriter writer = new StreamWriter( File.Create( Path ) ) )
                {
                    writer.WriteLine( "<?xml version=\"1.0\" encoding=\"utf-8\"?>" );
                    writer.WriteLine( "<xs:StringRessources xs:lang=\"" + LangInfo.ThreeLetterISOLanguageName + "\" xmlns:xs=\"https://github.com/xShadowArmy/digital-commissioning-tool/tree/main/DigitalCommissioningTool/Output/Ressources/Strings\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"https://github.com/xShadowArmy/digital-commissioning-tool/tree/main/DigitalCommissioningTool/Output/Ressources/Strings StringRessourceSchema.xsd\">" );
                    writer.WriteLine( "</xs:StringRessources>" );

                    writer.Flush( );
                }

                Doc.Load( Path );

                XPathNavigator nav = Doc.CreateNavigator( );

                nav.MoveToFirstChild( );

                for( int i = 0; i < stringRessources.Count; i++ )
                {
                    if ( !nav.HasChildren )
                    {
                        nav.AppendChildElement( "xs", "String", xmlns, "" );
                        nav.MoveToFirstChild( );
                        nav.CreateAttribute( "xs", "name", xmlns, stringRessources[i].Name );
                        nav.CreateAttribute( "xs", "id", xmlns, stringRessources[ i ].ID.ToString( ) );
                        nav.AppendChildElement( "xs", "Value", xmlns, stringRessources[ i ].Value );
                    }

                    else
                    {
                        CreateStringRessource( nav, stringRessources[i], xmlns );
                    }
                }
                               
                XmlTextWriter textWriter = new XmlTextWriter( Path, Encoding.UTF8 )
                {
                    Formatting  = Formatting.Indented,
                    Indentation = 4
                };

                Doc.Save( textWriter );

                textWriter.Dispose();
            }

            catch( Exception e )
            {
                LogManager.WriteLog( "Konnte StringRessourcen nicht in die Datei schreiben! Pfad: " + Path + " Fehler: " + e.Message, LogLevel.Error, true, "StringRessourceWriter", "WriteRessourceFile" );
            }
        }

        /// <summary>
        /// Gibt die nächste freie ID zurück.
        /// </summary>
        /// <param name="buffer">Der Puffer mit verwendeten IDs.</param>
        /// <returns>Gibt die nächste ID zurück.</returns>
        private long GetNextID( List<StringRessourceReader.StringRessourceData> buffer )
        {
            bool used = false;
            long[ ] ids = new long[ buffer.Count ];

            for ( int i = 0; i < ids.Length; i++ )
            {
                ids[ i ] = buffer[ i ].ID;
            }

            for ( int i = 0; ; i++ )
            {
                used = false;

                for ( int j = 0; j < ids.Length; j++ )
                {
                    if ( i + 1 == ids[ j ] )
                    {
                        used = true;
                        break;
                    }
                }

                if ( !used )
                {
                    return i + 1;
                }
            }
        }

        /// <summary>
        /// Schreibt einen String in die Datei.
        /// </summary>
        /// <param name="nav">Ein Positioniertes Navigator objekt.</param>
        /// <param name="data">Die StringRessource.</param>
        /// <param name="xmlns">Der Xml Namespace.</param>
        private void CreateStringRessource( XPathNavigator nav, StringRessourceReader.StringRessourceData data, string xmlns )
        {
            nav.InsertElementAfter( "xs", "string", xmlns, "" );
            nav.MoveToNext( );
            nav.CreateAttribute( "xs", "name", xmlns, data.Name );
            nav.CreateAttribute( "xs", "id", xmlns, data.ID.ToString() );
            nav.AppendChildElement( "xs", "value", xmlns, data.Value );
        }
    }
}
