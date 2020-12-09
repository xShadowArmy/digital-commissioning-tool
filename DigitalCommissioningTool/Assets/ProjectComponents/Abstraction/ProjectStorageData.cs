using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectComponents.Abstraction
{
    public struct ProjectStorageData
    {
        public long ID { get; set; }
        public ProjectTransformationData Transformation { get; set; }
        public List<ProjectItemData> Items { get; set; }

        public ProjectItemData[] GetItems
        {
            get
            {
                return Items.ToArray( );
            }
        }

        public ProjectStorageData( long id, ProjectTransformationData transformation )
        {
            ID = id;
            Transformation = transformation;
            Items = new List<ProjectItemData>( );
        }        
    }
}
