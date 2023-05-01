using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ToktersPlayground.Controls.SceneGraph
{
    public class SimpleGrid : SceneNode
    {
        private SKColor _originColor;
        private SKPaint _originPaint;
        private SKColor _mayorColor;
        private SKPaint _mayorPaint;
        private SKColor _minorColor;
        private SKPaint _minorPaint;

        public SKColor BackgroundColor = new SKColor(30, 30, 30, 255);

        public SimpleGrid()
        {
            Name = "Grid";
            _originColor = new SKColor(0x60, 0x60, 0x60, 255);
            _originPaint = CreatePaint(_originColor);

            _mayorColor = new SKColor(0x50, 0x50, 0x50, 255);
            _mayorPaint = CreatePaint(_mayorColor);

            _minorColor = new SKColor(0x40, 0x40, 0x40, 255);
            _minorPaint = CreatePaint(_minorColor);
        }

        public SKColor OriginColor
        {
            get { return _originColor; }
            set
            {
                _originColor = value;
                if (_originPaint != null) _originPaint.Dispose();
                _originPaint = CreatePaint(_originColor);
            }
        }

        public SKColor MayorColor
        {
            get { return _mayorColor; }
            set
            {
                _mayorColor = value;
                if (_mayorPaint != null) _mayorPaint.Dispose();
                _mayorPaint = CreatePaint(_mayorColor);
            }
        }

        public SKColor MinorColor
        {
            get { return _originColor; }
            set
            {
                _minorColor = value;
                if (_minorPaint != null) _minorPaint.Dispose();
                _minorPaint = CreatePaint(_minorColor);
            }
        }

        private SKPaint CreatePaint(SKColor color, float width = 1.0f)
        {
            return new SKPaint
            {
                Color = color,
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = width,
                StrokeCap = SKStrokeCap.Butt,
            };
        }

        public override void DrawScene(SKCanvas canvas, Camera camera)
        {
            canvas.Clear(BackgroundColor);

            var topLeft = camera.ToWorld(Vector2.Zero);
            var tenPixels = (camera.ToWorld(new Vector2(10, 0)) - topLeft).Length();

            var bottomRight = camera.ToWorld(new Vector2(camera.ScreenWidth - 1, camera.ScreenHeight - 1));
            int left = ((int)Math.Round(topLeft.X / 100.0f) - 1) * 100;
            int top = ((int)Math.Round(topLeft.Y / 100.0f) - 1) * 100;
            int bottom = ((int)Math.Round(bottomRight.Y / 100.0f) + 1) * 100;
            int right = ((int)Math.Round(bottomRight.X / 100.0f) + 1) * 100;

            int step = 10;
            if (tenPixels > 10) step = 100;

            for (int x = left; x <= right; x += step)
            {
                canvas.DrawLine(x, top, x, bottom, GetGridPaint(x, camera));
            }

            for (int y = top; y <= bottom; y += step)
            {
                canvas.DrawLine(left, y, right, y, GetGridPaint(y, camera));
            }
        }

        private SKPaint GetGridPaint(int pos, Camera camera)
        {
            if (pos == 0)
            {
                _originPaint.StrokeWidth = 2.0f / camera.Scale;
                return _originPaint;
            }
            else if (pos % 100 == 0)
            {
                _mayorPaint.StrokeWidth = 1.5f / camera.Scale;
                return _mayorPaint;
            }
            else
            {
                _minorPaint.StrokeWidth = 1.0f / camera.Scale;
                return _minorPaint;
            }
        }

        public override void DrawUI(SKCanvas canvas, Camera camera)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_originPaint != null) _originPaint.Dispose();
                if (_mayorPaint != null) _mayorPaint.Dispose();
                if (_minorPaint != null) _minorPaint.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
