using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToktersPlayground.ViewModels;

namespace ToktersPlayground.Components.System.Commands
{
    /// <summary>
    /// Deletes the selected component
    /// </summary>
    [PlaygroundCommand("Delete", PlaygroundCommandLocation.ComponentsTools, order:1000, icon: "M160 256H96a32 32 0 0 1 0-64h256V95.936a32 32 0 0 1 32-32h256a32 32 0 0 1 32 32V192h256a32 32 0 1 1 0 64h-64v672a32 32 0 0 1-32 32H192a32 32 0 0 1-32-32V256zm448-64v-64H416v64h192zM224 896h576V256H224v640zm192-128a32 32 0 0 1-32-32V416a32 32 0 0 1 64 0v320a32 32 0 0 1-32 32zm192 0a32 32 0 0 1-32-32V416a32 32 0 0 1 64 0v320a32 32 0 0 1-32 32z")]
    public class DeleteComponent : PlaygroundCommand
    {
        public override void Execute(IPlayground playground)
        {
        }

        public override bool CanExecute(IPlayground playground)
        {
            return playground.SelectedComponent != null;
        }
    }
}