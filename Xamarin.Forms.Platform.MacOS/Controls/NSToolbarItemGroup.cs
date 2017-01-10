using System;
using System.Runtime.InteropServices;
using AppKit;
using Foundation;
using ObjCRuntime;

[Register("NSToolbarItemGroup", true)]
public partial class NSToolbarItemGroup : NSToolbarItem
{
	const string selSetSubitems_ = "setSubitems:";
	static readonly IntPtr selSetSubitems_Handle = Selector.GetHandle("setSubitems:");
	const string selSubitems = "subitems";
	static readonly IntPtr selSubitemsHandle = Selector.GetHandle("subitems");
	const string selInitWithItemIdentifier_ = "initWithItemIdentifier:";
	static readonly IntPtr selInitWithItemIdentifier_Handle = Selector.GetHandle("initWithItemIdentifier:");

	static readonly IntPtr class_ptr = Class.GetHandle("NSToolbarItemGroup");

	public override IntPtr ClassHandle { get { return class_ptr; } }

	[DllImport("/usr/lib/libobjc.dylib", EntryPoint = "objc_msgSend")]
	public extern static IntPtr IntPtr_objc_msgSend(IntPtr receiver, IntPtr selector);
	[DllImport("/usr/lib/libobjc.dylib", EntryPoint = "objc_msgSendSuper")]
	public extern static IntPtr IntPtr_objc_msgSendSuper(IntPtr receiver, IntPtr selector);
	[DllImport("/usr/lib/libobjc.dylib", EntryPoint = "objc_msgSend")]
	public extern static void void_objc_msgSend_IntPtr(IntPtr receiver, IntPtr selector, IntPtr arg1);
	[DllImport("/usr/lib/libobjc.dylib", EntryPoint = "objc_msgSendSuper")]
	public extern static void void_objc_msgSendSuper_IntPtr(IntPtr receiver, IntPtr selector, IntPtr arg1);
	[DllImport("/usr/lib/libobjc.dylib", EntryPoint = "objc_msgSend")]
	public extern static IntPtr IntPtr_objc_msgSend_IntPtr(IntPtr receiver, IntPtr selector, IntPtr arg1);
	[DllImport("/usr/lib/libobjc.dylib", EntryPoint = "objc_msgSendSuper")]
	public extern static IntPtr IntPtr_objc_msgSendSuper_IntPtr(IntPtr receiver, IntPtr selector, IntPtr arg1);

	[Export("init")]
	public NSToolbarItemGroup() : base(NSObjectFlag.Empty)
	{
		if (IsDirectBinding)
		{
			InitializeHandle(IntPtr_objc_msgSend(this.Handle, Selector.GetHandle("init")), "init");
		}
		else {
			InitializeHandle(IntPtr_objc_msgSendSuper(this.SuperHandle, Selector.GetHandle("init")), "init");
		}
	}


	[Export("initWithItemIdentifier:")]
	public NSToolbarItemGroup(string itemIdentifier)
		: base(NSObjectFlag.Empty)
	{
		global::AppKit.NSApplication.EnsureUIThread();
		if (itemIdentifier == null)
			throw new ArgumentNullException("itemIdentifier");
		var nsitemIdentifier = NSString.CreateNative(itemIdentifier);

		if (IsDirectBinding)
		{
			InitializeHandle(IntPtr_objc_msgSend_IntPtr(this.Handle, selInitWithItemIdentifier_Handle, nsitemIdentifier), "initWithItemIdentifier:");
		}
		else {
			InitializeHandle(IntPtr_objc_msgSendSuper_IntPtr(this.SuperHandle, selInitWithItemIdentifier_Handle, nsitemIdentifier), "initWithItemIdentifier:");
		}
		NSString.ReleaseNative(nsitemIdentifier);

	}

	protected NSToolbarItemGroup(NSObjectFlag t) : base(t)
	{
	}

	protected internal NSToolbarItemGroup(IntPtr handle) : base(handle)
	{
	}

	public virtual NSToolbarItem[] Subitems
	{
		[Export("subitems", ArgumentSemantic.Copy)]
		get
		{
			global::AppKit.NSApplication.EnsureUIThread();
			NSToolbarItem[] ret;
			if (IsDirectBinding)
			{
				ret = NSArray.ArrayFromHandle<NSToolbarItem>(IntPtr_objc_msgSend(this.Handle, selSubitemsHandle));
			}
			else {
				ret = NSArray.ArrayFromHandle<NSToolbarItem>(IntPtr_objc_msgSendSuper(this.SuperHandle, selSubitemsHandle));
			}
			return ret;
		}

		[Export("setSubitems:", ArgumentSemantic.Copy)]
		set
		{
			global::AppKit.NSApplication.EnsureUIThread();
			if (value == null)
				throw new ArgumentNullException("value");
			var nsa_value = NSArray.FromNSObjects(value);

			if (IsDirectBinding)
			{
				void_objc_msgSend_IntPtr(this.Handle, selSetSubitems_Handle, nsa_value.Handle);
			}
			else {
				void_objc_msgSendSuper_IntPtr(this.SuperHandle, selSetSubitems_Handle, nsa_value.Handle);
			}
			nsa_value.Dispose();

		}
	}
}