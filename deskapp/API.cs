using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Security.Cryptography.Xml;

namespace deskapp
{
	enum MOVE
	{
		UP,
		DOWN,
		LEFT,
		RIGHT
	}
	class API
	{
		enum WindowMessage
		{
			WM_KEYDOWN = 0x0100,
		}

		public const uint PROCESS_VM_OPERATION = 0x8;
		public const uint PROCESS_VM_READ = 0x10;
		public const uint PROCESS_VM_WRITE = 0x20;
		public const uint MEM_RESERVE = 0x2000;
		public const uint MEM_COMMIT = 0x1000;
		public const uint PAGE_READWRITE = 0x4;
		public const int LVM_GETITEM = 0x1005;
		public const int LVM_GETITEMPOSITION = 0x1010;
		public const int LVM_SETITEMPOSITION = 0x100F;
		public const uint MEM_RELEASE = 0x8000;

		public const uint LVM_FIRST = 0x1000;
		public const uint LVM_GETITEMCOUNT = LVM_FIRST + 4;
		public const uint LVM_GETITEMW = LVM_FIRST + 75;

		public const int LVIF_TEXT = 0x0001;

		[DllImport("user32.dll")]
		public static extern IntPtr FindWindow(string strclassName, string strWindowName);

		[DllImport("user32", EntryPoint = "FindWindowEx")]
		public static extern IntPtr FindWindowEx(IntPtr hWnd1, IntPtr hWnd2, string lpsz1, string lpsz2);

		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, uint wMsg, int wParam, int lParam);

		[DllImport("user32.dll")]
		public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

		[DllImport("kernel32.dll")]
		public static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

		[DllImport("kernel32.dll")]
		public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

		[DllImport("kernel32.dll")]
		public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, int nSize, ref uint vNumberOfBytesRead);

		[DllImport("kernel32.dll")]
		public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, int nSize, ref uint vNumberOfBytesRead);

		[DllImport("kernel32.dll")]
		public static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint dwFreeType);

		[DllImport("kernel32.dll")]
		public static extern bool CloseHandle(IntPtr hObject);

		// アイコンアイテムの情報を所持する構造体
		// 構造体の情報は→を参照 https://docs.microsoft.com/ja-jp/windows/win32/api/commctrl/ns-commctrl-lvitema
		public struct LVITEM
		{
			public int mask;
			public int iItem;
			public int iSubItem;
			public int state;
			public int stateMask;
			public IntPtr pszText;
			public int cchTextMax;
			public int iImage;
			public IntPtr lParam;
			public int iIndent;
			public int iGroupId;
			public int cColumns;
			public IntPtr puColumns;
			public IntPtr piColFmt;
			public int iGroup;
		}

		public static int MakeLParam(int wLow, int wHigh)
		{
			return (((short)wHigh << 16) | (wLow & 0xffff));
		}

		public static void GetIconPosition(MOVE Move, string NAME)
		{
			// デスクトップのウィンドウハンドルを取得する
			IntPtr hWndDesktop;
			hWndDesktop = FindWindow("Progman", "Program Manager");
			hWndDesktop = FindWindowEx(hWndDesktop, IntPtr.Zero, "SHELLDLL_DefView", null);
			hWndDesktop = FindWindowEx(hWndDesktop, IntPtr.Zero, "SysListView32", null);

			if (hWndDesktop == IntPtr.Zero)
			{
				// 壁紙を変更した場合等Program Manager→SHELLDLL_DefView→SysListView32ではなく
				// WorkerW→SHELLDLL_DefView→SysListView32になることがあるので、両方試す。
				// WorkerWは複数あるのでループして目的の物が見つかるまで探す。
				IntPtr hWorkerW = IntPtr.Zero;
				IntPtr hShellViewWin = IntPtr.Zero;

				do
				{
					hWorkerW = FindWindowEx(IntPtr.Zero, hWorkerW, "WorkerW", null);
					hShellViewWin = FindWindowEx(hWorkerW, IntPtr.Zero, "SHELLDLL_DefView", null);
				} while (hShellViewWin == IntPtr.Zero && hWorkerW != IntPtr.Zero);

				hWndDesktop = FindWindowEx(hShellViewWin, IntPtr.Zero, "SysListView32", null);
			}

			if (hWndDesktop == IntPtr.Zero)
			{
				MessageBox.Show("Desktop hwnd could not be retrieved.", "Error");
				return;
			}

			// アイコンの数を取得
			int iconCount = SendMessage(hWndDesktop, LVM_GETITEMCOUNT, 0, 0);

			// デスクトップ(explorer.exe)のプロセスIDを取得する
			uint dwProcessId;
			GetWindowThreadProcessId(hWndDesktop, out dwProcessId);

			// 取得したプロセスIDをオープンする
			IntPtr hProcess = OpenProcess(PROCESS_VM_OPERATION | PROCESS_VM_READ | PROCESS_VM_WRITE, false, dwProcessId);
			if (hProcess == null)
			{
				MessageBox.Show("Desktop process could not be retrieved.", "Error");
				return;
			}

			// hProcessに関連するメモリーを確保する
			IntPtr pProcInfo = VirtualAllocEx(hProcess, IntPtr.Zero, 4096, MEM_RESERVE | MEM_COMMIT, PAGE_READWRITE);

			for (int i = 0; i < iconCount; i++)
			{
				byte[] iconNameBytes = new byte[256];

				LVITEM[] vItem = new LVITEM[1];
				vItem[0].mask = LVIF_TEXT;
				vItem[0].iItem = i;
				vItem[0].iSubItem = 0;
				vItem[0].cchTextMax = iconNameBytes.Length;
				vItem[0].pszText = (IntPtr)((int)pProcInfo + Marshal.SizeOf(typeof(LVITEM)));   // LVITEMの後ろの領域を指す領域とする
				uint vNumberOfBytesRead = 0;

				// vItemに情報を書き込む
				// Marshal.UnsafeAddrOfPinnedArrayElement(vItem, 0)は配列vItemの0番目を意味する
				WriteProcessMemory(hProcess, pProcInfo, Marshal.UnsafeAddrOfPinnedArrayElement(vItem, 0), Marshal.SizeOf(typeof(LVITEM)), ref vNumberOfBytesRead);

				// インデックスiのアイテムを取得する
				SendMessage(hWndDesktop, LVM_GETITEMW, i, pProcInfo.ToInt32());
				ReadProcessMemory(hProcess, (IntPtr)((int)pProcInfo + Marshal.SizeOf(typeof(LVITEM))), Marshal.UnsafeAddrOfPinnedArrayElement(iconNameBytes, 0), iconNameBytes.Length, ref vNumberOfBytesRead);

				// byte配列からアイコンの名前を取得する
				string vText = Encoding.Unicode.GetString(iconNameBytes, 0, (int)vNumberOfBytesRead);
				// \0以降にもデータがある可能性があるため最初の\0(=文字列の末尾)まで抜き出してそれをアイコン名とする
				int endOfTextIndex = vText.IndexOf('\0');
				string IconName = vText.Substring(0, endOfTextIndex);

				// アイコン位置を取得する
				SendMessage(hWndDesktop, LVM_GETITEMPOSITION, i, pProcInfo.ToInt32());
				Point[] vPoint = new Point[1];
				ReadProcessMemory(hProcess, pProcInfo, Marshal.UnsafeAddrOfPinnedArrayElement(vPoint, 0), Marshal.SizeOf(typeof(Point)), ref vNumberOfBytesRead);

				Point IconLocation = vPoint[0];

				// DEBUG:アイテム情報をすべて取得してみる
				SendMessage(hWndDesktop, LVM_GETITEM, i, pProcInfo.ToInt32());
				ReadProcessMemory(hProcess, pProcInfo, Marshal.UnsafeAddrOfPinnedArrayElement(vItem, 0), Marshal.SizeOf(typeof(LVITEM)), ref vNumberOfBytesRead);

				// リストボックスに情報を追加する
				//MessageBox.Show("location:" + IconLocation + "  name:" + IconName + "   iItem:" + vItem[0].iItem);

				IconName = IconName.Replace("\0", "");
				if (IconName.Equals(NAME))
				{
					if (Move == MOVE.UP)
					{
						SendMessage(hWndDesktop, LVM_SETITEMPOSITION, i, MakeLParam(IconLocation.X, IconLocation.Y - 80));
					}
					else if (Move == MOVE.DOWN)
					{
						SendMessage(hWndDesktop, LVM_SETITEMPOSITION, i, MakeLParam(IconLocation.X, IconLocation.Y + 80));
					}
					else if (Move == MOVE.RIGHT)
					{
						SendMessage(hWndDesktop, LVM_SETITEMPOSITION, i, MakeLParam(IconLocation.X + 80, IconLocation.Y));
					}
					else
					{
						SendMessage(hWndDesktop, LVM_SETITEMPOSITION, i, MakeLParam(IconLocation.X - 80, IconLocation.Y));
					}
				}
			}

			// hProcessに関連するメモリーを解放する
			VirtualFreeEx(hProcess, pProcInfo, 0, MEM_RELEASE);

			CloseHandle(hProcess);

			// 最後にアイコン描画が更新されないことがあるため
			//SendMessage(hWndDesktop, (uint)WindowMessage.WM_KEYDOWN, VK_F5, 0);
		}
	}
}
