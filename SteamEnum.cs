using Microsoft.Win32;
using Steamfiles;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SteamUpdateBlocker {
    public class App {
        public string ManifestPath { get; }
        public OrderedDictionary Manifest { get; }

        public Dictionary<string, string> latestDepots;
        public App(string manifestPath) {
            this.ManifestPath = manifestPath;
            this.Manifest = Steamfile.LoadACF(manifestPath);
        }

        // Name of the application
        public string Name => (string)((OrderedDictionary)this.Manifest["AppState"])["name"];
        // AppID
        public string AppID => (string)((OrderedDictionary)this.Manifest["AppState"])["appid"];
        // Returns a reference to the InstalledDepots OrderedDictionary
        public OrderedDictionary DepotReference => (OrderedDictionary)((OrderedDictionary)this.Manifest["AppState"])["InstalledDepots"];

        // Returns List<string> with id's of all InstalledDepots
        public List<string> InstalledDepots {
            get {
                return new List<string>(DepotReference.Keys.Cast<string>().ToList());
            }
        }

        // Updates a depot manifest associated with the depot id
        private void UpdateDepotManifest(string depotId, string manifest) {
            ((OrderedDictionary)DepotReference[depotId])["manifest"] = manifest;
        }

        public void PreventUpdate(bool disableAutoUpdate) {

            // Update the installed depots to latest manifest
            foreach (KeyValuePair<string, string> manifest in latestDepots) {
                this.UpdateDepotManifest(manifest.Key, manifest.Value);
            }

            // Reset StateFlags and ScheduledAutoUpdate
            ((OrderedDictionary)Manifest["AppState"])["StateFlags"] = "4";
            ((OrderedDictionary)Manifest["AppState"])["ScheduledAutoUpdate"] = "0";
            ((OrderedDictionary)Manifest["AppState"])["LastUpdated"] = ((uint)DateTimeOffset.Now.ToUnixTimeSeconds()).ToString();

            // optionally disable auto update
            if (disableAutoUpdate) {
                ((OrderedDictionary)Manifest["AppState"])["AutoUpdateBehavior"] = "1";

            }
            //Steamfile.SaveACF(ManifestPath, Manifest);
            Manifest.SaveACF(ManifestPath);
        }
    }
    public class SteamEnumerator {
        private readonly string installPath;
        private readonly List<string> manifestFiles;
        private readonly AppInfo appInfo;
        public Dictionary<string, App> InstalledApps { get; }

        public App GetAppByID(string appid) => InstalledApps.Values.FirstOrDefault(x => x.AppID == appid);
        public App GetAppByID(uint appid) => InstalledApps.Values.FirstOrDefault(x => x.AppID == appid.ToString());

        public delegate void ProgressUpdate(int value);
        public event ProgressUpdate OnProgressUpdate;

        public int UpdateProgress(int i) {
            OnProgressUpdate?.Invoke(i);
            return i;
        }
        public SteamEnumerator() {
            this.appInfo = new AppInfo();
            if (Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Valve\Steam", "InstallPath", null) != null) {
                this.installPath = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Valve\Steam", "InstallPath", null);
            } else {
                this.installPath = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Valve\Steam", "InstallPath", null);
            }
            Debug.WriteLine(this.installPath);
            this.InstalledApps = new Dictionary<string, App>();
            this.manifestFiles = new List<string>();

            // get appmanifest*.acf paths from default path
            this.manifestFiles = new List<string>(Directory.GetFiles(this.installPath + @"\steamapps\", "appmanifest*.acf"));

            //OrderedDictionary libraryFolders = (OrderedDictionary)Steamfile.LoadACF(installPath + @"\steamapps\libraryfolders.vdf")["LibraryFolders"];
            OrderedDictionary libraryFolders = (OrderedDictionary)new OrderedDictionary().LoadACF(installPath + @"\steamapps\libraryfolders.vdf")["LibraryFolders"];


            // get appmanifest*.acf paths from other libraries
            foreach (string item in libraryFolders.Keys) {
                // skip anything that isn't a library path
                if (item == "TimeNextStatsReport" || item == "ContentStatsID")
                    continue;
                manifestFiles.AddRange(Directory.GetFiles(libraryFolders[item] + @"\steamapps\", "appmanifest*.acf"));
            }
        }

        public List<string> GetNames() {
            List<string> names = new List<string>(InstalledApps.Keys.ToList<string>());
            names.Sort();
            return names;
        }

        public async Task<int> ProcessManifests(IProgress<int> progress) {
            int processed = await Task.Run<int>(() => {
                int count = 0;
                foreach (string path in manifestFiles) {
                    OrderedDictionary appmanifest = Steamfile.LoadACF(path);
                    string name = (string)((OrderedDictionary)appmanifest["AppState"])["name"];
                    InstalledApps[name] = new App(path);
                    count++;
                    string progressStr = count.ToString() + "/" + manifestFiles.Count.ToString();
                    int percentage = count * 100 / manifestFiles.Count;
                    if (progress != null) {
                        progress.Report(percentage);
                    }
                    OnProgressUpdate?.Invoke(percentage);
                }
                return count;
            });

            Debug.WriteLine("Found {0} installed applications", this.InstalledApps.Keys.Count);
            return processed;
        }

        public async Task<Dictionary<string, string>> GetUpdatedManifests(string name) {
            App app = InstalledApps[name];
            await Task.Run(async () => {
                await appInfo.Initialize();
                app.latestDepots = await appInfo.QueryDepots(app.AppID, app.InstalledDepots);
            });
            return app.latestDepots;
        }

        public void PreventUpdate(string name, bool disableAutoUpdate) => InstalledApps[name].PreventUpdate(disableAutoUpdate);
    }
}
