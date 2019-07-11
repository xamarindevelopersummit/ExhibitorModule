using System;
using ExhibitorModule.Controls;
using ExhibitorModule.Controls.Converters;
using FFImageLoading.Forms;
using FFImageLoading.Svg.Forms;
using FFImageLoading.Transformations;
using IntelliAbb.Xamarin.Controls;
using Prism.Services.Dialogs.Xaml;
using Xamarin.Forms;

namespace ExhibitorModule.Views
{
    public class NotesDialog : ContentView
    {
        public NotesDialog()
        {
            Content = BuildView();
        }

        private View BuildView()
        {
            var avatar = new CachedImage
            {
                HeightRequest = 64,
                WidthRequest = 64,
                Aspect = Aspect.AspectFit
            };
            avatar.Transformations.Add(new CircleTransformation());
            avatar.SetBinding(CachedImage.SourceProperty, new Binding("CurrentLead.GravatarUri",
                                                                    converter: new GravatarConverter(),
                                                                    converterParameter: 64));


            var activityIndicator = new ActivityIndicator
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            activityIndicator.SetBinding(IsVisibleProperty, new Binding("IsBusy"));

            var firstNameSpan = new Span();
            firstNameSpan.SetBinding(Span.TextProperty, new Binding("CurrentLead.FirstName", BindingMode.OneWay, stringFormat: "{0} "));
            var lastNameSpan = new Span();
            lastNameSpan.SetBinding(Span.TextProperty, new Binding("CurrentLead.LastName", BindingMode.OneWay));

            var name = new Label
            {
                FontSize = 18,
                TextColor = Color.Black,
                FontAttributes = FontAttributes.Bold,
                VerticalTextAlignment = TextAlignment.End,
                VerticalOptions = LayoutOptions.FillAndExpand,
                FormattedText = new FormattedString
                {
                    Spans = {
                            firstNameSpan,
                            lastNameSpan
                        }
                }
            };

            var company = new Label
            {
                FontSize = 14,
                TextColor = Color.Gray,
                VerticalOptions = LayoutOptions.FillAndExpand,
                VerticalTextAlignment = TextAlignment.Start
            };
            company.SetBinding(Label.TextProperty, new Binding("CurrentLead.Company"));

            var email = new Label
            {
                FontSize = 14,
                TextColor = Color.Gray
            };
            email.SetBinding(Label.TextProperty, new Binding("CurrentLead.Email"));
            
            var notesLabel = new Label
            {
                FontSize = 14,
                TextColor = Color.Gray,
                Text = "Notes",
                VerticalTextAlignment = TextAlignment.Center
            };

            var editor = new Editor
            {
                Keyboard = Keyboard.Text,
                HeightRequest = 100
            };
            editor.SetBinding(Editor.TextProperty, new Binding("CurrentLead.Notes", BindingMode.TwoWay));

            var saveButton = new XDSButton
            {
                Text = "Save"
            };
            saveButton.SetBinding(Button.CommandProperty, new Binding("SaveCommand"));

            var emailIcon = new SvgCachedImage
            {
                Source = new EmbeddedResourceImageSource("ExhibitorModule.Resources.email.svg", typeof(NotesDialog).Assembly),
                HeightRequest = 14,
                WidthRequest = 14,
            };

            var notesIcon = new SvgCachedImage
            {
                Source = new EmbeddedResourceImageSource("ExhibitorModule.Resources.notes.svg", typeof(NotesDialog).Assembly),
                HeightRequest = 14,
                WidthRequest = 14
            };

            var avatarStack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                    {
                        avatar,
                        new StackLayout
                        {
                            Children = {
                                name,
                                company
                            }
                        }
                    }
            };

            var emailStack = new StackLayout
            {
                Children = {
                            new StackLayout
                            {
                                Orientation = StackOrientation.Horizontal,
                                Children =
                                {
                                    emailIcon,
                                    email
                                }
                            },
                            new StackLayout
                            {
                                Orientation = StackOrientation.Horizontal,
                                Children =
                                {
                                    notesIcon,
                                    notesLabel,
                                    activityIndicator
                                }
                            }
                        }
            };

            var mainGrid = new Grid();
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            mainGrid.Children.Add(avatarStack);
            mainGrid.Children.Add(emailStack, 0, 1);
            mainGrid.Children.Add(editor, 0, 2);
            mainGrid.Children.Add(saveButton, 0, 3);

            DialogLayout.SetRelativeWidthRequest(this, 0.75);

            return new ScrollView { Content = new CardView
                                        {
                                            Visual = VisualMarker.Material,
                                            Padding = 16,
                                            Content = mainGrid
                                        }
            };
        }

    }
}
