using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ExhibitorModule.Common;
using ExhibitorModule.Models;
using ExhibitorModule.Services.Abstractions;
using ExhibitorModule.Services.Helpers;

namespace ExhibitorModule.Services
{
    public class LeadsService : ILeadsService
    {
        private readonly IApiService _apiService;
        private readonly ICacheService _cacheService;

        public LeadsService(IApiService apiService, ICacheService cacheService)
        {
            _apiService = apiService;
            _cacheService = cacheService;
        }

        public async Task AddUpdateLead(LeadContactInfo lead)
        {
            await _apiService.Post<HttpResponseMessage>(ApiKeys.AddLeadApi, lead.ToContent());
        }

        public async Task<List<Attendee>> GetAttendees()
        {
            var response = await _apiService.Get<HttpResponseMessage>(new Uri(ApiKeys.AttendeesApi));
            var result = await response.ReadAsAsync<List<Attendee>>();
            _cacheService?.Device?.AddOrUpdateValue(CacheKeys.AttendeesKey, result);
            return result;
        }

        public async Task<LeadContactInfo> GetLeadById(Guid id)
        {
            var tcs = new TaskCompletionSource<LeadContactInfo>();
            await Task.Run(() =>
            {
                try
                {
                    var leads = _cacheService.Device.GetObject<List<LeadContactInfo>>(CacheKeys.LeadsKey) ?? new List<LeadContactInfo>();
                    tcs.SetResult(leads.FirstOrDefault(_ => _.Id == id));
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });
            return tcs.Task.Result;
        }

        public async Task<List<LeadContactInfo>> GetLeads()
        {
            var response = await _apiService.Get<HttpResponseMessage>(new Uri(ApiKeys.LeadsApi));
            var leads = await response?.ReadAsAsync<List<LeadContactInfo>>();

            if (leads == null)
                return new List<LeadContactInfo>();
            
            _cacheService?.Device?.AddOrUpdateValue(CacheKeys.LeadsKey, leads.OrderByDescending(_=>_.LastUpdate).ToList());
            return leads;
        }

        public async Task<List<LeadContactInfo>> LookupLead(string query)
        {
            var tcs = new TaskCompletionSource<List<LeadContactInfo>>();
            await Task.Run(() =>
            {
                try
                {
                    if (query == null) query = string.Empty;

                    var leads = _cacheService.Device.GetObject<List<LeadContactInfo>>(CacheKeys.LeadsKey) ?? new List<LeadContactInfo>();
                    var result = leads.Where(_ => _.FirstName.Contains(query) || _.LastName.Contains(query));
                    tcs.SetResult(result.ToList());
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });
            return tcs.Task.Result;
        }

        public async Task<Attendee> GetAttendeeById(Guid id)
        {
            var tcs = new TaskCompletionSource<Attendee>();
            await Task.Run(() =>
            {
                try
                {
                    var attendees = _cacheService.Device.GetObject<List<Attendee>>(CacheKeys.AttendeesKey) ?? new List<Attendee>();
                    tcs.SetResult(attendees.FirstOrDefault(_=>_.Id == id));
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });
            return tcs.Task.Result;
        }

        public async Task<List<Attendee>> LookupAttendees(string query)
        {
            var tcs = new TaskCompletionSource<List<Attendee>>();
            await Task.Run(()=>
            {
                try
                {
                    var attendees = _cacheService.Device.GetObject<List<Attendee>>(CacheKeys.AttendeesKey) ?? new List<Attendee>();
                    tcs.SetResult(attendees.Where(_ => _.FirstName.Contains(query) || _.LastName.Contains(query))
                        .OrderBy(_ => _.FirstName)
                        .ThenBy(_ => _.LastName)
                        .ToList());
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });
            return tcs.Task.Result;
        }
    }
}
