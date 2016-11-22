using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace ArenaFighter
{
    class Functions
    {
        // Rotates a vector by an angle on the XZ-plane (ground)
        public static Vector3 rotateVectXZ(Vector3 vect, float degrees)
        {
            Vector3 result = Vector3.Zero;
            result.X = vect.X * (float)Math.Cos(degrees) - vect.Z * (float)Math.Sin(degrees);
            result.Y = vect.Y;
            result.Z = vect.X * (float)Math.Sin(degrees) + vect.Z * (float)Math.Cos(degrees);
            return result;
        }

        // Computes the angle of a vector's XZ-components
        public static float vectAngleXZ(Vector3 vect)
        {
            float result = 0;
            if (vect.X == 0)
            {
                if(vect.Z == 0)
                {
                    result = 0;
                }
                else if(vect.Z > 0)
                {
                    result = (float)Math.PI / 2;
                }
                else
                {
                    result = -(float)Math.PI / 2;
                }
            }
            else if (vect.X > 0)
            {
                result = (float)Math.Atan(vect.Z / vect.X);
            }
            else
            {
                result = (float)Math.Atan(vect.Z / vect.X) + (float)Math.PI;
            }
            return moduloFloats(result, 2 * (float)Math.PI);
        }

        // Computes modulo for float values
        public static float moduloFloats(float x, float y)
        {
            float result = x / y;
            result = y * (result - (float)Math.Floor(result));
            return result;
        }
    }
}
