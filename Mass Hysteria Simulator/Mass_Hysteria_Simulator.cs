using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Controls;
using HearthDb.Enums;
using Hearthstone_Deck_Tracker.Hearthstone.Entities;
using Hearthstone_Deck_Tracker.Enums.Hearthstone;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.LogReader;
using Hearthstone_Deck_Tracker.Enums;
using Hearthstone_Deck_Tracker.Plugins;
using Hearthstone_Deck_Tracker.Utility.Logging;


namespace Mass_Hysteria_Simulator
{
    public class Mass_Hysteria_Simulator
    {
        internal static void TurnStart(ActivePlayer player)
        {
        }

        internal static void GameStart()
        {
        }

    }

    public class Mass_Hysteria_Simulator_Plugin : IPlugin
    {
        public bool IsPoisonous(Entity Minion)
        {
            return Minion.Tags.TryGetValue(GameTag.POISONOUS, out int p) && p == 1;
        }

        public bool HasDivineShield(Entity Minion)
        {
            return Minion.Tags.TryGetValue(GameTag.DIVINE_SHIELD, out int d) && d == 1;
        }

        public void HandleStupidShit(Object sender, System.EventArgs e)
        {
            Log.Info("asdklasdlkjdajd");
        }

        public void ShowButton()
        {
            Log.Info("Showing Button");
            SimButton button = new SimButton();
            //button.Button.Click += HandleStupidShit;
            Core.OverlayCanvas.Children.Add(button);
        }

        public string BoardToString(IEnumerable<Entity> Board)
        {
            // shitty name I know
            string BoardString = "";

            foreach (Entity CurrentMinion in Board)
            {
                if (CurrentMinion.IsMinion && CurrentMinion.IsInPlay)
                {
                    BoardString += CurrentMinion.Attack + "%2F" + CurrentMinion.Health;

                    if (HasDivineShield(CurrentMinion))
                        BoardString += "d";
                    if (IsPoisonous(CurrentMinion))
                        BoardString += "p";

                    BoardString += "%20";
                }
            }
            return BoardString;
        }

        public void HandleAddToHand(Hearthstone_Deck_Tracker.Hearthstone.Card Card)
        {
            if (Card.Id == "TRL_258")
            {
                //ShowButton();
            }
        }

        public void OnLoad()
        {
            // Triggered upon startup and when the user ticks the plugin on
            GameEvents.OnPlayerGet.Add(HandleAddToHand);
            GameEvents.OnPlayerPlayToHand.Add(HandleAddToHand);
            GameEvents.OnPlayerDraw.Add(HandleAddToHand);
        }

        public void OnUnload()
        {
            // Triggered when the user unticks the plugin, however, HDT does not completely unload the plugin.
            // see https://git.io/vxEcH
        }

        public void OnButtonPress()
        {
            // ShowButton();
        }

        public void OpenWebsite()
        {

            // Triggered when the user clicks your button in the plugin list
            if (Core.Game.IsMinionInPlay)
            {
                string BaseURL = "https://mass-hysteria-sim.now.sh/?";
                // string BaseURL = "http://localhost:8080/";
                string ExportString = "?";

                try
                {
                    if (Core.Game.PlayerMinionCount > 0)
                    {
                        ExportString += "f=";

                        ExportString += BoardToString(Core.Game.Player.Board);
                    }

                    if (Core.Game.PlayerMinionCount > 0 && Core.Game.OpponentMinionCount > 0)
                    {
                        ExportString += "&";
                    }

                    if (Core.Game.OpponentMinionCount > 0)
                    {
                        ExportString += "e=";
                        ExportString += BoardToString(Core.Game.Opponent.Board);
                    }

                } catch (Exception e)
                {
                    Log.Info(e.ToString());
                }

                System.Diagnostics.Process.Start(BaseURL + ExportString);
            }
        }

        public void ClickHandler(Object sender, System.EventArgs e)
        {
            OpenWebsite();
        }

        public void OnUpdate()
        {
            // called every ~100ms
        }

        public string Name => "Mass Hysteria Simulator";

        public string Description => Name;

        public string ButtonText => "LOLOLLO";

        public string Author => "AyyKamp";

        public Version Version => new Version(1, 0, 3);

        public MenuItem MenuItem {
            get {
                MenuItem mi = new MenuItem();
                mi.Header = "Mass Hysteria Simulator";
                mi.Click += ClickHandler;
                return mi;
                //return new MenuItem("Mass Hysteria Simulator", ClickHandler);
            }
        }

    }
}
