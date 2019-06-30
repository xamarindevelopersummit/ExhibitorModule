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
        private CardView _cardView;
        private readonly Style _labelStyle;
        private readonly Style _labelValueStyle;

        public NotesDialog()
        {
            Content = BuildView();
            _labelStyle = GetLabelStyle();

            _cardView.TranslationY = 100;

            var x = (Width / 2) - _cardView.Width / 2;
            var y = (Height / 2 ) - _cardView.Height / 2;
            _cardView.TranslateTo(x, y, easing: Easing.SpringOut);
        }

        Style GetLabelStyle()
        {
            return new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter { Property = Label.TextColorProperty, Value = Color.Silver },
                    new Setter { Property = Label.FontSizeProperty, Value = 9 }
                }
            };
        }

        private View BuildView()
        {
            var avatar = new CachedImage
            {
                HeightRequest = 100,
                WidthRequest = 100,
                Aspect = Aspect.AspectFit
            };
            avatar.Transformations.Add(new CircleTransformation());
            avatar.SetBinding(CachedImage.SourceProperty, new Binding("CurrentLead.GravatarUri",
                                                                    converter: new GravatarConverter(),
                                                                    converterParameter: 100));


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
                FontSize = 24,
                TextColor = Color.Black,
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,
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
                Style = _labelValueStyle,
                HorizontalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 0, 0, 8)
            };
            company.SetBinding(Label.TextProperty, new Binding("CurrentLead.Company"));

            var email = new Label
            {
                Style = _labelValueStyle,
            };
            email.SetBinding(Label.TextProperty, new Binding("CurrentLead.Email"));

            var editor = new Editor
            {
                HeightRequest = 200,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Keyboard = Keyboard.Text
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
                HeightRequest = 20,
                WidthRequest = 20
            };

            var notesIcon = new SvgCachedImage
            {
                Source = new EmbeddedResourceImageSource("ExhibitorModule.Resources.notes.svg", typeof(NotesDialog).Assembly),
                HeightRequest = 20,
                WidthRequest = 20
            };

            var mainGrid = new Grid();
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            
            mainGrid.Children.Add(avatar);
            mainGrid.Children.Add(
                new StackLayout {
                        Children = {
                            name,
                            company,
                            new StackLayout
                            {
                                Margin=new Thickness(0, 8),
                                Orientation = StackOrientation.Horizontal,
                                Children =
                                {
                                    emailIcon,
                                    email
                                }
                            },
                            new StackLayout
                            {
                                HeightRequest = 32.0,
                                Orientation = StackOrientation.Horizontal,
                                Children =
                                {
                                    notesIcon,
                                    new Label
                                    {
                                        Text = "Notes",
                                        Style = _labelStyle,
                                        VerticalOptions = LayoutOptions.Center
                                    },
                                    activityIndicator
                                }
                            },
                            editor,
                            saveButton
                        }
                }, 0, 1
            );
            _cardView = new CardView
            {
                Visual = VisualMarker.Material,
                Padding = 16,
                Content = new ScrollView { Content = mainGrid },
                MinimumHeightRequest = 300
            };

            DialogLayout.SetRelativeWidthRequest(this, 0.75);
            return _cardView;
        }
    }
}
