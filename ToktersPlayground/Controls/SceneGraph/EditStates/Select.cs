using Avalonia.Input;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ToktersPlayground.Controls.SceneGraph.EditStates
{
    public enum SelectionMode
    {
        Select,
        AddToSelection,
        RemoveFromSelection,
    }

    public class Select : EditState, IDisposable
    {
        private SKPaint _selectionPaintStroke;
        private SKPaint _selectionPaintFill;
        private SelectionMode _selectionMode = SelectionMode.Select;
        private Vector2 _startMousePos;

        public Select()
        {
            _selectionPaintStroke = new SKPaint();
            _selectionPaintStroke.Color = SKColors.Red;
            _selectionPaintStroke.Style = SKPaintStyle.Stroke;
            _selectionPaintStroke.StrokeWidth = 1.0f;
            _selectionPaintStroke.IsAntialias = true;

            _selectionPaintFill = new SKPaint();
            _selectionPaintFill.Color = new SKColor(255, 0, 0, 10);
            _selectionPaintFill.Style = SKPaintStyle.Fill;
            _selectionPaintFill.StrokeWidth = 1.0f;
            _selectionPaintFill.IsAntialias = true;
        }

        public override InspectResult InspectEvent(InputEvent inputEvent)
        {
            if (inputEvent.Shift)
            {
                _selectionMode = SelectionMode.AddToSelection;
            }
            else if (inputEvent.Control)
            {
                _selectionMode = SelectionMode.RemoveFromSelection;
            }
            else
            {
                _selectionMode = SelectionMode.Select;
            }

            if (Scene.ActiveState == null && inputEvent.InputEventType == InputEventType.KeyDown && inputEvent.Key == Key.A)
            {
                Scene.Root.SelectAll();
            }

            if (Scene.ActiveState == null && inputEvent.InputEventType == InputEventType.MouseDown && inputEvent.Button == MouseButtons.Left)
            {
                _startMousePos = Scene.CurrentAbsMousePos;
                return InspectResult.ActivateMe;
            }

            return InspectResult.IgnoreMe;
        }

        public override ProcessResult ProcessEvent(InputEvent inputEvent)
        {
            switch (inputEvent.InputEventType)
            {

                case InputEventType.MouseUp:
                    SelectNodes();
                    return ProcessResult.ImDone;
            }

            return ProcessResult.KeepProcessing;
        }

        public override void DrawUI(SKCanvas canvas, Camera camera)
        {
            var p1 = camera.ToScreen(_startMousePos);
            canvas.DrawRect(p1.X, p1.Y, Scene.CurrentMousePos.X - p1.X, Scene.CurrentMousePos.Y - p1.Y, _selectionPaintFill);
            canvas.DrawRect(p1.X, p1.Y, Scene.CurrentMousePos.X - p1.X, Scene.CurrentMousePos.Y - p1.Y, _selectionPaintStroke);
        }

        /// <summary>
        /// Selects nodes
        /// </summary>
        private void SelectNodes()
        {
            List<SceneNode>? intersectedObjects = null;
            bool groupSelect = false;

            //We select the points that intersect with the _startMousePos
            if (Vector2.Distance(_startMousePos, Scene.CurrentAbsMousePos) < 4.0f)
            {
                intersectedObjects = Scene.Root.FindNodes(n => n is IIntersectable intersect && n.Visible && intersect.IntersectsWidth(_startMousePos)).ToList();
            }
            //We select the points that are inside the rectangle (_startMousePos, Scene.CurrentMousePos)
            else
            {
                intersectedObjects = Scene.Root.FindNodes(n => n is IIntersectable intersect && n.Visible && intersect.InRect(_startMousePos, Scene.CurrentAbsMousePos)).ToList();
                groupSelect = true;
            }

            if (intersectedObjects != null && intersectedObjects.Count > 0)
            {
                switch (_selectionMode)
                {
                    case SelectionMode.AddToSelection:
                        foreach (var obj in intersectedObjects) obj.Selected = true;
                        break;

                    case SelectionMode.RemoveFromSelection:
                        foreach (var obj in intersectedObjects) obj.Selected = false;
                        break;

                    case SelectionMode.Select:
                        if (!groupSelect)
                        {
                            int selectNext = 0;
                            for (int i = intersectedObjects.Count - 1; i >= 0; i--)
                            {
                                if (intersectedObjects[i].Selected)
                                {
                                    selectNext = (i + 1) % intersectedObjects.Count;
                                    break;
                                }
                            }
                            Scene.Root.SelectNone();
                            intersectedObjects[selectNext].Selected = true;
                        }
                        else
                        {
                            Scene.Root.SelectNone();
                            foreach (var obj in intersectedObjects) obj.Selected = true;
                        }
                        break;
                }
            }
            else
            {
                Scene.Root.SelectNone();
            }
        }

        public void Dispose()
        {
            _selectionPaintStroke.Dispose();
        }
    }
}
