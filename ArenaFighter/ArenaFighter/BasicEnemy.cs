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
    class BasicEnemy
    {

        Vector3 location;
        Vector3 speed;
        float rotationTheta;
        float rotationPhi;
        Game1 game;
        Model myModel;
        float aspectRatio;

        public BasicEnemy(Game1 g)
        {
            location = new Vector3(650, 0, -650);
            speed = Vector3.Zero;
            rotationTheta = 0.0f;
            rotationPhi = 0.0f;
            game = g;
            myModel = game.Content.Load<Model>("Models/player");
            aspectRatio = game.aspectRatio;
        }

        public void Update(GameTime gameTime)
        {
            location += speed;

            rotationPhi += MathHelper.ToRadians(1);
        }

        public void Draw()
        {
            // Copy any parent transforms.
            Matrix[] transforms = new Matrix[myModel.Bones.Count];
            myModel.CopyAbsoluteBoneTransformsTo(transforms);

            // Draw the model. A model can have multiple meshes, so loop.
            foreach (ModelMesh mesh in myModel.Meshes)
            {
                // This is where the mesh orientation is set, as well 
                // as our camera and projection.
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] *
                        Matrix.CreateRotationY(rotationPhi)
                        * Matrix.CreateRotationX(rotationTheta)
                        * Matrix.CreateTranslation(location);
                    effect.View = Matrix.CreateLookAt(game.cameraPosition,
                        game.cameraTarget, Vector3.Up);
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(
                        MathHelper.ToRadians(45.0f), aspectRatio,
                        1.0f, 10000.0f);
                }
                // Draw the mesh, using the effects set above.
                mesh.Draw();
            }

        }

    }
}
