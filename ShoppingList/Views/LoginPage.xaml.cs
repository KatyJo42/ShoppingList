using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ShoppingList.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }
    

    private void Login_OnClicked(object sender, EventArgs e)
    {
        App.SessionKey = "asdf";
        Navigation.PopModalAsync();
    }
}