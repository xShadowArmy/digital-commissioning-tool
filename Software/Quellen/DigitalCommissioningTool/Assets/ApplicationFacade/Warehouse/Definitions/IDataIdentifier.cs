using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationFacade.Warehouse
{
    /// <summary>
    /// Bietet ein einheitliches Interface für Objekte die eindeutig Identifizierbar sein sollen.
    /// </summary>
    public interface IDataIdentifier
    {
        /// <summary>
        /// Ändert die ID.
        /// </summary>
        /// <param name="id">Die neue ID.</param>
        void SetID( long id );

        /// <summary>
        /// Gibt die gespeichert ID zurück.
        /// </summary>
        /// <returns>Die gespeichert ID.</returns>
        long GetID();
    }
}
