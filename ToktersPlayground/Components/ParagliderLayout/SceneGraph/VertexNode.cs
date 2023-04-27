using Avalonia.Controls;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ToktersPlayground.Controls.SceneGraph;

namespace ToktersPlayground.Components.ParagliderLayout.SceneGraph
{
    public class VertexNode : SceneNode
    {
        private SKPaint _vertexPaint;
        private const float VertexSize = 4.0f;

        public VertexNode()
        {
            _vertexPaint = new SKPaint();
            _vertexPaint.Color = SKColors.White;
            _vertexPaint.StrokeWidth = 1.0f;
            _vertexPaint.IsAntialias = true;
        }

        public override void DrawScene(SKCanvas canvas, Camera camera)
        {
            var test = camera.ToWorld(new Vector2(0, 0));
            canvas.DrawLine(-VertexSize, 0, VertexSize, 0, _vertexPaint);
            canvas.DrawLine(0, -VertexSize, 0, VertexSize, _vertexPaint);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _vertexPaint.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
