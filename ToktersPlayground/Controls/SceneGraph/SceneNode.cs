using Avalonia;
using Avalonia.Media;
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
using System.Windows.Input;
using System.Xml;
using ToktersPlayground.Components;

namespace ToktersPlayground.Controls.SceneGraph
{
    public class SceneNodeList : Collection<SceneNode>, INotifyCollectionChanged
    {
        private readonly SceneNode _parent;

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

    public class SceneNode : ITransformable, IDisposable, INotifyPropertyChanged, ICanBeLoadedSaved
    {
        private static readonly Geometry IsVisibleIcon = Geometry.Parse("M12,9A3,3 0 0,0 9,12A3,3 0 0,0 12,15A3,3 0 0,0 15,12A3,3 0 0,0 12,9M12,17A5,5 0 0,1 7,12A5,5 0 0,1 12,7A5,5 0 0,1 17,12A5,5 0 0,1 12,17M12,4.5C7,4.5 2.73,7.61 1,12C2.73,16.39 7,19.5 12,19.5C17,19.5 21.27,16.39 23,12C21.27,7.61 17,4.5 12,4.5Z");
        private static readonly Geometry IsNotVisibleIcon = Geometry.Parse("M11.83,9L15,12.16C15,12.11 15,12.05 15,12A3,3 0 0,0 12,9C11.94,9 11.89,9 11.83,9M7.53,9.8L9.08,11.35C9.03,11.56 9,11.77 9,12A3,3 0 0,0 12,15C12.22,15 12.44,14.97 12.65,14.92L14.2,16.47C13.53,16.8 12.79,17 12,17A5,5 0 0,1 7,12C7,11.21 7.2,10.47 7.53,9.8M2,4.27L4.28,6.55L4.73,7C3.08,8.3 1.78,10 1,12C2.73,16.39 7,19.5 12,19.5C13.55,19.5 15.03,19.2 16.38,18.66L16.81,19.08L19.73,22L21,20.73L3.27,3M12,7A5,5 0 0,1 17,12C17,12.64 16.87,13.26 16.64,13.82L19.57,16.75C21.07,15.5 22.27,13.86 23,12C21.27,7.61 17,4.5 12,4.5C10.6,4.5 9.26,4.75 8,5.2L10.17,7.35C10.74,7.13 11.35,7 12,7Z");
        private readonly SceneNodeList _children;
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

            ToggleVisibility = new RelayCommand((o) =>
            {
                Visible = !Visible;
                OnPropertyChanged(nameof(VisibleIcon));
            }, (o) => true);
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
        private Vector2 _position = new(0, 0);
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
                OnPropertyChanged();
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

        public Geometry VisibleIcon => _visible ? IsVisibleIcon : IsNotVisibleIcon;

        public ICommand ToggleVisibility { get; set; }

        public virtual void DrawScene(SKCanvas canvas, Camera camera)
        {
        }

        public virtual void DrawUI(SKCanvas canvas, Camera camera)
        {
        }

        #endregion

        #region Heler Functions

        public static bool IsInRect(Vector2 point, Vector2 rect1, Vector2 rect2)
        {
            var topLeft = rect1;
            var bottomRight = rect2;
            //Switch topLeft and bottomRight if needed
            if (topLeft.X > bottomRight.X)
            {
                (bottomRight.X, topLeft.X) = (topLeft.X, bottomRight.X);
            }
            if (topLeft.Y > bottomRight.Y)
            {
                (bottomRight.Y, topLeft.Y) = (topLeft.Y, bottomRight.Y);
            }
            return topLeft.X < point.X && bottomRight.X > point.X && topLeft.Y < point.Y && bottomRight.Y > point.Y;
        }

        #endregion

        #region ICanBeLoadedSaved

        public void SaveTo(XmlWriter writer)
        {
            writer.WriteStartElement(Type);
            writer.WriteAttributeString("Name", Name);
            writer.WriteAttributeString("X", Position.X.ToString());
            writer.WriteAttributeString("Y", Position.Y.ToString());
            writer.WriteAttributeString("S", Scale.ToString());
            OnSave(writer);
            foreach (var child in Children)
            {
                child.SaveTo(writer);
            }
            writer.WriteEndElement();
        }

        protected virtual void OnSave(XmlWriter writer)
        {
        }

        public void LoadFrom(XmlElement element)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
