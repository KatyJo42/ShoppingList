using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ShoppingList.Models;

namespace ShoppingList.Views;

public partial class NewAccountPage : ContentPage
{
    public NewAccountPage()
    {
        InitializeComponent();
    }

    async void CreateAccount_OnClicked(object sender, EventArgs e)
    {
        //Do the passswords match??
        if (txtPassword1.Text!=txtPassword2.Text)
        {
            DisplayAlert("Error", "Your Passwords Don't Match!", "OK");
            return;
        }
        
        var data = JsonConvert.SerializeObject(new UserAccount(txtUser.Text, txtPassword1.Text, txtEmail.Text));

        var client = new HttpClient();
        var response = await client.PostAsync(new Uri("https://joewetzel.com/fvtc/account/createuser"),
            new StringContent(data, Encoding.UTF8, "application/json"));
        var AccountStatus = response.Content.ReadAsStringAsync().Result;
        
        
        
        //Does the User Exist
        if (AccountStatus == "user exists")
        {
            DisplayAlert("Error", "Sorry, User Exists", "OK");
            return;
        }
        
        //Does the Email Exist
        if (AccountStatus == "email exists")
        {
            DisplayAlert("Error", "Sorry, Email Exists", "OK");
            return;
        }
        
        //Login
        if (AccountStatus == "complete")
        {
            response = await client.PostAsync(new Uri("https://joewetzel.com/fvtc/account/login"),
                new StringContent(data, Encoding.UTF8, "application/json"));
            var SKey = response.Content.ReadAsStringAsync().Result;

            if (!string.IsNullOrEmpty(SKey) && SKey.Length<50)
            {
                //login
                App.SessionKey = SKey;
                Navigation.PopModalAsync();
            }
            else
            {
                DisplayAlert("Error", "Sorry, an Error Occured", "OK");
                return;
            }
        }
        else
        {
            DisplayAlert("Error", "Sorry, An Error Occured", "OK");
            return;
        }
        
        
        
        AccountStatus = AccountStatus;
        
    }
}