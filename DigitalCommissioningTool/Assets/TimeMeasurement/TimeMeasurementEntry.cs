using SystemTools;

<<<<<<< HEAD:DigitalCommissioningTool/Assets/ApplicationFacade/Application/TimeMeasurementEntry.cs

namespace ApplicationFacade.Application
=======
namespace TimeMeasurement
>>>>>>> fbdb78b6510546bf6ff17a91f60102851603858d:DigitalCommissioningTool/Assets/TimeMeasurement/TimeMeasurementEntry.cs
{
    public class TimeMeasurementEntry: ISerialConfigData
    {
        public int Index { get; set; }
        public string CreatedAt { get; set; }
        public float Duration { get; set; }

        public TimeMeasurementEntry()
        {
            Index = 0;
            CreatedAt = null;
            Duration = 0.0f;
        }
        public TimeMeasurementEntry(int index, string createdAt, float duration)
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