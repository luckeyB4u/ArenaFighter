﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        // character attributes
        public static int PLAYER_INITIAL_HEALTH = 200;
        public static int HEALTHBAR_WIDTH = 20;
        public static int PLAYER_HEIGHT = 400;
    }
}
