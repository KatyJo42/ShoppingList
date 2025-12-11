using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ShoppingList.Models;

namespace ShoppingList.Views;

public partial class MainPage : ContentPage
{
    private LoginPage LP = new LoginPage();
    public MainPage()
    {
        InitializeComponent();
        this.Loaded += OnLoaded;
        LP.Unloaded += LPOnUnLoaded;

        lstData.Refreshing += delegate
        {
            LoadData();
            lstData.IsRefreshing = false;
        };
    }

    private void LPOnUnLoaded(object sender, EventArgs e)
    {
        OnAppearing1();
    }

    private void OnLoaded(object sender, EventArgs e)
    {
        OnAppearing1();
    }

    public void OnAppearing1()
    {
        if (string.IsNullOrEmpty(App.SessionKey))
        {
             Navigation.PushModalAsync( new NavigationPage(LP));
        }
        else
        {
            LoadData();
        }
       
    }

    async void Logout_OnClicked(object sender, EventArgs e)
    {
        var data = JsonConvert.SerializeObject(new UserAccount(App.SessionKey));

        var client = new HttpClient();

        await client.PostAsync(new Uri("https://joewetzel.com/fvtc/account/logout"),
            new StringContent(data, Encoding.UTF8, "application/json"));
        
        App.SessionKey = "";
        OnAppearing1();
    }

    async void AddData_OnClicked(object sender, EventArgs e)
    {
        var data = JsonConvert.SerializeObject(new UserData(App.SessionKey, txtInput.Text, null));
        var client = new HttpClient();
        var response = await client.PostAsync(new Uri("https://joewetzel.com/fvtc/account/data"),
            new StringContent(data, Encoding.UTF8, "application/json"));
        
        txtInput.Text = "";
        
        LoadData();
    }


    async void LoadData()
    {
        await Task.Delay(TimeSpan.FromSeconds(20));   
        var client = new HttpClient();
        var response = await client.GetAsync(new Uri("https://joewetzel.com/fvtc/account/data/" + App.SessionKey));
        var wsJson = response.Content.ReadAsStringAsync().Result;
        
        var UserDataObject = JsonConvert.DeserializeObject<UserDataCollection> (wsJson);

        lstData.ItemsSource = UserDataObject.UserDataItems;
    }



    async void DeleteItem_OnClicked(object sender, EventArgs e)
    {
        var dataID = ((MenuItem)sender).CommandParameter.ToString();
        var data = JsonConvert.SerializeObject(new UserData(App.SessionKey, null, dataID));

        var client = new HttpClient();
        var response = new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri("https://joewetzel.com/fvtc/account/data"),
            Content = new StringContent(data, Encoding.UTF8, "application/json")
        };

        await client.SendAsync(response);
        
        LoadData();
    }
}