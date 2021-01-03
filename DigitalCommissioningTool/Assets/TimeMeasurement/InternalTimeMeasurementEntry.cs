using SystemTools;

namespace TimeMeasurement
{
    public class InternalTimeMeasurementEntry: ISerialConfigData
    {
        public int Index { get; set; }
        public string CreatedAt { get; set; }
        public float Duration { get; set; }

        public InternalTimeMeasurementEntry()
        {
            Index = 0;
            CreatedAt = null;
            Duration = 0.0f;
        }
        public InternalTimeMeasurementEntry( int index, string createdAt, float duration)
        {
            this.Index = index;
            this.CreatedAt = createdAt;
            this.Duration = duration;
        }

        public void Serialize(SerialConfigData storage)
        {
            storage.AddData(Index);
            storage.AddData(CreatedAt);
            storage.AddData(Duration);
        }

        public void Restore(SerialConfigData storage)
        {
            Index = storage.GetValueAsInt();
            CreatedAt = storage.GetValueAsString();
            Duration = storage.GetValueAsFloat();
        }
    }
}