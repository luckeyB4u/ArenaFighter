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
        int attackCooldown;

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
            attackCooldown = 0;

            rotationXAxis = 0.0f;
            rotationYAxis = GameConstants.ZOMBIE_INITIAL_Y_ROTATION;
            rotationZAxis = 0.0f;

            game = g;
            myModel = g.Content.Load<Model>(GameConstants.ZOMBIE_MODEL);
            aspectRatio = g.aspectRatio;
        }

        // Gets zombie location
        public Vector3 getLocation()
        {
            return location;
        }

        // Gets zombie health
        public int getHealth()
        {
            return health;
        }

        // Changes the zombie health by 'amount'
        public void changeHealth(int amount)
        {
            health += amount;
        }

        public void Update(GameTime gameTime, Player player)
        {
            // Moves zombie if not touching wall
            Vector3 newLoc = location + walkDirection * GameConstants.ZOMBIE_SPEED;
            if (newLoc.Length() < GameConstants.ARENA_SIZE)
            {
                location = newLoc;
            }

            if(attackCooldown > 0)
            {
                attackCooldown++;
            }
            if(attackCooldown == GameConstants.ZOMBIE_ATTACK_COOLDOWN)
            {
                attackCooldown = 0;
            }

            updateAI(player);

            //rotationYAxis += MathHelper.ToRadians(1);
        }

        public void updateAI(Player player)
        {
            Vector3 vect1 = location;
            Vector3 vect2 = player.getLocation();
            vect1.Z *= -1;
            vect2.Z *= -1;
            Vector3 enemyPlayerVect = vect2 - vect1;
            float enemyPlayerAngle = Functions.moduloFloats(Functions.vectAngleXZ(enemyPlayerVect) - rotationYAxis, 2 * (float)Math.PI);
            float enemyPlayerDistance = enemyPlayerVect.Length();

            if(enemyPlayerAngle > GameConstants.ZOMBIE_ROTATION_LENIENCY)
            {
                rotationYAxis += GameConstants.ZOMBIE_ROTATION_SPEED;
            }
            else if(enemyPlayerAngle < GameConstants.ZOMBIE_ROTATION_LENIENCY)
            {
                rotationYAxis -= GameConstants.ZOMBIE_ROTATION_SPEED;
            }

            if(enemyPlayerDistance > GameConstants.ZOMBIE_ATTACK_DISTANCE)
            {
                walkDirection = Functions.rotateVectXZ(GameConstants.RIGHT, -rotationYAxis);
            }
            else
            {
                walkDirection = Vector3.Zero;
                if (enemyPlayerAngle <= GameConstants.ZOMBIE_ATTACK_ANGLE & enemyPlayerAngle >= -GameConstants.ZOMBIE_ATTACK_ANGLE & attackCooldown == 0)
                {
                    attackCooldown++;
                    player.changeHealth(-20);
                }
            }
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
                        * Matrix.CreateRotationY(rotationYAxis - (float)Math.PI / 2)
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
