namespace PayDemo.Subprocess
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            return Chromely.CefGlue.Subprocess.Subprocess.Execute(args);
        }
    }
}