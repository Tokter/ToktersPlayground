using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ToktersPlayground.ViewModels;

namespace ToktersPlayground
{
    public class LoadSaveOptions
    {
        public IPlayground Playground { get; private set; }

        public LoadSaveOptions(IPlayground playground)
        {
            Playground = playground;
        }

        public string GetPathRelativeToBasePath(string fullPath)
        {
            var basePath = System.IO.Path.GetDirectoryName(Playground.ProjectFileName)!;
            return System.IO.Path.GetRelativePath(basePath, fullPath);
        }

        public string GetFullPathFromBasePath(string relativePath)
        {
            var basePath = System.IO.Path.GetDirectoryName(Playground.ProjectFileName)!;
            return System.IO.Path.GetFullPath(System.IO.Path.Combine(basePath, relativePath));
        }
    }

    public interface ICanBeLoadedSaved
    {
        public void SaveTo(XmlWriter writer, LoadSaveOptions options);
        public void LoadFrom(XmlElement element, LoadSaveOptions options);
    }
}
