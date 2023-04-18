using Avalonia.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ToktersPlayground.Controls.SceneGraph
{
    public enum InputEventType
    {
        MouseWheel,
        MouseMove,
        MouseDown,
        MouseDoubleClick,
        MouseUp,
        KeyUp,
        KeyDown,
    }

    [Flags]
    public enum MouseButtons
    {
        None = 0,
        Left = 1,
        Right = 2,
        Middle = 4,
        XButton1 = 8,
        XButton2 = 16
    }

    public class InputEvent : EventArgs
    {
        public InputEventType InputEventType { get; private set; }
        public bool Processed { get; set; }
        public Vector2 MousePos { get; set; }
        public MouseButtons Button { get; set; }
        public float MouseDelta { get; set; }
        public bool Shift { get; set; }
        public bool Control { get; set; }
        public bool Alt { get; set; }
        public Key Key { get; set; }

        public InputEvent(InputEventType inputEventType)
        {
            InputEventType = inputEventType;
            Processed = false;
        }

        public static InputEvent MouseMove(float x, float y, MouseButtons button, bool shift, bool control, bool alt)
        {
            return new InputEvent(InputEventType.MouseMove)
            {
                MousePos = new Vector2(x, y),
                Button = button,
                Shift = shift,
                Control = control,
                Alt = alt
            };
        }

        public static InputEvent MouseWheel(float delta, bool shift, bool control, bool alt)
        {
            return new InputEvent(InputEventType.MouseWheel)
            {
                MouseDelta = delta,
                Shift = shift,
                Control = control,
                Alt = alt
            };
        }

        public static InputEvent MouseDown(float x, float y, MouseButtons button, bool shift, bool control, bool alt)
        {
            return new InputEvent(InputEventType.MouseDown)
            {
                MousePos = new Vector2(x, y),
                Button = button,
                Shift = shift,
                Control = control,
                Alt = alt
            };
        }

        public static InputEvent MouseDoubleClick(float x, float y, MouseButtons button, bool shift, bool control, bool alt)
        {
            return new InputEvent(InputEventType.MouseDoubleClick)
            {
                MousePos = new Vector2(x, y),
                Button = button,
                Shift = shift,
                Control = control,
                Alt = alt
            };
        }

        public static InputEvent MouseUp(float x, float y, MouseButtons button, bool shift, bool control, bool alt)
        {
            return new InputEvent(InputEventType.MouseUp)
            {
                MousePos = new Vector2(x, y),
                Button = button,
                Shift = shift,
                Control = control,
                Alt = alt
            };
        }

        public static InputEvent KeyDown(Key key, bool shift, bool control, bool alt)
        {
            return new InputEvent(InputEventType.KeyDown)
            {
                Key = key,
                Shift = shift,
                Control = control,
                Alt = alt
            };
        }

        public static InputEvent KeyUp(Key key, bool shift, bool control, bool alt)
        {
            return new InputEvent(InputEventType.KeyUp)
            {
                Key = key,
                Shift = shift,
                Control = control,
                Alt = alt
            };
        }

    }

}
