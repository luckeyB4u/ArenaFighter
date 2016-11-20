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
    class GameConstants
    {
        // models
        public static string PLAYER_MODEL = "Models/alienplayer";
        public static string ZOMBIE_MODEL = "Models/ZombieEnemy";
        public static string ARENA_MODEL = "Models/roundArena";

        // sounds
        public static string JUMP_SOUND = "SoundFX/jump";

        // movement & position
        public static int JUMP_UP = 1;
        public static int JUMP_DOWN = -1;

        // player attributes
        public static Vector3 PLAYER_INITIAL_POSITION = new Vector3(0, 0, 1200);
        public static int PLAYER_SPEED = 7;
        public static Vector2 PLAYER_HEALTHBAR_INITIAL_POSITION = new Vector2(10, 10);
        public static int PLAYER_INITIAL_HEALTH = 200;
        public static int PLAYER_HEIGHT = 400;

        // enemy attributes
        public static Vector3 ZOMBIE_INITIAL_POSITION = new Vector3(0, 0, -1200);
        public static int ZOMBIE_SPEED = 7;
        public static Vector2 ZOMBIE_HEALTHBAR_INITIAL_POSITION = new Vector2(10, 10);
        public static int ZOMBIE_INITIAL_HEALTH = 200;
        public static int ZOMBIE_COLLISION_BUBBLE_SIZE = 100;

        // various constants
        public static int GRAVITY = 10;
        public static Vector3 CAMERA_INITIAL_POSITION = new Vector3(0, 200, 5000);
        public static Vector3 CAMERA_TARGET_INITIAL_POSITION = Vector3.Zero;
        public static Vector3 CAMERA_OFFSET = new Vector3(0, 500, 1500);
        public static float CAMERA_ROTATION_SENSITIVITY = 0.01f;
        public static Vector3 FORWARD = new Vector3(0, 0, -1);
        public static int HEALTHBAR_WIDTH = 20;
        public static Vector3 ARENA_INITIAL_LOCATION = Vector3.Zero;
        public static int ARENA_SIZE = 2500;
    }
}
