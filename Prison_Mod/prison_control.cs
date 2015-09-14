
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
        bool doit = false;
        int time;
        public void prison_controle_Tick()
        {
            if (arrested)
            {
                stop_scripts();
            }
            if (inprison)
            {
                stop_scripts();
                if (Game.Player.Character.Position.DistanceTo(new Vector3(1752.894f, 2592.242f, 45.56502f)) < 1f)
                {
                    arrested = true;
                    
                    escape = false;
                    Function.Call(Hash.ENABLE_ALL_CONTROL_ACTIONS, new InputArgument[] { 1 });
                    Game.FadeScreenOut(0x9c4);
                    Game.Player.Character.Weapons.RemoveAll();
                    while (!Function.Call<bool>(Hash.IS_SCREEN_FADED_OUT, new InputArgument[0]))
                    {
                        stop_scripts();
                        Script.Wait(0);
                    }
                    Game.Player.Character.Position = new Vector3(1617.643f, 2523.981f, 45.56489f);
                    Game.Player.Character.Heading = 264.718f;
                    clothes_changer();
                    Game.FadeScreenIn(0x9c4);
                    Game.Player.CanControlCharacter = true;
                    Function.Call(Hash.SET_ENABLE_HANDCUFFS, new InputArgument[] { Game.Player.Character, 0 });
                    Function.Call(Hash.SET_ENABLE_BOUND_ANKLES, new InputArgument[] { Game.Player.Character, 0 });
                    int num = 0x7ea26372;
                    int num2 = Function.Call<int>(Hash.GET_PED_RELATIONSHIP_GROUP_HASH, new InputArgument[] { Game.Player.Character });
                    Function.Call(Hash.CLEAR_RELATIONSHIP_BETWEEN_GROUPS, new InputArgument[] { 0xff, num, num2 });
                    Function.Call(Hash.CLEAR_RELATIONSHIP_BETWEEN_GROUPS, new InputArgument[] { 0xff, num2, num });
                    Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, new InputArgument[] { 1, num, num2 });
                    bail = World.CreateBlip(new Vector3(1691.709f, 2565.036f, 45.56487f));
                    bail.Color = BlipColor.White;
                    Function.Call(Hash.SET_CLOCK_TIME, new InputArgument[] { 13, 0, 0 });
                    roit = World.CreateBlip(escape_Ped.Position);
                    roit.Sprite = BlipSprite.DollarSign;
                    _headsup = new UIText(string.Concat(new object[] { "Time: ~b~", hours, ":", minit }), new Point(2, 520), 0.7f, Color.WhiteSmoke, GTA.Font.HouseScript, false);
                    _headsupRectangle = new UIRectangle(new Point(0, 520), new Size(0xd7, 0x41), Color.FromArgb(100, 0, 0, 0));
                    if (police != null)
                    {
                        police.Delete();
                    }
                    if (policecar != null)
                    {
                        policecar.Delete();
                    }
                    inprison = false;
                }
                if ((Game.Player.Character.Position.DistanceTo(new Vector3(1764.323f, 2604.595f, 45.56498f)) < 5f) && (Game.Player.Character.CurrentVehicle == policecar))
                {
                    Game.Player.Character.Task.GoTo(new Vector3(1752.894f, 2592.242f, 45.56502f));
                }
            }
            if (arrested)
            {
                

                if (Game.Player.Character.IsDead)
                {
                    respawn.RespawnPlayer(new Vector3(1617.643f, 2523.981f, 45.56489f), 264.718f);
                }

                if (player_status == status.In_roit && Game.GameTime > time +1000)
                {
                    Model mod = new Model(PedHash.Prisoner01);
                    mod.Request();
                    while (!mod.IsLoaded)
                    {
                        stop_scripts();
                        Script.Wait(0);
                    }
                    Ped[] allped = World.GetAllPeds(mod);
                    for (int i = 0; i < allped.Length; i++)
                    {
                        bool find = false;
                        for (int j = 0; j < prisoner.Count; j++)
                            if (prisoner[j] == allped[i])
                                find = true;



                        if (!find)
                            prisoner.Add(allped[i]);
                    }
                    mod.MarkAsNoLongerNeeded();
                    //===================================================
                    mod = new Model(PedHash.Prisoner01SMY);
                    mod.Request();
                    while (!mod.IsLoaded)
                    {
                        stop_scripts();
                        Script.Wait(0);
                    }
                    allped = World.GetAllPeds(mod);
                    for (int i = 0; i < allped.Length; i++)
                    {
                        bool find = false;
                        for (int j = 0; j < prisoner.Count; j++)
                            if (prisoner[j] == allped[i])
                                find = true;



                        if (!find)
                            prisoner.Add(allped[i]);
                    }
                    mod.MarkAsNoLongerNeeded();
                    //===================================================
                    mod = new Model(PedHash.Prisguard01SMM);
                    mod.Request();
                    while (!mod.IsLoaded)
                    {
                        stop_scripts();
                        Script.Wait(0);
                    }
                    allped = World.GetAllPeds(mod);
                    for (int i = 0; i < allped.Length; i++)
                    {
                        bool find = false;
                        for (int j = 0; j < garde.Count; j++)
                            if (garde[j] == allped[i])
                                find = true;



                        if (!find)
                            garde.Add(allped[i]);
                    }
                    mod.MarkAsNoLongerNeeded();

                    
                    for (int j = 0; j < prisoner.Count; j++)
                    {
                        GiveWeapons_prisoner(prisoner[j]);
                        prisoner[j].CanSwitchWeapons = true;
                        //Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, prisoner[j], true);
                    }
                    for (int j = 0; j < garde.Count; j++)
                    {
                        GiveWeapons_Garde(garde[j]);
                        garde[j].CanSwitchWeapons = true;
                       // Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, garde[j], true);
                    }
                    time = Game.GameTime;
                }
                
                Function.Call(Hash.SET_MAX_WANTED_LEVEL, new InputArgument[] { 0 });
                stop_scripts();
                int hh = Function.Call<int>(Hash.GET_CLOCK_HOURS, new InputArgument[0]);
                int mm = Function.Call<int>(Hash.GET_CLOCK_MINUTES, new InputArgument[0]);
                if ((hh == 14) && (mm == 0))
                {
                    Function.Call(Hash.SET_CLOCK_TIME, new InputArgument[] { 13, 0, 0 });
                    time = Game.GameTime;
                }
                if (!((mm != 0) || hhh))
                {
                    minit = 0;
                    hours--;
                    hhh = true;
                }
                else if (mm > 0)
                {
                    minit = 60 - mm;
                    hhh = false;
                }
                if ((mm == 0 || mm == 5 || mm == 10 || mm == 15 || mm == 20 || mm == 25 || mm == 30 || mm == 35 || mm == 40 || mm == 45 || mm == 50 || mm == 55 || mm == 60) && !doit)
                {
                    Model mod = new Model(PedHash.Prisoner01);
                    mod.Request();
                    while (!mod.IsLoaded)
                    {
                        stop_scripts();
                        Script.Wait(0);
                    }
                    Ped[] allped = World.GetAllPeds(mod);
                    for (int i = 0; i < allped.Length;i++ )
                    {
                        bool find = false;
                        for (int j = 0; j < prisoner.Count; j++)
                            if (prisoner[j] == allped[i])
                                find = true;



                        if (!find)
                            prisoner.Add(allped[i]);
                    }
                    mod.MarkAsNoLongerNeeded();
                    //===================================================
                     mod = new Model(PedHash.Prisoner01SMY);
                    mod.Request();
                    while (!mod.IsLoaded)
                    {
                        stop_scripts();
                        Script.Wait(0);
                    }
                     allped = World.GetAllPeds(mod);
                    for (int i = 0; i < allped.Length; i++)
                    {
                        bool find = false;
                        for (int j = 0; j < prisoner.Count; j++)
                            if (prisoner[j] == allped[i])
                                find = true;



                        if (!find)
                            prisoner.Add(allped[i]);
                    }
                    mod.MarkAsNoLongerNeeded();
                    //===================================================
                    mod = new Model(PedHash.Prisguard01SMM);
                    mod.Request();
                    while (!mod.IsLoaded)
                    {
                        stop_scripts();
                        Script.Wait(0);
                    }
                    allped = World.GetAllPeds(mod);
                    for (int i = 0; i < allped.Length; i++)
                    {
                        bool find = false;
                        for (int j = 0; j < garde.Count; j++)
                            if (garde[j] == allped[i])
                                find = true;



                        if (!find)
                            garde.Add(allped[i]);
                    }
                    mod.MarkAsNoLongerNeeded();

                    

                        doit = true;
                }
                else if ((mm != 0 && mm != 5 && mm != 10 && mm != 15 && mm != 20 && mm != 25 && mm != 30 && mm != 35 && mm != 40 && mm != 45 && mm != 50 && mm != 55 && mm != 60) && doit)
                {
                    doit = false;
                }

               
                stop_scripts();
                _headsup = new UIText(string.Concat(new object[] { "Time: ~b~", hours, ":", minit }), new Point(2, 520), 0.7f, Color.WhiteSmoke, GTA.Font.ChaletComprimeCologne, false);
                _headsupRectangle = new UIRectangle(new Point(0, 520), new Size(0xd7, 0x41), Color.FromArgb(100, 0, 0, 0));
                _headsup.Draw();
                _headsupRectangle.Draw();
                if (Game.Player.Character.Position.DistanceTo(new Vector3(1691.709f, 2565.036f, 45.56487f)) < 2f)
                {
                    UI.Notify("press [E] to bail : ~g~ " + 0x4c4b40 + "~w~$");
                }
                else if (Game.Player.Character.Position.DistanceTo(new Vector3(1625.474f, 2491.485f, 45.62026f)) < 4f)
                {
                    UI.Notify("press [E] to escape for : ~g~ " + 0x4c4b40 + "~w~$");
                    UI.Notify("press [Y] to make roit for : ~g~ " + 0x4c4b40 + "~w~$");
                }
                if ((hours == 0) && (minit == 0))
                {
                    bail.Remove();
                    roit.Remove();
                    escape_Ped.MarkAsNoLongerNeeded();
                    Game.Player.Character.Position = new Vector3(1849.555f, 2586.085f, 45.67202f);
                    Game.Player.Character.Heading = 258.4564f;
                    Function.Call(Hash.SET_MAX_WANTED_LEVEL, new InputArgument[] { 5 });
                    Function.Call(Hash.PAUSE_CLOCK, new InputArgument[] { 0 });
                    arrested = false;
                    _headsup = null;
                    _headsupRectangle = null;
                }
            }
        }
    }

}