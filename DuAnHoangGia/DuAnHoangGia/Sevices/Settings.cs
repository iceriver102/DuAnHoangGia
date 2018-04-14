using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace DuAnHoangGia.Sevices
{
    public class Settings : INotifyPropertyChanged
    {
        private static Lazy<Settings> SettingsInstance = new Lazy<Settings>(() => new Settings());

        public static Settings Current => SettingsInstance.Value;

        private Settings()
        {
        }

        private static ISettings AppSettings
        {
            get { return CrossSettings.Current; }
        }

        public void Clear()
        {
            AppSettings.Clear();
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            if (!string.IsNullOrWhiteSpace(propertyName))
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        bool SetProperty<T>(T value, [CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrWhiteSpace(propertyName)) return false;
            switch (value)
            {
                case int i:
                    AppSettings.AddOrUpdateValue(propertyName, i);
                    break;
                case float i:
                    AppSettings.AddOrUpdateValue(propertyName, i);
                    break;
                case double i:
                    AppSettings.AddOrUpdateValue(propertyName, i);
                    break;
                case bool b:
                    AppSettings.AddOrUpdateValue(propertyName, b);
                    break;
                case string s:
                    AppSettings.AddOrUpdateValue(propertyName, s);
                    break;
                default:
                    string json = JsonConvert.SerializeObject(value);
                    if (json == AppSettings.GetValueOrDefault(propertyName, "")) return false;
                    AppSettings.AddOrUpdateValue(propertyName, json);
                    break;
            }
            RaisePropertyChanged(propertyName);
            return true;
        }


        #endregion INotifyPropertyChanged

        T GetProperty<T>(T defaultValue, [CallerMemberName]string propertyName = null)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                return defaultValue;
            switch (defaultValue)
            {
                case int i:
                    return (T)Convert.ChangeType(AppSettings.GetValueOrDefault(propertyName, i), typeof(T));
                case double i:
                    return (T)Convert.ChangeType(AppSettings.GetValueOrDefault(propertyName, i), typeof(T));
                case bool b:
                    return (T)Convert.ChangeType(AppSettings.GetValueOrDefault(propertyName, b), typeof(T));
                case string s:
                    return (T)Convert.ChangeType(AppSettings.GetValueOrDefault(propertyName, s), typeof(T));
                default:
                    string json = JsonConvert.SerializeObject(defaultValue);
                    string result = AppSettings.GetValueOrDefault(propertyName, json);
                    return JsonConvert.DeserializeAnonymousType<T>(result, defaultValue);
            }
        }
        public string UserName
        {
            get { return GetProperty<string>(""); }
            set { SetProperty(value); }
        }

        public string Token
        {
            get { return GetProperty<string>(""); }
            set { SetProperty(value); }
        }

        public int IDProfile
        {
            get
            {
                return GetProperty<int>(-1);
            }
            set
            {
                SetProperty(value);
            }
        }

        public string CacheUser
        {
            get { return GetProperty<string>(""); }
            set { SetProperty(value); }
        }
        public (double lat, double log) Position { get => this.GetProperty<(double lat, double log)>((lat: -1, log: -1)); set => SetProperty(value); }
        public bool Auto
        {
            get { return GetProperty<bool>(false); }
            set { SetProperty(value); }
        }
    }
}
