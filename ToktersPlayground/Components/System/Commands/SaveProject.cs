﻿using Avalonia.Controls;
using Avalonia.OpenGL;
using Avalonia.Platform.Storage;
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
    /// Saves the current project
    /// </summary>
    [PlaygroundCommand("Project/Save", PlaygroundCommandLocation.MainMenu, order: 30, icon: "M9.467 3.106h3.030c0.266 0 0.482 0.278 0.482 0.621v4.374c0 0.343-0.216 0.621-0.482 0.621h-3.030c-0.266 0-0.482-0.278-0.482-0.621v-4.374c0-0.343 0.216-0.621 0.482-0.621zM21.964 14.524l7.987 7.987h-3.993v7.987h-7.987v-7.987h-3.993l7.987-7.987zM17.422 24.768h-10.309c-0.276 0-0.499-0.223-0.499-0.499v-11.481c0-0.276 0.223-0.499 0.499-0.499h17.721c0.276 0 0.499 0.223 0.499 0.499v4.402l3.12 3.12v-13.262c0-0.276-4.216-4.493-4.493-4.493h-4.741c0.169 0.069 0.287 0.223 0.287 0.404v6.181c0 0.244-0.216 0.442-0.482 0.442h-10.594c-0.266 0-0.482-0.198-0.482-0.442v-6.181c0-0.18 0.118-0.335 0.287-0.404h-4.242c-0.277 0-0.499 0.223-0.499 0.499v23.961c0 0.277 0.223 0.499 0.499 0.499h13.429v-2.746zM26.506 22.903v4.611h1.449c0.277 0 0.499-0.223 0.499-0.499v-4.111l-1.948 0z")]
    public class SaveProject : PlaygroundCommand
    {
        private readonly FilePickerSaveOptions _saveOptions;

        public SaveProject()
        {
            _saveOptions = new FilePickerSaveOptions
            {
                Title = "Save Playground Project",
                DefaultExtension = "playground"
            };

            var fileTypes = new List<FilePickerFileType>
            {
                new FilePickerFileType("Playground File") { Patterns = new[] { "*.playground" } }
            };

            _saveOptions.FileTypeChoices = fileTypes;
        }

        public override bool CanExecute(object? parameter)
        {
            if (parameter is IPlayground playground)
            {
                return playground.Components.Count > 0;
            }
            return false;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            if (StorageProvider== null) return;
            if (parameter is IPlayground playground)
            {
                _saveOptions.SuggestedFileName = playground.ProjectFileName;

                var file = await StorageProvider.SaveFilePickerAsync(_saveOptions);
                if (file!=null)
                {
                    playground.ProjectFileName = file.Path.AbsolutePath;
                    LoaderSaver.SaveProject(playground);
                }
            }
        }

    }
}