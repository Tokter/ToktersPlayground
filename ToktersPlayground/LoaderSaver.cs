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

            using (var xmlWriter = XmlWriter.Create(playground.ProjectFileName, settings))
            {
                xmlWriter.WriteStartDocument();

                xmlWriter.WriteStartElement(MainElement);
                xmlWriter.WriteAttributeString("Version", Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "Unknown");

                foreach (var component in playground.Components)
                {
                    if (component is ICanBeLoadedSaved ls)
                    {
                        xmlWriter.WriteStartElement("Component");
                        xmlWriter.WriteAttributeString("Name", component.Name);
                        xmlWriter.WriteAttributeString("Type", component.Type);
                        ls.SaveTo(xmlWriter);
                        xmlWriter.WriteEndElement();
                    }
                }

                xmlWriter.WriteEndDocument();
                xmlWriter.Close();
            }
        }

        public static void LoadProject(IPlayground playground)
        {
            playground.Clear();
            var doc = new XmlDocument();
            doc.Load(playground.ProjectFileName);
            if (doc.DocumentElement?.Name == MainElement)
            {
                foreach (var componentNode in doc.DocumentElement.ChildNodes.OfType<XmlElement>())
                {
                    if (componentNode.Name == "Component")
                    {
                        var name = componentNode.GetAttribute("Name");
                        var type = componentNode.GetAttribute("Type");
                        var component = playground.CreateComponent(type);
                        if (component is ICanBeLoadedSaved ls)
                        {
                            ls.LoadFrom(componentNode);
                        }
                    }
                }
            }
            else throw new Exception("Not a playground file!");
        }
    }
}
