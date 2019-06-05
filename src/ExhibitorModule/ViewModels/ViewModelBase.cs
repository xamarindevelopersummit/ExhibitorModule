using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ExhibitorModule.Helpers;
using ExhibitorModule.Models;
using ExhibitorModule.Services.Abstractions;
using MvvmHelpers;
using Prism;
using Prism.AppModel;
using Prism.Navigation;
using Prism.Services;

namespace ExhibitorModule.ViewModels
{
    public class ViewModelBase : BaseViewModel, IActiveAware, INavigationAware, IDestructible, IConfirmNavigation, IConfirmNavigationAsync, IApplicationLifecycleAware, IPageLifecycleAware
    {
        protected ICacheService CacheService { get; }

        protected IPageDialogService PageDialogService { get; }

        protected IDeviceService DeviceService { get; }

        public INavigationService NavigationService { get; }

        public ViewModelBase(IBase @base)
        {
            PageDialogService = @base.PageDialogService;
            DeviceService = @base.DeviceService;
            CacheService = @base.CacheService;
            NavigationService = @base.NavigationService;
        }

        #region Runners

        /// <summary>
        /// Runs the task.
        /// </summary>
        /// <returns>The task.</returns>
        /// <param name="task">Task.</param>
        /// <param name="handleException">If set to <c>true</c> handle exception. Otherwise, throw the exeption to be handled by calling ViewModel.</param>
        /// <param name="actionOnException">If handleException is <c>false</c>, then invoke this method to handle exception.</param>
        /// <param name="messageOnException">Message on display when exception occurs.</param>
        /// <param name="lockNavigation">Lock the navigation and keep the user on the current view.</param>
        /// <param name="ignoreIsBusy">tells the Task Runner that it should not modify the state of Is Busy</param>
        /// <param name="caller">Calling method.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        protected async Task RunTask(Task task, bool handleException = true, Action<Exception, string> actionOnException = null, string messageOnException = null, bool? lockNavigation = null,
            bool ignoreIsBusy = false, [CallerMemberName] string caller = null)
        {
            UpdateIsBusy(!ignoreIsBusy || IsBusy, lockNavigation);

            await task.ContinueWith((thisTask) => {

                UpdateIsBusy(ignoreIsBusy && IsBusy);

                if (thisTask.IsFaulted && thisTask.Exception != null)
                {
                    Exception exception = null;
                    if (thisTask.Exception.InnerException != null)
                    {
                        exception = thisTask.Exception.InnerException;
                        while (exception.InnerException != null)
                            exception = exception.InnerException;
                    }
                    else
                    {
                        exception = thisTask.Exception;
                    }

                    if (exception is OfflineException)
                    {
                        DeviceService.BeginInvokeOnMainThread(() =>
                        {
                            //NavigationService.NavigateAsync(nameof(ServiceInfoPage), new NavigationParameters { { AppConstants.MaintenanceMessageKey, (exception.InnerException ?? exception).Message } }, true, false);
                            Acr.UserDialogs.UserDialogs.Instance.Toast(Strings.Resources.OfflineMessage);
                        });
                    }
                    else if (!handleException)
                    {
                        if (actionOnException == null)
                            throw exception;

                        DeviceService.BeginInvokeOnMainThread(() => actionOnException.Invoke(exception, messageOnException));
                    }
                }
            });
        }

        /// <summary>
        /// Runs the task.
        /// </summary>
        /// <returns>The task.</returns>
        /// <param name="task">Task.</param>
        /// <param name="handleException">If set to <c>true</c> handle exception. Otherwise, throw the exeption to be handled by calling ViewModel.</param>
        /// <param name="actionOnException">If handleException is <c>false</c>, then invoke this method to handle exception.</param>
        /// <param name="messageOnException">Message on display when exception occurs.</param>
        /// <param name="lockNavigation">Lock the navigation and keep the user on the current view.</param>
        /// <param name="ignoreIsBusy">tells the Task Runner that it should not modify the state of Is Busy</param>
        /// <param name="caller">Calling method.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        protected async Task<T> RunTask<T>(Task<T> task, bool handleException = true, Action<Exception, string> actionOnException = null, string messageOnException = null, bool? lockNavigation = null,
            bool ignoreIsBusy = false, [CallerMemberName] string caller = null)
        {
            try
            {
                await RunTask((Task)task, handleException, actionOnException, messageOnException, lockNavigation, ignoreIsBusy, caller);
                return task.Result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"{(ex.InnerException ?? ex).Message}");
                UpdateIsBusy(false);
                return default(T);
            }
        }

        private void UpdateIsBusy(bool isBusy, bool? lockNavigation = null)
        {
            IsBusy = isBusy;
            //CanNavigate = lockNavigation.HasValue ? !lockNavigation.Value : !isBusy;
        }

        #endregion

        #region IActiveAware

        public bool IsActive { get; set; }

        public event EventHandler IsActiveChanged;

        private void OnIsActiveChanged()
        {
            IsActiveChanged?.Invoke(this, EventArgs.Empty);

            if (IsActive)
            {
                OnIsActive();
            }
            else
            {
                OnIsNotActive();
            }
        }

        protected virtual void OnIsActive() { }

        protected virtual void OnIsNotActive() { }

        #endregion IActiveAware

        #region INavigationAware

        public virtual void OnNavigatingTo(INavigationParameters parameters) { }

        public virtual void OnNavigatedTo(INavigationParameters parameters) { }

        public virtual void OnNavigatedFrom(INavigationParameters parameters) { }

        #endregion INavigationAware

        #region IDestructible

        public virtual void Destroy() { }

        #endregion IDestructible

        #region IConfirmNavigation

        public virtual bool CanNavigate(INavigationParameters parameters) => true;

        public virtual Task<bool> CanNavigateAsync(INavigationParameters parameters) =>
            Task.FromResult(CanNavigate(parameters));

        #endregion IConfirmNavigation

        #region IApplicationLifecycleAware

        public virtual void OnResume() { }

        public virtual void OnSleep() { }

        #endregion IApplicationLifecycleAware

        #region IPageLifecycleAware

        public virtual void OnAppearing() { }

        public virtual void OnDisappearing() { }

        #endregion IPageLifecycleAware
    }
}