using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemTools;

namespace ProjectComponents.Abstraction
{
    public class ProjectItemData : ISerialConfigData
    {
        public int Count { get; set; }
        public double Weight { get; set; }
        public string Name { get; set; }
        public long IDRef { get; set; }
        public ProjectTransformationData Transformation { get; private set; }
         
        public ProjectItemData()
        {

        }

        public ProjectItemData( long idRef, int count, double weight, string name, ProjectTransformationData transformation )
        {
            Count = count;
            Weight = weight;
            Name = name;
            IDRef = idRef;
            Transformation = transformation;
        }
        
        public void Serialize( SerialConfigData storage )
        {
            storage.AddData( Name );
            storage.AddData( Weight );
            storage.AddData( Count );
            storage.AddData( IDRef );
        }

        public void Restore( SerialConfigData storage )
        {
            Name   = storage.GetValueAsString( );
            Weight = storage.GetValueAsDouble( );
            Count  = storage.GetValueAsInt( );
            IDRef  = storage.GetValueAsLong( );
        }
    }
}
