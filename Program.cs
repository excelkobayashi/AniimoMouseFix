using System.Runtime.InteropServices;
using System.Text;

namespace AniimoMouseFix;

internal class Program
{
	private const string WindowClass = "Qt5159QWindow";
	private const string WindowTitle = "Aniimo";

	public static void Main(string[] args)
	{
		Console.WriteLine("Running fix every 30 seconds...");
		while(true)
		{
			Thread.Sleep(TimeSpan.FromSeconds(30));
			SearchWindows(IntPtr.Zero);
		}
	}

	private static void SearchWindows(IntPtr parent)
	{
		Console.WriteLine($"Searching for windows blocking mouse from {parent:X}");

		IntPtr handle = IntPtr.Zero;
		bool found = false;

		do
		{
			handle = Win.FindWindowEx(parent, handle, WindowClass, WindowTitle);
			if(handle == IntPtr.Zero)
				break;

			HideWindow(handle);
			SearchWindows(handle);

			found = true;
		} while(true);

		if(!found)
			Console.WriteLine("- no windows found");
	}

	private static void HideWindow(IntPtr handle)
	{
		Console.Write($"Hiding {handle:X}... ");
		bool hid = Win.ShowWindow(handle, Win.SW_HIDE);

		if(hid)
			Console.WriteLine("done");
		else
			Console.WriteLine("already hidden");

		CheckParent(handle);
	}

	private static void CheckParent(IntPtr handle)
	{
		IntPtr parent = Win.GetParent(handle);
		if(parent == IntPtr.Zero)
			return;

		StringBuilder className = new(32);
		int ret = Win.GetClassName(parent, className, className.Capacity);
		if(ret == 0)
			return;

		if(className.ToString() == WindowClass)
			HideWindow(parent);
	}

	private static class Win
	{
		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr FindWindowEx(IntPtr parent, IntPtr after, string className, string title);

		[DllImport("user32.dll")]
		public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		[DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
		public static extern IntPtr GetParent(IntPtr hWnd);

		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

		public const int SW_HIDE = 0;
	}
}
