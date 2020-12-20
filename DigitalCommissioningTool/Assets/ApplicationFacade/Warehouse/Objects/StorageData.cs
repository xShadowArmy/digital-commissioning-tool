using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectComponents.Abstraction;
using SystemFacade;
using UnityEngine;

namespace ApplicationFacade
{
    public class StorageData : GameObjectData
    {
        public delegate void StorageChangedEventHandlder( StorageData storage );

        public event StorageChangedEventHandlder StorageChanged;

        public int SlotCount { get; internal set; }

        public int LayerCount { get; internal set; }

        public Vector3 LayerSize { get; internal set; }
        
        public float LayerDistance { get; internal set; }

        public int SlotsPerLayer
        {
            get
            {
                return SlotCount / LayerCount;
            }
        }

        public static int DefaultSlotCount
        {
            get
            {
                return 16;
            }
        }

        public static int DefaultLayerCount
        {
            get
            {
                return 4;
            }
        }

        public static Vector3 DefaultLayerSize
        {
            get
            {
                return new Vector3( 0.8f, 0.04f, 2f );
            }

        }

        public static float DefaultLayerDistance
        {
            get
            {
                return 0.5f;
            }
        }

        private static System.Random RNG { get; set; }

        private ItemData[] Data { get; set; }

        internal GameObject[] Slots { get; set; }

        public ItemData[] GetItems
        {
            get
            {
                return Data;
            }
        }

        internal StorageData( ) : base( GameObjectDataType.StorageReck )
        {
            Data = new ItemData[DefaultSlotCount];
            Slots = new GameObject[DefaultSlotCount];
            SlotCount = DefaultSlotCount;
            LayerCount = DefaultLayerCount;
            LayerSize = DefaultLayerSize;
            LayerDistance = DefaultLayerDistance;
            RNG = new System.Random( );
        }

        internal StorageData( long id ) : base( GameObjectDataType.StorageReck, id )
        {
            Data = new ItemData[DefaultSlotCount];
            Slots = new GameObject[DefaultSlotCount];
            SlotCount = DefaultSlotCount;
            LayerCount = DefaultLayerCount;
            LayerSize = DefaultLayerSize;
            LayerDistance = DefaultLayerDistance;
            RNG = new System.Random( );
        }

        internal StorageData( long id, Vector3 position, Quaternion rotation, Vector3 scale ) : base( GameObjectDataType.StorageReck, id, position, rotation, scale )
        {
            Data = new ItemData[DefaultSlotCount];
            Slots = new GameObject[DefaultSlotCount];
            SlotCount = DefaultSlotCount;
            LayerCount = DefaultLayerCount;
            LayerSize = DefaultLayerSize;
            LayerDistance = DefaultLayerDistance;
            RNG = new System.Random( );
        }

        internal StorageData( long id, Vector3 position, Quaternion rotation, Vector3 scale, GameObject obj ) : base( GameObjectDataType.StorageReck, id, position, rotation, scale, obj )
        {
            Data = new ItemData[DefaultSlotCount];
            Slots = new GameObject[DefaultSlotCount];
            SlotCount = DefaultSlotCount;
            LayerCount = DefaultLayerCount;
            LayerSize = DefaultLayerSize;
            LayerDistance = DefaultLayerDistance;
            RNG = new System.Random( );
        }
        
        public ItemData GetItem( int slot )
        {
            if ( IsDestroyed( ) )
            {
                return null;
            }

            if ( slot > SlotCount )
            {
                LogManager.WriteWarning( "Ein Objekt soll aus einem Slot abgefragt werden der nicht existiert!", "StorageData", "AddItem" );

                return null;
            }

            if ( Data[slot] == null )
            {
                return null;
            }

            return Data[slot];
        }

        public ItemData GetItem( long id )
        {
            if ( IsDestroyed( ) )
            {
                return null;
            }

            for ( int i = 0; i < Data.Length; i++ )
            {
                if ( Data[i].GetID() == id )
                {
                    return Data[i];
                }
            }

            return null;
        }

        public ItemData GetItem( GameObject obj )
        {
            if ( IsDestroyed( ) )
            {
                return null;
            }

            for ( int i = 0; i < Data.Length; i++ )
            {
                if ( Data[i].Object == obj )
                {
                    return Data[ i ];
                }
            }

            return null;
        }
        
        public int GetSlot( GameObject obj )
        {
            if ( IsDestroyed( ) )
            {
                return -1;
            }

            for ( int i = 0; i < Data.Length; i++ )
            {
                if ( Data[i].Object == obj )
                {
                    return i;
                }
            }

            return -1;
        }

        public int GetSlot( ItemData item )
        {
            if ( IsDestroyed( ) )
            {
                return -1;
            }

            for ( int i = 0; i < Data.Length; i++ )
            {
                if ( Data[i].GetID() == item.GetID() )
                {
                    return i;
                }
            }

            return -1;
        }

        internal void AddItem( ItemData item, int slot )
        {
            if ( IsDestroyed( ) )
            {
                return;
            }

            if ( slot > SlotCount )
            {
                LogManager.WriteWarning( "Ein Objekt soll auf ein Slot abgelegt werden der nicht existiert!", "StorageData", "AddItem" );

                return;
            }

            Data[slot] = item;
            item.ChangeGameObject( Slots[slot] );
            Data[slot].Object.SetActive( true );
            
            OnChange( this );
        }

        internal bool RemoveItem( ItemData item )
        {
            if ( IsDestroyed( ) )
            {
                return false;
            }

            for ( int i = 0; i < Data.Length; i++ )
            {
                if ( Data[i].GetID() == item.GetID() )
                {
                    Data[i].Object.SetActive( false );                    

                    OnChange( this );

                    return true;
                }
            }

            return false;
        }

        internal bool ContainsItem( ItemData item )
        {
            if ( IsDestroyed( ) )
            {
                return false;
            }

            for ( int i = 0; i < Data.Length; i++ )
            {
                if ( Data[i].GetID() == item.GetID() )
                {
                    return true;
                }
            }

            return false;
        }

        internal void Initialize( )
        {
            CalcSlots( );
        }

        /*public void ChangeSlotCount( int slots )
        {
            if ( IsDestroyed( ) )
            {
                return;
            }

            ItemData[] tmp = Data;

            SlotCount = slots;

            Data = new ItemData[slots];

            for( int i = 0; i < Data.Length; i++ )
            {
                if ( i < tmp.Length )
                {
                    Data[i] = tmp[i];
                }
            }

            OnChange( );
        } */
        
        protected virtual void OnChange( StorageData data )
        {
            base.OnChange( );
            StorageChanged?.Invoke( data );
        }

        private void CalcSlots( )
        {
            List<GameObject> layers = new List<GameObject>( );

            int n = 0;
            float xMargin = 0.1f;
            float zInnerMargin = 0.1f;
            float zOuterMargin = 0.05f;
            float yMargin = 0.01f;

            Vector3 slotScale = new Vector3( LayerSize.x - xMargin * 2, (LayerDistance / LayerSize.y) * ( 1 - ( LayerDistance - yMargin ) ), ( LayerSize.z / 2 - zOuterMargin * 2 - ( SlotsPerLayer - 1 ) * zInnerMargin ) / SlotsPerLayer );

            foreach( Transform obj in Object.transform )
            {
                if ( obj.tag.Equals( "StorageRackLayer" ) )
                {
                    layers.Add( obj.gameObject );
                }
            }

            LayerCount = layers.Count;

            for ( int i = 0; i < layers.Count; i++ )
            {
                for( int j = 0; j < SlotsPerLayer; j++, n++ )
                {
                    switch ( RNG.Next( 0, 3 ) )
                    {
                        case 0:

                            Slots[n] = GameObject.Instantiate( GameObject.FindGameObjectWithTag( "StorageBoxOpen" ), layers[i].transform );
                            Slots[n].transform.localScale = slotScale;
                            break;

                        case 1:

                            Slots[n] = GameObject.Instantiate( GameObject.FindGameObjectWithTag( "StorageBoxPartialOpen" ), layers[i].transform );
                            Slots[n].transform.localScale = slotScale;
                            break;

                        case 2:

                            Slots[n] = GameObject.Instantiate( GameObject.FindGameObjectWithTag( "StorageBoxClosed" ), layers[i].transform );
                            Slots[n].transform.localScale = slotScale;
                            break;

                        case 3:

                            Slots[n] = GameObject.Instantiate( GameObject.FindGameObjectWithTag( "StorageContainer" ), layers[i].transform );
                            Slots[n].transform.localScale = new Vector3( slotScale.x * 2.5f, slotScale.y * 5, slotScale.z * 5 );
                            break;

                        default:

                            break;
                    }

                    Slots[n].SetActive( false );
                    Slots[n].transform.localPosition = new Vector3( -slotScale.x / 2, ( LayerDistance - yMargin ), ( -slotScale.z / 2 * SlotsPerLayer + zOuterMargin - zInnerMargin * ( SlotsPerLayer / 2f ) ) + j * zInnerMargin + j * slotScale.z );
                }
            }
        }
    }
}
