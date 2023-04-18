using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ToktersPlayground.Controls.SceneGraph.EditStates
{
    public class Pan : EditState
    {
        private Vector2 _cameraStartPos;
        private Vector2 _mouseDownPos;

        public override InspectResult InspectEvent(InputEvent inputEvent)
        {
            if (Scene.ActiveState != this && inputEvent.InputEventType == InputEventType.MouseDown && inputEvent.Button == MouseButtons.Right)
            {
                _cameraStartPos = Scene.Camera.Position;
                _mouseDownPos = inputEvent.MousePos;
                return InspectResult.ActivateMe;
            }
            return InspectResult.Ignore;
        }

        public override ProcessResult ProcessEvent(InputEvent inputEvent)
        {
            switch (inputEvent.InputEventType)
            {
                case InputEventType.MouseMove:
                    var delta = _mouseDownPos - inputEvent.MousePos;
                    var worldDelta = Scene.Camera.ToWorld(delta) - Scene.Camera.ToWorld(Vector2.Zero);
                    Scene.Camera.Position = _cameraStartPos + worldDelta;
                    break;

                case InputEventType.MouseUp:
                    return ProcessResult.ImDone;
            }

            return ProcessResult.KeepProcessing;
        }
    }
}
