using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame;
using Model;
using System.Collections;

namespace Agents
{
    internal class DrawAgent
    {
        private State state;
        private readonly SpriteBatch spriteBatch;

        public DrawAgent(State state, SpriteBatch spriteBatch)
        {
            this.state = state;
            this.spriteBatch = spriteBatch;
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();// blendState: BlendState.AlphaBlend);
            {
                for (int i = 0; i < state.Corners.Count; i++)
                {
                    var corner = state.Corners[i];
                    var prevCorner = i > 0 ? state.Corners[i - 1] : null;

                    // main
                    spriteBatch.DrawCircle(corner.Center.ToVector2(), corner.Radius, 50, Color.Green);
                    var dirVector = new Vector2((float)Math.Cos(corner.Direction), (float)Math.Sin(corner.Direction));
                    spriteBatch.DrawLine(corner.Center.ToVector2(), corner.Center.ToVector2() + dirVector * corner.Radius, Color.Green);

                    // tangents
                    var colors = new[] { Color.Green, Color.Red, Color.GreenYellow, Color.OrangeRed };
                    if (prevCorner != null)
                    {
                        var tangents = FindConnectingTangent(prevCorner, corner);
                        for (var j = 0; j < tangents.Count(); j++)
                        {
                            var tangent = tangents.ElementAt(j);
                            spriteBatch.DrawLine(tangent.Item1, tangent.Item2, colors[j]);
                        }
                    }
                }

                if (state.StartPosition != null && state.EndPosition != null)
                {
                    spriteBatch.DrawLine(state.StartPosition.Value.ToVector2(), state.EndPosition.Value.ToVector2(), Color.Red);
                    spriteBatch.DrawCircle(
                        state.EndPosition.Value.ToVector2(),
                        Vector2.Distance(state.StartPosition.Value.ToVector2(),
                        state.EndPosition.Value.ToVector2()),
                        50,
                        Color.Red
                    );
                }
            }
            spriteBatch.End();
        }

        public static IEnumerable<(Vector2, Vector2)> FindConnectingTangent(Corner c1, Corner c2)
        {
            // Calculate the distance between the two circle centers
            float dx = c2.Center.X - c1.Center.X;
            float dy = c2.Center.Y - c1.Center.Y;
            float d = (float)Math.Sqrt(dx * dx + dy * dy);

            // Check if the circles are coincident
            if (d <= Math.Abs(c1.Radius - c2.Radius))
            {
                yield break;
            }

            // Calculate the angle between the two centers
            float angle = (float)Math.Atan2(dy, dx);

            // Calculate the angle between the tangent line and the line connecting the two centers
            float alpha = (float)Math.Acos((c1.Radius - c2.Radius) / d);

            // Calculate the points where the tangents intersect the circles
            var p1 = new Vector2(c1.Center.X + c1.Radius * (float)Math.Cos(angle + alpha), c1.Center.Y + c1.Radius * (float)Math.Sin(angle + alpha));
            var p2 = new Vector2(c1.Center.X + c1.Radius * (float)Math.Cos(angle - alpha), c1.Center.Y + c1.Radius * (float)Math.Sin(angle - alpha));
            var p3 = new Vector2(c2.Center.X + c2.Radius * (float)Math.Cos(angle + alpha), c2.Center.Y + c2.Radius * (float)Math.Sin(angle + alpha));
            var p4 = new Vector2(c2.Center.X + c2.Radius * (float)Math.Cos(angle - alpha), c2.Center.Y + c2.Radius * (float)Math.Sin(angle - alpha));

            yield return (p1, p3);
            yield return (p2, p4);

            // Calculate the points where the transverse tangents intersect the circles
            float beta = (float)Math.Acos((c1.Radius + c2.Radius) / d);
            var p5 = new Vector2(c1.Center.X + c1.Radius * (float)Math.Cos(angle + beta), c1.Center.Y + c1.Radius * (float)Math.Sin(angle + beta));
            var p6 = new Vector2(c1.Center.X + c1.Radius * (float)Math.Cos(angle - beta), c1.Center.Y + c1.Radius * (float)Math.Sin(angle - beta));
            var p7 = new Vector2(c2.Center.X + c2.Radius * (float)Math.Cos(angle + beta + Math.PI), c2.Center.Y + c2.Radius * (float)Math.Sin(angle + beta + Math.PI));
            var p8 = new Vector2(c2.Center.X + c2.Radius * (float)Math.Cos(angle - beta + Math.PI), c2.Center.Y + c2.Radius * (float)Math.Sin(angle - beta + Math.PI));

            yield return (p5, p7);
            yield return (p6, p8);
        }
    }
}
