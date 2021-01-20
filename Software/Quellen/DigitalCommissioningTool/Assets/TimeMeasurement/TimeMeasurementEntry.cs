using SystemTools;

namespace TimeMeasurement
{
    public class TimeMeasurementEntry: ISerialConfigData
    {
        /// <summary>
        /// Index des Zeitmessungseintrags
        /// </summary>
        public int Index { get; set; }
        
        /// <summary>
        /// Zeitstempel wann der Einrag erstellt wurde
        /// </summary>
        public string CreatedAt { get; set; }
        
        /// <summary>
        /// Dauer der Zeitmessung in Sekunden
        /// </summary>
        public float Duration { get; set; }

        /// <summary>
        /// Erstellt eine Instanz mit Defaultwerten
        /// </summary>
        public TimeMeasurementEntry()
        {
            Index = 0;
            CreatedAt = null;
            Duration = 0.0f;
        }
        
        /// <summary>
        /// Erstellt eine Instanz und initialisiert das Objekt mit den übergebenen Werten
        /// </summary>
        /// <param name="index">Index der Zeitmessung</param>
        /// <param name="createdAt">Zeitstempel des Erstellungszeitpunkts</param>
        /// <param name="duration">Dauer der Zeitmessung in Sekunden</param>
        public TimeMeasurementEntry( int index, string createdAt, float duration)
        {
            this.Index = index;
            this.CreatedAt = createdAt;
            this.Duration = duration;
        }

        /// <summary>
        /// Serialisiert das Objekt für die Speicherung in der Config Datei
        /// </summary>
        /// <param name="storage"></param>
        public void Serialize(SerialConfigData storage)
        {
            storage.AddData(Index);
            storage.AddData(CreatedAt);
            storage.AddData(Duration);
        }

        /// <summary>
        /// Stellt aus Config Datei geladenen Eintrag wieder her
        /// </summary>
        /// <param name="storage"></param>
        public void Restore(SerialConfigData storage)
        {
            Index = storage.GetValueAsInt();
            CreatedAt = storage.GetValueAsString();
            Duration = storage.GetValueAsFloat();
        }
    }
}