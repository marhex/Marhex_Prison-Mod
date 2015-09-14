
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
        private Ped escape_ped1;
        private Ped escape_ped2;
        private Blip ped1;
        private Blip ped2;
        private Vehicle escape_veh1;
        private Vehicle escape_veh2;
        private Vehicle escape_veh3;
        private Blip vehb;
        private int steps = 0;
        private int current_ped;


        private Ped original_ped;
        private void dispose_escape()
        {
            if (player_status == status.In_escape)
            {
                
                
                if (escape_veh1.Exists())
                {
                    escape_veh1.MarkAsNoLongerNeeded();
                }
                if (escape_veh2.Exists())
                {

                    escape_veh2.MarkAsNoLongerNeeded();
                }
                if (escape_veh3.Exists())
                {

                    escape_veh3.MarkAsNoLongerNeeded();
                }
                if (escape_ped1.Exists())
                {

                    escape_ped1.MarkAsNoLongerNeeded();
                }
                if (escape_ped2.Exists())
                {

                    escape_ped2.MarkAsNoLongerNeeded();
                }
               
            }
            
               

        }
        public void prison_escape_tick()
        {
           
                if (steps == 0)
                {
                    if (Game.Player.Character.IsInVehicle(escape_veh1))
                    {
                        vehb.Remove();
                        vehb = escape_veh2.AddBlip();
                        vehb.Color = BlipColor.Yellow;
                        vehb.ShowRoute = true;
                        UI.ShowSubtitle("Go to the Meeting",3000);
                        steps = 1;
                    }
                }
                if (steps == 1)
                {
                    if (Game.Player.Character.Position.DistanceTo(escape_veh2.Position) < 100f)
                    {
                        vehb.Color = BlipColor.Blue;
                        vehb.ShowRoute = false;
                        UI.ShowSubtitle("Enter the ~b~ Vehicle");

                        int pgroupe = Game.Player.Character.RelationshipGroup;
                        escape_ped1.RelationshipGroup = pgroupe;
                        GiveAllWeapons(escape_ped1);
                        escape_ped1.CanSwitchWeapons = true;

                        escape_ped2.RelationshipGroup = pgroupe;
                        GiveAllWeapons(escape_ped2);
                        escape_ped2.CanSwitchWeapons = true;

                        original_ped.RelationshipGroup = pgroupe;
                        GiveAllWeapons(original_ped);
                        original_ped.CanSwitchWeapons = true;


                        escape_ped2.Task.EnterVehicle(escape_veh2, VehicleSeat.Passenger, 5000);
                        steps = 2;
                    }
                }
                if (steps == 2)
                {
                    escape_veh1.MarkAsNoLongerNeeded();
                    if (Game.Player.Character.IsInVehicle(escape_veh2) && escape_ped2.IsInVehicle(escape_veh2))
                    {
                        vehb.Remove();
                        escape_veh3.Position = new Vector3(-1179.25f, -2845.386f, 13.5665f);
                        escape_veh3.Heading = 325.6199f;
                        vehb = escape_veh3.AddBlip();
                        vehb.Color = BlipColor.Blue;
                        UI.ShowSubtitle("Steel the ~b~ Valkyrie ~w~ from ~r~ Airport", 3000);
                        steps = 3;
                    }
                }






                if (steps == 3)
                {
                        if (Game.Player.Character.IsInVehicle(escape_veh3))
                        {
                            vehb.Remove();
                            escape_ped2.Task.EnterVehicle(escape_veh3, VehicleSeat.RightRear, 5000);
                            steps = 4;
                        }
                }
                if (steps == 4)
                {
                    if (Game.Player.Character.IsInVehicle(escape_veh3))
                    {
                        if (escape_ped2.IsInVehicle(escape_veh3))
                        {
                            UI.ShowSubtitle("");
                            vehb = World.CreateBlip(new Vector3(1649.463f, 2619.634f, 45.56486f));
                            vehb.Color = BlipColor.Yellow;
                            vehb.ShowRoute = true;
                            UI.Notify("You can change between you and your partner with ~g~ [E] ~w~ do not use the game Character switche");
                            original_ped.Task.GoTo(new Vector3(1649.463f, 2619.634f, 45.56486f));
                            escape_veh2.MarkAsNoLongerNeeded();
                            steps = 5;
                        }
                        else
                        UI.ShowSubtitle("Wait for the you partner to enter the Valkyrie");
                    }
                    else
                    {
                        UI.ShowSubtitle("Enter to the Valkyrie");
                    }

                }
                if(steps == 5)
                {
                    if (Game.Player.Character.Position.DistanceTo(new Vector3(1649.463f, 2619.634f, 45.56486f))<50f)
                    {
                        if (current_ped == 2)
                        {
                            Function.Call(Hash.CHANGE_PLAYER_PED, Game.Player, escape_ped1, 1, 0);
                            steps = 6;
                        }
                    }
                }
                if (steps == 6)
                {
                    if (escape_veh3.IsOnAllWheels && Game.Player.Character.Position.DistanceTo(new Vector3(1649.463f, 2619.634f, 45.56486f)) < 50f)
                    {
                        vehb.Remove();
                        
                        original_ped.Task.EnterVehicle(escape_veh3, VehicleSeat.LeftRear, 1000);
                        UI.ShowSubtitle("wait for the prisonier");
                        steps = 7;
                    }
                }
                if (steps == 7)
                {
                    if (original_ped.IsInVehicle(escape_veh3))
                    {
                        UI.ShowSubtitle("escape the police",5000);
                        vehb.Remove();
                        steps = 8;
                    }
                }


                if (Game.Player.WantedLevel == 0 && steps == 8)
                {
                    UI.ShowSubtitle("you have escaped");
                    player_status = status.Off;
                    escape = false;
                    arrested = false;
                    inprison = false;
                    bail.Remove();
                    roit.Remove();
                    vehb.Remove();




                }
            
        }
    }

}