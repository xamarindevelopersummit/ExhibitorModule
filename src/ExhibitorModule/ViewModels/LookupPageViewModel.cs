using System;
using System.Collections.ObjectModel;
using ExhibitorModule.Helpers;
using ExhibitorModule.Models;
using ExhibitorModule.Services.Abstractions;

namespace ExhibitorModule.ViewModels
{
    public class LookupPageViewModel : ViewModelBase
    {
        readonly ILeadsService _leadsService;

        public LookupPageViewModel(IBase @base, ILeadsService leadsService) : base(@base)
        {
            Title = Strings.Resources.LookupPageTitle;
            _leadsService = leadsService;
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                UpdateSearch(value);
            }
        }

        private Attendee selectedAttendee;
        public Attendee SelectedAttendee 
        { 
            get => selectedAttendee;
            set
            {
                selectedAttendee = value;
                if (value == null)
                    return;

                _leadsService.AddUpdateLead(new Lead
                {
                    Id = Guid.NewGuid(),
                    AttendeeId = value.Id
                    
                });
                NavigationService.GoBackAsync();
            }
        }

        public ObservableCollection<Attendee> SearchResults { get; } = new ObservableCollection<Attendee>();

        async void UpdateSearch(string query)
        {
            SearchResults.Clear();

            var results = await _leadsService.LookupAttendees(query);
            if (results == null)
                return;
            foreach (var attendee in results)
            {
                SearchResults.Add(attendee);
            }
        }
    }
}
