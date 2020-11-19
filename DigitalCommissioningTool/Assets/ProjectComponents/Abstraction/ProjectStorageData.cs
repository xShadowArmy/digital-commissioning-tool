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
        private List<ProjectItemData> Items { get; set; }

        public ProjectStorageData( long id, ProjectTransformationData transformation )
        {
            ID = id;
            Transformation = transformation;
            Items = new List<ProjectItemData>( );
        }

        public void SetID( long id )
        {
            ID = id;
        }

        public void SetTransformation( ProjectTransformationData transformation )
        {
            Transformation = transformation;
        }

        public void AddItem( ProjectItemData item )
        {
            Items.Add( item );
        }

        public bool RemoveItem( ProjectItemData item )
        {
            return Items.Remove( item );
        }

        public bool ContainsItem( ProjectItemData item )
        {
            return Items.Contains( item );
        }

        public ProjectItemData[] GetItems()
        {
            return Items.ToArray( );
        }
    }
}
