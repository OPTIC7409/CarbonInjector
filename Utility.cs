using DiscordRPC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CarbonInjector.Winapi;

namespace CarbonInjector
{
    public class Utility
    {
        public static readonly string InjectorPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\CarbonInjector";
        public static DiscordRpcClient client;
        public static Config config;
        public static EventHandler<EventArgs> InjectionCompleted;

        public static void Init()
        {
            Directory.CreateDirectory(Utility.InjectorPath);
            config = Config.LoadConfig();
            client = new DiscordRpcClient("1059570247348256829");
            InjectionCompleted += InjectionCompleted;
            client.Initialize();
        }

        public static void OpenGame()
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Normal,
                FileName = "explorer.exe",
                Arguments = "shell:appsFolder\\Microsoft.MinecraftUWP_8wekyb3d8bbwe!App",
            };
            process.StartInfo = startInfo;
            process.Start();
            MessageBox.Show("Attempted to open Minecraft");
        }

        public static bool IsGameOpen()
        {
            Process[] mc = Process.GetProcessesByName("Minecraft.Windows");
            return mc.Length > 0;
        }

        public static void updateRPC()
        {
            client.SetPresence(new RichPresence()
            {
                Details = "Carbon Injector",
                State = "W Injector",
                Assets = new Assets()
                {
                    LargeImageKey = "large",
                    LargeImageText = "Carbon Injector",
                    SmallImageKey = "image_small"
                }
            });
        }

        public static void Inject(string path)
        {
            try
            {
                if (!IsGameOpen())
                {

                    if(config.openGame)
                    {
                        OpenGame();
                    }else
                    {
                        MessageBox.Show("Open Minecraft First");
                    }

                    Thread.Sleep(TimeSpan.FromSeconds(5));
                }

                ApplyAppPackages(path);

                var targetProcess = Process.GetProcessesByName("Minecraft.Windows")[0];

                var procHandle = OpenProcess(PROCESS_CREATE_THREAD | PROCESS_QUERY_INFORMATION |
                                             PROCESS_VM_OPERATION | PROCESS_VM_WRITE | PROCESS_VM_READ,
                    false, targetProcess.Id);

                var loadLibraryAddress = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");

                var allocMemAddress = VirtualAllocEx(procHandle, IntPtr.Zero,
                    (uint)((path.Length + 1) * Marshal.SizeOf(typeof(char))), MEM_COMMIT
                                                                               | MEM_RESERVE, PAGE_READWRITE);

                WriteProcessMemory(procHandle, allocMemAddress, Encoding.Default.GetBytes(path),
                    (uint)((path.Length + 1) * Marshal.SizeOf(typeof(char))), out _);
                CreateRemoteThread(procHandle, IntPtr.Zero, 0, loadLibraryAddress,
                    allocMemAddress, 0, IntPtr.Zero);

            }
            catch (Exception e)
            {
            }
        }

        private static void ApplyAppPackages(string path)
        {
            var infoFile = new FileInfo(path);
            var fSecurity = infoFile.GetAccessControl();
            fSecurity.AddAccessRule(
                new FileSystemAccessRule(new SecurityIdentifier("S-1-15-2-1"),
                FileSystemRights.FullControl, InheritanceFlags.None,
                PropagationFlags.NoPropagateInherit, AccessControlType.Allow));

            infoFile.SetAccessControl(fSecurity);
        }

    }
}
