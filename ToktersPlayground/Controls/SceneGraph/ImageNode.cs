﻿using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using ToktersPlayground.Components;

namespace ToktersPlayground.Controls.SceneGraph
{
    public class ImageNode : SceneNode, IDraggable
    {
        private SKPaint _selectedPaint;
        private SKPaint _imagePaint;
        private SKBitmap? _bitmap = null;

        [Property("Is Locked")]
        public bool IsLocked { get; set; } = false;

        [Property("Alpha")]
        public byte Alpha { get; set; } = 128;

        public ImageNode()
        {
            _selectedPaint = new SKPaint();
            _selectedPaint.Color = SKColors.Blue;
            _selectedPaint.Style = SKPaintStyle.Stroke;
            _selectedPaint.StrokeWidth = 1.0f;
            _selectedPaint.IsAntialias = true;
            _selectedPaint.TextSize = 10.0f;

            _imagePaint = new SKPaint
            {
                IsAntialias = true,
                Color = new SKColor(255, 255, 255, Alpha),
                FilterQuality = SKFilterQuality.High
            };
        }

        public void LoadFromFile(string fileName, bool makeBackgroundTransparent = false)
        {
            _bitmap = SKBitmap.Decode(fileName);

            if (makeBackgroundTransparent)
            {
                var pixels = _bitmap.Pixels;
                for (int i = 0; i < pixels.Length; i++)
                {
                    var pixel = pixels[i];

                    float brightness = (pixel.Red + pixel.Green + pixel.Blue) / 3.0f;
                    byte alpha = (byte)(255 - brightness);

                    pixels[i] = new SKColor(alpha, alpha, alpha, alpha);
                }
                _bitmap.Pixels = pixels;
            }
        }

        public override void DrawScene(SKCanvas canvas, Camera camera)
        {
            if (_bitmap == null) return;

            if (Alpha != _imagePaint.Color.Alpha)
            {
                _imagePaint.Color = new SKColor(255, 255, 255, Alpha);
            }

            canvas.DrawBitmap(_bitmap, -_bitmap.Width / 2.0f, -_bitmap.Height / 2.0f, _imagePaint);

            if (Selected)
            {
                canvas.DrawRect(-_bitmap.Width / 2.0f, -_bitmap.Height / 2.0f, _bitmap.Width, _bitmap.Height, _selectedPaint);
            }
        }

        public bool CanBeDragged => !IsLocked;
        public bool CanBeSelected => !IsLocked;

        public bool IntersectsWidth(Vector2 pos)
        {
            if (_bitmap == null) return false;
            var localPos = ToLocal(pos);

            var half = new Vector2(_bitmap.Width / 2.0f, _bitmap.Height / 2.0f);

            return IsInRect(localPos, -half, half);
        }

        public bool InRect(Vector2 rect1, Vector2 rect2)
        {
            if (_bitmap == null) return false;
            var half = new Vector2(_bitmap.Width / 2.0f, _bitmap.Height / 2.0f);
            return IsInRect(-half, ToLocal(rect1), ToLocal(rect2)) && IsInRect(half, ToLocal(rect1), ToLocal(rect2));
        }

        public int Width => _bitmap?.Width ?? 0;
        public int Height => _bitmap?.Height ?? 0;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _imagePaint.Dispose();
                _selectedPaint.Dispose();
            }
        }
    }
}