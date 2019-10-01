using Hearthstone_Deck_Tracker.API;
using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using Hearthstone_Deck_Tracker.Utility.Logging;

namespace Mass_Hysteria_Simulator
{
    public partial class SimButton : MetroWindow
    {
        public SimButton()
        {
            InitializeComponent();
            Left += 1000;
        }
    }
}
