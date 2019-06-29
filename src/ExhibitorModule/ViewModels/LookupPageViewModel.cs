using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ExhibitorModule.Helpers;
using ExhibitorModule.Models;
using ExhibitorModule.Services.Abstractions;
using MvvmHelpers;
using Prism.Navigation;

namespace ExhibitorModule.ViewModels
{
    public class LookupPageViewModel : ViewModelBase
    {
        readonly ILeadsService _leadsService;
        readonly ICacheService _cacheService;
        readonly IModuleConfig _moduleConfig;

        List<Attendee> _attendeesCache;
        List<LeadContactInfo> _leads;

        public LookupPageViewModel(IBase @base, ILeadsService leadsService,
            ICacheService cacheService, IModuleConfig moduleConfig) : base(@base)
        {
            Title = Strings.Resources.LookupPageTitle;
            _leadsService = leadsService;
            _cacheService = cacheService;
            _moduleConfig = moduleConfig;
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

                AddLead(value);
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            _leads = _cacheService.Device.GetObject<List<LeadContactInfo>>(Common.CacheKeys.LeadsKey) ?? new List<LeadContactInfo>();
            _attendeesCache = _cacheService.Device.GetObject<List<Attendee>>(Common.CacheKeys.AttendeesKey).OrderBy(_ => _.FirstName)
                        .ThenBy(_ => _.LastName)
                        .ToList() ?? new List<Attendee>();
        }

        async void AddLead(Attendee attendee)
        {
            await RunTask(RequestAddLead(attendee));
        }

        async Task RequestAddLead(Attendee attendee)
        {

            await _leadsService.AddUpdateLead(new LeadContactInfo
            {
                Id = Guid.NewGuid(),
                AttendeeId = attendee.Id,
                LastUpdate = DateTime.Now
            });
            await NavigationService.GoBackAsync();
        }

        public ObservableRangeCollection<Attendee> SearchResults { get; } = new ObservableRangeCollection<Attendee>();

        void UpdateSearch(string query)
        {
#if DEBUG
            if (string.IsNullOrWhiteSpace(query))
            {
                SearchResults.ReplaceRange(_attendeesCache);
                return;
            }
#else
            if (string.IsNullOrWhiteSpace(query))
            {
                SearchResults.Clear();
                return; // don't volunteer info
            }
#endif
            var results = _attendeesCache?.Where(_=>(_.FirstName.Contains(query) || _.LastName.Contains(query)) && !IsALead(_.Id))?
                .Take(_moduleConfig.SearchResultsLimit)?.ToList();

            if (results == null)
                return;

            SearchResults.ReplaceRange(results);
        }

        bool IsALead(Guid id)
        {
            return _leads.FirstOrDefault(_ => _.AttendeeId.Equals(id)) != null;
        }
    }
}
