using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using ToktersPlayground.ViewModels;

namespace ToktersPlayground
{
    public static class LoaderSaver
    {
        public const string MainElement = "ToktersPlayground";

        public static void SaveProject(IPlayground playground)
        {
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\r\n"
            };

            var options = new LoadSaveOptions(playground);

            using (var xmlWriter = XmlWriter.Create(playground.ProjectFileName, settings))
            {
                xmlWriter.WriteStartDocument();

                xmlWriter.WriteStartElement(MainElement);
                xmlWriter.WriteAttributeString("Version", Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "Unknown");

                foreach (var component in playground.Components)
                {
                    if (component is ICanBeLoadedSaved ls)
                    {
                        ls.SaveTo(xmlWriter, options);
                    }
                }

                xmlWriter.WriteEndDocument();
                xmlWriter.Close();
            }
        }

        public static void LoadProject(IPlayground playground)
        {
            var options = new LoadSaveOptions(playground);

            playground.Clear();
            var doc = new XmlDocument();
            doc.Load(playground.ProjectFileName);
            if (doc.DocumentElement?.Name == MainElement)
            {
                foreach (var componentNode in doc.DocumentElement.ChildNodes.OfType<XmlElement>())
                {
                    if (componentNode.Name == "Component")
                    {
                        var type = componentNode.GetAttribute("Type");
                        var component = playground.CreateComponent(type);
                        if (component is ICanBeLoadedSaved ls)
                        {
                            ls.LoadFrom(componentNode, options);
                        }
                    }
                }
            }
            else throw new Exception("Not a playground file!");
        }
    }
}
