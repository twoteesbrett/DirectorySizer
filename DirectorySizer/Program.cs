using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace DirectorySizer
{
    class Program
    {
        static readonly string[] Units = { "Bytes", "KiB", "MiB", "GiB", "TiB", "PiB" };
        static readonly string DefaultRootDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        static void Main(string[] args)
        {
            var root = args.Length > 0 ? args[0] : DefaultRootDirectory;

            Console.WriteLine("Recursing directories...");

            var directories = Directory.GetDirectories(root, "*", new EnumerationOptions
            {
                IgnoreInaccessible = true,
                ReturnSpecialDirectories = true,
                RecurseSubdirectories = true
            });

            Console.WriteLine("Getting directory sizes...");

            var sizes = directories
                .Select(d => new DirectoryInfo(d))
                .Select(i => new { i.FullName, Size = i.EnumerateFiles().Sum(f => f.Length) })
                .Distinct()
                .OrderByDescending(i => i.Size);

            var top = sizes.Take(10).ToList();

            Console.WriteLine();
            Console.WriteLine($"Root directory: {root}");
            Console.WriteLine($"Total directories recursed: {directories.Count()}");
            Console.WriteLine();

            foreach (var item in top)
            {
                Console.WriteLine($"{FormatSize(item.Size)}\t{item.FullName}");
            }
        }

        public static string FormatSize(long bytes)
        {
            var counter = 0;
            decimal number = bytes;

            while (Math.Round(number / 1024) >= 1)
            {
                number /= 1024;
                counter++;
            }

            return $"{number:n1}{Units[counter]}";
        }
    }
}
