using System;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Collections.Generic;

using HearthDb.Enums;
using Hearthstone_Deck_Tracker.Hearthstone;
using Hearthstone_Deck_Tracker.Hearthstone.Entities;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Plugins;
using Hearthstone_Deck_Tracker.Utility.Logging;
using Hearthstone_Deck_Tracker.Utility.HotKeys;

using static HearthDb.CardIds;


namespace Mass_Hysteria_Simulator
{
    public class Mass_Hysteria_Simulator
    {
    }

    public class Mass_Hysteria_Simulator_Plugin : IPlugin
    {
        SimButton _button;

        public void HandleClick(Object sender, EventArgs e)
        {
            OpenWebsite();
        }

        public void HandleLostFocus(Object sender, EventArgs e)
        {
            Log.Info("Lost Focus");
            if (_button != null)
            {
                _button.Hide();
            }
        }

        public void HandleGotFocus(Object sender, EventArgs e)
        {
            Log.Info("Got Focus");
            if (_button != null)
            {
                _button.Show();
            }
        }

        public void HandleCloseButton(Object sender, EventArgs e)
        {
            CloseButton();
        }

        public void ShowButton()
        {
            Log.Info("Showing Button");

            _button = new SimButton()
            {
                Owner = Core.OverlayWindow
            };

            _button.Button.Click += HandleClick;
            _button.Closed += HandleCloseButton;

            _button.Show();
        }

        public void HideButton()
        {
            Log.Info("Hiding Button");
            _button.Hide();
        }

        public void CloseButton()
        {
            _button.Close();
            _button = null;
        }

        public void ToggleButton()
        {
            if (_button == null)
            {
                ShowButton();
            }
            else
            {
                HideButton();
            }
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

                    if (CurrentMinion.HasTag(GameTag.DIVINE_SHIELD))
                        BoardString += "d";

                    if (CurrentMinion.HasTag(GameTag.POISONOUS))
                        BoardString += "p";

                    BoardString += "%20";

                }
            }
            return BoardString;
        }

        public void HandleAddToHand(Card CurrentCard)
        {
            if (CurrentCard.Id == Collectible.Priest.MassHysteria && _button == null)
            {
                ShowButton();
            }
        }

        public void HandleRemoveFromHand(Card CurrentCard)
        {
            Log.Info(CurrentCard.FlavorText);
            if (CurrentCard.Id == Collectible.Priest.MassHysteria && _button != null)
            {
                CloseButton();
            }
        }

        public void OnLoad()
        {
            // Triggered upon startup and when the user ticks the plugin on
            HotKeyManager.RegisterHotkey(new HotKey(Keys.F3), new Action(OpenWebsite), "Open Mass Hysteria Sim");

            GameEvents.OnPlayerGet.Add(HandleAddToHand);
            GameEvents.OnPlayerPlayToHand.Add(HandleAddToHand);
            GameEvents.OnPlayerDraw.Add(HandleAddToHand);

            GameEvents.OnPlayerHandDiscard.Add(HandleRemoveFromHand);
            GameEvents.OnPlayerPlay.Add(HandleRemoveFromHand);

            GameEvents.OnGameEnd.Add(CloseButton);
            GameEvents.OnPlayerMulligan.Add(HandleRemoveFromHand);

            if (!Core.Game.IsInMenu)
            {
                foreach (Entity CurrentCard in Core.Game.Player.Hand)
                {
                    if (CurrentCard.CardId == Collectible.Priest.MassHysteria && _button == null)
                    {
                        ShowButton();
                    }
                }
            }
        }

        public void OnUnload()
        {
            // Triggered when the user unticks the plugin, however, HDT does not completely unload the plugin.
            // see https://git.io/vxEcH
        }

        public void OnButtonPress()
        {
            ToggleButton();
        }

        public void OpenWebsite()
        {

            if (Core.Game.IsMinionInPlay)
            {
                string BaseURL = "https://mass-hysteria-sim.now.sh/";
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

        public void MenuItemClick (Object sender, System.EventArgs e)
        {
            OpenWebsite();
        }

        public void OnUpdate()
        {
            // called every ~100ms
        }

        public string Name => "Mass Hysteria Simulator";

        public string Description => Name;

        public string ButtonText => "Dont Click";

        public string Author => "AyyKamp";

        public Version Version => new Version(1, 2);

        public System.Windows.Controls.MenuItem MenuItem {
            get {
                System.Windows.Controls.MenuItem mi = new System.Windows.Controls.MenuItem
                {
                    Header = "Mass Hysteria Simulator"
                };
                mi.Click += MenuItemClick;
                return mi;
            }
        }

    }
}
