using Avalonia.Input;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ToktersPlayground.Controls.SceneGraph.EditStates;

namespace ToktersPlayground.Controls.SceneGraph
{
    public class Scene : IDisposable
    {
        private ScreenCenterCamera _camera;
        private UICamera _uiCamera;
        private SceneNode _root;

        public Camera Camera => _camera;
        public Camera UICamera => _uiCamera;
        public SceneNode Root => _root;
        public Vector2 CurrentAbsMousePos { get; private set; }
        public Vector2 CurrentMousePos { get; private set; }

        public Scene()
        {
            _camera = new ScreenCenterCamera();
            _uiCamera = new UICamera();
            _root = new SceneNode();
        }

        public void SetScreenSize(float width, float height)
        {
            _camera.ScreenWidth = width;
            _camera.ScreenHeight = height;
            _uiCamera.ScreenWidth = width;
            _uiCamera.ScreenHeight = height;
        }

        #region Drawing

        private Matrix3x2 _modelTransform = Matrix3x2.Identity;
        private Matrix3x2 _modelInvTransform = Matrix3x2.Identity;
        private Stack<Matrix3x2> _modelTransforms = new Stack<Matrix3x2>();
        private Stack<Matrix3x2> _modelInvTransforms = new Stack<Matrix3x2>();
        private SKPaint _stateText = new SKPaint { Color = SKColors.Orange, TextSize = 16.0f };

        private void ClearModelTransform()
        {
            _modelTransform = Matrix3x2.Identity;
            _modelInvTransform = Matrix3x2.Identity;
        }

        private void PushModelTransform()
        {
            _modelTransforms.Push(_modelTransform);
            _modelInvTransforms.Push(_modelInvTransform);
        }

        public void SetModelTransform(Matrix3x2 matrix)
        {
            _modelTransform = matrix * _modelTransform;
            Matrix3x2.Invert(_modelTransform, out _modelInvTransform);
        }

        private void PopModelTransform()
        {
            _modelTransform = _modelTransforms.Pop();
            _modelInvTransform = _modelInvTransforms.Pop();
        }

        public void Draw(SKCanvas canvas)
        {
            ClearModelTransform();

            //Draw in object space
            canvas.Save();
            DrawSceneNode(canvas, Root, Camera);
            canvas.Restore();

            ClearModelTransform();

            //Draw in screen space
            canvas.Save();
            DrawUISceneNode(canvas, Root, UICamera);
            if (!string.IsNullOrEmpty(ActiveState?.Description))
            {
                canvas.DrawText($"{ActiveState.Description} - [Esc] to abort", 30, Camera.ScreenHeight - 30, _stateText);
            }
            canvas.Restore();
        }

        private void DrawSceneNode(SKCanvas canvas, SceneNode node, Camera camera)
        {
            PushModelTransform();
            SetModelTransform(node.GetTransform());
            camera.ApplyModelViewTransformToSurface(canvas, _modelTransform, _modelInvTransform);
            node.DrawScene(canvas, camera);
            foreach (var child in node.Where(n => n.Visible))
            {
                DrawSceneNode(canvas, child, camera);
            }
            PopModelTransform();
        }

        private void DrawUISceneNode(SKCanvas canvas, SceneNode node, Camera camera)
        {
            node.DrawUI(canvas, camera);

            for (int i = 0; i < node.Count; i++)
            {
                if (node[i].Visible)
                {
                    DrawUISceneNode(canvas, node[i], camera);
                }
            }

            ActiveState?.DrawUI(canvas, camera);
        }

        #endregion

        #region Event Handling

        private List<EditState> _availableStates = new List<EditState>();
        private Stack<EditState> _activeState = new Stack<EditState>();
        private StandardCursorType _currentCursor = StandardCursorType.Arrow;

        public void RegisterEditState(EditState state)
        {
            state.Scene = this;
            _availableStates.Add(state);
        }

        public EditState? ActiveState
        {
            get
            {
                if (_activeState.Count == 0) return null;
                return _activeState.Peek();
            }
        }

        public void Activate(string name)
        {
            if (ActiveState == null)
            {
                var newState = _availableStates.FirstOrDefault(s => s?.Name?.ToLower().Trim() == name.ToLower().Trim());
                if (newState != null)
                {
                    newState.Activated();
                    _activeState.Push(newState);
                }
            }
        }

        /// <summary>
        /// Processes the input event and returns true if the event was handled
        /// </summary>
        /// <param name="inputEvent">Input event to process</param>
        /// <returns>Whether the event was handled or not</returns>
        public bool ProcessEvent(InputEvent inputEvent)
        {
            //Store current mouse position
            if (inputEvent.InputEventType == InputEventType.MouseMove || inputEvent.InputEventType == InputEventType.MouseUp || inputEvent.InputEventType == InputEventType.MouseDown)
            {
                CurrentAbsMousePos = Camera.ToWorld(inputEvent.MousePos);
                CurrentMousePos = inputEvent.MousePos;
            }

            //Inspect Event
            //Check if one of the available edit states wants to be activated
            foreach (var s in _availableStates.Where(a => a.Available()))
            {
                if (s.InspectEvent(inputEvent) == InspectResult.ActivateMe)
                {
                    if (ActiveState != s)
                    {
                        _activeState.Push(s);
                        s.Activated();
                    }
                }
            }

            //If no state is active, check if we need to change the cursor back to the default
            if (ActiveState == null && _currentCursor != StandardCursorType.Arrow)
            {
                SetCursor(StandardCursorType.Arrow);
            }

            if (ActiveState == null) return false;

            //Process Event
            var done = false;
            while (!done && ActiveState != null)
            {
                switch (ActiveState.ProcessEvent(inputEvent))
                {
                    case ProcessResult.ImDone:
                        _activeState.Pop();
                        done = false;
                        break;

                    default:
                        done = true;
                        break;
                }
            }

            return true;
        }

        public Action<Cursor>? ChangeCursor { get; set; }


        public void SetCursor(StandardCursorType cursor)
        {
            _currentCursor = cursor;
            if (ChangeCursor != null) ChangeCursor(new Cursor(cursor));
        }

        #endregion

        #region Snapping

        public Vector2 GetGridSnapPoint(Camera camera, Vector2 absPosition)
        {
            return absPosition;
        }

        public Vector2 GetClosestSnapPoint(Camera camera, Vector2 absPosition)
        {
            return absPosition;
        }

        #endregion


        public void Dispose()
        {
        }
    }
}
