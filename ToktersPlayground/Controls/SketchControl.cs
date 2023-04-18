﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToktersPlayground.Controls.SceneGraph;

namespace ToktersPlayground.Controls
{
    public class SketchControl : Control, IDisposable
    {
        private Scene _scene;
        private bool disposedValue;

        public Scene Scene => _scene;
        public bool AutoRedraw { get; set; } = false;

        public SketchControl()
        {
            _scene = new Scene();
            _scene.ChangeCursor += (c) => { this.Cursor = c; };
            PointerMoved += DemoControl_PointerMoved;
            PointerPressed += DemoControl_PointerPressed;
            PointerReleased += DemoControl_PointerReleased;
            PointerWheelChanged += DemoControl_PointerWheelChanged;
            KeyDown += DemoControl_KeyDown;
            KeyUp += DemoControl_KeyUp;
        }

        public override void Render(DrawingContext context)
        {
            _scene.SetScreenSize((float)Bounds.Width, (float)Bounds.Height);
            context.Custom(new DrawOp(new Rect(0, 0, Bounds.Width, Bounds.Height), _scene));

            Dispatcher.UIThread.InvokeAsync(InvalidateVisual, DispatcherPriority.Background);
        }

        #region Input Handling

        private void DemoControl_KeyUp(object? sender, KeyEventArgs e)
        {
            if (_scene != null)
            {
                var inputEvent = InputEvent.KeyUp(e.Key,
                    e.KeyModifiers.HasFlag(KeyModifiers.Shift),
                    e.KeyModifiers.HasFlag(KeyModifiers.Control),
                    e.KeyModifiers.HasFlag(KeyModifiers.Alt));
                _scene.ProcessEvent(inputEvent);
            }
        }

        private void DemoControl_KeyDown(object? sender, KeyEventArgs e)
        {
            if (_scene != null)
            {
                var inputEvent = InputEvent.KeyDown(e.Key,
                    e.KeyModifiers.HasFlag(KeyModifiers.Shift),
                    e.KeyModifiers.HasFlag(KeyModifiers.Control),
                    e.KeyModifiers.HasFlag(KeyModifiers.Alt));
                _scene.ProcessEvent(inputEvent);
            }
        }

        private void DemoControl_PointerMoved(object? sender, PointerEventArgs e)
        {
            if (_scene != null)
            {
                var point = e.GetCurrentPoint(this);

                var inputEvent = InputEvent.MouseMove((float)point.Position.X, (float)point.Position.Y, ToButton(point.Properties),
                    e.KeyModifiers.HasFlag(KeyModifiers.Shift),
                    e.KeyModifiers.HasFlag(KeyModifiers.Control),
                    e.KeyModifiers.HasFlag(KeyModifiers.Alt));
                _scene.ProcessEvent(inputEvent);
            }
        }

        private void DemoControl_PointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            if (_scene != null)
            {
                var point = e.GetCurrentPoint(this);

                var inputEvent = InputEvent.MouseUp((float)point.Position.X, (float)point.Position.Y, ToButton(point.Properties),
                    e.KeyModifiers.HasFlag(KeyModifiers.Shift),
                    e.KeyModifiers.HasFlag(KeyModifiers.Control),
                    e.KeyModifiers.HasFlag(KeyModifiers.Alt));
                _scene.ProcessEvent(inputEvent);
            }
        }

        private void DemoControl_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (_scene != null)
            {
                var point = e.GetCurrentPoint(this);

                var inputEvent = InputEvent.MouseDown((float)point.Position.X, (float)point.Position.Y, ToButton(point.Properties),
                    e.KeyModifiers.HasFlag(KeyModifiers.Shift),
                    e.KeyModifiers.HasFlag(KeyModifiers.Control),
                    e.KeyModifiers.HasFlag(KeyModifiers.Alt));
                _scene.ProcessEvent(inputEvent);
            }
        }

        private void DemoControl_PointerWheelChanged(object? sender, PointerWheelEventArgs e)
        {
            if (_scene != null)
            {
                var point = e.GetCurrentPoint(this);

                var inputEvent = InputEvent.MouseWheel((float)e.Delta.Y,
                    e.KeyModifiers.HasFlag(KeyModifiers.Shift),
                    e.KeyModifiers.HasFlag(KeyModifiers.Control),
                    e.KeyModifiers.HasFlag(KeyModifiers.Alt));
                _scene.ProcessEvent(inputEvent);
            }
        }

        private MouseButtons ToButton(PointerPointProperties prop)
        {
            var result = MouseButtons.None;

            if (prop.IsLeftButtonPressed) result |= MouseButtons.Left;
            if (prop.IsMiddleButtonPressed) result |= MouseButtons.Middle;
            if (prop.IsRightButtonPressed) result |= MouseButtons.Right;
            if (prop.IsXButton1Pressed) result |= MouseButtons.XButton1;
            if (prop.IsXButton2Pressed) result |= MouseButtons.XButton2;

            return result;
        }

        #endregion

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _scene.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    public class DrawOp : ICustomDrawOperation
    {
        private Scene _scene;
        public Rect Bounds { get; }

        public DrawOp(Rect bounds, SceneGraph.Scene scene)
        {
            Bounds = bounds;
            _scene = scene;
        }

        public void Dispose()
        {
        }

        public bool Equals(ICustomDrawOperation? other)
        {
            return false;
        }

        public bool HitTest(Point p)
        {
            return true;
        }

        public void Render(IDrawingContextImpl context)
        {
            var skia = context.GetFeature<ISkiaSharpApiLeaseFeature>();
            if (skia != null)
            {
                using (var lease = skia.Lease())
                {
                    _scene.Draw(lease.SkCanvas);
                }
            }
        }
    }
}
