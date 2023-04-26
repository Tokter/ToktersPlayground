using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToktersPlayground.ViewModels;

namespace ToktersPlayground
{
    public interface IPlaygoundComponent
    {
        string Type { get; }
        string Name { get; set; }
        ViewModelBase ViewModel { get; }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class PlaygroundComponentAttribute : Attribute
    {
        public string Type { get; set; }

        public PlaygroundComponentAttribute(string typeName)
        {
            Type = typeName;   
        }
    }
}
