using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace LabExer5
{
    [Activity(Label = "LabExer5", MainLauncher = true)]
    public class Login : Activity
    {
        EditText username, password;
        Button login;
        HttpClient client;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.login_layout);

            // Create your application here

            username = FindViewById<EditText>(Resource.Id.username);
            password = FindViewById<EditText>(Resource.Id.password);
            login = FindViewById<Button>(Resource.Id.login_button);

            login.Click += Login_Click;

            client = new HttpClient();
        }

        private void Login_Click(object sender, EventArgs e)
        {
            bool isValid = valid_user(username.Text, password.Text);
            if (!isValid)
            {
                Toast.MakeText(this, "Invalid Username/Password", ToastLength.Long).Show();
                return;
            }
            Console.WriteLine("VALID LOGIN");
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            Finish();
        }

        public bool valid_user(string username, string password)
        {
            string api = "http://192.168.68.186:8080/Lab5/login.php";
            api += $"?username={username}";
            api += $"&password={password}";

            var api_uri = new Uri(api);

            var response = client.GetAsync(api_uri).Result;
            var parsed_response = response.Content.ReadAsStringAsync().Result;

            if (String.IsNullOrEmpty(parsed_response))
            {
                return false;
            }

            return true;

        }
    }
}