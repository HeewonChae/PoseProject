using HtmlAgilityPack;
using Plugin.LatestVersion;
using PoseSportsPredict.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace PoseSportsPredict.Logics
{
    public static class UpdateChecker
    {
        public static async Task Execute()
        {
            try
            {
                if (await CrossLatestVersion.Current.IsUsingLatestVersion())
                    return;

                string latestVersionNumber = await CrossLatestVersion.Current.GetLatestVersionNumber();
                string installedVersionNumber = CrossLatestVersion.Current.InstalledVersionNumber;

                if (CompareVersion(latestVersionNumber, installedVersionNumber))
                {
                    var ret = await MaterialDialog.Instance.ConfirmAsync(LocalizeString.Exist_App_Update,
                        LocalizeString.App_Title,
                        LocalizeString.Update,
                        LocalizeString.Later,
                        DialogConfiguration.AppTitleAlterDialogConfiguration);

                    if (ret ?? false)
                        await CrossLatestVersion.Current.OpenAppInStore();
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private static bool CompareVersion(string latest, string current)
        {
            //major.minor.build
            string[] latestVersion = latest.Trim().Split('.');
            int latest_major = int.Parse(latestVersion[0]);
            int latest_minor = int.Parse(latestVersion[1]);

            string[] currentVersion = current.Trim().Split('.');
            int current_major = int.Parse(currentVersion[0]);
            int current_minor = int.Parse(currentVersion[1]);

            if (latest_major > current_major || latest_minor > current_minor)
                return true;

            return false;
        }
    }
}