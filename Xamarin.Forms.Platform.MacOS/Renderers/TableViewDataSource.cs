using System;
using System.Collections.Generic;
using AppKit;

namespace Xamarin.Forms.Platform.MacOS
{
	internal class TableViewDataSource : NSTableViewSource
	{
		readonly Dictionary<nint, Cell> _headerCells = new Dictionary<nint, Cell>();

		protected ITableViewController Controller => View;

		protected bool HasBoundGestures;
		protected NSTableView Table;

		protected TableView View;

		public TableViewDataSource(TableView model)
		{
			View = model;
			Controller.ModelChanged += (s, e) =>
			{
				if (Table != null)
					Table.ReloadData();
			};
			AutomaticallyDeselect = true;
		}

		public bool AutomaticallyDeselect { get; set; }

		//public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		//{
		//	Controller.Model.RowSelected(indexPath.Section, indexPath.Row);
		//	if (AutomaticallyDeselect)
		//		tableView.DeselectRow(indexPath, true);
		//}

		public override nint GetRowCount(NSTableView tableView)
		{
			nint count = 0;
			var sections = Controller.Model.GetSectionCount();
			for (int i = 0; i < sections; i++)
			{

				count += Controller.Model.GetRowCount(i) + 1;
			}

			count = Controller.Model.GetRowCount(0);
			return count;
		}
		public override NSView GetViewForItem(NSTableView tableView, NSTableColumn tableColumn, nint row)
		{
			var id = "Hello";
			var cell = Controller.Model.GetCell(0, (int)row);

			var nativeCell = CellNSView.GetNativeCell(tableView, cell, id.ToString());
			return nativeCell;
		}
	}

	internal class UnEvenTableViewModelRenderer : TableViewDataSource
	{
		public UnEvenTableViewModelRenderer(TableView model) : base(model)
		{
		}

		//public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		//{
		//	var cell = View.Model.GetCell(indexPath.Section, indexPath.Row);
		//	var h = cell.Height;

		//	if (View.RowHeight == -1 && h == -1 && cell is ViewCell && Forms.IsiOS8OrNewer) {
		//		return UITableView.AutomaticDimension;
		//	} else if (h == -1)
		//		return tableView.RowHeight;
		//	return (nfloat)h;
		//}
	}
}
