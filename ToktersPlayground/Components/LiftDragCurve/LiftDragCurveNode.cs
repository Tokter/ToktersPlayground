using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Platform;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ToktersPlayground.Controls.SceneGraph;
using static System.Net.Mime.MediaTypeNames;

namespace ToktersPlayground.Components.LiftDragCurve
{
    public class LiftDragCurveNode : SceneNode
    {
        private SKPaint _aoaPaint;
        private SKPaint _liftPaint;
        private SKPaint _dragPaint;
        private const float AOA_WIDTH = 10.0f;
        private const float LIFT_WIDTH = 100.0f;

        public LiftDragCurve? Curve { get; set; }

        public LiftDragCurveNode()
        {
            _aoaPaint = new SKPaint();
            _aoaPaint.Color = SKColors.White;
            _aoaPaint.StrokeWidth = 1.0f;
            _aoaPaint.IsAntialias = true;


            _liftPaint = new SKPaint();
            _liftPaint.Color = SKColors.Green;
            _liftPaint.StrokeWidth = 1.0f;
            _liftPaint.IsAntialias = true;

            _dragPaint = new SKPaint();
            _dragPaint.Color = SKColors.Red;
            _dragPaint.StrokeWidth = 1.0f;
            _dragPaint.IsAntialias = true;
        }

        public override void DrawScene(SKCanvas canvas, Camera camera)
        {
            if (Curve != null)
            {
                DrawAOAAxis(canvas, camera, _aoaPaint);
                DrawLiftAxis(canvas, camera, _liftPaint);
            }            
        }

        private void DrawAOAAxis(SKCanvas canvas, Camera camera, SKPaint paint)
        {
            var start = (float)Math.Floor(Curve?.AngleOfAttackMin ?? 0.0f);
            var end = (float)Math.Ceiling(Curve?.AngleOfAttackMax ?? 0.0f);
            var y = 0.0f;

            canvas.DrawLine(start * AOA_WIDTH, y, end * AOA_WIDTH, y, paint);
            var title = "Angle of attack";
            var titleWidth = paint.MeasureText(title);
            canvas.DrawText(title, (start + (end - start) / 2.0f) * AOA_WIDTH - titleWidth / 2.0f, y + 25, paint);

            var pos = start;
            while (pos <= end)
            {
                canvas.DrawLine(pos * AOA_WIDTH, y - 2, pos * AOA_WIDTH, y + 2, paint);

                var label = $"{pos:n0}";
                var labelWidth = paint.MeasureText(label);
                canvas.DrawText(label, pos * AOA_WIDTH - labelWidth / 2.0f, y + 12, paint);
                pos += 5.0f;
            }            
        }

        private void DrawLiftAxis(SKCanvas canvas, Camera camera, SKPaint paint)
        {
            var start = (float)Math.Floor(Curve?.LiftCoefficientMin ?? 0.0f);
            var end = (float)Math.Ceiling(Curve?.LiftCoefficientMax ?? 0.0f);
            var x = (float)Math.Floor(Curve?.AngleOfAttackMin ?? 0.0f) * AOA_WIDTH;

            canvas.DrawLine(x, -start * LIFT_WIDTH, x, -end * LIFT_WIDTH, paint);
            var title = "Coefficient of Lift";
            var titleWidth = paint.MeasureText(title);

            canvas.Save();
            canvas.RotateDegrees(-90);
            canvas.DrawText(title, (start + (end - start) / 2.0f) * LIFT_WIDTH - titleWidth / 2.0f, x - 25, paint);
            canvas.Restore();            
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _aoaPaint.Dispose();
                _liftPaint.Dispose();
                _dragPaint.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
