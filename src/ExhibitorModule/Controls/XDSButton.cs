using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace ExhibitorModule.Controls
{
    [DesignTimeVisible(true)]
    public class XDSButton : Button
    {
        public XDSButton()
        {
            Visual = VisualMarker.Material;
            TextColor = Color.White;
            UpdateUI(true);
        }

        bool _isSecondary;
        public bool IsSecondary
        {
            get { return _isSecondary; }
            set { _isSecondary = value; UpdateUI(!value); }
        }

        void UpdateUI(bool isPrimary)
        {
            BackgroundColor = isPrimary ? (Color)Application.Current.Resources["Primary"] : (Color)Application.Current.Resources["Accent"];
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName.Equals("Height"))
                CornerRadius = (int) Height / 2;
        }
    }
}
