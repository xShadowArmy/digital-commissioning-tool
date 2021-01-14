using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemTools;

namespace ProjectComponents.Abstraction
{
    /// <summary>
    /// Darstellung eines Items in den Projektdateien.
    /// </summary>
    public class ProjectItemData : ISerialConfigData
    {
        /// <summary>
        /// Die Anzahl des Items.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Das Gewicht des Items.
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// Der Name des Items.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Die ID des Items.
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// Die ID des Parent Items.
        /// </summary>
        public long ParentItemID { get; set; }

        /// <summary>
        /// Die ID des Regals vom Parent Item.
        /// </summary>
        public long ParentStorageID { get; set; }

        /// <summary>
        /// Die ID Referenz des Parent Items.
        /// </summary>
        public long IDRef { get; set; }

        /// <summary>
        /// Gibt an ob sich das Item gerade in der Queue befindet.
        /// </summary>
        public bool InQueue { get; set; }

        /// <summary>
        /// Gibt die Position in der Queue an.
        /// </summary>
        public int QueuePosition { get; set; }

        /// <summary>
        /// Die Transformationsdaten des Items.
        /// </summary>
        public ProjectTransformationData Transformation { get; private set; }
         
        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        public ProjectItemData()
        {

        }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        /// <param name="idRef">Die ID Referenz des Parent Items.</param>
        /// <param name="id">Die ID des Items.</param>
        /// <param name="count">Die Anzahl des Items.</param>
        /// <param name="weight">Das Gewicht des Items.</param>
        /// <param name="name">Der Name des Items.</param>
        /// <param name="transformation">Die Transformationsdaten des Items.</param>
        public ProjectItemData( long idRef, long id, int count, double weight, string name, bool inQueue, int queuePosition, ProjectTransformationData transformation )
        {
            ID = id;
            Count = count;
            Weight = weight;
            Name = name;
            IDRef = idRef;
            Transformation = transformation;
            InQueue = inQueue;
            QueuePosition = queuePosition;
            ParentItemID = 0;
            ParentStorageID = 0;
        }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        /// <param name="idRef">Die ID Referenz des Parent Items.</param>
        /// <param name="id">Die ID des Items.</param>
        /// <param name="count">Die Anzahl des Items.</param>
        /// <param name="weight">Das Gewicht des Items.</param>
        /// <param name="name">Der Name des Items.</param>
        /// <param name="transformation">Die Transformationsdaten des Items.</param>
        /// <param name="inQueue">Gibe an ob sich das Item in der Queue befindet.</param>
        /// <param name="queuePosition">Gibt die Position des Items in der Queue an.</param>
        /// <param name="parentItemID">Die ID des Parents.</param>
        /// <param name="parentStorageID">Die ID des Parent Regals.</param>
        public ProjectItemData( long idRef, long id, int count, double weight, string name, bool inQueue, int queuePosition, long parentItemID, long parentStorageID, ProjectTransformationData transformation )
        {
            ID = id;
            Count = count;
            Weight = weight;
            Name = name;
            IDRef = idRef;
            Transformation = transformation;
            InQueue = inQueue;
            QueuePosition = queuePosition;
            ParentItemID = parentItemID;
            ParentStorageID = parentStorageID;
        }
        
        /// <summary>
        /// Serialisiert das Objekt.
        /// </summary>
        /// <param name="storage">Wird zum Speichern der Werte verwendet.</param>
        public void Serialize( SerialConfigData storage )
        {
            storage.AddData( Name );
            storage.AddData( Weight );
            storage.AddData( Count );
            storage.AddData( IDRef );
        }

        /// <summary>
        /// Stellt die Werte wieder her.
        /// </summary>
        /// <param name="storage">Wird zum wiederherstellen der Items verwendet.</param>
        public void Restore( SerialConfigData storage )
        {
            Name   = storage.GetValueAsString( );
            Weight = storage.GetValueAsDouble( );
            Count  = storage.GetValueAsInt( );
            IDRef  = storage.GetValueAsLong( );
        }
    }
}
