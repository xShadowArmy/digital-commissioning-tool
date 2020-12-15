using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityUITable
{

	[ExecuteInEditMode]
	public class ScrollTableContainer : MonoBehaviour
	{

		public ScrollRect scrollView;

		public Transform headerContainer;

		Table _table;
		Table table
		{
			get
			{
				if (_table == null)
					_table = GetComponentInChildren<Table>();
				return _table;
			}
		}

		void Update()
		{
			if (table.horizontal)
			{
				((RectTransform)headerContainer.transform).sizeDelta = new Vector2(table.rowHeight, table.GetComponent<RectTransform>().rect.height);
				((RectTransform)scrollView.transform).anchoredPosition = new Vector2(table.rowHeight - 1, 0f);
				((RectTransform)scrollView.transform).sizeDelta = new Vector2(-table.rowHeight, 0f);
				scrollView.horizontal = true;
				scrollView.vertical = false;
			}
			else
			{
				((RectTransform)headerContainer.transform).sizeDelta = new Vector2(table.GetComponent<RectTransform>().rect.width, table.rowHeight);
				((RectTransform)scrollView.transform).anchoredPosition = new Vector2(0f, -table.rowHeight + 1);
				((RectTransform)scrollView.transform).sizeDelta = new Vector2(0f, -table.rowHeight);
				scrollView.horizontal = false;
				scrollView.vertical = true;
			}
		}

	}

}
