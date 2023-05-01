using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ToktersPlayground.Components.ParagliderLayout.SceneGraph;
using ToktersPlayground.Controls.SceneGraph;
using ToktersPlayground.ViewModels;

namespace ToktersPlayground.Components.ParagliderLayout.Commands
{
    /// <summary>
    /// Adds a vertex to the paraglider layout.
    /// </summary>
    [PlaygroundCommand("Add Image", PlaygroundCommandLocation.ComponentsTools, order: 40, icon: "M18 15V18H15V20H18V23H20V20H23V18H20V15H18M13.3 21H5C3.9 21 3 20.1 3 19V5C3 3.9 3.9 3 5 3H19C20.1 3 21 3.9 21 5V13.3C20.4 13.1 19.7 13 19 13C17.9 13 16.8 13.3 15.9 13.9L14.5 12L11 16.5L8.5 13.5L5 18H13.1C13 18.3 13 18.7 13 19C13 19.7 13.1 20.4 13.3 21Z", ComponentType = typeof(ParagliderLayout))]

    public class AddImage : PlaygroundCommand
    {
        private FilePickerOpenOptions _openOptions;

        public AddImage()
        {
            _openOptions = new FilePickerOpenOptions();
            _openOptions.Title = "Open Image";
            _openOptions.AllowMultiple = false;
            var fileTypes = new List<FilePickerFileType>();
            fileTypes.Add(new FilePickerFileType("Image Files") { Patterns = new[] { "*.png", "*.jpg" } });

            _openOptions.FileTypeFilter = fileTypes;
        }

        public override bool CanExecute(object? parameter)
        {
            return true;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            if (StorageProvider == null) return;

            string fileName = string.Empty;
            var file = await StorageProvider.OpenFilePickerAsync(_openOptions);
            if (file != null)
            {
                fileName = file[0].Path.AbsolutePath;
            }
            if (!string.IsNullOrEmpty(fileName))
            {
                if (parameter is ParagliderLayout layout && layout.LayoutControl != null)
                {                    
                    var layoutNode = (ParagliderLayoutNode)layout.LayoutControl.Scene.Root.Children.First(n => n is ParagliderLayoutNode);
                    if (layoutNode == null) return;

                    var image = new ImageNode();
                    image.LoadFromFile(fileName, makeBackgroundTransparent: true);
                    if (layoutNode.Children.Count(c => c is ImageNode) == 0)
                    {
                        image.Name = "Background Image";
                    }
                    else
                    {
                        image.Name = "Image " + layoutNode.Children.Count(n => n is ImageNode);
                    }
                    layoutNode.Add(image);
                    image.Selected = true;                }
            }
        }
    }
}