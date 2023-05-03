using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public enum EditorState
    {
        None,
        AddNewPointPosition,
        AddBetweenPointPosition,
        MovePointPosition,
        DeletePointPosition,
        SetPointRadius,
        SetPointWidth
    }

    public class State
    {
        public EditorState editorState = default;
        public Point? StartPosition;
        public Point? EndPosition;

        public List<Corner> Corners = new List<Corner>();
    }

    public class Corner
    {
        public Point Center;
        public float Radius;
        public float Direction;
    }
}
