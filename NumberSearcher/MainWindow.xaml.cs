using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using NumberSearcher.Annotations;

namespace NumberSearcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly LinearGradientBrush greenBrush = new LinearGradientBrush(Colors.Green,
            Colors.White,
            new Point(0.5, 0),
            new Point(0.5, 1.5));

        private static readonly LinearGradientBrush redBrush = new LinearGradientBrush(Colors.Red,
            Colors.White,
            new Point(0.5, 0),
            new Point(0.5, 1.5));

        private static List<SearchThread> threadList = new List<SearchThread>();
        private static ObservableCollection<ThreadResultArgs> results = new ObservableCollection<ThreadResultArgs>(); 
        private static int value;
        private static bool found = false;
        private int threads;
        private int interval;


        public MainWindow()
        {
            InitializeComponent();
            resultsList.ItemsSource = results;
        }

        enum ButtonState
        {
            Пуск,
            Стоп
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if ((string) StartButton.Content == nameof(ButtonState.Пуск))
                Start();
            else
                Stop();

        }

        private void Stop()
        {
            foreach (var thread in threadList)
            {
                thread.Stop();
            }
            threadList.Clear();
            StartButton.Content = nameof(ButtonState.Пуск);
        }

        private void Start()
        {
            try
            {
                found = false;
                threads = Int32.Parse(theardsNumberBox.Text);
                value = Int32.Parse(soughtValueBox.Text);
                interval = Int32.Parse(intervalBox.Text);

                if ((value > 100 && value < 0) ||
                    threads <= 0 || interval <= 0)
                    throw new FormatException();
                results.Clear();

                //Create new thread to avoid UI FREEZ
                new Thread(CreateThreads).Start();

                StartButton.Content = nameof(ButtonState.Стоп);
            }
            catch (FormatException)
            {
                MessageBox.Show("Неправильный ввод данный.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Unexpected error");
            }
        }

        private void CreateThreads()
        {
            Random rnd = new Random();
            for (int i = 0; i < threads; i++)
            {
                SearchThread st = new SearchThread(interval, "Поток " + i);
                st.ReturnHandler += OnThredsReturn;
                threadList.Add(st);
                //Threads are created too fast
                //Sleep a bit before create next thread in order to avoid results repetition of random.next
                Thread.Sleep(rnd.Next(50,100));
                st.Start();
            }
        }

        private void OnThredsReturn(object sender, ThreadResultArgs eventResultArgs)
        {
            UiInvoke(() =>
            {
                if (eventResultArgs.Result == value && !found)
                {
                    found = true;
                    Stop();
                    AddResultToList(eventResultArgs);
                    MessageBox.Show(eventResultArgs.Name + " нашёл значение", "Найдено!");
                }
                if (!found) AddResultToList(eventResultArgs);
            });
        }

        private void AddResultToList(ThreadResultArgs eventResultArgs)
        {
            eventResultArgs.Brush = eventResultArgs.Result <= value ? greenBrush : redBrush;

            results.Add(eventResultArgs);
            resultsList.Items.MoveCurrentToLast();
            resultsList.ScrollIntoView(resultsList.Items.CurrentItem);
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            Stop();
        }

        public void UiInvoke(Action action)
        {
            Application.Current.Dispatcher.Invoke(action, DispatcherPriority.Background);
        }
    }
}
