namespace Sdk.RemoteConfig
{
    public static class FlagController
    {
        public static bool DoShowRewardedAds => Flags.GetInt("do_show_rewarded_ads", 1) == 1;
        public static float ChunkDepth => Flags.GetFloat("chunk_depth", 10f);
        public static bool IsAIEnabled => Flags.GetInt("is_ai_enabled", 1) == 1;
        public static int IncomeMultiplier => Flags.GetInt("income_multiplier", 10);
    }
}