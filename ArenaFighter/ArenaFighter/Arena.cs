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
    class Arena
    {
        Vector3 location;
        Model myModel;
        Game1 game;
        float aspectRatio;

        public Arena(Game1 g)
        {
            location = GameConstants.ARENA_INITIAL_LOCATION;
            myModel = g.Content.Load<Model>(GameConstants.ARENA_MODEL);
            game = g;
            aspectRatio = g.aspectRatio;
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
                    effect.World = transforms[mesh.ParentBone.Index]
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
