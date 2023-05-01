using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ToktersPlayground.Components.ParagliderLayout;
using ToktersPlayground.Controls.SceneGraph;

namespace ToktersPlayground.Components.ParagliderLayout.SceneGraph
{
    public class ParagliderLayoutNode : SceneNode
    {
        private SKPaint _symmetryPaint;
        public ParagliderLayout? Layout { get; set; }

        public ParagliderLayoutNode()
        {
            Name = "Paraglider Layout";
            _symmetryPaint = new SKPaint();
            _symmetryPaint.Style = SKPaintStyle.Stroke;
            _symmetryPaint.Color = SKColors.White;
            _symmetryPaint.StrokeWidth = 1.0f;
            _symmetryPaint.IsAntialias = true;
        }

        public override void DrawScene(SKCanvas canvas, Camera camera)
        {
            if (Layout != null)
            {
                var halfWidthCM = 100 * Layout.FlatSpan / 2.0f;
                var chord = Layout.FlatSpan / Layout.FlatAspectRatio;
                var halfHeightCM = 100 * chord / 2.0f;

                //Draw Symmetry Axis
                canvas.DrawLine(0, -2 * halfHeightCM, 0, 2 * halfHeightCM, _symmetryPaint);

                //Draw aspect ratio outline
                canvas.DrawRect(-halfWidthCM, -halfHeightCM, 2 * halfWidthCM, 2 * halfHeightCM, _symmetryPaint);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _symmetryPaint.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
