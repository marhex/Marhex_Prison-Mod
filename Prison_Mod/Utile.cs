using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Linq;

namespace Prison_Mod
{
    public partial class Prison_Mod
    {
        Vector3[] places = new Vector3[] {
            new Vector3(1636.981f, 2495.046f, 45.0793f),
new Vector3(1625.254f, 2517.977f, 45.0724f),
new Vector3(1618.041f, 2537.57f, 45.0491f),
new Vector3(1606.693f, 2555.412f, 45.0393f),
new Vector3(1619.672f, 2573.754f, 45.0768f),
new Vector3(1632.494f, 2560.391f, 45.0746f),
new Vector3(1651.8f, 2561.016f, 45.0687f),
new Vector3(1677.801f, 2561.105f, 45.0648f),
new Vector3(1704.398f, 2560.417f, 45.0625f),
new Vector3(1734.842f, 2558.8f, 45.0597f),
new Vector3(1762.83f, 2559.915f, 45.0731f),
new Vector3(1766.144f, 2540.521f, 45.0613f),
new Vector3(1758.976f, 2523.867f, 45.0699f),
new Vector3(1752.025f, 2508.212f, 45.0708f),
new Vector3(1733.399f, 2509.638f, 45.0742f),
new Vector3(1719.375f, 2497.476f, 45.0629f),
new Vector3(1708.258f, 2486.847f, 45.074f),
new Vector3(1691.747f, 2475.106f, 45.0424f),
new Vector3(1691.549f, 2495.996f, 45.0711f),
new Vector3(1709.964f, 2515.75f, 45.0648f),
new Vector3(1733.279f, 2534.5f, 45.0679f),
new Vector3(1717.25f, 2549.015f, 45.0711f),
new Vector3(1703.685f, 2535.388f, 45.0641f),
new Vector3(1694.443f, 2523.642f, 45.0453f),
new Vector3(1683.89f, 2530.644f, 45.0736f),
new Vector3(1677.204f, 2546.95f, 45.0304f),
new Vector3(1649.789f, 2548.322f, 45.0736f),
new Vector3(1634.534f, 2536.363f, 45.0685f),
new Vector3(1634.604f, 2518.353f, 45.0684f),
new Vector3(1650.443f, 2523.027f, 45.074f),
new Vector3(1666.626f, 2526.317f, 45.0736f),
new Vector3(1673.313f, 2506.494f, 45.07f),
new Vector3(1691.063f, 2510.84f, 45.0733f),
new Vector3(1750.466f, 2563.415f, 55.4425f),
new Vector3(1751.663f, 2581.151f, 55.4434f),
new Vector3(1758.955f, 2569.42f, 55.4421f),
new Vector3(1762.181f, 2568.169f, 55.4403f),
new Vector3(1764.077f, 2565.081f, 55.4581f),
new Vector3(1791.498f, 2565.507f, 55.4662f),
new Vector3(1759.297f, 2595.05f, 55.4427f),
new Vector3(1758.871f, 2611.252f, 55.4431f),
new Vector3(1761.911f, 2621.585f, 55.442f),
new Vector3(1788.567f, 2621.934f, 55.4694f),
new Vector3(1751.131f, 2594.176f, 55.4441f),
new Vector3(1728.666f, 2595.571f, 55.4429f),
new Vector3(1721.499f, 2579.441f, 55.4431f),
new Vector3(1718.489f, 2569.444f, 55.4408f),
new Vector3(1699.811f, 2565.782f, 55.4408f),
new Vector3(1690.962f, 2556.386f, 55.0375f),
new Vector3(1692.001f, 2528.784f, 54.8772f),
new Vector3(1676.253f, 2565.84f, 55.4408f),
new Vector3(1662.656f, 2575.832f, 55.4408f),
new Vector3(1662.625f, 2596.875f, 55.4408f),
new Vector3(1662.815f, 2616.497f, 55.4408f),
new Vector3(1674.594f, 2621.792f, 55.4408f),
new Vector3(1689.449f, 2622.134f, 55.4408f),
new Vector3(1691.529f, 2635.59f, 55.0345f),
new Vector3(1690.793f, 2659.089f, 54.8768f),
new Vector3(1702.688f, 2621.96f, 55.4408f),
new Vector3(1715.874f, 2621.592f, 55.4408f),
new Vector3(1721.858f, 2631.303f, 55.4432f),
new Vector3(1721.679f, 2651.099f, 55.4431f),
new Vector3(1739.39f, 2651.851f, 55.4475f),
new Vector3(1751.635f, 2647.574f, 55.443f),
new Vector3(1751.299f, 2622.16f, 55.4425f),
        };
        public void GiveAllWeapons(Ped ped)
        {
            foreach (WeaponHash hash in weapons)
            {
                ped.Weapons.Give(hash, 0, true, true);
                ped.Weapons.Current.Ammo = ped.Weapons.Current.MaxAmmo;
                ped.Weapons.Current.AmmoInClip = ped.Weapons.Current.MaxAmmoInClip;
                ped.CanSwitchWeapons = true;
            }
        }

        public void GiveWeapons_prisoner(Ped ped)
        {
            ped.Weapons.RemoveAll();
            WeaponHash stunGun = WeaponHash.SMG;
            ped.Weapons.Give(stunGun, 0, true, true);
            ped.Weapons.Current.Ammo = ped.Weapons.Current.MaxAmmo;
            ped.Weapons.Current.AmmoInClip = ped.Weapons.Current.MaxAmmoInClip;
            ped.Task.FightAgainstHatedTargets(150f);
            ped.AlwaysKeepTask = true;
        }

        public void GiveWeapons_Garde(Ped ped)
        {
            ped.Weapons.RemoveAll();
            WeaponHash stunGun = WeaponHash.SMG;
            ped.Weapons.Give(stunGun, 0, true, true);
            ped.Weapons.Current.Ammo = ped.Weapons.Current.MaxAmmo;
            ped.Weapons.Current.AmmoInClip = ped.Weapons.Current.MaxAmmoInClip;
            ped.Task.FightAgainstHatedTargets(150f);
            ped.AlwaysKeepTask = true;
        }
        public void clothes_changer()
        {
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
        }
        public void intial1()
        {
            Game.Player.WantedLevel = 0;
            Game.Player.Character.Weapons.RemoveAll();
            Function.Call(Hash.DISABLE_ALL_CONTROL_ACTIONS, new InputArgument[] { 1 });



            
            Function.Call(Hash.SET_ENABLE_HANDCUFFS, new InputArgument[] { Game.Player.Character, 1 });
            Function.Call(Hash.SET_ENABLE_BOUND_ANKLES, new InputArgument[] { Game.Player.Character, 1 });
            Game.Player.Character.Task.ClearAllImmediately();
            Game.Player.Character.Task.HandsUp(0xbb8);
            Script.Wait(0xbb8);
            Function.Call(Hash.CLEAR_RELATIONSHIP_BETWEEN_GROUPS, new InputArgument[] { 5, 0x7ea26372, -183807561 });
            Function.Call(Hash.CLEAR_RELATIONSHIP_BETWEEN_GROUPS, new InputArgument[] { 5, -183807561, 0x7ea26372 });
            Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, new InputArgument[] { 0xff, 0x7ea26372, -183807561 });
            Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, new InputArgument[] { 0xff, -183807561, 0x7ea26372 });
            escape_Ped = World.CreatePed(0x7b9b4bc0, new Vector3(1625.474f, 2491.485f, 45.62026f));
            escape_Ped.Task.StandStill(-1);
            escape_Ped.AlwaysKeepTask = true;
            escape_Ped.Heading = 320.5151f;
            Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, new InputArgument[] { escape_Ped, 1, 1 });
            Function.Call(Hash.SET_MAX_WANTED_LEVEL, new InputArgument[] { 0 });
            Model model = new Model(PedHash.Prisguard01SMM);
            model.Request(0x3e8);
            while (!model.IsLoaded)
            {
                Script.Wait(0);
            }
            police = World.CreatePed(model, Game.Player.Character.Position.Around(10f));
            Model model2 = new Model(VehicleHash.Police);
            model2.Request(0x3e8);
            while (!model2.IsLoaded)
            {
                Script.Wait(0);
            }
            policecar = World.CreateVehicle(model2, Game.Player.Character.Position.Around(10f));
            Game.Player.Character.Task.ClearAllImmediately();
            Game.Player.Character.Task.EnterVehicle(policecar, VehicleSeat.LeftRear);
            while (!Game.Player.Character.IsInVehicle(policecar))
            {
                Script.Wait(0);
            }
            police.Task.ClearAllImmediately();
            police.RelationshipGroup = Game.Player.Character.RelationshipGroup;
            police.Task.ClearAllImmediately();
            police.Task.WarpIntoVehicle(policecar, VehicleSeat.Driver);
            while (!police.IsInVehicle(policecar))
            {
                Script.Wait(0);
            }

            player_status = status.In_road;
            UI.Notify("press ~g~[E]~w~ to skip");
            policecar.SirenActive = true;
            string s = "402c1827";
            int num = int.Parse(s, NumberStyles.HexNumber);
            Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE, new InputArgument[] { police, policecar, new Vector3(1764.323f, 2604.595f, 45.56498f).X, new Vector3(1764.323f, 2604.595f, 45.56498f).Y, new Vector3(1764.323f, 2604.595f, 45.56498f).Z, 20f, num, 1f });
            testt = true;
        }


        public void stop_scripts()
        {
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, new InputArgument[] { "respawn_controller" });
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, new InputArgument[] { "re_prison" });
            Function.Call(Hash.STOP_ALARM, new InputArgument[] { "PRISON_ALARMS", 0 });
            Function.Call(Hash.CLEAR_AMBIENT_ZONE_STATE, new InputArgument[] { "AZ_COUNTRYSIDE_PRISON_01_ANNOUNCER_GENERAL", 0 });
            Function.Call(Hash.CLEAR_AMBIENT_ZONE_STATE, new InputArgument[] { "AZ_COUNTRYSIDE_PRISON_01_ANNOUNCER_WARNING", 0 });
        }

    }

}