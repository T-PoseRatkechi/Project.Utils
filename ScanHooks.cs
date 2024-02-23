using Reloaded.Hooks.Definitions;
using Reloaded.Memory.SigScan.ReloadedII.Interfaces;

namespace Project.Utils;

public class ScanHooks
{
    private static readonly List<ScanHook> scans = new();

    public static void Add(string name, string? pattern, Action<IReloadedHooks, nint> success)
        => scans.Add(new(name, pattern, success));

    public static void Initialize(IStartupScanner scanner, IReloadedHooks hooks)
    {
        foreach (var scan in scans)
        {
            if (string.IsNullOrEmpty(scan.Pattern))
            {
                Log.Verbose($"{scan.Name}: No pattern given.");
                continue;
            }

            scanner.Scan(scan.Name, scan.Pattern, result => scan.Success(hooks, result));
        }
    }

    private record ScanHook(string Name, string? Pattern, Action<IReloadedHooks, nint> Success);
}