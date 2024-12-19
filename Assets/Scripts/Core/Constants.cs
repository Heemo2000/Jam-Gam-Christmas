using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class Constants
    {
        public const string MUSIC_VOLUME = "musicVol";
        public const string SFX_VOLUME = "sfxVol";

        public const string PLAYER_MOVE_INPUT = "move_input";
        public const string PLAYER_ROLL = "roll";
        public const string PLAYER_DIE = "die";
        public const string BOW_SHOOT = "bow_shoot";

        public const float PLAYER_SETUP_INTERVAL = 1.0f;
        public const float PILLAR_CHECK_RADIUS = 2.0f;
        public const float AIM_POINTER_OFFSET = 5.0f;
        public const float SUSCRIBE_TO_GAME_STATE_MGR_TIME = 1.0f;
        public const int MAX_INTERACT_COLLIDERS_COUNT = 20;
        public const int MAX_DETECT_ENEMIES_COUNT = 20;
        public const int MAX_DETECT_COUNT = 20;
        
    }
}
