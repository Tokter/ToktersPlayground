﻿using Avalonia.Controls;
using Avalonia.OpenGL;
using Avalonia.Platform.Storage;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ToktersPlayground.ViewModels;

namespace ToktersPlayground.Components.System.Commands
{
    /// <summary>
    /// Loads the current project
    /// </summary>
    [PlaygroundCommand("Project/Load", PlaygroundCommandLocation.MainMenu, order: 20, icon: "M9.467 9.609h3.030c0.266 0 0.482-0.278 0.482-0.621v-4.374c0-0.343-0.216-0.621-0.482-0.621h-3.030c-0.266 0-0.482 0.278-0.482 0.621v4.374c0 0.343 0.216 0.621 0.482 0.621zM25.958 23.399v-7.987h-7.987v7.987h-3.993l7.987 7.987 7.987-7.987zM7.113 25.708c-0.276 0-0.499-0.223-0.499-0.499v-11.481c0-0.276 0.223-0.499 0.499-0.499h17.721c0.276 0 0.499 0.223 0.499 0.499v0.536h1.554v8.607h1.566v-14.884c0-0.276-4.216-4.493-4.493-4.493h-4.741c0.169 0.069 0.287 0.223 0.287 0.404v6.181c0 0.244-0.216 0.442-0.482 0.442h-10.594c-0.266 0-0.482-0.198-0.482-0.442v-6.181c0-0.18 0.118-0.335 0.287-0.404h-4.242c-0.277 0-0.499 0.223-0.499 0.499v23.961c0 0.277 0.223 0.499 0.499 0.499h14.412l-2.746-2.746h-8.546z")]
    public class LoadProject : PlaygroundCommand
    {
        private readonly FilePickerOpenOptions _openOptions;

        public LoadProject()
        {
            _openOptions = new FilePickerOpenOptions
            {
                Title = "Open Playground Project",
                AllowMultiple = false
            };
            //_openOptions.SuggestedStartLocation

            var fileTypes = new List<FilePickerFileType>
            {
                new FilePickerFileType("Playground File") { Patterns = new[] { "*.playground" } }
            };

            _openOptions.FileTypeFilter = fileTypes;
        }

        public override bool CanExecute(object? parameter)
        {
            return true;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            if (StorageProvider == null) return;
            if (parameter is IPlayground playground)
            {
                var file = await StorageProvider.OpenFilePickerAsync(_openOptions);
                if (file != null)
                {
                    playground.ProjectFileName = file[0].Path.AbsolutePath;
                    LoaderSaver.LoadProject(playground);
                }
            }
        }
    }
}