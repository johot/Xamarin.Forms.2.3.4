using System;
using AppKit;
using Foundation;

namespace Xamarin.Forms.Platform.MacOS
{
	internal class TableViewDataSource : NSTableViewSource
	{
		static int s_sectionCount;

		const string headerIdentifier = nameof(TextCell);
		const string itemIdentifier = nameof(ViewCell);

		protected ITableViewController Controller => _tableView;

		readonly NSTableView _nsTableView;
		readonly TableView _tableView;

		public TableViewDataSource(TableViewRenderer tableViewRenderer)
		{
			_tableView = tableViewRenderer.Element;
			_nsTableView = tableViewRenderer.TableView;
			Controller.ModelChanged += (s, e) =>
			{
				if (_nsTableView != null)
					_nsTableView.ReloadData();
			};
			AutomaticallyDeselect = true;
		}

		public bool AutomaticallyDeselect { get; set; }

		public override void SelectionDidChange(NSNotification notification)
		{
			var row = _nsTableView.SelectedRow;
			if (row == -1)
				return;

			var sectionIndex = 0;
			var isHeader = false;
			var itemIndexInSection = 0;

			GetComputedIndexes(row, out sectionIndex, out itemIndexInSection, out isHeader);


			var cell = Controller.Model.GetCell(sectionIndex, itemIndexInSection - 1);
			Controller.Model.RowSelected(cell);
			if (AutomaticallyDeselect)
				_nsTableView.DeselectRow(row);
		}

		public override nint GetRowCount(NSTableView tableView)
		{
			nint count = 0;
			var sections = Controller.Model.GetSectionCount();
			for (int i = 0; i < sections; i++)
			{

				count += Controller.Model.GetRowCount(i) + 1;
			}

			s_sectionCount = Controller.Model.GetSectionCount();

			return count;
		}


		public override bool ShouldSelectRow(NSTableView tableView, nint row)
		{
			var sectionIndex = 0;
			var isHeader = false;
			var itemIndexInSection = 0;

			GetComputedIndexes(row, out sectionIndex, out itemIndexInSection, out isHeader);

			return !isHeader;
		}

		public override NSView GetViewForItem(NSTableView tableView, NSTableColumn tableColumn, nint row)
		{
			var sectionIndex = 0;
			var isHeader = false;
			var itemIndexInSection = 0;

			GetComputedIndexes(row, out sectionIndex, out itemIndexInSection, out isHeader);

			var id = string.Empty;
			Cell cell = null;
			if (isHeader)
			{
				id = headerIdentifier;
				cell = Controller.Model.GetHeaderCell(sectionIndex) ?? new TextCell { Text = Controller.Model.GetSectionTitle(sectionIndex) };
			}
			else
			{
				id = itemIdentifier;
				cell = Controller.Model.GetCell(sectionIndex, itemIndexInSection - 1);
			}

			var nativeCell = CellNSView.GetNativeCell(tableView, cell, id, isHeader);
			return nativeCell;
		}

		void GetComputedIndexes(nint row, out int sectionIndex, out int itemIndexInSection, out bool isHeader)
		{
			var totalItems = 0;
			isHeader = false;
			sectionIndex = 0;
			itemIndexInSection = 0;

			for (int i = 0; i < s_sectionCount; i++)
			{
				var itemsInSection = Controller.Model.GetRowCount(i) + i + 1;

				if (row < totalItems + itemsInSection)
				{
					sectionIndex = i;
					itemIndexInSection = (int)row - totalItems;
					isHeader = itemIndexInSection == 0;
					break;
				}
				totalItems += itemsInSection;
			}
		}
	}

	//TODO: Implement Uneven rows
	internal class UnEvenTableViewModelRenderer : TableViewDataSource
	{
		public UnEvenTableViewModelRenderer(TableViewRenderer model) : base(model)
		{
		}
	}
}
