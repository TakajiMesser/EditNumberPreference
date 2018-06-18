using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Messert.Controls.Droid;
using Fragment = Android.App.Fragment;
using View = Android.Views.View;
using System.Collections.Generic;
using System.Linq;
using Android.Support.V4.Content;
using System.Threading.Tasks;
using Android.Graphics.Drawables;
using Android.Preferences;

namespace Sample.Fragments
{
    public class SettingsFragment : PreferenceFragment
    {
        public static SettingsFragment Instantiate() => new SettingsFragment();

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            InitializePreferences();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return base.OnCreateView(inflater, container, savedInstanceState);
        }

        private void InitializePreferences()
        {
            AddPreferencesFromResource(Resource.Layout.fragment_settings);

            var exampleValue = PreferenceManager.FindPreference("example_value") as EditNumberPreference;
            exampleValue.InvalidInput += (s, args) => DisplayInputError(args.Message);

            var resetDefaults = PreferenceManager.FindPreference("reset_defaults");
            resetDefaults.PreferenceClick += ResetDefaults_PreferenceClick;

            var version = PreferenceManager.FindPreference("version");
            version.Summary = Activity.PackageManager.GetPackageInfo(Activity.PackageName, 0).VersionName;
        }

        private void DisplayInputError(string message)
        {
            var alert = new Android.App.AlertDialog.Builder(Context, Resource.Style.AlertsDialogTheme)
                .SetTitle("Invalid value")
                .SetMessage(message)
                .SetCancelable(true)
                .SetNegativeButton("OK", (s, args) => { })
                .Create();

            alert.Window.SetBackgroundDrawable(ContextCompat.GetDrawable(Context, Resource.Drawable.rounded_border_dark));
            alert.Show();
        }

        private void ResetDefaults_PreferenceClick(object sender, Preference.PreferenceClickEventArgs e)
        {
            var titleView = new TextView(Context)
            {
                Text = "Reset settings to default",
                TextSize = 20,
                Gravity = GravityFlags.Center,
            };
            titleView.SetTextColor(Android.Graphics.Color.White);
            titleView.SetPadding(5, 5, 5, 2);
            titleView.SetCompoundDrawablesWithIntrinsicBounds(Resource.Drawable.ic_menu_notifications, 0, 0, 0);

            var alertDialog = new Android.App.AlertDialog.Builder(Context, Resource.Style.AlertsDialogTheme)
                .SetCustomTitle(titleView)
                .SetMessage("This will reset ALL settings to default values! Are you sure?")
                .SetCancelable(true)
                .SetNegativeButton("Cancel", (s, args) => { })
                .SetPositiveButton("OK", (s, args) =>
                {
                    PreferenceManager.GetDefaultSharedPreferences(Application.Context).Edit()
                        .Clear()
                        .Commit();

                    PreferenceScreen = null;
                    InitializePreferences();
                })
                .Create();

            alertDialog.Window.SetBackgroundDrawable(ContextCompat.GetDrawable(Context, Resource.Drawable.rounded_border_dark));
            alertDialog.Show();
        }
    }
}
