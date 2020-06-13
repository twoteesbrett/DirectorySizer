using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DirectorySizer
{
    class Program
    {
        static readonly string DefaultRootDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        static void Main(string[] args)
        {
            var root = new DirectoryInfo(args.Length > 0 ? args[0] : DefaultRootDirectory);

            Console.WriteLine("Recursing directories...");

            var directories = root.GetDirectories("*", new EnumerationOptions
            {
                IgnoreInaccessible = true,
                ReturnSpecialDirectories = true,
                RecurseSubdirectories = true
            });

            Console.WriteLine("Getting directory sizes...");

            var infos = directories
                .Where(d => d.Name != "." && d.Name != ".." && Directory.Exists(d.FullName))
                .Select(d => new SubdirectoryInfo(d))
                .OrderByDescending(i => i.Depth)
                .ToList();

            Console.WriteLine("Calculating deep sizes...");

            foreach (var item in infos)
            {
                item.DeepSize = SubdirectoryInfo.GetDeepSize(infos, item.FullName);
            }

            var top = infos.OrderByDescending(i => i.DeepSize).Take(10).ToList();

            Console.WriteLine();
            Console.WriteLine($"Root directory: {root.FullName}");
            Console.WriteLine($"Total directories recursed: {directories.Count()}");
            Console.WriteLine();

            foreach (var item in top)
            {
                Console.WriteLine($"{item.DeepSize.ToUnits()}\t{item.Depth}\t{item.FullName}");
            }
        }
    }
}
