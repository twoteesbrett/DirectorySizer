using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DirectorySizer
{
    public class SubdirectoryInfo
    {
        public SubdirectoryInfo(string path) : this(new DirectoryInfo(path)) { }

        public SubdirectoryInfo(DirectoryInfo info)
        {
            FullName = info.FullName;
            Depth = GetDepth(info.FullName);
            Size = info.EnumerateFiles().Sum(f => f.Length);
        }

        public string FullName { get; set; }
        public int Depth { get; set; }
        public long Size { get; set; }
        public long DeepSize { get; set; }

        private string[] _names;

        public string[] Names
        {
            get
            {
                return _names ??= FullName.Split(Path.DirectorySeparatorChar);
            }
        }

        public static int GetDepth(string path)
            => path.Count(p => p == Path.DirectorySeparatorChar);

        public static long GetDeepSize(IEnumerable<SubdirectoryInfo> directories, string path)
            => directories.Where(d => d.FullName.StartsWith(path)).Sum(d => d.Size);
    }
}
