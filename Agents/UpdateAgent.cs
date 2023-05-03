using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents
{
    internal class UpdateAgent
    {
        private State state;

        public UpdateAgent(State state)
        {
            this.state = state;
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState keybState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (state.StartPosition == null)
                {
                    state.StartPosition = mouseState.Position;
                }
                else
                {
                    state.EndPosition = mouseState.Position;
                }
            }
            else if (mouseState.LeftButton == ButtonState.Released)
            {
                if (state.StartPosition != null && state.EndPosition != null)
                {
                    var diff = state.EndPosition.Value.ToVector2() - state.StartPosition.Value.ToVector2();
                    state.Corners.Add(new Corner()
                    {
                        Center = state.EndPosition.Value,
                        Radius = Vector2.Distance(state.StartPosition.Value.ToVector2(), state.EndPosition.Value.ToVector2()),
                        Direction = (float)Math.Atan2(diff.Y, diff.X) - MathHelper.Pi
                    });

                    state.StartPosition = null;
                    state.EndPosition = null;
                }
            }
        }
    }
}
