﻿using System;
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
        private Label _title;
        private Editor _editor;
        private XDSButton _saveButton;
        private ActivityIndicator _activityIndicator;

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
            _activityIndicator = new ActivityIndicator
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            _activityIndicator.SetBinding(IsVisibleProperty, new Binding("IsBusy"));

            _title = new Label
            {
                FontSize = 14,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            _title.SetBinding(Label.TextProperty, new Binding("CurrentLead.FirstName", stringFormat: "Notes for {0}"));

            _editor = new Editor
            {
                MinimumHeightRequest = 150,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Keyboard = Keyboard.Text
            };
            _editor.SetBinding(Editor.TextProperty, new Binding("CurrentLead.Notes", BindingMode.TwoWay));

            _saveButton = new XDSButton
            {
                Text = "Save"
            };
            _saveButton.SetBinding(Button.CommandProperty, new Binding("SaveCommand"));

            _cardView = new CardView
            {
                Visual = VisualMarker.Material,
                Padding = 16,
                Content = new StackLayout {
                    Children = {
                        new StackLayout
                        {
                            HeightRequest = 32.0,
                            Orientation = StackOrientation.Horizontal,
                            Children =
                            {
                                _title,
                                _activityIndicator
                            }
                        },
                        _editor,
                        _saveButton
                    }
                },
                HeightRequest = 200
            };

            DialogLayout.SetRelativeWidthRequest(this, 0.75);
            return _cardView;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            var x = BindingContext;
        }
    }
}
