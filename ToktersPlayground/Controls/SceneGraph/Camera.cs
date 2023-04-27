using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ToktersPlayground.Controls.SceneGraph
{
    public class Camera
    {
        private float _rotation = 0.0f;
        private Vector2 _position = new Vector2(0, 0);
        private float _scale = 1.0f;
        private bool _viewTransformIsDirty = true;

        private Matrix3x2 _modelViewtransform;
        private Matrix3x2 _modelViewInvTransform;

        private Matrix3x2 _viewTransform;
        private Matrix3x2 _viewInvTransform;

        public float Rotation
        {
            get => _rotation;
            set
            {
                if (_rotation != value)
                {
                    _rotation = value;
                    _viewTransformIsDirty = true;
                }
            }
        }

        public Vector2 Position
        {
            get => _position;
            set 
            {
                if (_position != value)
                {
                    _position = value; 
                    _viewTransformIsDirty = true;
                }
            }
        }

        public float Scale
        {
            get => _scale;
            set
            {
                if ( _scale != value)
                {
                    _scale = value;
                    _viewTransformIsDirty = true;
                }
            }
        }

        private float _screenWidth;
        private float _screenHeight;

        public float ScreenWidth
        {
            get => _screenWidth;
            set 
            {
                if (_screenWidth != value)
                {
                    _screenWidth = value;
                    _viewTransformIsDirty = true;
                }
            }
        }

        public float ScreenHeight
        {
            get => _screenHeight;
            set 
            {
                if (_screenHeight != value)
                {
                    _screenHeight = value;
                    _viewTransformIsDirty = true;
                }
            }
        }

        public void ApplyModelViewTransformToSurface(SKCanvas canvas, Matrix3x2 modelTransform, Matrix3x2 invModelTransform)
        {
            if (_viewTransformIsDirty)
            {
                _viewTransform = CalculateViewTransform();
                Matrix3x2.Invert(_viewTransform, out _viewInvTransform);
                _viewTransformIsDirty = false;
            }

            var deviceClip = canvas.DeviceClipBounds;

            _modelViewtransform = modelTransform * _viewTransform * Matrix3x2.CreateTranslation(new Vector2(deviceClip.Left, deviceClip.Top));
            _modelViewInvTransform = _viewInvTransform * invModelTransform;
            canvas.SetMatrix(new SKMatrix(_modelViewtransform.M11, _modelViewtransform.M21, _modelViewtransform.M31, _modelViewtransform.M12, _modelViewtransform.M22, _modelViewtransform.M32, 0, 0, 1));
        }

        public Vector2 ToWorld(Vector2 screenPos)
        {
            return Vector2.Transform(screenPos, _viewInvTransform);
        }

        public Vector2 ToScreen(Vector2 worldPos)
        {
            return Vector2.Transform(worldPos, _viewTransform);
        }

        protected virtual Matrix3x2 CalculateViewTransform()
        {
            return Matrix3x2.CreateTranslation(-Position.X, -Position.Y)
                * Matrix3x2.CreateRotation(Rotation)
                * Matrix3x2.CreateScale(Scale);
        }
    }

    public class ScreenCenterCamera : Camera
    {
        protected override Matrix3x2 CalculateViewTransform()
        {
            var screenCenter = new Vector2(ScreenWidth / 2.0f, ScreenHeight / 2.0f);

            return
                Matrix3x2.CreateTranslation(-Position.X, -Position.Y)
                * Matrix3x2.CreateRotation(Rotation)
                * Matrix3x2.CreateScale(Scale)
                * Matrix3x2.CreateTranslation(screenCenter);

        }
    }

    public class UICamera : Camera
    {
        protected override Matrix3x2 CalculateViewTransform()
        {
            return Matrix3x2.Identity;
        }
    }
}
