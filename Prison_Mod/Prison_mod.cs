using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Timers;

public class Prison_mod : Script
{
    bool arrested = false;
    int modulehash;

    int hours = 1000;
    int minit = 1000;

    bool hhh = false;



    Ped escape_Ped;
    bool escape = false;

    Vehicle help_veh;



    List<Ped> prisoner = new List<Ped>();
    List<Ped> garde = new List<Ped>();
    List<Ped> friend = new List<Ped>();

    Ped tttt;
    bool shoted = false;
    bool testt = false;
    bool open1 = false;
    bool open2 = false;
    bool inprison = false;
    Vehicle test_veh;
    Ped driver;

    Ped[] test;
    int type = 0;
    Ped police;
    Vehicle policecar;

    private UIText _headsup;
    private UIRectangle _headsupRectangle;
    private UIContainer mContainer;



    private List<WeaponHash> weapons = Enum.GetValues(typeof(WeaponHash)).Cast<WeaponHash>().ToList();
    private Weapon currentWeapon;


    protected override void Dispose(bool A_0)
    {
        if (A_0)
        {
            Function.Call(Hash.ENABLE_ALL_CONTROL_ACTIONS, 1);
            Game.Player.CanControlCharacter = true;
            Function.Call(Hash.SET_ENABLE_HANDCUFFS, Game.Player.Character, false);
            Function.Call(Hash.SET_ENABLE_BOUND_ANKLES, Game.Player.Character, false);
        }
    }

    public void GiveAllWeapons(Ped ped)
    {
        
        foreach (WeaponHash w in weapons)
        {
            ped.Weapons.Give(w, 0, true, true);
            currentWeapon = ped.Weapons.Current;
            currentWeapon.Ammo = currentWeapon.MaxAmmo;
        }
    }

    public void GiveAllWeapons1(Ped ped)
    {

        WeaponHash w = WeaponHash.Knife;
        
            ped.Weapons.Give(w, 0, true, true);
            currentWeapon = ped.Weapons.Current;
            currentWeapon.Ammo = currentWeapon.MaxAmmo;
        
    }

    public void GiveAllWeapons2(Ped ped)
    {
        ped.Weapons.RemoveAll();
        WeaponHash w = WeaponHash.StunGun;

        ped.Weapons.Give(w, 0, true, true);
        currentWeapon = ped.Weapons.Current;
        currentWeapon.Ammo = currentWeapon.MaxAmmo;

    }



    Blip bail;
    Blip roit;
    public Prison_mod()
    {
        Tick += Ontick;
        KeyUp += Onkeyup;

        Interval = 0;
    }


    

    void Ontick(object sender, EventArgs e)
    {


            
        //  prisoner groupe   2124571506
        //  garde    groupe   -183807561
        //  police   groupe   -1533126372

       

        // SOLARITe  1651,726 : 2568,254 : 51,51672 :=> 348,7881

        // 1   1858,746 : 2609,113 : 45,29342
        // 2   1831,064 : 2607,884 : 45,20006
        // 3   1754,864 : 2605,129 : 45,18484

        if (Function.Call<bool>(Hash.IS_PLAYER_BEING_ARRESTED, Game.Player, true))
        {
            hours = 20;
            intial1();
        }

       

        #region escape
        if (escape)
        {
            roit.Remove();
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "respawn_controller");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_prison");
            
                if (Game.Player.Character.IsInVehicle())
                {
                    if (Game.Player.Character.CurrentVehicle == help_veh)
                    {
                        bail.Remove();
                        
                        if (Game.Player.WantedLevel == 0)
                        {
                            GTA.UI.ShowSubtitle("you have escape", 1000);
                            escape = false;
                            escape_Ped.MarkAsNoLongerNeeded();
                            help_veh.MarkAsNoLongerNeeded();
                            arrested = false;
                        }
                        else
                            GTA.UI.ShowSubtitle("escape the police", 10);
                    }
                    else
                    {
                        GTA.UI.ShowSubtitle("get to the ~b~ MAVERICK", 10);
                        bail.Remove();
                        bail = World.CreateBlip(help_veh.Position);
                        bail.Color = BlipColor.Blue;
                        bail.ShowRoute = true;
                    }
                }
                else
                {
                    GTA.UI.ShowSubtitle("get to the ~b~ MAVERICK", 10);
                    bail.Remove();
                    bail = World.CreateBlip(help_veh.Position);
                    bail.Color = BlipColor.Blue;
                    bail.ShowRoute = true;
                }

            if (Game.Player.IsDead)
            {
                escape = false;
                arrested = false;
                bail.Remove();
                help_veh.MarkAsNoLongerNeeded();
            }
        }
        #endregion
        //=================================================================================================================================================

        if (arrested)
        {
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "respawn_controller");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_prison");
        }
        if (inprison)
        {
            
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "respawn_controller");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_prison");
            if (Game.Player.Character.Position.DistanceTo(new Vector3(1752.894f, 2592.242f, 45.56502f)) < 1f)
            {
                arrested = true;
                escape = false;
                Function.Call(Hash.ENABLE_ALL_CONTROL_ACTIONS, 1);
                Game.FadeScreenOut(2500);
                Game.Player.Character.Position = new Vector3(1617.643f, 2523.981f, 45.56489f);
                clothes_changer();
                Game.Player.CanControlCharacter = true;
                Function.Call(Hash.SET_ENABLE_HANDCUFFS, Game.Player.Character, false);
                Function.Call(Hash.SET_ENABLE_BOUND_ANKLES, Game.Player.Character, false);
                Game.FadeScreenIn(2500);

               
                int group = 2124571506;
                int player_group = Function.Call<int>(Hash.GET_PED_RELATIONSHIP_GROUP_HASH, Game.Player.Character);


                Function.Call(Hash.CLEAR_RELATIONSHIP_BETWEEN_GROUPS, (int)Relationship.Pedestrians, group, player_group);
                Function.Call(Hash.CLEAR_RELATIONSHIP_BETWEEN_GROUPS, (int)Relationship.Pedestrians, player_group, group);

                Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, 1, group, player_group);
                

                bail = World.CreateBlip(new Vector3(1691.709f, 2565.036f, 45.56487f));
                bail.Color = BlipColor.White;
                Function.Call(Hash.SET_CLOCK_TIME, 13, 00, 0);

                roit = World.CreateBlip(escape_Ped.Position);
                roit.Sprite = BlipSprite.DollarSign;


                _headsup = new UIText("Time: ~b~" + hours + ":" + minit, new Point(2, 520), 0.7f, Color.WhiteSmoke, GTA.Font.HouseScript, false);

                _headsupRectangle = new UIRectangle(new Point(0, 520), new Size(215, 65), Color.FromArgb(100, 0, 0, 0));

                inprison = false;
            }



            if (Game.Player.Character.Position.DistanceTo(new Vector3(1754.864f, 2605.129f, 45.18484f)) < 5f)
            {
                Game.Player.Character.Task.GoTo(new Vector3(1752.894f, 2592.242f, 45.56502f));
            }
        }
        if (testt)
        {
            Function.Call(Hash.DISABLE_ALL_CONTROL_ACTIONS, 1);
            if (open1)
            {
                int t = Function.Call<int>(Hash.GET_HASH_KEY, "prop_gate_prison_01");
                Function.Call(Hash._0x9B12F9A24FABEDB0, t, 1845.0f, 2605.0f, 45.0f, 0, 0.0f, 50.0f, 0f);
            }
            else
            {
                int t = Function.Call<int>(Hash.GET_HASH_KEY, "prop_gate_prison_01");
                Function.Call(Hash._0x9B12F9A24FABEDB0, t, 1845.0f, 2605.0f, 45.0f, 1, 0.0f, 50.0f, 0f);
            }
            if (open2)
            {
                int t = Function.Call<int>(Hash.GET_HASH_KEY, "prop_gate_prison_01");
                Function.Call(Hash._0x9B12F9A24FABEDB0, t, 1819.27f, 2608.53f, 44.61f, 0, 0.0f, 50.0f, 0f);
            }
            else
            {
                int t = Function.Call<int>(Hash.GET_HASH_KEY, "prop_gate_prison_01");
                Function.Call(Hash._0x9B12F9A24FABEDB0, t, 1819.27f, 2608.53f, 44.61f, 1, 0.0f, 50.0f, 0f);
            }



            if (Game.Player.Character.Position.DistanceTo(new Vector3(1855.855f, 2606.756f, 45.9304f)) < 4f)
            {
                open1 = true;
                open2 = false;
            }

            if (Game.Player.Character.Position.DistanceTo(new Vector3(1831.152f, 2606.738f, 45.83254f)) < 3f && open1)
            {
                open1 = false;
                open2 = true;
                Game.Player.WantedLevel = 0;
                Function.Call(Hash.SET_MAX_WANTED_LEVEL, 0);
            }
            if (Game.Player.Character.Position.DistanceTo(new Vector3(1754.018f, 2604.271f, 45.82404f)) < 4f  && open2)
            {
                open1 = false;
                open2 = false;
                testt = false;
                Game.Player.WantedLevel = 0;
                Function.Call(Hash.SET_MAX_WANTED_LEVEL, 0);
                inprison = true;

            }

            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "respawn_controller");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_prison");

        }


        if (arrested)
        {

            if (Game.Player.Character.IsDead)
                Function.Call(Hash.SET_MAX_WANTED_LEVEL, 5);

            Function.Call(Hash.SET_MAX_WANTED_LEVEL, 0);

            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "respawn_controller");
            Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_prison");
            Function.Call(Hash.STOP_ALARM, "PRISON_ALARMS", 0);
            Function.Call(Hash.CLEAR_AMBIENT_ZONE_STATE, "AZ_COUNTRYSIDE_PRISON_01_ANNOUNCER_GENERAL", 0);
            Function.Call(Hash.CLEAR_AMBIENT_ZONE_STATE, "AZ_COUNTRYSIDE_PRISON_01_ANNOUNCER_WARNING", 0);
        

      

        

                int hh = Function.Call<int>(Hash.GET_CLOCK_HOURS);
                int mm = Function.Call<int>(Hash.GET_CLOCK_MINUTES);

                if (hh == 14 && mm == 0)
                {
                    Function.Call(Hash.SET_CLOCK_TIME, 13, 00, 0);
                }
                if (mm == 0 && !hhh )
                {
                    minit = 00;
                    hours -= 1;
                    hhh = true;
                }
                else if (mm>0)
                {
                    minit = 60 - mm;
                    hhh = false;
                }
               
                test = World.GetNearbyPeds(Game.Player.Character,150f);
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "respawn_controller");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_prison");

                for (int i = 0; i < test.Length; i++)
                {

                    Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "respawn_controller");
                    Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_prison");

                    if (Function.Call<int>(Hash.GET_PED_RELATIONSHIP_GROUP_HASH, test[i]) == 2124571506 || Function.Call<int>(Hash.GET_PED_RELATIONSHIP_GROUP_HASH, test[i]) == -86095805)
                    {
                        bool trouve = false;
                        for (int m = 0; m < prisoner.Count; m++)
                            if (prisoner[m] == test[i])
                            {
                                trouve = true;
                                break;
                            }

                        if (!trouve)
                            prisoner.Add(test[i]);


                    }
                    else if (Function.Call<int>(Hash.GET_PED_RELATIONSHIP_GROUP_HASH, test[i]) == -183807561)
                    {

                        bool trouve = false;
                        for (int m = 0; m < garde.Count; m++)
                            if (garde[m] == test[i])
                            {
                                trouve = true;
                                break;
                            }
                        if (!trouve)
                            garde.Add(test[i]);

                    }
                    Ped[] test2 = World.GetNearbyPeds(test[i], 150f);

                    for (int j = 0 ;j<test2.Length;j++)
                    {
                        Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "respawn_controller");
                        Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_prison");

                        if (Function.Call<int>(Hash.GET_PED_RELATIONSHIP_GROUP_HASH, test2[j]) == 2124571506 || Function.Call<int>(Hash.GET_PED_RELATIONSHIP_GROUP_HASH, test2[j]) == -86095805)
                        {
                            bool trouve = false;
                            for (int m = 0; m < prisoner.Count; m++)
                                if (prisoner[m] == test2[j])
                                {
                                    trouve = true;
                                    break;
                                }

                            if (!trouve)
                                prisoner.Add(test2[j]);


                        }
                        else if (Function.Call<int>(Hash.GET_PED_RELATIONSHIP_GROUP_HASH, test2[j]) == -183807561)
                        {

                            bool trouve = false;
                            for (int m = 0; m < garde.Count; m++)
                                if (garde[m] == test2[j])
                                {
                                    trouve = true;
                                    break;
                                }
                            if (!trouve)
                                garde.Add(test2[j]);

                        }
                    }                
                }
                //===================================================================================================
                
            
                   
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "respawn_controller");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_prison");

                
                _headsup = new UIText("Time: ~b~" + hours + ":" + minit, new Point(2, 520), 0.7f, Color.WhiteSmoke, GTA.Font.ChaletComprimeCologne, false);

                _headsupRectangle = new UIRectangle(new Point(0, 520), new Size(215, 65), Color.FromArgb(100, 0, 0, 0));

                _headsup.Draw();
                _headsupRectangle.Draw();

                if (Game.Player.Character.Position.DistanceTo(new Vector3(1691.709f, 2565.036f, 45.56487f)) < 2f)
                    {
                        GTA.UI.Notify("press [E] to bail : ~g~ " + 5000000 + "~w~$");
                    }
                else if (Game.Player.Character.Position.DistanceTo(new Vector3(1625.474f, 2491.485f, 45.62026f)) < 4f)
                {
                    GTA.UI.Notify("press [E] to escape for : ~g~ " + 5000000 + "~w~$");

                    GTA.UI.Notify("press [Y] to make roit for : ~g~ " + 5000000 + "~w~$");
                }
               

            if( hours == 0 && minit == 0)
            {
                bail.Remove();
                Game.Player.Character.Position = new Vector3(1849.555f, 2586.085f, 45.67202f);
                Game.Player.Character.Heading = 258.4564f;
               
                Function.Call(Hash.SET_MAX_WANTED_LEVEL, 5);

                Function.Call(Hash.PAUSE_CLOCK, false);
                arrested = false;
                _headsup = null;
                _headsupRectangle = null;
            }

        }
        
       

    }


    void intial1()
    {
        Game.Player.WantedLevel = 0;
        Game.Player.Character.Weapons.RemoveAll();

        Function.Call(Hash.DISABLE_ALL_CONTROL_ACTIONS, 1);
        Game.Player.CanControlCharacter = false;
        Function.Call(Hash.SET_ENABLE_HANDCUFFS, Game.Player.Character, true);
        Function.Call(Hash.SET_ENABLE_BOUND_ANKLES, Game.Player.Character, true);
        Game.Player.Character.Task.ClearAllImmediately();
        Game.Player.Character.Task.HandsUp(3000);
        Script.Wait(2500);

        Function.Call(Hash.CLEAR_RELATIONSHIP_BETWEEN_GROUPS, (int)Relationship.Hate, 2124571506, -183807561);
        Function.Call(Hash.CLEAR_RELATIONSHIP_BETWEEN_GROUPS, (int)Relationship.Hate, -183807561, 2124571506);

        Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, (int)Relationship.Pedestrians, 2124571506, -183807561);
        Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, (int)Relationship.Pedestrians, -183807561, 2124571506);

        escape_Ped = World.CreatePed(PedHash.Prisoner01, new Vector3(1625.474f, 2491.485f, 45.62026f));
        escape_Ped.Task.StandStill(-1);
        escape_Ped.AlwaysKeepTask = true;
        escape_Ped.Heading = 320.5151f;



        Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, escape_Ped, true, true);
        Function.Call(Hash.SET_MAX_WANTED_LEVEL, 0);


        // get the closest policeman
        float dis = 35f;
        Ped[] test = World.GetNearbyPeds(Game.Player.Character, 30f);
        for(int i =0;i<test.Length;i++)
        {
            if (test[i].RelationshipGroup == -1533126372)
            {
                if (Game.Player.Character.Position.DistanceTo(test[i].Position)<dis)
                {
                    dis = Game.Player.Character.Position.DistanceTo(test[i].Position);
                    police = test[i];
                }
            }
        }
        //get the closest police vehicle
        dis = 55f;
        Vehicle[] test2 = World.GetNearbyVehicles(Game.Player.Character, 50f);
        for (int i = 0; i < test2.Length; i++)
        {
            int mod_hash = test2[i].Model.GetHashCode();
            if (mod_hash == VehicleHash.Police.GetHashCode() || mod_hash == VehicleHash.Police2.GetHashCode()
                || mod_hash == VehicleHash.Police3.GetHashCode() || mod_hash == VehicleHash.Police4.GetHashCode()
                || mod_hash == VehicleHash.Policeb.GetHashCode() || mod_hash == VehicleHash.PoliceOld1.GetHashCode()
                || mod_hash == VehicleHash.PoliceOld2.GetHashCode())
            {
                if (Game.Player.Character.Position.DistanceTo(test2[i].Position) < dis)
                {
                    dis = Game.Player.Character.Position.DistanceTo(test2[i].Position);
                    policecar = test2[i];
                }
            }
                
        }

        // make the player enter the vehicle
         Game.Player.Character.Task.ClearAllImmediately();
         Game.Player.Character.Task.EnterVehicle(policecar, VehicleSeat.LeftRear);

         while (!Game.Player.Character.IsInVehicle(policecar))
             Script.Wait(0);

        // disable the player control
         Function.Call(Hash.DISABLE_ALL_CONTROL_ACTIONS, 1);
         Game.Player.CanControlCharacter = false;
         Function.Call(Hash.SET_ENABLE_HANDCUFFS, Game.Player.Character, true);
         Function.Call(Hash.SET_ENABLE_BOUND_ANKLES, Game.Player.Character, true);

         police.Task.ClearAllImmediately();
         police.RelationshipGroup = Game.Player.Character.RelationshipGroup;
         police.Task.ClearAllImmediately();
         
        TaskSequence task = new TaskSequence();
        task.AddTask.EnterVehicle(policecar, VehicleSeat.Driver);
        task.AddTask.DriveTo(test_veh, new Vector3(1855.855f, 2606.756f, 45.9304f), 3f, 10f, (int)DrivingStyle.Normal);
        task.AddTask.StandStill(5000);
        task.AddTask.DriveTo(test_veh, new Vector3(1831.152f, 2606.738f, 45.83254f), 3f, 10f, (int)DrivingStyle.Normal);
        task.AddTask.StandStill(5000);
        task.AddTask.DriveTo(test_veh, new Vector3(1754.018f, 2604.271f, 45.82404f), 3f, 10f, (int)DrivingStyle.Normal);
        task.AddTask.StandStill(50000);
        police.Task.ClearAllImmediately();
        police.Task.PerformSequence(task);
        task.Close();


        testt = true;
    }



    void intial()
    {
        Game.Player.WantedLevel = 0;
        Game.Player.Character.Weapons.RemoveAll();

        Function.Call(Hash.CLEAR_RELATIONSHIP_BETWEEN_GROUPS, (int)Relationship.Hate, 2124571506, -183807561);
        Function.Call(Hash.CLEAR_RELATIONSHIP_BETWEEN_GROUPS, (int)Relationship.Hate, -183807561, 2124571506);

        Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, (int)Relationship.Pedestrians, 2124571506, -183807561);
        Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, (int)Relationship.Pedestrians, -183807561, 2124571506);

        escape_Ped = World.CreatePed(PedHash.Prisoner01, new Vector3(1625.474f, 2491.485f, 45.62026f));
        escape_Ped.Task.StandStill(-1);
        escape_Ped.Heading = 320.5151f;



        Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, escape_Ped, true, true);

        Game.Player.WantedLevel = 0;
        Function.Call(Hash.SET_MAX_WANTED_LEVEL, 0);

        Game.Player.Character.Task.PlayAnimation("RANDOM@ARRESTS", "idle_2_hands_up", 2f, -1, false, 0);
        Script.Wait(500);
        Game.FadeScreenIn(2500);
        //   while (Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Game.Player.Character, "RANDOM@ARRESTS", "idle_2_hands_up", 3)) Script.Wait(0);
        while (!Function.Call<bool>(Hash.IS_SCREEN_FADED_OUT)) Script.Wait(0);

        Game.FadeScreenIn(0);

        int group = 2124571506;
        int player_group = Function.Call<int>(Hash.GET_PED_RELATIONSHIP_GROUP_HASH, Game.Player.Character);


        Function.Call(Hash.CLEAR_RELATIONSHIP_BETWEEN_GROUPS, (int)Relationship.Pedestrians, group, player_group);
        Function.Call(Hash.CLEAR_RELATIONSHIP_BETWEEN_GROUPS, (int)Relationship.Pedestrians, player_group, group);

        Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, 1, group, player_group);
        //     Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, 1, player_group, group);

        //   Function.Call(Hash.CLEAR_AREA_OF_PEDS, 1637.856f, 2608.986f, 45.56487f, 1000f, true);
        
        clothes_changer();

        bail = World.CreateBlip(new Vector3(1691.709f, 2565.036f, 45.56487f));
        bail.Color = BlipColor.White;
        Function.Call(Hash.SET_CLOCK_TIME, 13, 00, 0);

        roit = World.CreateBlip(escape_Ped.Position);
        roit.Sprite = BlipSprite.DollarSign;

        
        _headsup = new UIText("Time: ~b~" + hours + ":" + minit, new Point(2, 520), 0.7f, Color.WhiteSmoke, GTA.Font.HouseScript, false);

        _headsupRectangle = new UIRectangle(new Point(0, 520), new Size(215, 65), Color.FromArgb(100, 0, 0, 0));

        _headsup.Draw();
        _headsupRectangle.Draw();

        arrested = true;
        escape = false;
        Game.Player.Character.Position = new Vector3(1617.643f, 2523.981f, 45.56489f);
        Game.FadeScreenOut(1);
        Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "respawn_controller");
        Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_prison");
        Game.Player.Character.Heading = 264.718f;
        Game.FadeScreenOut(0);
        Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "respawn_controller");
        Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_prison");
        Game.FadeScreenIn(3000);
        

        


    }
    void Onkeyup(object sender, KeyEventArgs e)
    {

        if (e.KeyCode == Keys.F12 && Game.Player.WantedLevel > 1)
        {
            
            hours = 10 + Game.Player.WantedLevel *10;
            Game.Player.WantedLevel = 0;
            intial1();

            
        }
      /*  if (e.KeyCode == Keys.F11)
        {
            Ped[] test = World.GetNearbyPeds(Game.Player.Character, 100f);
            int player_group = Function.Call<int>(Hash.GET_PED_RELATIONSHIP_GROUP_HASH, test[0]);
            GTA.UI.ShowSubtitle(player_group + " ");
        }
        */

        if (e.KeyCode == Keys.E && arrested)
        {
            if (Game.Player.Character.Position.DistanceTo(new Vector3(1691.709f, 2565.036f, 45.56487f))<2f)
            {
                // bail
                if (Game.Player.Money > 5000000)
                {
                    bail.Remove();
                    roit.Remove();
                    Game.Player.Character.Position = new Vector3(1849.555f, 2586.085f, 45.67202f);
                    Game.Player.Character.Heading = 258.4564f;
                    Game.Player.Money -= 5000000;
                    Function.Call(Hash.SET_MAX_WANTED_LEVEL, 5);

                    Function.Call(Hash.SET_PED_DEFAULT_COMPONENT_VARIATION, Game.Player.Character);
                    arrested = false;

                    _headsup = null;
                    _headsupRectangle = null;
                }
                else
                {

                }


            }

            if (Game.Player.Character.Position.DistanceTo(new Vector3(1625.474f, 2491.485f, 45.62026f)) < 4f && !escape)
            {
                
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "respawn_controller");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_prison");
                bail.Remove();
                roit.Remove();
                Game.Player.Money -= 5000000;
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "respawn_controller");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_prison");
                // 182.3307f , -885.7198f , 31.11671f :=> 51.15437f

                
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "respawn_controller");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_prison");
                //  -1083,942 : -2915,736 : 13,23366 :=> 285,5021
                if (help_veh == null)
                    help_veh = World.CreateVehicle(VehicleHash.Maverick, new Vector3(1649.463f, 2619.634f, 45.56486f));
                else
                {
                    help_veh.Delete();
                    help_veh = World.CreateVehicle(VehicleHash.Maverick, new Vector3(1649.463f, 2619.634f, 45.56486f));
                }
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "respawn_controller");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_prison");

                Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, help_veh, true, true);
                help_veh.Heading = 10.83339f;


                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "respawn_controller");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_prison");

                bail = World.CreateBlip(help_veh.Position);
                bail.Color = BlipColor.Blue;

                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "respawn_controller");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_prison");


                GiveAllWeapons(Game.Player.Character);

                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "respawn_controller");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_prison");


                Function.Call(Hash.PAUSE_CLOCK, false);

                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "respawn_controller");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_prison");

                arrested = false;
                _headsup = null;
                _headsupRectangle = null;

                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "respawn_controller");
                Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_prison");

                Function.Call(Hash.SET_CLOCK_TIME, 13, 00, 0);
                Function.Call(Hash.SET_MAX_WANTED_LEVEL, 5);
                Game.Player.WantedLevel = 4;



                escape = true;
            }





        }
        if (e.KeyCode == Keys.Y && arrested)
        {
            //  prisoner groupe   2124571506
            //  garde    groupe   -183807561
            if (Game.Player.Character.Position.DistanceTo(escape_Ped.Position) < 4f  )
            {
                for (int i = 0; i < prisoner.Count; i++)
                {
                    GiveAllWeapons1(prisoner[i]);
                    prisoner[i].CanSwitchWeapons = true;
                }

                for (int i = 0; i < garde.Count; i++)
                {
                    GiveAllWeapons2(garde[i]);
                    garde[i].CanSwitchWeapons = true;
                }


                GiveAllWeapons1(Game.Player.Character);
                Game.Player.Money -= 5000000;

                Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, (int)Relationship.Hate, 2124571506, -183807561);
                Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, (int)Relationship.Hate, -183807561, 2124571506);
            }
        }
        if (e.KeyCode == Keys.U)
        {
            
            Function.Call(Hash.ENABLE_ALL_CONTROL_ACTIONS, 1);
            Game.Player.CanControlCharacter = true;
            Function.Call(Hash.SET_ENABLE_HANDCUFFS, Game.Player.Character, false);
            Function.Call(Hash.SET_ENABLE_BOUND_ANKLES, Game.Player.Character, false);
        }
        if(e.KeyCode == Keys.Y)
            Function.Call(Hash.SET_MAX_WANTED_LEVEL, 5);
        if (e.KeyCode == Keys.F6)
        {
            // PBUS
            // VehicleHash.PBus
            // 1971.267f , 2635.439f , 46.17389f :=> 120.792f
            // 1608.376f , 2685.54f , 45.32081f :=> 143.341f
            testt = true;
            if (test_veh == null)
            {
                test_veh = World.CreateVehicle(VehicleHash.PBus, new Vector3(1971.267f, 2635.439f, 46.17389f));
                test_veh.Heading = 120.792f;
            }
            else
            {
                test_veh.Delete();
                test_veh = World.CreateVehicle(VehicleHash.PBus, new Vector3(1971.267f, 2635.439f, 46.17389f));
                test_veh.Heading = 120.792f;
            }

            if (driver == null)
            {
                driver = World.CreatePed(PedHash.Prisguard01SMM, new Vector3(1971.267f, 2635.439f, 46.17389f));
                driver.Heading = 120.792f;
            }
            else
            {
                driver.Delete();
                driver = World.CreatePed(PedHash.Prisguard01SMM, new Vector3(1971.267f, 2635.439f, 46.17389f).Around(5f));
                driver.Heading = 120.792f;
            }
            driver.Task.WarpIntoVehicle(test_veh, VehicleSeat.Driver);
            
          //  Game.Player.CanControlCharacter = false;
            Game.Player.Character.Position = new Vector3(1971.267f, 2635.439f, 46.17389f).Around(10f);

           // Function.Call(Hash.DISABLE_ALL_CONTROL_ACTIONS, 1);


            Game.Player.Character.Task.WarpIntoVehicle(test_veh, VehicleSeat.Passenger);
           
            Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, driver, -1533126372);
            Function.Call(Hash.SET_PED_AS_COP, driver, true);

            // 1   1858,746 : 2609,113 : 45,29342
            // 2   1831,064 : 2607,884 : 45,20006
            // 3   1754,864 : 2605,129 : 45,18484
            while (!Game.Player.Character.IsInVehicle(test_veh))
                Script.Wait(0);

            TaskSequence task = new TaskSequence();
       //     task.AddTask.DriveTo(test_veh, new Vector3(1900.457f, 2609.054f, 46.00166f), 3f, 10f, (int)DrivingStyle.Normal);
       //     task.AddTask.StandStill(5000);
            task.AddTask.DriveTo(test_veh, new Vector3(1855.855f, 2606.756f, 45.9304f), 3f, 10f, (int)DrivingStyle.Normal);
            task.AddTask.StandStill(5000);
            task.AddTask.DriveTo(test_veh, new Vector3(1831.152f, 2606.738f, 45.83254f), 3f, 10f, (int)DrivingStyle.Normal);
            task.AddTask.StandStill(5000);
            task.AddTask.DriveTo(test_veh, new Vector3(1754.018f, 2604.271f, 45.82404f), 3f, 10f, (int)DrivingStyle.Normal);
            task.AddTask.StandStill(50000);
            driver.Task.PerformSequence(task);
            task.Close();

            // new Vector3(1858.746f, 2609.113f, 45.29342f)
            // new Vector3(1831.064f, 2607.884f, 45.20006f)
            // new Vector3(1754.864f, 2605.129f, 45.18484f)
  
           
       //     int t = Function.Call<int>(Hash.GET_HASH_KEY,"prop_gate_prison_01");
       //     Function.Call(Hash._0x9B12F9A24FABEDB0, t, 1845.0f, 2605.0f, 45.0f, 0, 0.0f, 50.0f, 0f);
       //     Function.Call(Hash._0x9B12F9A24FABEDB0, t, 1819.27f, 2608.53f, 44.61f, 0, 0.0f, 50.0f, 0f);



        }
        if (e.KeyCode == Keys.O)
        {
            Function.Call(Hash.IGNORE_NEXT_RESTART, true);
        }
     /*  if (e.KeyCode == Keys.O)
        {
            int t = Function.Call<int>(Hash.GET_HASH_KEY, "prop_gate_prison_01");
            Function.Call(Hash._0x9B12F9A24FABEDB0, t, 1845.0f, 2605.0f, 45.0f, 0, 0.0f, 50.0f, 0f);
            Function.Call(Hash._0x9B12F9A24FABEDB0, t, 1819.27f, 2608.53f, 44.61f, 0, 0.0f, 50.0f, 0f);
        }

        if (e.KeyCode == Keys.P)
        {
            int t = Function.Call<int>(Hash.GET_HASH_KEY, "prop_gate_prison_01");
            Function.Call(Hash._0x9B12F9A24FABEDB0, t, 1845.0f, 2605.0f, 45.0f, 1, 0.0f, 50.0f, 0f);
            Function.Call(Hash._0x9B12F9A24FABEDB0, t, 1819.27f, 2608.53f, 44.61f, 1, 0.0f, 50.0f, 0f);
        }
      */


    }
    void clothes_changer()
    {
        //  Shoes Slot 6 Drawable 1/3 #5 Texture #0
        //  Michael Slot 6 Drawable 1/3 #1 Texture #0



        int modulehash1 = Function.Call<int>(Hash.GET_ENTITY_MODEL, Game.Player.Character);

        if (modulehash1 == Function.Call<int>(Hash.GET_HASH_KEY, "player_zero"))
        {
            Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 3, 12, 4, 2);
            Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 4, 11, 4, 2);
        }
        else if (modulehash1 == Function.Call<int>(Hash.GET_HASH_KEY, "player_one"))
        {
            Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 3, 1, 5, 2);
            Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 4, 1, 5, 2);
        }
        else if (modulehash1 == Function.Call<int>(Hash.GET_HASH_KEY, "player_two"))
        {
            Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 3, 5, 2, 1);
            Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 4, 5, 2, 1);
        }
    }
}