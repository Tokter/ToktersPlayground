using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToktersPlayground.ViewModels;

namespace ToktersPlayground
{
    /// <summary>
    /// Playground command that can be placed somewhere in the user interface
    /// </summary>
    public interface IPlaygroundCommand
    {
        void Execute(IPlayground playgound);
        bool CanExecute(IPlayground playgound);
    }

    public enum PlaygroundCommandLocation
    {
        MainMenu,
        ComponentsMenu,
        ComponentsTools,
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class PlaygroundCommandAttribute : Attribute
    {
        public string Name { get; set; } = "Command Name";
        public PlaygroundCommandLocation Location { get; set;}
        public int Order { get; set; } = 100;
        public string? Icon { get; set; }

        public PlaygroundCommandAttribute(string name, PlaygroundCommandLocation location, int order = 100, string? icon = null)
        {
            Name = name;
            Location = location;
            Order= order;
            Icon = icon;
        }
    }

    public class PlaygroundCommand : IPlaygroundCommand
    {
        public virtual bool CanExecute(IPlayground playgound)
        {
            return true;
        }

        public virtual void Execute(IPlayground playgound)
        {
        }
    }
}
