using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using AppKit;
using Foundation;

namespace Xamarin.Forms.Platform.MacOS
{
	internal class ListViewDataSource : NSTableViewSource
	{
		IVisualElementRenderer _prototype;
		const string GroupHeaderCellKey = "GroupHeaderCell";
		const int DefaultItemTemplateId = 1;
		static int s_dataTemplateIncrementer = 2; // lets start at not 0 because
		static int s_sectionCount;
		static int s_totalCount;
		readonly nfloat _defaultSectionHeight;
		readonly Dictionary<DataTemplate, int> _templateToId = new Dictionary<DataTemplate, int>();
		readonly NSTableView _nsTableView;
		protected readonly ListView List;
		IListViewController Controller => List;
		ITemplatedItemsView<Cell> TemplatedItemsView => List;
		bool _selectionFromNative;
		bool _selectionFromForms;

		public virtual bool IsGroupingEnabled => List.IsGroupingEnabled;
		public Dictionary<int, int> Counts { get; set; }

		public ListViewDataSource(ListViewDataSource source)
		{
			List = source.List;
			_nsTableView = source._nsTableView;
			_defaultSectionHeight = source._defaultSectionHeight;
			_selectionFromNative = source._selectionFromNative;
			Counts = new Dictionary<int, int>();
		}

		public ListViewDataSource(ListView list, NSTableView tableView)
		{
			List = list;
			List.ItemSelected += OnItemSelected;
			_nsTableView = tableView;
			Counts = new Dictionary<int, int>();
		}

		public void Update()
		{
			_nsTableView.ReloadData();
		}

		public void OnItemSelected(object sender, SelectedItemChangedEventArgs eventArg)
		{
			if (_selectionFromNative)
			{
				_selectionFromNative = false;
				return;
			}

			var location = TemplatedItemsView.TemplatedItems.GetGroupAndIndexOfItem(eventArg.SelectedItem);
			if (location.Item1 == -1 || location.Item2 == -1)
			{
				var row = _nsTableView.SelectedRow;
				int groupIndex = 1;
				var selectedIndexPath = NSIndexPath.FromItemSection(row, groupIndex);
				if (selectedIndexPath != null)
					_nsTableView.DeselectRow(selectedIndexPath.Item);
				return;
			}
			_selectionFromForms = true;
			var groupId = location.Item1;
			var rowId = location.Item2;

			_nsTableView.SelectRow(rowId, false);
		}

		public override void SelectionDidChange(NSNotification notification)
		{
			if (_selectionFromForms)
			{
				_selectionFromForms = false;
				return;
			}

			var selectedRow = _nsTableView.SelectedRow;
			if (selectedRow == -1)
				return;

			NSIndexPath indexPath = null;
			Cell cell = null;
			indexPath = GetPathFromRow(selectedRow, ref cell);

			if (cell == null)
				return;

			_selectionFromNative = true;
			Controller.NotifyRowTapped((int)indexPath.Section, (int)indexPath.Item, cell);
		}

		public override bool IsGroupRow(NSTableView tableView, nint row)
		{
			if (!IsGroupingEnabled)
				return false;

			var sectionIndex = 0;
			var isGroupHeader = false;
			var itemIndexInSection = 0;

			GetComputedIndexes(row, out sectionIndex, out itemIndexInSection, out isGroupHeader);
			return isGroupHeader;
		}

		public override bool ShouldSelectRow(NSTableView tableView, nint row)
		{
			return !IsGroupRow(tableView, row);
		}

		public override nfloat GetRowHeight(NSTableView tableView, nint row)
		{
			if (!List.HasUnevenRows)
				return List.RowHeight == -1 ? ListViewRenderer.DefaultRowHeight : List.RowHeight;

			NSIndexPath indexPath = null;
			Cell cell = null;
			indexPath = GetPathFromRow(row, ref cell);

			return CalculateHeightForCell(tableView, cell);
		}

		public override nint GetRowCount(NSTableView tableView)
		{
			var templatedItems = TemplatedItemsView.TemplatedItems;
			nint count = 0;

			if (!IsGroupingEnabled)
			{
				count = templatedItems.Count;
			}
			else
			{
				var sections = templatedItems.Count;
				for (int i = 0; i < sections; i++)
				{
					var group = (IList)((IList)templatedItems)[i];
					count += group.Count + 1;
				}
				s_sectionCount = sections;

			}
			s_totalCount = (int)count;
			return count;
		}

		public override NSView GetViewForItem(NSTableView tableView, NSTableColumn tableColumn, nint row)
		{
			var sectionIndex = 0;
			var itemIndexInSection = (int)row;
			var id = string.Empty;
			Cell cell = null;

			var isHeader = false;

			if (IsGroupingEnabled)
				GetComputedIndexes(row, out sectionIndex, out itemIndexInSection, out isHeader);

			var indexPath = NSIndexPath.FromItemSection(itemIndexInSection, sectionIndex);
			id = isHeader ? "headerCell" : TemplateIdForPath(indexPath).ToString();
			cell = GetCellForPath(indexPath, isHeader);
			var nativeCell = CellNSView.GetNativeCell(tableView, cell, id);
			return nativeCell;
		}

		protected virtual Cell GetCellForPath(NSIndexPath indexPath, bool isGroupHeader)
		{
			var templatedItems = TemplatedItemsView.TemplatedItems;
			if (IsGroupingEnabled)
				templatedItems = (TemplatedItemsList<ItemsView<Cell>, Cell>)((IList)templatedItems)[(int)indexPath.Section];

			var cell = isGroupHeader ? templatedItems.HeaderContent : templatedItems[(int)indexPath.Item];
			return cell;
		}

		int TemplateIdForPath(NSIndexPath indexPath)
		{
			var itemTemplate = List.ItemTemplate;
			var selector = itemTemplate as DataTemplateSelector;
			if (selector == null)
				return DefaultItemTemplateId;

			var templatedList = TemplatedItemsView.TemplatedItems;
			if (List.IsGroupingEnabled)
				templatedList = (TemplatedItemsList<ItemsView<Cell>, Cell>)((IList)templatedList)[(int)indexPath.Section];

			var item = templatedList.ListProxy[(int)indexPath.Item];

			itemTemplate = selector.SelectTemplate(item, List);
			int key;
			if (!_templateToId.TryGetValue(itemTemplate, out key))
			{
				s_dataTemplateIncrementer++;
				key = s_dataTemplateIncrementer;
				_templateToId[itemTemplate] = key;
			}
			return key;
		}

		NSIndexPath GetPathFromRow(nint row, ref Cell cell)
		{
			NSIndexPath indexPath;
			if (IsGroupingEnabled)
			{
				var sectionIndex = 0;
				var isGroupHeader = false;
				var itemIndexInSection = 0;

				GetComputedIndexes(row, out sectionIndex, out itemIndexInSection, out isGroupHeader);
				indexPath = NSIndexPath.FromItemSection(itemIndexInSection, sectionIndex);
			}
			else
			{
				indexPath = NSIndexPath.FromItemSection(row, 0);
				cell = GetCellForPath(indexPath, false);
			}

			return indexPath;
		}

		nfloat CalculateHeightForCell(NSTableView tableView, Cell cell)
		{
			var viewCell = cell as ViewCell;
			double renderHeight = -1;
			if (List.RowHeight == -1 && viewCell != null && viewCell.View != null)
			{
				var target = viewCell.View;
				if (_prototype == null)
				{
					_prototype = Platform.CreateRenderer(target);
					Platform.SetRenderer(target, _prototype);
				}
				else
				{
					_prototype.SetElement(target);
					Platform.SetRenderer(target, _prototype);
				}

				var req = target.Measure(tableView.Frame.Width, double.PositiveInfinity, MeasureFlags.IncludeMargins);

				target.ClearValue(Platform.RendererProperty);
				foreach (var descendant in target.Descendants())
					descendant.ClearValue(Platform.RendererProperty);

				renderHeight = req.Request.Height;
			}
			else
			{
				renderHeight = cell.RenderHeight;
			}

			return renderHeight > 0 ? (nfloat)renderHeight : ListViewRenderer.DefaultRowHeight;
		}

		void GetComputedIndexes(nint row, out int sectionIndex, out int itemIndexInSection, out bool isHeader)
		{
			var templatedItems = TemplatedItemsView.TemplatedItems;
			var totalItems = 1;
			isHeader = false;
			sectionIndex = 0;
			itemIndexInSection = 0;

			for (int i = 0; i < s_sectionCount; i++)
			{
				var group = (IList)((IList)templatedItems)[i];
				var itemsInSection = group.Count + i;

				if (row < totalItems + itemsInSection)
				{
					sectionIndex = i;
					itemIndexInSection = (int)row - totalItems;
					isHeader = itemIndexInSection == -1;
					break;
				}
				totalItems += itemsInSection;
			}
		}
	}
}
