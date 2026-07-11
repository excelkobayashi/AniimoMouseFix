using System.Runtime.InteropServices;
using System.Text;

namespace AniimoMouseFix;

internal class Program
{
	private const string WindowClass = "Qt5159QWindow";
	private const string WindowTitle = "Aniimo";
	private const int Attempts = 100;
	private const int RepeatSecs = 10;

	public static void Main(string[] args)
	{
		Console.WriteLine($"Attempting fix every {RepeatSecs} seconds...");
		for(int attempt = 1; attempt <= RepeatSecs; attempt++)
		{
			Thread.Sleep(TimeSpan.FromSeconds(RepeatSecs));
			Console.WriteLine($"Attempt {attempt} / {Attempts}");
			bool found = SearchWindows();
			if(found)
			{
				Console.WriteLine("Done");
				return;
			}
		}

		Console.WriteLine("Giving up");
	}

	private static bool SearchWindows()
	{
		Console.Write($"Searching for windows blocking mouse... ");

		IntPtr handle = Win.FindWindowEx(IntPtr.Zero, IntPtr.Zero, WindowClass, WindowTitle);
		if(handle == IntPtr.Zero)
		{
			Console.WriteLine("not found");
			return false;
		}

		Console.WriteLine("found");
		return HideWindow(handle);
	}

	private static bool HideWindow(IntPtr handle)
	{
		Console.Write($"Hiding {handle:X}... ");
		bool hid = Win.ShowWindow(handle, Win.SW_HIDE);

		if(hid)
			Console.WriteLine("done");
		else
			Console.WriteLine("already hidden");

		bool hidParent = CheckParent(handle);
		return hid || hidParent;
	}

	private static bool CheckParent(IntPtr handle)
	{
		IntPtr parent = Win.GetParent(handle);
		if(parent == IntPtr.Zero)
			return false;

		StringBuilder className = new(32);
		int ret = Win.GetClassName(parent, className, className.Capacity);
		if(ret == 0)
			return false;

		if(className.ToString() != WindowClass)
			return false;

		return HideWindow(parent);
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
