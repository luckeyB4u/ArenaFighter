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
        Vector3 walkDirection;
        int health;
        Boolean collidingWithEnemy;

        float rotationXAxis; // Rotates on YZ-plane
        float rotationYAxis; // Rotates on XZ-plane (ground)
        float rotationZAxis; // Rotates on XY-plane

        // Jumping
        Boolean isInAir;
        int airDirection;
        int airSpeed;
        int maxJumpHeight;
        private SoundEffect jumpSound;

        Game1 game;
        Model myModel;
        float aspectRatio;

        Vector3 cameraOffset;
        float camRotation;
        float camRotationSensitivity;
        int zoomCount;

        KeyboardState oldState = Keyboard.GetState();

        public Player(Game1 g)
        {
            location = GameConstants.PLAYER_INITIAL_POSITION;
            walkDirection = Vector3.Zero;
            health = GameConstants.PLAYER_INITIAL_HEALTH;
            collidingWithEnemy = false;

            rotationXAxis = 0.0f;
            rotationYAxis = 0.0f;
            rotationZAxis = 0.0f;

            isInAir = false;
            airDirection = 0;
            airSpeed = 15;
            maxJumpHeight = 175;
            jumpSound = g.Content.Load<SoundEffect>(GameConstants.JUMP_SOUND);

            game = g;
            myModel = g.Content.Load<Model>(GameConstants.PLAYER_MODEL);
            aspectRatio = g.aspectRatio;

            cameraOffset = GameConstants.CAMERA_OFFSET;
            camRotation = 0.0f;
            camRotationSensitivity = GameConstants.CAMERA_ROTATION_SENSITIVITY;
            zoomCount = 0;
        }

        // Gets player health
        public int getHealth()
        {
            return health;
        }

        // Determines if player is inside the enemy's collision bubble
        public Boolean isCollisionWithEnemy(Vector3 loc)
        {
            Vector3 dist = location - loc;
            if (dist.Length() < GameConstants.ZOMBIE_COLLISION_BUBBLE_SIZE)
            {
                return true;
            }
            return false;
        }

        // Rotates a vector by an angle on the XZ-plane (ground)
        public Vector3 rotateVectXZ(Vector3 vect, float degrees)
        {
            Vector3 result = Vector3.Zero;
            result.X = vect.X * (float)Math.Cos(degrees) - vect.Z * (float)Math.Sin(degrees);
            result.Y = vect.Y;
            result.Z = vect.X * (float)Math.Sin(degrees) + vect.Z * (float)Math.Cos(degrees);
            return result;
        }

        public void rotateCamera(float distance)
        {
            // Rotates the camera offset vector on the XZ-plane based on how much
            // the mouse moved and the sensitivity
            float degrees = distance * camRotationSensitivity;
            cameraOffset = rotateVectXZ(cameraOffset, degrees);
            camRotation += degrees;
        }

        public void jump()
        {
            if (isInAir == false)
            {
                isInAir = true;
                airDirection = GameConstants.JUMP_UP; // Changes direction of movement to upwards
            }
            jumpSound.Play(.005f, 0f, 0f);
        }

        public void updateJump()
        {
            // Exits the function if the box is not in the air
            if (!isInAir)
            {
                return;
            }

            // Changes direction if the box reaches the max height
            if (location.Y >= maxJumpHeight)
            {
                airDirection = GameConstants.JUMP_DOWN;
            }

            // Changes location of the box in a certain direction
            location.Y += airSpeed * airDirection;

            // Puts the box back on the ground if it goes on or below the ground
            if (location.Y <= 0)
            {
                location.Y = 0;
                isInAir = false;
                airSpeed = 10;
            }
        }

        public void Update(GameTime gameTime, Zombie enemy)
        {
            // Moves player if not touching wall
            Vector3 newLoc = location + walkDirection * GameConstants.PLAYER_SPEED;
            if (newLoc.Length() < GameConstants.ARENA_SIZE)
            {
                location = newLoc;
            }

            // Decreases player health if colliding with enemy
            if (isCollisionWithEnemy(enemy.getLocation()) && !collidingWithEnemy)
            {
                health -= 20;
                collidingWithEnemy = true;
            }
            else if(!isCollisionWithEnemy(enemy.getLocation()))
            {
                collidingWithEnemy = false;
            }

            KeyboardState newState = Keyboard.GetState();

            //WASD movement on the XZ-plane
            walkDirection = Vector3.Zero;
            if (newState.IsKeyDown(Keys.W))
            {
                walkDirection += GameConstants.FORWARD;
                rotationYAxis = -camRotation;
            }
            if (newState.IsKeyDown(Keys.A))
            {
                walkDirection += GameConstants.LEFT;
                rotationYAxis = -camRotation + (float)Math.PI / 2;
            }
            if (newState.IsKeyDown(Keys.S))
            {
                walkDirection += GameConstants.BACK;
                rotationYAxis = -camRotation + (float)Math.PI;
            }
            if (newState.IsKeyDown(Keys.D))
            {
                walkDirection += GameConstants.RIGHT;
                rotationYAxis = -camRotation - (float)Math.PI / 2;
            }
            if (newState.IsKeyDown(Keys.W) && newState.IsKeyDown(Keys.A))
            {
                rotationYAxis = -camRotation + (float)Math.PI / 4;
            }
            if (newState.IsKeyDown(Keys.A) && newState.IsKeyDown(Keys.S))
            {
                rotationYAxis = -camRotation + (float)Math.PI * 3 / 4;
            }
            if (newState.IsKeyDown(Keys.S) && newState.IsKeyDown(Keys.D))
            {
                rotationYAxis = -camRotation - (float)Math.PI * 3 / 4;
            }
            if (newState.IsKeyDown(Keys.W) && newState.IsKeyDown(Keys.D))
            {
                rotationYAxis = -camRotation - (float)Math.PI / 4;
            }

            // Normalizes the walk direction vector then rotates it to match
            // the camera rotation
            if (walkDirection != Vector3.Zero)
            {
                Vector3.Normalize(walkDirection);
            }
            walkDirection = rotateVectXZ(walkDirection, camRotation);

            // Zooms in and out with Q/E keys
            if (newState.IsKeyDown(Keys.Q) && oldState.IsKeyUp(Keys.Q) && zoomCount < 5)
            {
                zoomCount++;
                cameraOffset *= 1.1f;
            }
            if (newState.IsKeyDown(Keys.E) && oldState.IsKeyUp(Keys.E) && zoomCount > -5)
            {
                zoomCount--;
                cameraOffset *= 0.9f;
            }

            // Jumps when the space key is pressed
            if (newState.IsKeyDown(Keys.Space) && !isInAir)
            {
                jump();
            }

            updateJump();

            oldState = newState;
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
                        * Matrix.CreateRotationZ(rotationZAxis)
                        * Matrix.CreateRotationY(rotationYAxis)
                        * Matrix.CreateRotationX(rotationXAxis)
                        * Matrix.CreateTranslation(location);
                    game.cameraPosition = location + cameraOffset;
                    game.cameraTarget = location;
                    game.cameraTarget.Y += GameConstants.PLAYER_HEIGHT / 2;
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
