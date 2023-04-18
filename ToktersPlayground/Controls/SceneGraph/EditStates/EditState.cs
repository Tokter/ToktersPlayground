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
    public enum InspectResult
    {
        Ignore,
        ActivateMe,
    }

    public enum ProcessResult
    {
        ImDone,
        KeepProcessing,
    }

    public abstract class EditState
    {
        public Scene Scene { get; set; } = null!;
        public string? Name { get; set; }
        public string? Description { get; set; }

        /// <summary>
        /// Returns whether this state is currently available for processing or not.
        /// </summary>
        /// <returns>True if it's available, false otherwise.</returns>
        public virtual bool Available()
        {
            return true;
        }

        /// <summary>
        /// Gets called before the edit state gets activated
        /// </summary>
        public virtual void Activated()
        {
        }

        /// <summary>
        /// Gets called after the edit state got deactivated
        /// </summary>
        public virtual void Deactivated()
        {
        }

        /// <summary>
        /// Allows the edit state to inspect the event, and returns whether it wants to be activated or not.
        /// </summary>
        /// <param name="inputEvent">Event to inspect</param>
        /// <returns>InspectResult that tells us whether we want to be activated or not.</returns>
        public virtual InspectResult InspectEvent(InputEvent inputEvent)
        {
            return InspectResult.Ignore;
        }

        /// <summary>
        /// Processes an input event, and returns whether we can remove this edit state, or if it wants to keep processing events.
        /// </summary>
        /// <param name="inputEvent">Event to process</param>
        /// <returns>ProcessResult that tells us whether we are done, or want to keep processing.</returns>
        public virtual ProcessResult ProcessEvent(InputEvent inputEvent)
        {
            return ProcessResult.ImDone;
        }

        /// <summary>
        /// Allows the edit state to draw in the screen space
        /// </summary>
        public virtual void DrawUI(SKCanvas canvas, Camera camera)
        {
        }

        public Vector2 Snap(InputEvent inputEvent, Camera camera, Vector2 absPos)
        {
            if (inputEvent.Control)
            {
                absPos = Scene.GetClosestSnapPoint(camera, absPos);
            }
            else if (inputEvent.Shift)
            {
                //no snapping
            }
            else
            {
                absPos = Scene.GetGridSnapPoint(camera, absPos);
            }

            return absPos;
        }
    }
}
