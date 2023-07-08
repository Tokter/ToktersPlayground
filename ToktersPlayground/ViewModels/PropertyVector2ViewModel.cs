using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ToktersPlayground.ViewModels
{
    public class PropertyVector2ViewModel : PropertyBase
    {
        public PropertyVector2ViewModel(PropertyEditorViewModel propertyEditor, PropertyDetail details) : base(propertyEditor, details)
        {
        }

        public Vector2 Vector
        {
            get => (Vector2)Editor.GetValue(Details, Vector2.Zero);
            set
            {
                Editor.SetValue(Details, value);
                this.RaisePropertyChanged();
            }
        }

        public float X
        {
            get => Vector.X;
            set
            {
                Editor.SetValue(Details, new Vector2(value, Vector.Y));
                this.RaisePropertyChanged();
            }
        }

        public float Y
        {
            get => Vector.Y;
            set
            {
                Editor.SetValue(Details, new Vector2(Vector.X, value));
                this.RaisePropertyChanged();
            }
        }

        private string? _valueText = null;
        public string ValueText
        {
            get
            {
                if (_valueText != null) return _valueText;
                return $"{X:0.#####}, {Y:0.#####}";
            }
            set
            {
                var parts = value.Split(",");

                if (parts.Length==2 && float.TryParse(parts[0], out float resultX) && float.TryParse(parts[1], out float resultY))
                {
                    Vector = new Vector2(resultX, resultY);
                    _valueText = null;
                    ClearError();
                }
                else
                {
                    _valueText = value;
                    SetError($"{_valueText} is not a valid Vector2!");
                }
                this.RaisePropertyChanged();
            }
        }

        public override void Update()
        {
            this.RaisePropertyChanged(nameof(X));
            this.RaisePropertyChanged(nameof(Y));
            this.RaisePropertyChanged(nameof(ValueText));
        }
    }
}
