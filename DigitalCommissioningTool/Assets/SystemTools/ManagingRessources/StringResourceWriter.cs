using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using SystemTools.Handler;
using System.Globalization;

namespace SystemTools.ManagingResources
{
    /// <summary>
    /// Ermöglicht das Speichern von StringRessourcen.
    /// </summary>
    internal class StringResourceWriter
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
        /// Wird fuer das Schreiben von LogDateien verwendet.
        /// </summary>
        private LogHandler Logger;

        /// <summary>
        /// Initialisiert den StringResourceWriter.
        /// </summary>
        /// <param name="path">Pfad an dem die StringRessourcen liegen.</param>
        /// <param name="info">Informationen über die Systemsprache.</param>
        /// <param name="doc">Die StringRessource Datei.</param>
        internal StringResourceWriter( string path, XmlDocument doc, CultureInfo info )
        {
            Logger = new LogHandler( );

            Logger.WriteInfo( "Initialisierung des StringResourceWriter", "StringResourceWriter", "StringResourceWriter" );

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
        /// <param name="stringResources">Der Datenpuffer.</param>
        /// <returns>Gibt true zurück wenn Erfolgreich.</returns>
        public bool StoreString( string name, string content, bool overwrite, List<StringResourceReader.StringResourceData> stringResources )
        {
            StringResourceReader.StringResourceData newData;

            foreach ( StringResourceReader.StringResourceData data in stringResources )
            {
                if ( data.Name.Equals( name ) )
                {
                    if ( !overwrite )
                    {
                        return false;
                    }

                    newData = data;

                    stringResources.Remove( data );

                    newData.Value = content;

                    stringResources.Add( newData );

                    return true;
                }
            }

            newData = new StringResourceReader.StringResourceData
            {
                Name  = name,
                Value = content,
                ID    = GetNextID( stringResources )
            };

            stringResources.Add( newData );

            return true;
        }
        
        /// <summary>
        /// Schreibt den Puffer in die Datei.
        /// </summary>
        /// <param name="stringResources">Der Datenpuffer.</param>
        internal void WriteResourceFile( List<StringResourceReader.StringResourceData> stringResources )
        {
            try
            {
                string xmlns = "https://github.com/xShadowArmy/digital-commissioning-tool/tree/main/DigitalCommissioningTool/Output/Resources/";

                if ( File.Exists( Path ) )
                {
                    File.Delete( Path );
                }

                using ( StreamWriter writer = new StreamWriter( File.Create( Path ) ) )
                {
                    writer.WriteLine( "<?xml version=\"1.0\" encoding=\"utf-8\"?>" );
                    writer.WriteLine( "<xs:StringResources xs:lang=\"" + LangInfo.ThreeLetterISOLanguageName + "\" xmlns:xs=\"https://github.com/xShadowArmy/digital-commissioning-tool/tree/main/DigitalCommissioningTool/Output/Resources/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"https://github.com/xShadowArmy/digital-commissioning-tool/tree/main/DigitalCommissioningTool/Output/Resources/ StringResourceSchema.xsd\">" );
                    writer.WriteLine( "</xs:StringResources>" );
                    
                    writer.Flush( );
                }
                
                Doc.Load( Path );

                XPathNavigator nav = Doc.CreateNavigator( );

                nav.MoveToFirstChild( );

                for ( int i = 0; i < stringResources.Count; i++ )
                {
                    if ( !nav.HasChildren )
                    {
                        nav.AppendChildElement( "xs", "String", xmlns, "" );
                        nav.MoveToFirstChild( );
                        nav.CreateAttribute( "xs", "name", xmlns, stringResources[ i ].Name );
                        nav.CreateAttribute( "xs", "id", xmlns, stringResources[ i ].ID.ToString( ) );
                        nav.AppendChildElement( "xs", "Value", xmlns, stringResources[ i ].Value );
                    }

                    else
                    {
                        CreateStringRessource( nav, stringResources[ i ], xmlns );
                    }
                }

                XmlTextWriter textWriter = new XmlTextWriter( Path, Encoding.UTF8 )
                {
                    Formatting = Formatting.Indented,
                    Indentation = 4
                };

                Doc.Save( textWriter );

                textWriter.Dispose( );
            }

            catch( Exception e )
            {
                Logger.WriteLog( "Konnte StringRessourcen nicht in die Datei schreiben! Pfad: " + Path + " Fehler: " + e.Message, 3, true, "StringResourceWriter", "WriteResourceFile" );
            }
        }

        /// <summary>
        /// Gibt die nächste freie ID zurück.
        /// </summary>
        /// <param name="buffer">Der Puffer mit verwendeten IDs.</param>
        /// <returns>Gibt die nächste ID zurück.</returns>
        private long GetNextID( List<StringResourceReader.StringResourceData> buffer )
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
        private void CreateStringRessource( XPathNavigator nav, StringResourceReader.StringResourceData data, string xmlns )
        {
            nav.InsertElementAfter( "xs", "string", xmlns, "" );
            nav.MoveToNext( );
            nav.CreateAttribute( "xs", "name", xmlns, data.Name );
            nav.CreateAttribute( "xs", "id", xmlns, data.ID.ToString() );
            nav.AppendChildElement( "xs", "value", xmlns, data.Value );
        }
    }
}
