using SteamKit2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SteamUpdateBlocker {
    class AppInfo {
        private static SteamClient steamClient;
        private static SteamApps steamApps;
        private static SteamUser steamUser;
        private static CallbackManager manager;

        private static bool inLogin;
        private static bool inQuery;

        private static JobID infoRequest = JobID.Invalid;
        private string manifestId = "";
        private static bool loggedIn = false;
        private List<string> depotRequest;
        private Dictionary<string, string> manifestData;
        private static uint currentQuery = 218620;

        ~AppInfo() {
            steamUser?.LogOff();
        }

        public async Task<bool> Initialize() {
            if (loggedIn)
                return loggedIn;
            // initialize a client and a callback manager for responding to events
            steamClient = new SteamClient();
            manager = new CallbackManager(steamClient);

            // get a handler and register to callbacks we are interested in
            steamUser = steamClient.GetHandler<SteamUser>();
            steamApps = steamClient.GetHandler<SteamApps>();
            manager.Subscribe<SteamApps.PICSProductInfoCallback>(OnProductInfo);
            manager.Subscribe<SteamClient.ConnectedCallback>(OnConnected);
            manager.Subscribe<SteamClient.DisconnectedCallback>(OnDisconnected);
            manager.Subscribe<SteamUser.LoggedOnCallback>(OnLoggedOn);
            manager.Subscribe<SteamUser.LoggedOffCallback>(OnLoggedOff);

            inLogin = true;

            Debug.WriteLine("Connecting to Steam...");
            await Task.Run(() => {
                steamClient.Connect();
                while (inLogin) {
                    manager.RunWaitCallbacks(TimeSpan.FromSeconds(1));
                }
            });
            return loggedIn;
        }

        public async Task<string> TryRetrieve() {
            manifestId = null;

            // initialize a client and a callback manager for responding to events
            steamClient = new SteamClient();
            manager = new CallbackManager(steamClient);

            // get a handler and register to callbacks we are interested in
            steamUser = steamClient.GetHandler<SteamUser>();
            steamApps = steamClient.GetHandler<SteamApps>();
            manager.Subscribe<SteamApps.PICSProductInfoCallback>(OnProductInfo);
            manager.Subscribe<SteamClient.ConnectedCallback>(OnConnected);
            manager.Subscribe<SteamClient.DisconnectedCallback>(OnDisconnected);
            manager.Subscribe<SteamUser.LoggedOnCallback>(OnLoggedOn);
            manager.Subscribe<SteamUser.LoggedOffCallback>(OnLoggedOff);

            inLogin = true;

            await Task.Run(() => {
                Debug.WriteLine("Connecting to Steam...");

                steamClient.Connect();

                while (inLogin)
                    manager.RunWaitCallbacks(TimeSpan.FromSeconds(1));
            });

            return manifestId;
        }

        static void OnConnected(SteamClient.ConnectedCallback callback) {
            Debug.WriteLine("Successfully connected to Steam");
            // log on anonymously
            steamUser.LogOnAnonymous();
        }

        static void OnDisconnected(SteamClient.DisconnectedCallback callback) {
            Debug.WriteLine("Disconnected from Steam");
            inLogin = false;
        }

        static void OnLoggedOn(SteamUser.LoggedOnCallback callback) {
            if (callback.Result != EResult.OK) {
                if (callback.Result == EResult.AccountLogonDenied) {
                    // this shouldn't happen when logging in anonymously
                    Debug.WriteLine("Unable to login to Steam");
                    inLogin = false;
                    return;
                }
                // if there is another error
                Debug.WriteLine("Unable to login to Steam: {0} / {1}", callback.Result, callback.ExtendedResult);
                inLogin = false;
                return;
            }

            inLogin = false;
            loggedIn = true;
            Debug.WriteLine("Successfully logged in to Steam");
        }

        public async Task<Dictionary<string, string>> QueryDepots(string appid, List<string> installedDepots) {
            currentQuery = uint.Parse(appid);
            depotRequest = installedDepots;

            await Task.Run(() => {
                Debug.WriteLine("Quering for appid {0}", appid);
                inQuery = true;
                manifestData = null; // reset manifestData to null
                // request appid
                infoRequest = steamApps.PICSGetProductInfo(currentQuery, null);

                while (inQuery)
                    manager.RunWaitCallbacks(TimeSpan.FromSeconds(1));
            });
            // query should be done, can return manifestData
            return manifestData;
        }

        private void OnProductInfo(SteamApps.PICSProductInfoCallback callback) {
            Debug.WriteLine("In OnProductInfo callback");
            if (callback.JobID != infoRequest) {
                return;
            }
            // pass callback to GetLatestDepots
            manifestData = GetLatestDepots(callback.Apps);
            inQuery = false;
            Debug.WriteLine("Finished parsing manifest data");
        }

        // returns a dictionary where key is the depot id and the value is the latest manifest
        private Dictionary<string, string> GetLatestDepots(Dictionary<uint, SteamApps.PICSProductInfoCallback.PICSProductInfo> apps) {
            Dictionary<string, string> latestDepots = new Dictionary<string, string>();
            // iterate over all installed depots to find the latest manifest
            foreach (string depotId in depotRequest) {
                try {
                    var manifest = apps[currentQuery]?.KeyValues.Children
                    .FirstOrDefault(x => x.Name == "depots")?.Children
                    .FirstOrDefault(x => x.Name == depotId.ToString())?.Children
                    .FirstOrDefault(x => x.Name == "manifests")?.Children
                    .FirstOrDefault(x => x.Name == "public")?.Value;

                    latestDepots.Add(depotId, manifest);
                } catch (KeyNotFoundException) {
                    Debug.WriteLine("Key wasn't found for depot id {0}", depotId);
                }
            }
            return latestDepots;
        }

        static void OnLoggedOff(SteamUser.LoggedOffCallback callback) {
            Debug.WriteLine("Logged off Steam: {0}", callback.Result);
            inLogin = false;
        }
    }
}
