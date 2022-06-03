using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;

namespace LabExer5
{
    [Activity(Label = "LabExer5", Theme = "@style/AppTheme")]
    public class MainActivity : AppCompatActivity
    {
        EditText name, school;
        TextView country_option;
        ImageView sortdown_img;
        RadioGroup gender_group;
        Button add, update, delete, search;

        HttpClient client;
        string selected_gender;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            name = FindViewById<EditText>(Resource.Id.name);
            school = FindViewById<EditText>(Resource.Id.school);

            gender_group = FindViewById<RadioGroup>(Resource.Id.gender_group);
            gender_group.CheckedChange += Gender_group_CheckedChange;

            country_option = FindViewById<TextView>(Resource.Id.country_option);
            country_option.Click += Country_option_Click;

            sortdown_img = FindViewById<ImageView>(Resource.Id.sortdown_img);
            sortdown_img.Click += Country_option_Click;

            add = FindViewById<Button>(Resource.Id.add_button);
            add.Click += Add_Click;

            update = FindViewById<Button>(Resource.Id.update_button);
            update.Click += Update_Click;

            delete = FindViewById<Button>(Resource.Id.delete_button);
            delete.Click += Delete_Click;

            search = FindViewById<Button>(Resource.Id.search_button);
            search.Click += Search_Click;

            client = new HttpClient();
        }

        private void Search_Click(object sender, EventArgs e)
        {
            string api = "http://192.168.68.186:8080/Lab5/search_data.php";
            api += $"?name={name.Text}";

            var api_uri = new Uri(api);

            var response = client.GetAsync(api_uri).Result;
            var parsed_response = response.Content.ReadAsStringAsync().Result;

            User_Data data;

            try
            {
                data = JsonConvert.DeserializeObject<User_Data>(parsed_response);
            }
            catch
            {
                Toast.MakeText(this, "Data Does Not Exist", ToastLength.Long).Show();
                return;
            }

            name.Text = data.Name;
            school.Text = data.School;
            gender_group.Check(GetCheckedButton(data.Gender));
            country_option.Text = data.Country;

            Toast.MakeText(this, "Search Successful", ToastLength.Long).Show();
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            string api = "http://192.168.68.186:8080/Lab5/delete_data.php";
            api += $"?name={name.Text}";

            var api_uri = new Uri(api);

            var response = client.GetAsync(api_uri).Result;
            var parsed_response = response.Content.ReadAsStringAsync().Result;

            Toast.MakeText(this, "Data Deleted In Database", ToastLength.Long).Show();
        }

        private void Update_Click(object sender, EventArgs e)
        {
            string api = "http://192.168.68.186:8080/Lab5/update_data.php";
            api += $"?name={name.Text}";
            api += $"&school={school.Text}";
            api += $"&gender={selected_gender}";
            api += $"&country={country_option.Text}";

            var api_uri = new Uri(api);

            var response = client.GetAsync(api_uri).Result;
            var parsed_response = response.Content.ReadAsStringAsync().Result;

            Toast.MakeText(this, "Data Updated To Database", ToastLength.Long).Show();
        }

        private void Add_Click(object sender, EventArgs e)
        {
            string api = "http://192.168.68.186:8080/Lab5/add_data.php";
            api += $"?name={name.Text}";
            api += $"&school={school.Text}";
            api += $"&gender={selected_gender}";
            api += $"&country={country_option.Text}";

            var api_uri = new Uri(api);

            var response = client.GetAsync(api_uri).Result;
            var parsed_response = response.Content.ReadAsStringAsync().Result;

            Toast.MakeText(this, "Data Added To Database", ToastLength.Long).Show();
        }

        private void Country_option_Click(object sender, System.EventArgs e)
        {
            PopupMenu popup_menu = new PopupMenu(this, country_option);
            popup_menu.MenuItemClick += Popup_menu_MenuItemClick;

            List<string> countries = new List<string>() { "Cambodia", "Indonesia", "Philippines", "Thailand", "Singapore" };

            for (var i = 0; i < countries.Count; i++)
            {
                popup_menu.Menu.Add(IMenu.None, i+1, i+1, countries[i]);
            }

            popup_menu.Show();
        }

        private void Popup_menu_MenuItemClick(object sender, PopupMenu.MenuItemClickEventArgs e)
        {
            var chosen_country = e.Item.TitleFormatted.ToString();
            country_option.Text = chosen_country;
        }

        private void Gender_group_CheckedChange(object sender, RadioGroup.CheckedChangeEventArgs e)
        {
            int checkedItemId = gender_group.CheckedRadioButtonId;
            RadioButton checkedRadioButton = FindViewById<RadioButton>(checkedItemId);
            selected_gender = checkedRadioButton.Text;
            gender_group.Check(checkedItemId);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private int GetCheckedButton(string text)
        {
            RadioButton nan = FindViewById<RadioButton>(Resource.Id.no_gender);
            RadioButton male = FindViewById<RadioButton>(Resource.Id.male);
            RadioButton female = FindViewById<RadioButton>(Resource.Id.female);

            List<RadioButton> buttons = new List<RadioButton> { nan, male, female };

            foreach(var button in buttons)
            {
                if (button.Text == text) { return button.Id; }
            }

            return 0;
        }
    }

    public class User_Data
    {
        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("school")]
        public string School { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

    }
}