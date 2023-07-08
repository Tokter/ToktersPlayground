using Avalonia.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ToktersPlayground.Controls.SceneGraph;
using ToktersPlayground.Controls.SceneGraph.EditStates;

namespace ToktersPlayground.Components.ParagliderLayout.SceneGraph
{
    public class InsertVertex : EditState
    {
        private VertexNode? _vertex;
        private ParagliderLayoutNode? _layoutNode;

        public InsertVertex()
        {
            Name = "InsertVertex";
            Description = "Insert Vertex";
        }

        public override void Activated(object? parameter = null)
        {
            base.Activated();
            Scene.SetCursor(StandardCursorType.Cross);
            _layoutNode = (ParagliderLayoutNode)Scene.Root.Children.First(n => n is ParagliderLayoutNode);
        }

        public override InspectResult InspectEvent(InputEvent inputEvent)
        {
            if (Scene.ActiveState == null && inputEvent.InputEventType == InputEventType.KeyDown && inputEvent.Key == Key.V)
            {
                return InspectResult.ActivateMe;
            }
            return InspectResult.IgnoreMe;
        }

        public override ProcessResult ProcessEvent(InputEvent inputEvent)
        {
            if (_layoutNode == null) return ProcessResult.ImDone;

            switch (inputEvent.InputEventType)
            {
                case InputEventType.KeyDown:
                    switch (inputEvent.Key)
                    {
                        case Key.Escape:
                            if (_vertex != null)
                            {
                                _layoutNode.Children.Remove(_vertex);
                                _vertex.Dispose();
                                _vertex = null;
                            }
                            return ProcessResult.ImDone;
                    }
                    break;

                case InputEventType.MouseDown:
                    _vertex = new VertexNode
                    {
                        Name = "Vertex " + _layoutNode.Children.Count(n => n is VertexNode)
                    };
                    _layoutNode.Add(_vertex);
                    _vertex.Selected = true;
                    _vertex.Position = Scene.Camera.ToWorld(inputEvent.MousePos);
                    _vertex = null;
                    return ProcessResult.ImDone;
            }

            return ProcessResult.KeepProcessing;
        }
    }
}
