using Avalonia;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ToktersPlayground.Components;

namespace ToktersPlayground.Controls.SceneGraph
{
    public class SceneNodeList : Collection<SceneNode>, INotifyCollectionChanged
    {
        private SceneNode _parent;

        public SceneNodeList(SceneNode parent)
        {
            _parent = parent;
        }

        protected override void InsertItem(int index, SceneNode item)
        {
            item.Parent = _parent;
            base.InsertItem(index, item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
        }

        protected override void ClearItems()
        {
            for (int i = 0; i < Count; i++)
            {
                this[i].Dispose();
            }
            base.ClearItems();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        protected override void RemoveItem(int index)
        {
            SceneNode removedItem = this[index];
            base.RemoveItem(index);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItem, index));
            this[index].Dispose();
        }

        protected override void SetItem(int index, SceneNode item)
        {
            SceneNode oldItem = this[index];
            item.Parent = _parent;
            base.SetItem(index, item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, item, oldItem, index));
        }

        #region INotifyCollectionChanged

        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        #endregion
    }

    public class SceneNode : ITransformable, IDisposable, INotifyPropertyChanged
    {
        private SceneNodeList _children;
        private string _name = "New Node";

        [Property("(Name)")]
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Type => this.GetType().Name;

        public SceneNode()
        {
            _children = new SceneNodeList(this);
        }


        #region Scene Graph

        private SceneNode _parent = null!;

        public SceneNodeList Children => _children;

        public SceneNode this[int index] => _children[index];

        public int Count => _children.Count;

        public void Add(SceneNode node)
        {
            _children.Add(node);
        }

        public SceneNode Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        public SceneNode Root
        {
            get
            {
                SceneNode root = this;
                while (root.Parent != null)
                {
                    root = root.Parent;
                }
                return root;
            }
        }



        protected virtual void Dispose(bool disposing)
        {
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            _children.Clear();
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public IEnumerable<SceneNode> FindNodes(Predicate<SceneNode> predicate)
        {
            foreach (var child in _children)
            {
                var nodes = child.FindNodes(predicate);
                foreach (var node in nodes) yield return node;
            }
            if (predicate(this))
            {
                yield return this;
            }
        }

        #endregion

        #region ITransformable

        private float _rotation = 0.0f;
        private Vector2 _position = new Vector2(0, 0);
        private float _scale = 1.0f;
        private bool _isDirty = true;

        //Contains the transformation for the node
        private Matrix3x2 _transform;
        private Matrix3x2 _invTransform;

        //Converts from the local to the world space
        private Matrix3x2 _worldTransform;
        private Matrix3x2 _invWorldTransform;

        public float Rotation
        {
            get => _rotation;
            set
            {
                if (value != _rotation)
                {
                    _rotation = value;
                    MarkTransformDirty();
                }
            }
        }

        [Property("Position")]
        public Vector2 Position
        {
            get => _position;
            set
            {
                if (value != _position)
                {
                    _position = value;
                    MarkTransformDirty();
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(AbsPosition));
                }
            }
        }

        [Property("Abs Position")]
        public Vector2 AbsPosition
        {
            get => ToAbs(Vector2.Zero);
            set
            {
                Position = Parent.ToLocal(value);
                OnPropertyChanged();
            }
        }

        [Property("Scale")]
        public float Scale
        {
            get => _scale;
            set
            {
                if (value != _scale)
                {
                    _scale = value;
                    MarkTransformDirty();
                    OnPropertyChanged();
                }
            }
        }

        ITransformable ITransformable.Parent => Parent;

        public Matrix3x2 GetTransform()
        {
            CalculateTransforms();
            return _transform;
        }

        public Matrix3x2 GetInvTransform()
        {
            CalculateTransforms();
            return _invTransform;
        }

        private void CalculateTransforms()
        {
            if (_isDirty)
            {
                _transform = CalculateTransform();
                Matrix3x2.Invert(_transform, out _invTransform);

                //Calculate world transforms
                var stack = new Stack<Matrix3x2>();
                stack.Push(_invTransform);
                _worldTransform = _transform;
                var parent = Parent;
                while (parent != null)
                {
                    _worldTransform *= parent.GetTransform();
                    stack.Push(parent.GetInvTransform());
                    parent = parent.Parent;
                }

                _invWorldTransform = Matrix3x2.Identity;
                while (stack.Count > 0)
                {
                    _invWorldTransform *= stack.Pop();
                }

                _isDirty = false;
            }
        }

        private Matrix3x2 CalculateTransform()
        {
            return Matrix3x2.CreateScale(_scale)
                   * Matrix3x2.CreateRotation(_rotation)
                   * Matrix3x2.CreateTranslation(_position.X, _position.Y);
        }

        /// <summary>
        /// Converts a scene object local coordinate to an absolute (World) coordinate
        /// </summary>
        /// <param name="pos">Local coordinate</param>
        /// <returns>Absolute coordinate</returns>
        public Vector2 ToAbs(Vector2 pos)
        {
            CalculateTransforms();
            return Vector2.Transform(pos, _worldTransform);
        }

        /// <summary>
        /// Converts an absolute (World) coordinate to a scene object local coordinate
        /// </summary>
        /// <param name="pos">Absolute coordinate</param>
        /// <returns>Local coordinate</returns>
        public Vector2 ToLocal(Vector2 pos)
        {
            CalculateTransforms();
            return Vector2.Transform(pos, _invWorldTransform);
        }

        public void MarkTransformDirty()
        {
            _isDirty = true;
            foreach (var child in Children) child.MarkTransformDirty();
        }

        #endregion

        #region Selection

        private bool _selected = false;

        public bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                OnSelection();
            }
        }

        public void SelectNone()
        {
            Selected = false;
            foreach (var item in Children) item.SelectNone();
        }

        public void SelectAll()
        {
            Selected = true;
            foreach (var item in Children) item.SelectAll();
        }

        //Override to intercept selection
        protected virtual void OnSelection()
        {
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Drawing

        private bool _visible = true;

        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }


        public virtual void DrawScene(SKCanvas canvas, Camera camera)
        {
        }

        public virtual void DrawUI(SKCanvas canvas, Camera camera)
        {
        }

        #endregion

        #region Heler Functions

        public bool IsInRect(Vector2 point, Vector2 rect1, Vector2 rect2)
        {
            var topLeft = rect1;
            var bottomRight = rect2;
            //Switch topLeft and bottomRight if needed
            if (topLeft.X > bottomRight.X)
            {
                var temp = topLeft.X;
                topLeft.X = bottomRight.X;
                bottomRight.X = temp;
            }
            if (topLeft.Y > bottomRight.Y)
            {
                var temp = topLeft.Y;
                topLeft.Y = bottomRight.Y;
                bottomRight.Y = temp;
            }
            return topLeft.X < point.X && bottomRight.X > point.X && topLeft.Y < point.Y && bottomRight.Y > point.Y;
        }

        #endregion
    }
}
