﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace OneTooCalendar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            StartInitializeApplication();
        }

        private void StartInitializeApplication()
        {
            var initTimeToken = new CancellationTokenSource(TimeSpan.FromSeconds(60)).Token;
            InitializeApplicationAsync(initTimeToken).RunCatchingFailure();
        }

        async Task InitializeApplicationAsync(CancellationToken token)
        {
            var apiService = new GoogleCalendarService();
            var connectSucceed = (await apiService.TryConnectAsync(token)) && !token.IsCancellationRequested;
            MainWindowViewModel.CurrentView = connectSucceed ? new CalendarViewModel() : new InitializationErrorViewModel(StartInitializeApplication);
        }

        public MainWindowViewModel MainWindowViewModel { get; } = new();
    }

    public static class TaskExtensions
    {
        public static T RunCatchingFailure<T>(this T task) where T : Task
        {
            if (task.IsCompleted)
            {
                if (task.IsFaulted)
                {
                    Debug.Fail(task.Exception?.Message ?? "No exception");
                }
                return task;
            }

            task.ContinueWith(
                _ => Debug.Fail(task.Exception?.Message ?? "No exception"),
                TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously
                );

            return task;
        }
    }
}
