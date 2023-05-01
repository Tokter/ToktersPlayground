using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ToktersPlayground.Controls.SceneGraph
{
    public interface ITransformable
    {
        float Rotation { get; set; }
        Vector2 Position { get; set; }
        Vector2 AbsPosition { get; set; }
        float Scale { get; set; }
        Matrix3x2 GetTransform();
        Matrix3x2 GetInvTransform();
        Vector2 ToAbs(Vector2 pos);
        Vector2 ToLocal(Vector2 pos);
        ITransformable Parent { get; }
    }

    public interface IIntersectable
    {
        bool IntersectsWidth(Vector2 pos);
        bool InRect(Vector2 rect1, Vector2 rect2);
    }

    public interface IDragable : IIntersectable, ITransformable
    {
    }
}
