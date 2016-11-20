using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ArenaFighter
{
    class Zombie
    {
        Vector3 location;
        Vector3 walkDirection;
        int health;

        float rotationXAxis; // Rotates on YZ-plane
        float rotationYAxis; // Rotates on XZ-plane (ground)
        float rotationZAxis; // Rotates on XY-plane

        Game1 game;
        Model myModel;
        float aspectRatio;

        public Zombie(Game1 g)
        {
            location = GameConstants.ZOMBIE_INITIAL_POSITION;
            walkDirection = Vector3.Zero;
            health = GameConstants.ZOMBIE_INITIAL_HEALTH;

            rotationXAxis = 0.0f;
            rotationYAxis = 0.0f;
            rotationZAxis = 0.0f;

            game = g;
            myModel = g.Content.Load<Model>(GameConstants.ZOMBIE_MODEL);
            aspectRatio = g.aspectRatio;
        }

        // Gets zombie's health
        public int getHealth()
        {
            return health;
        }

        public Vector3 getLocation()
        {
            return location;
        }

        public void Update(GameTime gameTime)
        {
            // Moves zombie if not touching wall
            Vector3 newLoc = location + walkDirection * GameConstants.ZOMBIE_SPEED;
            if (newLoc.Length() < GameConstants.ARENA_SIZE)
            {
                location = newLoc;
            }

            rotationYAxis += MathHelper.ToRadians(1);
        }

        public void Draw()
        {
            // Copy any parent transforms
            Matrix[] transforms = new Matrix[myModel.Bones.Count];
            myModel.CopyAbsoluteBoneTransformsTo(transforms);

            // Draws the model. A model can have multiple meshes, so loop
            foreach (ModelMesh mesh in myModel.Meshes)
            {
                // This is where the mesh orientation is set, as well 
                // as our camera and projection
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] *
                        Matrix.CreateRotationY(rotationYAxis)
                        * Matrix.CreateRotationX(rotationXAxis)
                        * Matrix.CreateTranslation(location);
                    effect.View = Matrix.CreateLookAt(game.cameraPosition,
                        game.cameraTarget, Vector3.Up);
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(
                        MathHelper.ToRadians(45.0f), aspectRatio,
                        1.0f, 10000.0f);
                }
                // Draws the mesh, using the effects set above
                mesh.Draw();
            }
        }
    }
}
