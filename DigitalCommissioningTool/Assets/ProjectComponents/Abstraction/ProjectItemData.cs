using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectComponents.Abstraction
{
    public struct ProjectItemData
    {
        public int Count { get; set; }
        public double Weight { get; set; }
        public string Name { get; set; }
        public long IDRef { get; set; }
        public ProjectTransformationData Transformation { get; private set; }
         
        public ProjectItemData( long idRef, int count, double weight, string name, ProjectTransformationData transformation )
        {
            Count = count;
            Weight = weight;
            Name = name;
            IDRef = idRef;
            Transformation = transformation;
        }
    }
}
