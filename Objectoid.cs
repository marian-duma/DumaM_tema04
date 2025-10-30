using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ProgramMain
{
    internal class Objectoid
    {
        private bool visibility;
        private bool isGravityBound;
        private Color color;
        private List<Vector3> vertices;

        private const int GRAVITY_OFFEST = 1;

        private Randomizer rando;
        public Objectoid()
        {
            rando = new Randomizer();
            visibility = true;
            isGravityBound = true;
            color = rando.GetRandomColor();

            int size_offset = rando.RandomInt(3, 7);
            int height_offset = rando.RandomInt(40, 60);
            int radial_offset = rando.RandomInt(5, 15);
            vertices = new List<Vector3>();
            vertices.Add(new Vector3(0 * size_offset + radial_offset, 0 * size_offset + height_offset, 1 * size_offset + radial_offset));
            vertices.Add(new Vector3(0 * size_offset + radial_offset, 0 * size_offset + height_offset, 0 * size_offset + radial_offset));
            vertices.Add(new Vector3(1 * size_offset + radial_offset, 0 * size_offset + height_offset, 1 * size_offset + radial_offset));
            vertices.Add(new Vector3(1 * size_offset + radial_offset, 0 * size_offset + height_offset, 0 * size_offset + radial_offset));
            vertices.Add(new Vector3(1 * size_offset + radial_offset, 1 * size_offset + height_offset, 1 * size_offset + radial_offset));
            vertices.Add(new Vector3(1 * size_offset + radial_offset, 1 * size_offset + height_offset, 0 * size_offset + radial_offset));
            vertices.Add(new Vector3(0 * size_offset + radial_offset, 1 * size_offset + height_offset, 1 * size_offset + radial_offset));
            vertices.Add(new Vector3(0 * size_offset + radial_offset, 1 * size_offset + height_offset, 0 * size_offset + radial_offset));
            vertices.Add(new Vector3(0 * size_offset + radial_offset, 0 * size_offset + height_offset, 1 * size_offset + radial_offset));
            vertices.Add(new Vector3(0 * size_offset + radial_offset, 0 * size_offset + height_offset, 0 * size_offset + radial_offset));

        }
        public void ToggleVisibility()
        {
            visibility = !visibility;
        }
        public void ToggleGravity()
        {
            isGravityBound = !isGravityBound;
        }
        public void SetGravity()
        {
            isGravityBound = true;
        }
        public void UnsetGravity()
        {
            isGravityBound = false;
        }
        public void setGravity(bool gravity)
        {
            isGravityBound = gravity;
        }
        public void Draw()
        {
            if (visibility)
            {
                GL.Color3(color);
                GL.Begin(PrimitiveType.QuadStrip);
                foreach (Vector3 vertex in vertices)
                {
                    GL.Vertex3(vertex);
                }
                GL.End();
            }
        }
        public void UpdatePosition()
        {
            if (visibility && isGravityBound && !GroundCollisionDetected())
            {
                for (int i = 0; i < vertices.Count; i++)
                {
                    vertices[i] = new Vector3(vertices[i].X, vertices[i].Y - GRAVITY_OFFEST, vertices[i].Z);
                }
            }
        }
        public bool GroundCollisionDetected()
        {
            foreach (Vector3 vertex in vertices)
            {
                if (vertex.Y <= 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
