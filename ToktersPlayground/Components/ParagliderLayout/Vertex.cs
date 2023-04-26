using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ToktersPlayground.Components.ParagliderLayout
{
    public enum VertexType
    {
        Simple,
        AttachmentPoint,
    }

    /// <summary>
    /// Represent a point on the paraglider.
    /// </summary>
    public class Vertex
    {
        public Vector2 Position { get; set; }
        public VertexType Type { get; set; }
        public bool IsOnPerimeter { get; set; }
        public string Name { get; set; }
    }
}
