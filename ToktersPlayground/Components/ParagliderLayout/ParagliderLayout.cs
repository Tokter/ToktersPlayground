using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ToktersPlayground.Components.LiftDragCurve.ViewModels;
using ToktersPlayground.Components.ParagliderLayout.SceneGraph;
using ToktersPlayground.Components.ParagliderLayout.ViewModels;
using ToktersPlayground.ViewModels;

namespace ToktersPlayground.Components.ParagliderLayout
{
    [PlaygroundComponent("Paraglider Layout")]
    public class ParagliderLayout : PlaygroundComponent
    {
        public ParagliderLayoutControl? LayoutControl => this.Sketch as ParagliderLayoutControl;

        public ParagliderLayout()
        {
            Name = "New Paraglider Layout";
        }

        [Property("Number of Cells")]
        public int NumberOfCells { get; set; } = 51;

        [Property("Flat Span (m)")]
        public float FlatSpan { get; set; } = 12.7f;

        [Property("Flat Aspect Ratio")]
        public float FlatAspectRatio { get; set; } = 5.4f;

        [Property("Flat Area (m^2)")]
        public float FlatArea { get; set; } = 29.8f;

        [Property("Weight (kg)")]
        public float Weight { get; set; } = 5.5f;

        protected override ViewModelBase CreateViewModel()
        {
            return new ParagliderLayoutViewModel(this);
        }

        #region Loading/Saving

        protected override void OnSave(XmlWriter writer, LoadSaveOptions options)
        {
            writer.WriteAttributeString("NumberOfCells", NumberOfCells.ToString());
            writer.WriteAttributeString("FlatSpan", FlatSpan.ToString());
            writer.WriteAttributeString("FlatAspectRatio", FlatAspectRatio.ToString());
            writer.WriteAttributeString("FlatArea", FlatArea.ToString());
            writer.WriteAttributeString("Weight", Weight.ToString());
        }

        protected override void OnLoad(XmlElement element, LoadSaveOptions options)
        {
            NumberOfCells = int.Parse(element.GetAttribute("NumberOfCells"));
            FlatSpan = float.Parse(element.GetAttribute("FlatSpan"));
            FlatAspectRatio = float.Parse(element.GetAttribute("FlatAspectRatio"));
            FlatArea = float.Parse(element.GetAttribute("FlatArea"));
            Weight = float.Parse(element.GetAttribute("Weight"));
        }

        #endregion
    }
}