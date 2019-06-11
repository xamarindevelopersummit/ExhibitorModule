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
                    FirstName = selectedAttendee.FirstName,
                    LastName = selectedAttendee.LastName,
                    Company = selectedAttendee.Company,
                    Avatar = selectedAttendee.Avatar,
                    AdmissionType = selectedAttendee.AdmissionType,
                    Title = selectedAttendee.Title,
                    VisitedAt = DateTime.Now
                });
                NavigationService.GoBackAsync();
            }
        }

        public ObservableCollection<Attendee> SearchResults { get; } = new ObservableCollection<Attendee>();

        async void UpdateSearch(string query)
        {
            SearchResults.Clear();

            var results = await _leadsService.LooupAttendees(query);
            foreach (var attendee in results)
            {
                SearchResults.Add(attendee);
            }
        }
    }
}
