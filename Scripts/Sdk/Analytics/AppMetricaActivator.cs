using Io.AppMetrica;
using Sdk.Saving;

namespace Sdk.Analytics
{
    public static class AppMetricaActivator
    {
        public static void Init(AppMetricaConfig config) 
        {
            config.FirstActivationAsUpdate = !IsFirstLaunch();
            AppMetrica.Activate(config);
        }

        private static bool IsFirstLaunch()
        {
            var isFirstLaunch = SavesFacade.IsFirstLaunch;
            SavesFacade.IsFirstLaunch = false;
            return isFirstLaunch;
        }
    }

 
}