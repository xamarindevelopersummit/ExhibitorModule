using System;
using ExhibitorModule.Controls;
using ExhibitorModule.Models;
using IntelliAbb.Xamarin.Controls;
using Prism.Services.Dialogs;
using Prism.Services.Dialogs.Xaml;
using Xamarin.Forms;

namespace ExhibitorModule.Views
{
    public class NotesDialog : ContentView
    {
        private CardView _cardView;

        public NotesDialog()
        {
            Content = BuildView();

            _cardView.TranslationY = 100;

            var x = (Width / 2) - _cardView.Width / 2;
            var y = (Height / 2 ) - _cardView.Height / 2;
            _cardView.TranslateTo(x, y, easing: Easing.SpringOut);
        }

        private View BuildView()
        {
            var title = new Label
            {
                FontSize = 14
            };
            title.SetBinding(Label.TextProperty, new Binding("Title"));

            var editor = new Editor
            {
                MinimumHeightRequest = 150,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            editor.SetBinding(Editor.TextProperty, new Binding("Notes", BindingMode.TwoWay));

            var saveButton = new XDSButton
            {
                Text = "Save"
            };
            saveButton.SetBinding(Button.CommandProperty, new Binding("SaveCommand"));
            _cardView = new CardView
            {
                Visual = VisualMarker.Material,
                Padding = 16,
                Content = new StackLayout {
                    Children = { 
                        title,
                        editor,
                        saveButton
                    }
                },
                HeightRequest = 200
            };
            DialogLayout.SetRelativeWidthRequest(this, 0.75);
            return _cardView;
        }
    }
}
