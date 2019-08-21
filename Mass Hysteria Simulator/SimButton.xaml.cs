using Hearthstone_Deck_Tracker.API;
using System;
using System.Windows;
using System.Windows.Controls;
using Hearthstone_Deck_Tracker.Utility.Logging;

namespace Mass_Hysteria_Simulator
{
    public partial class SimButton : UserControl
    {
        public SimButton()
        {
            InitializeComponent();
            UpdatePosition();
			
            //Hide();
        }

        private double ScreenRatio => (4.0 / 3.0) / (Core.OverlayCanvas.Width / Core.OverlayCanvas.Height);

        public void HandleClick(Object sender, System.EventArgs e)
        {
            Log.Info("Click!");
        }

        public void UpdatePosition()
        {
            /* OLD CODE; positions near top of screen
            Canvas.SetTop(this, Core.OverlayCanvas.Height * 3 / 100);
            var xPos = Hearthstone_Deck_Tracker.Helper.GetScaledXPos(8.0 / 100, (int)Core.OverlayCanvas.Width, ScreenRatio);
            if (isLocal)
            {                
                Canvas.SetRight(this, xPos);
            }
            else
            {
                Canvas.SetLeft(this, xPos);
            }
            */

            Canvas.SetRight(this, Hearthstone_Deck_Tracker.Helper.GetScaledXPos(5.0 / 100, (int)Core.OverlayCanvas.Width, ScreenRatio));
            
            Canvas.SetBottom(this, Core.OverlayCanvas.Height * 75 / 100);
            
        }

        public void Show()
        {
            this.Visibility = Visibility.Visible;
        }

        public void Hide()
        {
            this.Visibility = Visibility.Hidden;
        }
    }
}
