using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ToktersPlayground
{
    public interface ICanBeLoadedSaved
    {
        public void SaveTo(XmlWriter writer);
        public void LoadFrom(XmlElement element);
    }
}
