using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToktersPlayground.ViewModels;

namespace ToktersPlayground.Components.ParagliderLayout.Commands
{
    /// <summary>
    /// Adds a vertex to the paraglider layout.
    /// </summary>
    [PlaygroundCommand("Add Vertex", PlaygroundCommandLocation.ComponentsTools, order: 20, icon: "M12,20C7.59,20 4,16.41 4,12C4,7.59 7.59,4 12,4C16.41,4 20,7.59 20,12C20,16.41 16.41,20 12,20M12,2A10,10 0 0,0 2,12A10,10 0 0,0 12,22A10,10 0 0,0 22,12A10,10 0 0,0 12,2M13,7H11V11H7V13H11V17H13V13H17V11H13V7Z", ComponentType = typeof(ParagliderLayout))]
    public class AddVertex : PlaygroundCommand
    {
        public override bool CanExecute(object? parameter)
        {
            return true;
        }

        public override Task ExecuteAsync(object? parameter)
        {
            if (parameter is ParagliderLayout layout && layout.LayoutControl != null)
            {
                layout.LayoutControl.Scene.Activate("InsertVertex");
            }
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// Adds a vertex to the paraglider layout.
    /// </summary>
    [PlaygroundCommand("Delete Vertex", PlaygroundCommandLocation.ComponentsTools, order: 30, icon: "M12,20C7.59,20 4,16.41 4,12C4,7.59 7.59,4 12,4C16.41,4 20,7.59 20,12C20,16.41 16.41,20 12,20M12,2A10,10 0 0,0 2,12A10,10 0 0,0 12,22A10,10 0 0,0 22,12A10,10 0 0,0 12,2M7,13H17V11H7", ComponentType = typeof(ParagliderLayout))]
    public class DeleteVertex : PlaygroundCommand
    {
        public override bool CanExecute(object? parameter)
        {
            return true;
        }

        public override Task ExecuteAsync(object? parameter)
        {
            return Task.CompletedTask;
        }
    }

}
