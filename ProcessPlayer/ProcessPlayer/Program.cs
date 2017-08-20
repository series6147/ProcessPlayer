using ProcessPlayer.Engine;
using System;
using System.IO;
using System.Threading;

namespace ProcessPlayer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0 && File.Exists(args[0]))
            {
                var sp = new ScriptPlayer() { IsConsole = true };

                for (var i = 0; i < args.Length; i++)
                    using (var reader = File.OpenText(args[i]))
                    {
                        Console.WriteLine(string.Format("{0} - started", args[i]));

                        sp.PrepareAndDiagnostics(reader.ReadToEnd(), 120000).Wait();

                        if (sp.IsPrepared)
                            sp.Play().Wait();

                        Thread.Sleep(100);

                        Console.WriteLine("press any key to continue.");
                        Console.ReadKey();
                    }
            }
        }
    }
}
