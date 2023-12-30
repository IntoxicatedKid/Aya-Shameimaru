using HarmonyLib;

namespace AyaShameimaru
{
    public static class PluginInfo
    {
        // each loaded plugin needs to have a unique GUID. usually author+generalCategory+Name is good enough
        public const string GUID = "intoxicatedkid.ayashameimaru";
        public const string Name = "Aya Shameimaru";
        public const string version = "0.0.8";
        public static readonly Harmony harmony = new Harmony(GUID);

    }
}
