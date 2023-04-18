using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToktersPlayground.Controls.SceneGraph.EditStates
{
    public class Zoom : EditState
    {
        public override InspectResult InspectEvent(InputEvent inputEvent)
        {
            return inputEvent.InputEventType == InputEventType.MouseWheel ? InspectResult.ActivateMe : InspectResult.Ignore;
        }

        public override ProcessResult ProcessEvent(InputEvent inputEvent)
        {
            if (inputEvent.InputEventType == InputEventType.MouseWheel)
            {
                var newScale = Scene.Camera.Scale * (float)Math.Pow(1.2d, inputEvent.MouseDelta);

                if (newScale < 0.05f) newScale = 0.05f;
                if (newScale > 1000) newScale = 1000;
                Scene.Camera.Scale = newScale;

                inputEvent.Processed = true;
            }

            return ProcessResult.ImDone;
        }
    }
}
