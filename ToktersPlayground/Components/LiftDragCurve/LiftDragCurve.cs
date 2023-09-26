using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ToktersPlayground.Components.LiftDragCurve.ViewModels;
using ToktersPlayground.ViewModels;

namespace ToktersPlayground.Components.LiftDragCurve
{
    [PlaygroundComponent("Lift/Drag Curve")]
    public class LiftDragCurve : PlaygroundComponent, ICanBeLoadedSaved
    {
        public LiftDragCurve()
        {
            Name = "New Curve";
        }

        [Property("AOA Min", "n0")]
        public float AngleOfAttackMin { get; set; } = 0.0f;

        [Property("AOA Max", "n0")]
        public float AngleOfAttackMax { get; set; } = 25.0f;

        [Property("CoL Min", "n1")]
        public float LiftCoefficientMin { get; set; } = 0.0f;

        [Property("CoL Max", "n1")]
        public float LiftCoefficientMax { get; set; } = 2.0f;

        [Property("CoD Min", "n2")]
        public float DragCoefficientMin { get; set; } = 0.0f;

        [Property("CoD Max", "n2")]
        public float DragCoefficientMax { get; set; } = 0.2f;

        protected override ViewModelBase CreateViewModel()
        {
            return new LiftDragCurveViewModel(this);
        }

        #region ICanBeLoadedSaved

        protected override void OnSave(XmlWriter writer, LoadSaveOptions options)
        {
            writer.WriteAttributeString("AngleOfAttackMin", AngleOfAttackMin.ToString());
            writer.WriteAttributeString("AngleOfAttackMax", AngleOfAttackMax.ToString());
            writer.WriteAttributeString("LiftCoefficientMin", LiftCoefficientMin.ToString());
            writer.WriteAttributeString("LiftCoefficientMax", LiftCoefficientMax.ToString());
            writer.WriteAttributeString("DragCoefficientMin", DragCoefficientMin.ToString());
            writer.WriteAttributeString("DragCoefficientMax", DragCoefficientMax.ToString());
        }

        protected override void OnLoad(XmlElement element, LoadSaveOptions options)
        {
            AngleOfAttackMin = float.Parse(element.GetAttribute("AngleOfAttackMin"));
            AngleOfAttackMax = float.Parse(element.GetAttribute("AngleOfAttackMax"));
            LiftCoefficientMin = float.Parse(element.GetAttribute("LiftCoefficientMin"));
            LiftCoefficientMax = float.Parse(element.GetAttribute("LiftCoefficientMax"));
            DragCoefficientMin = float.Parse(element.GetAttribute("DragCoefficientMin"));
            DragCoefficientMax = float.Parse(element.GetAttribute("DragCoefficientMax"));
        }

        #endregion
    }
}
