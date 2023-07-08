using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ToktersPlayground.Controls.SceneGraph.EditStates
{
    public class Drag : EditState
    {
        private Vector2 _startMousePos;
        private readonly List<SelectedNode> _selectedNodes = new();

        public override void Activated(object? parameter = null)
        {
            base.Activated();
            this.Scene.SetCursor(Avalonia.Input.StandardCursorType.Hand);
        }

        public override InspectResult InspectEvent(InputEvent inputEvent)
        {
            var result = InspectResult.IgnoreMe;

            //If we press G
            if (Scene.ActiveState == null && inputEvent.InputEventType == InputEventType.KeyDown && inputEvent.Key == Avalonia.Input.Key.G)
            {
                if (Scene.Root.FindNodes(node => node is IDraggable d && d.CanBeDragged && node.Selected).Any())
                {
                    result = InspectResult.ActivateMe;
                }
            }
            
            //Left click if no other state is active
            else if (Scene.ActiveState == null && inputEvent.InputEventType == InputEventType.MouseDown && inputEvent.Button == MouseButtons.Left)
            {
                //do we click on a selectedNode?
                var clickOnSelectedNode = Scene.Root.FindNodes(node =>
                    node is IDraggable d
                    && d.CanBeDragged
                    && node.Selected
                    && d.IntersectsWidth(Scene.CurrentAbsMousePos)).Any();

                if (clickOnSelectedNode)
                {
                    result = InspectResult.ActivateMe;
                }
            }

            if (result == InspectResult.ActivateMe)
            {
                _startMousePos = Scene.CurrentAbsMousePos;

                //Stored the selected nodes
                _selectedNodes.Clear();
                foreach (IDraggable node in Scene.Root.FindNodes(node => node is IDraggable d && d.CanBeDragged && node.Selected).Cast<IDraggable>())
                {
                    _selectedNodes.Add(new SelectedNode(node));
                }
            }

            return result;
        }

        public override ProcessResult ProcessEvent(InputEvent inputEvent)
        {
            switch (inputEvent.InputEventType)
            {
                case InputEventType.MouseMove:
                    var delta = Scene.CurrentAbsMousePos - _startMousePos;

                    foreach (var node in _selectedNodes)
                    {
                        node.Node.AbsPosition = node.StartPosition + delta;
                    }
                    break;

                case InputEventType.MouseUp:
                    return ProcessResult.ImDone;
            }

            return ProcessResult.KeepProcessing;
        }

        private class SelectedNode
        {
            public IDraggable Node { get; set; }
            public Vector2 StartPosition { get; set; }
            public SelectedNode(IDraggable node)
            {
                Node = node;
                StartPosition = node.AbsPosition;
            }
        }
    }
}
