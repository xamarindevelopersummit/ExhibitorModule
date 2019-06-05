using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Plugin.FirebasePushNotification;

namespace ExhibitorModule.Droid
{
    [Application]
    public class MainApplication : Application
    {
        protected MainApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

        public override void OnCreate()
        {
            base.OnCreate();

            //Set the default notification channel for your app when running Android Oreo 
            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                //Change for your default notification channel id here
                FirebasePushNotificationManager.DefaultNotificationChannelId = "General";

                //Change for your default notification channel name here
                FirebasePushNotificationManager.DefaultNotificationChannelName = "General";
            }


            //If debug you should reset the token each time.
#if DEBUG
              FirebasePushNotificationManager.Initialize(this, false);
#else
            FirebasePushNotificationManager.Initialize(this, false);
#endif
        }
    }
}
