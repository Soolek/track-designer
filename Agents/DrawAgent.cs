using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame;
using Model;

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
                    if (prevCorner != null)
                    {
                        var tangent = FindConnectingTangent(prevCorner, corner);
                        spriteBatch.DrawLine(tangent.Item1, tangent.Item2, Color.Yellow);
                    }
                }

                if (state.StartPosition != null && state.EndPosition != null)
                {
                    spriteBatch.DrawLine(state.StartPosition.Value.ToVector2(), state.EndPosition.Value.ToVector2(), Color.Red);
                    spriteBatch.DrawCircle(state.EndPosition.Value.ToVector2(),
                        Vector2.Distance(state.StartPosition.Value.ToVector2(), state.EndPosition.Value.ToVector2()),
                        50,
                        Color.Red);
                }
            }
            spriteBatch.End();
        }

        // ChatGPT failed?
        public static (Vector2,Vector2) FindConnectingTangent(Corner c1, Corner c2)
        {
            var x1 = c1.Center.X;
            var y1 = c1.Center.Y;
            var r1 = c1.Radius;
            var x2 = c2.Center.X;
            var y2 = c2.Center.Y;
            var r2 = c2.Radius;

            // Calculate the distance between the centers of the circles
            double d = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));

            // Calculate the angle between the line connecting the centers of the circles and the x-axis
            double theta = Math.Atan2(y2 - y1, x2 - x1);

            // Calculate the radius vectors of the circles
            double r1x = r1 * Math.Cos(theta);
            double r1y = r1 * Math.Sin(theta);
            double r2x = r2 * Math.Cos(theta + Math.PI);
            double r2y = r2 * Math.Sin(theta + Math.PI);

            // Calculate the angle between the radius vector and the tangent
            double alpha = Math.Acos((r2 - r1) / d);

            // Calculate the direction of the tangent
            double dir = Math.Sign(r2 - r1);

            // Calculate the coordinates of the point of tangency
            double tx = x1 + (r1 + dir * r1 * Math.Cos(alpha)) * Math.Cos(theta);
            double ty = y1 + (r1 + dir * r1 * Math.Cos(alpha)) * Math.Sin(theta);

            // Calculate the endpoints of the tangent line segment
            double l = Math.Sqrt(Math.Pow(r1 + dir * r1 * Math.Cos(alpha), 2) - Math.Pow(r1, 2));
            double lx1 = tx + l * Math.Cos(theta + dir * Math.PI / 2);
            double ly1 = ty + l * Math.Sin(theta + dir * Math.PI / 2);
            double lx2 = tx + l * Math.Cos(theta - dir * Math.PI / 2);
            double ly2 = ty + l * Math.Sin(theta - dir * Math.PI / 2);

            return new(new Vector2((float)lx1, (float)ly1), new Vector2((float)lx2, (float)ly2));
        }
    }
}
