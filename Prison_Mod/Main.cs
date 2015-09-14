using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Linq;
using System.Reflection;
using SimpleLogger;
namespace Prison_Mod
{
public partial class Prison_Mod : Script
{
    private List<WeaponHash> weapons = Enum.GetValues(typeof(WeaponHash)).Cast<WeaponHash>().ToList<WeaponHash>();
    public Ped escape_Ped;
    public Ped police;
    public Vehicle policecar;
    private UIText _headsup;
    private UIRectangle _headsupRectangle;
    private bool arrested = false;
    private Blip bail;
    private bool escape = false;

    private List<Ped> friend = new List<Ped>();
    private List<Ped> garde = new List<Ped>();
    private Vehicle help_veh;
    private bool hhh = false;
    private int hours = 0x3e8;
    private bool inprison = false;
    private int minit = 0x3e8;
    private bool open1 = false;
    private bool open2 = false;
    public status player_status = status.Off;

    private List<Ped> prisoner = new List<Ped>();
    private Blip roit;
    private bool sciped = false;
    private Ped[] test;
    private bool testt = false;
    private bool first_tick = true;
    private List<Ped> peds = new List<Ped>();



    protected override void Dispose(bool A_0)
    {
        if (A_0)


        {
            dispose_escape();
            SimpleLog.Info("Prison Mod is Restarted");
            Function.Call(Hash.ENABLE_ALL_CONTROL_ACTIONS, new InputArgument[] { 1 });
            Game.Player.CanControlCharacter = true;
            Function.Call(Hash.SET_ENABLE_HANDCUFFS, new InputArgument[] { Game.Player.Character, 0 });
            Function.Call(Hash.SET_ENABLE_BOUND_ANKLES, new InputArgument[] { Game.Player.Character, 0 });
            Function.Call(Hash.SET_MAX_WANTED_LEVEL, new InputArgument[] { 5 });

            if (police != null)
            {
                police.Delete();
            }
            if (escape_Ped != null)
            {
                escape_Ped.Delete();
            }
            if (policecar != null)
            {
                policecar.Delete();
            }
        }
    }

     


    public Prison_Mod()
    {
        
        Tick += Ontick;
        KeyDown += Onkeydown;
        KeyUp += Onkeyup;

        Interval = 0;
    }
    void Ontick(object sender, EventArgs e)
    {
        if (first_tick)
        {
            SimpleLog.Info("Prison Mod is Enabled");
            UI.Notify("~b~Prison Mod ~w~ is ~g~ Enabled");
            first_tick = false;
        }


        if (player_status == status.In_Prison || player_status == status.In_roit)
        {
            prison_controle_Tick();
        }

        if (player_status == status.In_escape)
        {
            prison_escape_tick();
        }


        if ((player_status == status.Off) && Function.Call<bool>(Hash.IS_PLAYER_BEING_ARRESTED,  Game.Player, true ))
        {
            hours = 20;
            intial1();
        }



        if ((player_status == status.In_road) && testt)
        {
            Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, (int)GTA.Control.VehicleExit, true);
            int num;
            if (Game.Player.Character.Position.DistanceTo(new Vector3(2124.503f, 2760.919f, 49.1893f)) < 10f && !sciped)
            {
                sciped = true;
            }



            if (open1)
            {
                num = Function.Call<int>(Hash.GET_HASH_KEY,  "prop_gate_prison_01" );
                Function.Call(Hash._DOOR_CONTROL,  num, 1845f, 2605f, 45f, 0, 0f, 50f, 0f );
            }
            else
            {
                num = Function.Call<int>(Hash.GET_HASH_KEY, "prop_gate_prison_01" );
                Function.Call(Hash._DOOR_CONTROL,num, 1845f, 2605f, 45f, 1, 0f, 50f, 0f );
            }
            if (open2)
            {
                num = Function.Call<int>(Hash.GET_HASH_KEY,  "prop_gate_prison_01" );
                Function.Call(Hash._DOOR_CONTROL, num, 1819.27f, 2608.53f, 44.61f, 0, 0f, 50f, 0f );
            }
            else
            {
                num = Function.Call<int>(Hash.GET_HASH_KEY,  "prop_gate_prison_01" );
                Function.Call(Hash._DOOR_CONTROL, num, 1819.27f, 2608.53f, 44.61f, 1, 0f, 50f, 0f );
            }
            if ((Game.Player.Character.Position.DistanceTo(new Vector3(1855.855f, 2606.756f, 45.9304f)) < 4f) && (policecar.Speed > 5f))
            {
                open1 = true;
                open2 = false;
                Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD, police, policecar, new Vector3(1764.323f, 2604.595f, 45.56498f).X, new Vector3(1764.323f, 2604.595f, 45.56498f).Y, new Vector3(1764.323f, 2604.595f, 45.56498f).Z, 5f, 1f, 0x79fbb0c5, 0xc0025, 1f, 1f );
            }
            if ((Game.Player.Character.Position.DistanceTo(new Vector3(1831.152f, 2606.738f, 45.83254f)) < 3f) && open1)
            {
                open1 = false;
                open2 = true;
                Game.Player.WantedLevel = 0;
                Function.Call(Hash.SET_MAX_WANTED_LEVEL,0 );
            }
            if ((Game.Player.Character.Position.DistanceTo(new Vector3(1764.323f, 2604.595f, 45.56498f)) < 4f) && open2)
            {
                open1 = false;
                inprison = true;
                player_status = status.In_Prison;
                sciped = false;
                open2 = false;
                testt = false;
                Game.Player.WantedLevel = 0;
                Function.Call(Hash.SET_MAX_WANTED_LEVEL, new InputArgument[] { 0 });
            }
            stop_scripts();
        }
    }



    void Onkeydown(object sender, KeyEventArgs e)
    {

    }
    void Onkeyup(object sender, KeyEventArgs e)
    {
        if ((e.KeyCode == Keys.F12) && (Game.Player.WantedLevel > 1))
        {
            hours = 10 + (Game.Player.WantedLevel * 10);
            Game.Player.WantedLevel = 0;
            intial1();
        }
        if ((e.KeyCode == Keys.E) && arrested)
        {
            if ((Game.Player.Character.Position.DistanceTo(new Vector3(1691.709f, 2565.036f, 45.56487f)) < 2f) && (Game.Player.Money > 0x4c4b40))
            {
                bail.Remove();
                roit.Remove();
                Game.Player.Character.Position = new Vector3(1849.555f, 2586.085f, 45.67202f);
                Game.Player.Character.Heading = 258.4564f;
                Player player = Game.Player;
                player.Money -= 0x4c4b40;
                Function.Call(Hash.SET_MAX_WANTED_LEVEL, 5 );
                Function.Call(Hash.SET_PED_DEFAULT_COMPONENT_VARIATION, Game.Player.Character);
                arrested = false;
                _headsup = null;
                _headsupRectangle = null;
            }
            if ((Game.Player.Character.Position.DistanceTo(new Vector3(1625.474f, 2491.485f, 45.62026f)) < 4f) && !escape)
            {
                stop_scripts();
                bail.Remove();
                roit.Remove();
                Player player2 = Game.Player;
                player2.Money -= 0x4c4b40;
                stop_scripts();
                arrested = false;
                _headsup = null;
                _headsupRectangle = null;
               
                Model mod = new Model(VehicleHash.Valkyrie);
                mod.Request();
                while (!mod.IsLoaded)
                {
                    stop_scripts();
                    Script.Wait(0);
                }
                escape_veh3 = World.CreateVehicle(mod, new Vector3(-1179.25f, -2845.386f, 13.5665f));
                escape_veh3.Heading = 325.6199f;
                escape_veh3.IsPersistent = true;
                mod.MarkAsNoLongerNeeded();
                //============================================================================================================
                mod = new Model(VehicleHash.Comet2);
                mod.Request();
                while (!mod.IsLoaded)
                {
                    stop_scripts();
                    Script.Wait(0);
                }
                escape_veh1 = World.CreateVehicle(mod, new Vector3(-1729.416f, -1109.523f, 12.7468f));
                escape_veh1.Heading = 321.1888f;
                escape_veh1.IsPersistent = true;
                mod.MarkAsNoLongerNeeded();
                //===========================================================================================================
                mod = new Model(VehicleHash.Insurgent2);
                mod.Request();
                while (!mod.IsLoaded)
                {
                    stop_scripts();
                    Script.Wait(0);
                }
                escape_veh2 = World.CreateVehicle(mod, new Vector3(1373.179f, -2077.577f, 51.6181f));
                escape_veh2.Heading = 332.3142f;
                escape_veh2.IsPersistent = true;
                mod.MarkAsNoLongerNeeded();

                int hash = Function.Call<int>(Hash.GET_ENTITY_MODEL, new InputArgument[] { Game.Player.Character });
                if (hash == Function.Call<int>(Hash.GET_HASH_KEY, new InputArgument[] { "player_zero" }))    //m
                {
                    mod = new Model(PedHash.Franklin);
                    mod.Request();
                    while (!mod.IsLoaded)
                    {
                        stop_scripts();
                        Script.Wait(0);
                    }
                    escape_ped1 = World.CreatePed(mod, new Vector3(-1726.644f, -1112.538f, 13.2474f));
                    escape_ped1.Heading = 42.1536f;
                    escape_ped1.IsPersistent = true;
                    mod.MarkAsNoLongerNeeded();

                    mod = new Model(PedHash.Trevor);
                    mod.Request();
                    while (!mod.IsLoaded)
                    {
                        stop_scripts();
                        Script.Wait(0);
                    }
                    escape_ped2 = World.CreatePed(mod, new Vector3(1376.8f, -2076.873f, 51.9985f));
                    escape_ped2.Heading = 47.19263f;
                    escape_ped2.IsPersistent = true;
                    mod.MarkAsNoLongerNeeded();





                }
                else if (hash == Function.Call<int>(Hash.GET_HASH_KEY, new InputArgument[] { "player_one" })) //f
                {
                    mod = new Model(PedHash.Michael);
                    mod.Request();
                    while (!mod.IsLoaded)
                    {
                        stop_scripts();
                        Script.Wait(0);
                    }
                    escape_ped1 = World.CreatePed(mod, new Vector3(-1726.644f, -1112.538f, 13.2474f));
                    escape_ped1.Heading = 42.1536f;
                    escape_ped1.IsPersistent = true;
                    mod.MarkAsNoLongerNeeded();

                    mod = new Model(PedHash.Trevor);
                    mod.Request();
                    while (!mod.IsLoaded)
                    {
                        stop_scripts();
                        Script.Wait(0);
                    }
                    escape_ped2 = World.CreatePed(mod, new Vector3(1376.8f, -2076.873f, 51.9985f));
                    escape_ped2.Heading = 47.19263f;
                    escape_ped2.IsPersistent = true;
                    mod.MarkAsNoLongerNeeded();
                }
                else if (hash == Function.Call<int>(Hash.GET_HASH_KEY, new InputArgument[] { "player_two" }))//tr   
                {
                    mod = new Model(PedHash.Michael);
                    mod.Request();
                    while (!mod.IsLoaded)
                    {
                        stop_scripts();
                        Script.Wait(0);
                    }
                    escape_ped1 = World.CreatePed(mod, new Vector3(-1726.644f, -1112.538f, 13.2474f));
                    escape_ped1.Heading = 42.1536f;
                    escape_ped1.IsPersistent = true;
                    mod.MarkAsNoLongerNeeded();

                    mod = new Model(PedHash.Franklin);
                    mod.Request();
                    while (!mod.IsLoaded)
                    {
                        stop_scripts();
                        Script.Wait(0);
                    }
                    escape_ped2 = World.CreatePed(mod, new Vector3(1376.8f, -2076.873f, 51.9985f));
                    escape_ped2.Heading = 47.19263f;
                    escape_ped2.IsPersistent = true;
                    mod.MarkAsNoLongerNeeded();
                }
                original_ped = Game.Player.Character;

                Game.FadeScreenOut(3000);
                while (!Function.Call<bool>(Hash.IS_SCREEN_FADED_OUT))
                    Script.Wait(0);
                Function.Call(Hash.CHANGE_PLAYER_PED, Game.Player, escape_ped1, 1, 0);
                Game.FadeScreenIn(3000);
                while (!Function.Call<bool>(Hash.IS_SCREEN_FADED_IN))
                    Script.Wait(0);
                current_ped = 1;
                vehb = escape_veh1.AddBlip();
                vehb.Color = BlipColor.Blue;
                UI.ShowSubtitle("Get in the ~b~ Vehicle",3000);
                steps = 0;

                player_status = status.In_escape;
                Function.Call(Hash.SET_MAX_WANTED_LEVEL,  5);

                escape = true;
            }
        }
        if (e.KeyCode == Keys.E && steps == 5)
        {
            if (current_ped == 1)
            {
                Function.Call(Hash.CHANGE_PLAYER_PED, Game.Player, escape_ped2, 1, 0);
                //drive task

                current_ped = 2;
            }
            else
            {
                Function.Call(Hash.CHANGE_PLAYER_PED, Game.Player, escape_ped1, 1, 0);
                //shot task
                escape_ped2.Task.FightAgainstHatedTargets(150f);
                escape_ped2.AlwaysKeepTask = true;
                current_ped = 1;
            }
        }
        if (((e.KeyCode == Keys.Y) && arrested) && (Game.Player.Character.Position.DistanceTo(escape_Ped.Position) < 4f))
        {
            time = Game.GameTime;
            int i;
            for (i = 0; i < prisoner.Count; i++)
            {
                GiveWeapons_prisoner(prisoner[i]);
                prisoner[i].CanSwitchWeapons = true;


            }
            for (i = 0; i < garde.Count; i++)
            {
                GiveWeapons_Garde(garde[i]);
                garde[i].CanSwitchWeapons = true;
                //Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, garde[i], true);

            }
            GiveWeapons_prisoner(Game.Player.Character);
            Player player3 = Game.Player;
            player3.Money -= 0x4c4b40;
            Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, new InputArgument[] { 5, 0x7ea26372, -183807561 });
            Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, new InputArgument[] { 5, -183807561, 0x7ea26372 });
        }
        
       
        if (((e.KeyCode == Keys.E) && (player_status == status.In_road)) && !sciped)
        {
            try
            {
                Game.FadeScreenOut(3000);
                while (!Function.Call<bool>(Hash.IS_SCREEN_FADED_OUT))
                {
                    Script.Wait(0);
                }
               
                policecar.Position = new Vector3(2124.503f, 2760.919f, 49.1893f);
                policecar.Heading = 130.5902f;
                
                Game.FadeScreenIn(3000);
                sciped = true;
            }
            catch (Exception ex)
            {
                SimpleLog.Error(ex);
                throw;
            }

            
        }
    }


}
}