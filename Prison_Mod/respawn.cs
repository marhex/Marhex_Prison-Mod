using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;
using GTA.Native;


    public class respawn 
    {
        public static void RespawnPlayer(Vector3 spawn, float heading)
        {
            Game.Player.Character.IsInvincible = true;
            Function.Call(Hash.SET_ENTITY_HEALTH, new InputArgument[] { Game.Player.Character, 200 });

            int hh = Function.Call<int>(Hash.GET_CLOCK_HOURS);
            int mm = Function.Call<int>(Hash.GET_CLOCK_MINUTES);
            int ss = Function.Call<int>(Hash.GET_CLOCK_SECONDS);
            Game.Player.Character.CanRagdoll = true;
            Function.Call(Hash.SET_PED_TO_RAGDOLL, Game.Player.Character, 0x2710, 0x2710, 0, 1, 1, 1);
            Function.Call(Hash.IGNORE_NEXT_RESTART, 1);
            Function.Call(Hash.DISABLE_HOSPITAL_RESTART, 1);
            Function.Call(Hash.SET_FADE_OUT_AFTER_DEATH, 0);
            Function.Call(Hash._SET_CAM_EFFECT, 1);
            Function.Call(Hash.CLEAR_PLAYER_WANTED_LEVEL, Game.Player);
            Function.Call(Hash.DISPLAY_RADAR, 0);
            Function.Call(Hash.DISPLAY_HUD, 0);
            Function.Call(Hash._START_SCREEN_EFFECT, "DeathFailNeutralIn", 0x7d0, 0);
            Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "ScreenFlash", "BustedSounds", 1);
            Function.Call(Hash.SET_TIME_SCALE, 0.4f);
            Function.Call(Hash.CLEAR_PED_BLOOD_DAMAGE, Game.Player.Character);
            Script.Wait(0x7d0);
            Function.Call(Hash._START_SCREEN_EFFECT, "DeathFailOut", 0x1388, 0);
            Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "TextHit", "BustedSounds");
            Function.Call(Hash.SET_TIME_SCALE, 0.1f);
            Script.Wait(0xfa0);
            Function.Call(Hash.DO_SCREEN_FADE_OUT, 0x3e8);
            Script.Wait(0x1388);
            Function.Call(Hash.DO_SCREEN_FADE_IN, 500);
            Function.Call(Hash.SET_TIME_SCALE, 1f);
            Function.Call(Hash.DISPLAY_RADAR, 1);
            Function.Call(Hash.DISPLAY_HUD, 1);
            Script.Wait(200);
            Function.Call(Hash._SET_CAM_EFFECT, 0);
            Function.Call(Hash._START_SCREEN_EFFECT, "ExplosionJosh3", 0x3e8, 0);
            Game.Player.Character.IsInvincible = false;
            Function.Call(Hash.DISABLE_HOSPITAL_RESTART, 0);
            Function.Call(Hash.SET_FADE_OUT_AFTER_DEATH, 1);
            Function.Call(Hash.ENABLE_ALL_CONTROL_ACTIONS, new InputArgument[] { 1 });
            Game.Player.CanControlCharacter = true;

            Function.Call(Hash.SET_ENABLE_HANDCUFFS, new InputArgument[] { Game.Player.Character, 0 });
            Function.Call(Hash.SET_ENABLE_BOUND_ANKLES, new InputArgument[] { Game.Player.Character, 0 });
            int num = Function.Call<int>(Hash.GET_ENTITY_MODEL, new InputArgument[] { Game.Player.Character });
            if (num == Function.Call<int>(Hash.GET_HASH_KEY, new InputArgument[] { "player_zero" }))
            {
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, new InputArgument[] { Game.Player.Character, 3, 12, 4, 2 });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, new InputArgument[] { Game.Player.Character, 4, 11, 4, 2 });
            }
            else if (num == Function.Call<int>(Hash.GET_HASH_KEY, new InputArgument[] { "player_one" }))
            {
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, new InputArgument[] { Game.Player.Character, 3, 1, 5, 2 });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, new InputArgument[] { Game.Player.Character, 4, 1, 5, 2 });
            }
            else if (num == Function.Call<int>(Hash.GET_HASH_KEY, new InputArgument[] { "player_two" }))
            {
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, new InputArgument[] { Game.Player.Character, 3, 5, 2, 1 });
                Function.Call(Hash.SET_PED_COMPONENT_VARIATION, new InputArgument[] { Game.Player.Character, 4, 5, 2, 1 });
            }
            Function.Call(Hash.SET_CLOCK_TIME, hh, mm, ss);

            Game.Player.Character.Position = spawn;
            Game.Player.Character.Heading = heading;
        }
        
    }

