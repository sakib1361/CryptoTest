﻿using System;
using UDPChat.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace UDPChat
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
#if DEBUG
            // LiveReload.Init();
#endif
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
