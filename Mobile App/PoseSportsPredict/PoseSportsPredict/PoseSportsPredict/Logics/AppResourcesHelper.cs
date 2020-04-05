using System;
using System.Linq;
using Xamarin.Forms;

namespace PoseSportsPredict.Logics
{
    public static class AppResourcesHelper
    {
        public static T GetResource<T>(string key)
        {
            if (Application.Current.Resources.TryGetValue(key, out var value))
            {
                if (value is OnPlatform<T> onValue)
                {
                    return (T)onValue.Platforms.FirstOrDefault(p => p.Platform[0] == Device.RuntimePlatform)?.Value;
                }

                return (T)value;
            }

            throw new InvalidOperationException($"key {key} not found in the resource dictionary");
        }

        public static Color GetResourceColor(string key)
        {
            if (Application.Current.Resources.TryGetValue(key, out var value))
            {
                if (value is OnPlatform<Color> onValue)
                {
                    return (Color)onValue.Default;
                }

                return (Color)value;
            }

            throw new InvalidOperationException($"key {key} not found in the resource dictionary");
        }

        public static void SetDynamicResource(string targetResourceName, string sourceResourceName)
        {
            if (!Application.Current.Resources.TryGetValue(sourceResourceName, out var value))
            {
                throw new InvalidOperationException($"key {sourceResourceName} not found in the resource dictionary");
            }

            Application.Current.Resources[targetResourceName] = value;
        }

        public static void SetDynamicResource(this Page page, string targetResourceName, string sourceResourceName)
        {
            if (!page.Resources.TryGetValue(sourceResourceName, out var value))
            {
                throw new InvalidOperationException($"key {sourceResourceName} not found in the resource dictionary");
            }

            Application.Current.Resources[targetResourceName] = value;
        }

        public static void SetDynamicResource<T>(string targetResourceName, T value)
        {
            Application.Current.Resources[targetResourceName] = value;
        }
    }
}