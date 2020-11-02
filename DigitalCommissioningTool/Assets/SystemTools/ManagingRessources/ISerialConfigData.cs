using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemTools.ManagingRessources
{
    /// <summary>
    /// Bietet die Möglichkeit für eine einfache Obektserialisierung für die Konfigurations Daten.
    /// </summary>
    public interface ISerialConfigData
    {
        /// <summary>
        /// Wird implementiert um Daten zu serialisieren.
        /// </summary>
        /// <param name="storage">Kann dazu verwendet werden um Daten zu serialisieren.</param>
        void Serialize( SerialConfigData storage );

        /// <summary>
        /// Wird implementiert um Daten wiederherzustellen.
        /// </summary>
        /// <param name="storage">Kann dazu verwendet werden um die Daten wiederherzustellen.</param>
        void Restore( SerialConfigData storage );
    }
}
