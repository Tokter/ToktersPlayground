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
    public enum VertexType
    {
        Simple,
        AttachmentPoint,
    }

    public class VertexNode : SceneNode, IDraggable
    {
        private SKPaint _vertexPaint;
        private const float VertexSize = 4.0f;

        [Property("Vertex Type")]
        public VertexType VertexType { get; set; } = VertexType.Simple;

        [Property("Is On Perimeter")]
        public bool IsOnPerimeter { get; set; }

        public VertexNode()
        {
            _vertexPaint = new SKPaint();
            _vertexPaint.Color = SKColors.White;
            _vertexPaint.StrokeWidth = 1.0f;
            _vertexPaint.IsAntialias = true;
            _vertexPaint.TextSize = 10.0f;
        }

        public override void DrawScene(SKCanvas canvas, Camera camera)
        {
            _vertexPaint.Style = SKPaintStyle.Stroke;

            if (Selected)
            {
                _vertexPaint.Style = SKPaintStyle.Stroke;
                _vertexPaint.Color = SKColors.Red;
                canvas.DrawCircle(0, 0, VertexSize + 2.0f, _vertexPaint);
            }

            switch (VertexType)
            {
                case VertexType.Simple:
                    _vertexPaint.Color = SKColors.White;
                    break;

                case VertexType.AttachmentPoint:
                    _vertexPaint.Color = SKColors.Green;
                    break;
            }

            if (IsOnPerimeter)
            {
                canvas.DrawCircle(0, 0, VertexSize, _vertexPaint);
            }
            else
            {
                canvas.DrawLine(-VertexSize, 0, VertexSize, 0, _vertexPaint);
                canvas.DrawLine(0, -VertexSize, 0, VertexSize, _vertexPaint);
            }

            _vertexPaint.Style = SKPaintStyle.Fill;
            var width = _vertexPaint.MeasureText(Name);
            canvas.DrawText(Name, -width / 2.0f, _vertexPaint.TextSize + 5.0f, _vertexPaint);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _vertexPaint.Dispose();
            }
            base.Dispose(disposing);
        }

        public bool CanBeDragged => true;
        public bool CanBeSelected => true;

        public bool IntersectsWidth(Vector2 pos)
        {
            var localPos = ToLocal(pos);
            return Vector2.Distance(Vector2.Zero, localPos) < 4.0f;
        }

        public bool InRect(Vector2 rect1, Vector2 rect2) => IsInRect(Vector2.Zero, ToLocal(rect1), ToLocal(rect2));
    }
}
