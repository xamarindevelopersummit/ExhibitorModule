using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExhibitorModule.Common;
using ExhibitorModule.Helpers;
using ExhibitorModule.Models;
using ExhibitorModule.Services.Abstractions;
using MvvmHelpers;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services.Dialogs;

namespace ExhibitorModule.ViewModels
{
    class LeadsPageViewModel : ViewModelBase
    {
        private readonly ILeadsService _leadsService;
        private readonly IDialogService _dialogService;
        private readonly ICacheService _cacheService;

        public LeadsPageViewModel(IBase @base, ILeadsService leadsService,
            IDialogService dialogService, ICacheService cacheService)
            : base(@base)
        {
            Title = Strings.Resources.LeadsPageTitle;
            _leadsService = leadsService;
            _dialogService = dialogService;
            _cacheService = cacheService;
            LoadLeadsCommand = new DelegateCommand(OnLoadLeadsCommandsTapped);
            ShowNotes = new DelegateCommand<LeadContactInfo>(OnNotesTapped);
            RemoveLeadCommand = new DelegateCommand<LeadContactInfo>(OnRemoveLeadTapped);
        }

        #region Properties & Commands
        
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                Search(value);
            }
        }

        private ObservableRangeCollection<LeadContactInfo> _leads = new ObservableRangeCollection<LeadContactInfo>();
        public ObservableRangeCollection<LeadContactInfo> Leads
        {
            get => _leads;
            set { SetProperty(ref _leads, value); }
        }

        public DelegateCommand LoadLeadsCommand { get; set; }
        public DelegateCommand<LeadContactInfo> RemoveLeadCommand { get; set; }
        public DelegateCommand<LeadContactInfo> ShowNotes { get; set; }

        #endregion

        #region Command Handlers

        private void OnNotesTapped(LeadContactInfo lead)
        {
            if (lead == null) return;

            _dialogService.ShowDialog(Dialogs.Notes, new DialogParameters {
                    { AppConstants.LeadKey, lead },
                    { KnownDialogParameters.CloseOnBackgroundTapped, true }
                }, OnNotesDialogClosed);
        }

        void OnNotesDialogClosed(IDialogResult result)
        {
            if (result.Parameters == null || !result.Parameters.Any())
                return;

            foreach (var key in result.Parameters.Keys)
            {
                Console.WriteLine($"Key:{key}, Value:{result.Parameters.GetValue<object>(key).ToString()}");
            }
        }

        private async void OnLoadLeadsCommandsTapped()
        {
            LoadLeads(await RunTask(GetLeads()));
        }

        private async void OnRemoveLeadTapped(LeadContactInfo lead)
        {
            if (!await PageDialogService.DisplayAlertAsync(string.Format(Strings.Resources.Remove0, lead.FirstName),
                Strings.Resources.ConfirmRemovePrompt,
                Strings.Resources.YES, Strings.Resources.NO))
                return;
            await RunTask(RemoveLead(lead));
            LoadLeadsCommand?.Execute();
        }

        #endregion

        #region Private Methods

        private void LoadLeads(List<LeadContactInfo> leads)
        {
            if (!(leads?.Any() ?? false)) return;

            Leads.ReplaceRange(leads);
        }

        private Task<List<LeadContactInfo>> GetLeads()
        {
            return _leadsService.GetLeads();
        }

        Task RemoveLead(LeadContactInfo lead)
        {
            return _leadsService.RemoveLead(lead);
        }

        private void Search(string query)
        {
            if(string.IsNullOrWhiteSpace(query))
                LoadCache();

            LoadLeads(Leads.Where(_ => _.FirstName.Contains(query) || _.LastName.Contains(query)).ToList());
        }

        void LoadCache()
        {
            LoadLeads(_cacheService.Device?.GetObject<List<LeadContactInfo>>(CacheKeys.LeadsKey));
        }

        #endregion

        #region Overrides

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            LoadCache();
            LoadLeadsCommand?.Execute();

            if (parameters.GetNavigationMode() != NavigationMode.Back)
                _leadsService.GetAttendees();
        }

        #endregion
    }
}
