﻿using System;
using ExhibitorModule.Common;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Plugin.DeviceInfo;
using Plugin.DeviceInfo.Abstractions;
using ExhibitorModule.Helpers;
using ExhibitorModule.Views;
using ExhibitorModule.Services.Abstractions;
using System.Collections.ObjectModel;
using ExhibitorModule.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExhibitorModule.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly ILeadsService _leadsService;

        public MainPageViewModel(IBase @base, ILeadsService leadsService)
            : base(@base)
        {
            Title = Strings.Resources.MainPageTitle;
            _leadsService = leadsService;
            LoadLeadsCommand = new DelegateCommand(OnLoadLeadsCommandsTapped);
        }

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

        private ObservableCollection<Lead> _leads = new ObservableCollection<Lead>();
        public ObservableCollection<Lead> Leads
        {
            get => _leads;
            set { SetProperty(ref _leads, value); }
        }

        public DelegateCommand LoadLeadsCommand { get; set; }

        private async void OnLoadLeadsCommandsTapped()
        {
            Leads.Clear();
            var results = await RunTask(GetLeads());
            foreach (var lead in results)
            {
                Leads.Add(lead);
            }
        }

        private Task<List<Lead>> GetLeads(string query = null) => _leadsService.GetLeads(query);

        private async void Search(string query)
        {
            Leads.Clear();
            var results = await RunTask(GetLeads(query));
            foreach (var lead in results)
            {
                Leads.Add(lead);
            }
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            LoadLeadsCommand?.Execute();
        }
    }
}
