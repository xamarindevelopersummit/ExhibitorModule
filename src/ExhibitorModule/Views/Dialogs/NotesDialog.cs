using System;
using ExhibitorModule.Controls;
using ExhibitorModule.Models;
using Prism.Services.Dialogs;
using Xamarin.Forms;

namespace ExhibitorModule.Views
{
    public class NotesDialog : ContentView
    {
        public NotesDialog()
        {
            BuildView();
        }

        private void BuildView()
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
            editor.SetBinding(Editor.TextProperty, new Binding("CurrentLead.Notes", BindingMode.TwoWay));

            var saveButton = new XDSButton
            {
                Text = "Save"
            };
            saveButton.SetBinding(Button.CommandProperty, new Binding("SaveCommand"));

            Content = new IntelliAbb.Xamarin.Controls.CardView
            {
                Visual = VisualMarker.Material,
                Padding = 8,
                Margin = new Thickness(40,80),
                WidthRequest=200,
                HeightRequest=250,
                Content = new StackLayout {
                    Children = { 
                        title,
                        editor,
                        saveButton
                    }
                }
            };
        }
    }
}
