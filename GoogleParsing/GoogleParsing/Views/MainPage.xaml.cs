using GoogleParsing.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GoogleParsing.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        Task task;

        private async void Button_Clicked(object sender, EventArgs e)
        {
            string message = "Input Query String";
            Button button = sender as Button;
            if(string.IsNullOrEmpty(entry.Text))
            {
                entry.Text = message;
                return;
            }

            if (button.Text == "Google Search")
            {
                listView.ItemsSource = new List<string>() { "LOADING" };
                button.Text = "Stop";
                Reference reference = new Reference(entry.Text);
                if(task == null) task = reference.GoToGoogleAsync();
                await task;
                try
                {
                    if(!(task is null))
                    {
                        if (task.IsCompleted)
                        {
                            listView.ItemsSource = reference.SearchResult;
                            button.Text = "Google Search";
                            task = null;
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    if(!(task is null))
                    {
                        if (task.IsFaulted)
                        {
                            listView.ItemsSource = new List<string>() { $"Error - {ex.Message}" };
                            task = null;
                            return;
                        }
                    }
                }

            }
            else if (button.Text == "Stop")
            {
                listView.ItemsSource = new List<string>() { "SEARCHING WAS STOPPED" };
                button.Text = "Google Search";
                task = null;
                return;
            }
        }

    }
}
