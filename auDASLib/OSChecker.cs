namespace auDASLib
{
    public class OSChecker
    {
        public static bool IsWindows()
        {
            return Environment.OSVersion.Platform == PlatformID.Win32NT;
        }

        public static bool IsLinux()
        {
            return Environment.OSVersion.Platform == PlatformID.Unix;
        }
    }

}
