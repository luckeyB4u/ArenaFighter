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
        Vector2 speed;

        Vector3 cameraOffset;
        float camRotation;
        float camRotationSensitivity = 0.01f;

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

        int health;
        Boolean prevCollided;
        GraphicsDeviceManager graphics;

        Healthbar healthbar;

        private SoundEffect jumpSound;

        KeyboardState oldState = Keyboard.GetState();


        public Player(Game1 g, GraphicsDeviceManager graph)
        {
            location = new Vector3(0, 0, 1000);
            speed = Vector2.Zero;
            cameraOffset = new Vector3(0.0f, 500.0f, 1500.0f);
            camRotation = 0.0f;

            rotationTheta = 0.0f;
            rotationPhi = 0.0f;

            game = g;
            myModel = game.Content.Load<Model>(GameConstants.PLAYER_MODEL);
            aspectRatio = game.aspectRatio;

            // Jumping
            isInAir = false;
            airSpeed = 15;
            maxJumpHeight = 175;

            // Speed and rotation
            speedMultiplier = 7;
            rotationSpeed = MathHelper.ToRadians(3);
            health = 200;
            prevCollided = false;

            jumpSound = game.Content.Load<SoundEffect>(GameConstants.JUMP_SOUND);

            graphics = graph;

            //healthbar = new ArenaFighter.Healthbar(g, graphics, health, new Vector2(0, 0));

        }

        public int getHealth()
        {
            return health;
        }

        public Boolean isCollision(Vector3 loc)
        {
            Vector3 dist = location - loc;
            if (dist.Length() < 100)
            {
                return true;
            }
            return false;
        }

        public Vector2 rotateVect(Vector2 vect, float degrees)
        {
            Vector2 result = Vector2.Zero;
            result.X = vect.X * (float)Math.Cos(degrees) - vect.Y * (float)Math.Sin(degrees);
            result.Y = vect.X * (float)Math.Sin(degrees) + vect.Y * (float)Math.Cos(degrees);
            return result;
        }

        public void rotateCamera(float distance)
        {
            //rotates the camera offset vector on the XZ-plane based on how much
            //the mouse moved and the sensitivity
            float currentX = cameraOffset.X;
            float currentZ = cameraOffset.Z;
            float degrees = distance * camRotationSensitivity;
            cameraOffset.X = currentX * (float)Math.Cos(degrees) - currentZ * (float)Math.Sin(degrees);
            cameraOffset.Z = currentX * (float)Math.Sin(degrees) + currentZ * (float)Math.Cos(degrees);
            camRotation += degrees;
        }

        public void Update(GameTime gameTime, BasicEnemy enemy, SpriteBatch sprite)
        {
            //Collision detection with enemy
            if(isCollision(enemy.getLocation()) && !prevCollided)
            {
                health -= 20;
                prevCollided = true;
            }
            else if(!isCollision(enemy.getLocation()))
            {
                prevCollided = false;
            }

            //Collision detection for walls, moves player if not touching wall
            float newLocX = location.X + rotateVect(speed, camRotation).X * speedMultiplier;
            float newLocZ = location.Z + rotateVect(speed, camRotation).Y * speedMultiplier;
            if ((newLocX < 2000) && (newLocX > -2000))
            {
                location.X = newLocX;
            }
            if ((newLocZ < 2000) && (newLocZ > -2000))
            {
                location.Z = newLocZ;
            }

            KeyboardState newState = Keyboard.GetState();

            //WASD movement on the XZ-plane
            if (newState.IsKeyDown(Keys.W))
            {
                rotationPhi = -camRotation;
                speed.Y = -1;
            }
            if (newState.IsKeyDown(Keys.A))
            {
                rotationPhi = -camRotation + (float)Math.PI / 2;
                speed.X = -1;
            }
            if (newState.IsKeyDown(Keys.S))
            {
                rotationPhi = -camRotation + (float)Math.PI;
                speed.Y = 1;
            }
            if (newState.IsKeyDown(Keys.D))
            {
                rotationPhi = -camRotation - (float)Math.PI / 2;
                speed.X = 1;
            }

            //Orients player correctly in case multiple buttons pressed
            if (newState.IsKeyDown(Keys.W) && newState.IsKeyDown(Keys.A))
            {
                rotationPhi = -camRotation + (float)Math.PI / 4;
            }
            if (newState.IsKeyDown(Keys.A) && newState.IsKeyDown(Keys.S))
            {
                rotationPhi = -camRotation + (float)Math.PI * 3 / 4;
            }
            if (newState.IsKeyDown(Keys.S) && newState.IsKeyDown(Keys.D))
            {
                rotationPhi = -camRotation - (float)Math.PI * 3 / 4;
            }
            if (newState.IsKeyDown(Keys.W) && newState.IsKeyDown(Keys.D))
            {
                rotationPhi = -camRotation - (float)Math.PI / 4;
            }

            //Returns speeds to 0 if no keys pressed
            if (newState.IsKeyUp(Keys.W) && newState.IsKeyUp(Keys.S))
                speed.Y = 0;
            if (newState.IsKeyUp(Keys.A) && newState.IsKeyUp(Keys.D))
                speed.X = 0;

            //Zooms in and out with Q/E keys
            if(newState.IsKeyDown(Keys.E) && oldState.IsKeyUp(Keys.E))
            {
                cameraOffset *= 0.9f;
            }
            if (newState.IsKeyDown(Keys.Q) && oldState.IsKeyUp(Keys.Q))
            {
                cameraOffset *= 1.1f;
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

            oldState = newState;

            //healthbar.Draw(sprite, graphics.GraphicsDevice);
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
                airDirection = GameConstants.JUMP_UP; //change direction of movement to upwards
            }
            jumpSound.Play(.005f,0f,0f);
        }

        public void updateJump()
        {
            // exit the function if the box isnt even in the air
            if (!isInAir)
            {
                return;
            }

            // change direction if the box reaches the max height
            if (location.Y >= maxJumpHeight)
            {
                airDirection = GameConstants.JUMP_DOWN;
            }

            // change location of the box in a certain direction
            location.Y += airSpeed * airDirection;

            //put the box back on the ground if it goes on or below the ground
            if (location.Y <= 0)
            {
                location.Y = 0;
                isInAir = false;
                airSpeed = 10;
            }
        }


    }
}
