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
    class Player
    {
        Vector3 location;
        Vector3 speed;
        Vector3 cameraOffset;

        float rotationTheta;
        float rotationPhi;

        Game1 game;
        Model myModel;
        float aspectRatio;

        int speedMultiplier;

        Boolean isInAir;
        int airDirection;
        int airSpeed;
        int maxJumpHeight;
        float rotationSpeed;

        private SoundEffect jumpSound;


        public Player(Game1 g)
        {
            location = new Vector3(-650, 0, 650);
            speed = Vector3.Zero;
            cameraOffset = new Vector3(0.0f, 1000.0f, 2000.0f);

            rotationTheta = 0.0f;
            rotationPhi = 0.0f;

            game = g;
            myModel = game.Content.Load<Model>("Models/player");
            aspectRatio = game.aspectRatio;

            speedMultiplier = 7;
            isInAir = false;
            airSpeed = 15;
            maxJumpHeight = 175;
            rotationSpeed = MathHelper.ToRadians(3);
            jumpSound = game.Content.Load<SoundEffect>("SoundFX/jump");

        }

        KeyboardState oldState = Keyboard.GetState();

        public void Update(GameTime gameTime)
        {
            location += speed * speedMultiplier;

            //currently don't need oldstate/newstate stuff but left it in case
            //we need it later
            KeyboardState newState = Keyboard.GetState();

            // Is the W key down?
            if (newState.IsKeyDown(Keys.W))
            {
                speed.Z = -1;
            }

            // Is the A key down?
            if (newState.IsKeyDown(Keys.A))
            {
                speed.X = -1;
            }

            // Is the S key down?
            if (newState.IsKeyDown(Keys.S))
            {
                speed.Z = 1;
            }

            // Is the D key down?
            if (newState.IsKeyDown(Keys.D))
            {
                speed.X = 1;
            }

            // Space Key for Jumping
            if (newState.IsKeyDown(Keys.Space) && !isInAir)
            {
                jump();
            }

            // Right key for rotating right
            if (newState.IsKeyDown(Keys.Right))
            {
                rotationPhi -= rotationSpeed;
            }

            // Left key for rotating left
            if (newState.IsKeyDown(Keys.Left))
            {
                rotationPhi += rotationSpeed;
            }

            updateJump();

            //Returns speeds to 0 if no keys pressed
            if (newState.IsKeyUp(Keys.W) && newState.IsKeyUp(Keys.S))
                speed.Z = 0;
            if (newState.IsKeyUp(Keys.A) && newState.IsKeyUp(Keys.D))
                speed.X = 0;

            oldState = newState;
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
                    game.cameraPosition = location + cameraOffset;
                    game.cameraTarget = location;
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

        public void jump()
        {
            if (isInAir == false)
            {
                isInAir = true;
                airDirection = GameConstants.JUMP_UP;
            }
            jumpSound.Play(.005f,0f,0f);
        }

        public void updateJump()
        {
            if (!isInAir)
            {
                return;
            }

            if (location.Y >= maxJumpHeight)
            {
                airDirection = GameConstants.JUMP_DOWN;
            }

            location.Y += airSpeed * airDirection;

            if (location.Y <= 0)
            {
                location.Y = 0;
                isInAir = false;
                airSpeed = 10;
            }
        }


    }
}
