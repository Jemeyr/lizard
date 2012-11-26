using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace lizard
{
    class Camera
    {
        private Vector2 position;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        

        private Vector2 origin;

        public Camera(Viewport viewport)
        {
            position = Vector2.Zero;
            origin = new Vector2(viewport.Width / 2.0f, viewport.Height / 2.0f);
        }

        /// <summary>
        /// Move the camera in the x,y axes given the delta position value.
        /// </summary>
        /// <param name="delta_position">Amount to translate the camera.</param>
        public void MoveCamera(Vector2 delta_position)
        {
            position += delta_position;
        }

        public void LookAtCenter(Vector2 position)
        {
            this.position = position;
        }
        
        public void LookAt(Vector2 position)
        {
            this.position = position - origin;
        }

        /// <summary>
        /// Returns the transformation of the camera after being translated, rotated and scaled upon 
        /// the center of the screen.
        /// </summary>
        /// <returns>The camera transformation matrix that is translated, rotated, and scaled.</returns>
        public Matrix GetCameraTransformation()
        {
            return Matrix.CreateTranslation(new Vector3(-position, 0)) *
                Matrix.CreateTranslation(new Vector3(-origin, 0)) *
                Matrix.CreateTranslation(new Vector3(origin, 0));
        }
    }
}
