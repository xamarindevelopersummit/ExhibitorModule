using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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

        string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                UpdateSearch(value);
            }
        }

        ObservableCollection<Attendee> _searchResults = new ObservableCollection<Attendee>();
        public ObservableCollection<Attendee> SearchResults { get => _searchResults; private set => _searchResults = value; }

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
