using System.Reflection;
using UnityEngine;

namespace Evos
{
#if EVOS
    public class EvosAssetBundleManager: MonoBehaviour
    {
        private static EvosAssetBundleManager s_instance;
    
        private AssetBundle EvosBundle;

        private const string Path = "../Bundles/evos.bundle";
    
        public static EvosAssetBundleManager Get()
        {
            return s_instance;
        }
    
        public void Awake()
        {
            if (s_instance == null)
            {
                LoadBundle();
                s_instance = this;
            }
        }

        private void LoadBundle()
        {
            string assemblyLocation = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (assemblyLocation is null)
            {
                Log.Error("Can't load Evos bundle: Failed to get assembly location.");
                return;
            }

            EvosBundle = AssetBundle.LoadFromFile(System.IO.Path.Combine(assemblyLocation, Path));
            if (EvosBundle == null) 
            {
                Log.Error("Can't load Evos bundle: File is missing or corrupt.");
                return;
            }
        }

        public T LoadAsset<T>(string assetName) where T : Object
        {
            if (EvosBundle == null)
            {
                Log.Warning("Evos bundle is not loaded");
                return null;
            }
            
            Log.Info($"Loading evos asset {assetName} from {string.Join(",", EvosBundle.GetAllAssetNames())}");
            return EvosBundle.LoadAsset<T>(assetName);
        }
    }
#endif
}