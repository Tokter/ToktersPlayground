using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ToktersPlayground.Controls.SceneGraph;

namespace ToktersPlayground.Components.ParagliderLayout
{
    public class ParagliderLayoutNode : SceneNode
    {
        private SKPaint _symmetryPaint;
        private List<Vertex> _vertices = new List<Vertex>();

        public ParagliderLayout? Layout { get; set; }

        public ParagliderLayoutNode()
        {
            _symmetryPaint = new SKPaint();
            _symmetryPaint.Color = SKColors.White;
            _symmetryPaint.StrokeWidth = 1.0f;
            _symmetryPaint.IsAntialias = true;
        }

        public override void DrawScene(SKCanvas canvas, Camera camera)
        {
            if (Layout != null)
            {
                DrawSymmetryAxis(canvas, camera);
            }
        }

        private void DrawSymmetryAxis(SKCanvas canvas, Camera camera)
        {
            var x = 0.0f;
            canvas.DrawLine(x, -100.0f, x, 100.0f, _symmetryPaint);
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
