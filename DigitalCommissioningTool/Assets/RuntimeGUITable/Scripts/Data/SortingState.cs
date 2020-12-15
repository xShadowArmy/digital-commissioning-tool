using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityUITable
{

	[Serializable]
	public struct SortingState
	{
		public enum SortMode { None, Ascending, Descending }
		[SerializeField] public SortMode sortMode;
		[SerializeField] public int defaultSortingColumnIndex;

		public TableColumnInfo sortingColumn { get; private set; }

		public void Init(List<TableColumnInfo> columns)
		{
			sortingColumn = columns[defaultSortingColumnIndex];
		}

		public void ClickOnColumn(TableColumnInfo column)
		{
			if (column == sortingColumn)
				sortMode = (SortMode)((((int)sortMode) + 1) % Enum.GetValues(typeof(SortMode)).Length);
			else
			{
				sortingColumn = column;
				sortMode = SortMode.Ascending;
			}
		}

		object KeySelector(object elmt)
		{
			PropertyOrFieldInfo property = new PropertyOrFieldInfo(elmt.GetType().GetMember(sortingColumn.fieldName)[0]);
			return property.GetValue(elmt);
		}

		public IEnumerable<object> GetSorted(IEnumerable<object> collection)
		{
			if (sortMode == SortMode.Ascending)
				return collection.OrderBy(KeySelector);
			else if (sortMode == SortMode.Descending)
				return collection.OrderByDescending(KeySelector);
			else
				return collection;
		}
	}

}
