using System;
using System.Diagnostics;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using LibraryDesktop.Context;

namespace LibraryDesktop;

public partial class LogInWindow : Window
{
    private TextBox usernameTextBox;
    private TextBox passwordBox;
    public LogInWindow()
    {
        InitializeComponent();
    }
    
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        usernameTextBox = this.FindControl<TextBox>("UsernameTextBox");
        passwordBox = this.FindControl<TextBox>("PasswordBox");
    }
    
    private void Login_Click(object sender, RoutedEventArgs e)
    {
        // Implement your login logic here using EF Core to check credentials
        string username = usernameTextBox.Text;
        string password = passwordBox.Text;

        // If login is successful, navigate to the main window
        if (AuthenticateUser(username, password))
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
        else
        {
            // Display an error message or handle failed login
        }
    }
    
    private bool AuthenticateUser(string username, string password)
    {
        try
        {
            
            var _context = new AppDbContext();
            var user = _context.Users.Single(u => u.UserName == username);
            
            
            
            

            if (user!=null)
            {
                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.Password);

                return isPasswordValid;
            }

            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine("An error ocurred during log in: " + e.Message);
            
            return false;
        }
    }
}