using System;
using Xamarin.Forms;

namespace ExhibitorModule.Controls
{
    public class LeadViewCell : ViewCell
    {
        public LeadViewCell()
        {
            View = GetView();
        }

        private View GetView()
        {
            return new Grid();
        }
    }
}

/*
<ViewCell>
                        <Grid>
                            <ia:CardView Margin="16,8" Visual="Material" Padding="8">
                                <Grid>
                                    <Grid.RowDefinitions>
                                        <RowDefinition Height="*"/>
                                        <RowDefinition Height="*"/>
                                    </Grid.RowDefinitions>
                                    <Grid.ColumnDefinitions>
                                        <ColumnDefinition Width="*"/>
                                        <ColumnDefinition Width="3*"/>
                                    </Grid.ColumnDefinitions>
                                    
                                    <ff:CachedImage Source="{Binding Avatar}" 
                                                    Grid.RowSpan="2" 
                                                    HorizontalOptions="Start"
                                                    InputTransparent="true">
                                        <ff:CachedImage.Transformations>
                                            <fft:CircleTransformation />
                                        </ff:CachedImage.Transformations>
                                    </ff:CachedImage>
                                    <Label Text="{Binding FirstName}" FontSize="Medium" Grid.Column="1"
                                            InputTransparent="true"/>
                                    <StackLayout Orientation="Horizontal" Grid.Column="1" Grid.Row="1" 
                                                 InputTransparent="true">
                                        <Label Text="{Binding Title}" FontSize="Small" TextColor="Gray"/>
                                        <Label Text="|" FontSize="Small" TextColor="Gray"/>
                                        <Label Text="{Binding Company}" FontSize="Small" TextColor="Gray"/>
                                    </StackLayout>
                                    
                                    <ImageButton Source="notes" 
                                                 Command="{Binding BindingContext.ShowNotes, Source={x:Reference this}}" 
                                                 CommandParameter="{Binding .}"
                                                 HeightRequest="24"
                                                 WidthRequest="24"
                                                 Grid.RowSpan="2"
                                                 Grid.Column="1"
                                                 HorizontalOptions="End"/>
                                </Grid>
                            </ia:CardView>
                        </Grid>
                    </ViewCell>



*/
