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
        public static string ENEMY_MODEL = "Models/ZombieEnemy";
        public static string ARENA_MODEL = "Models/roundArena";

        // sounds
        public static string JUMP_SOUND = "SoundFX/jump";

        // movement & position
        public static int JUMP_UP = 1;
        public static int JUMP_DOWN = -1;

        // player attributes
        public static int PLAYER_SPEED = 7;
        public static Vector2 PLAYER_HEALTHBAR_INITIAL_POSITION = new Vector2(10.0f, 10.0f);
        public static int PLAYER_INITIAL_HEALTH = 200;
        public static int PLAYER_HEIGHT = 400;

        // various constants
        public static Vector3 CAMERA_INITIAL_POSITION = new Vector3(0.0f, 200.0f, 5000.0f);
        public static Vector3 CAMERA_TARGET_INITIAL_POSITION = Vector3.Zero;
        public static int HEALTHBAR_WIDTH = 20;
        public static Vector3 ARENA_INITIAL_LOCATION = Vector3.Zero;
    }
}
