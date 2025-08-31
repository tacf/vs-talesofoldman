using Vintagestory.API.Common;

namespace TalesOfOldMan
{
    class ModConfig
    {
        static ModConfig()
        {
        }

        private ModConfig()
        {
        }
        
        
        public static ModConfig Instance { get; set; } = new();

        /// <summary>
        /// Item Highlighter Config.
        /// </summary>
        public ItemHighlighterConfig ItemHighlighter { get; set; } = new();


        /// <summary>
        /// Configure several item interaction properties.
        /// </summary>
        public WorldInteractionsConfigs ItemInteractions { get; set; } = new();

        public static  void Load(ICoreAPI api, string configFilePath)
        {
            try
            {
                ModConfig modConfig;
                if ((modConfig = api.LoadModConfig<ModConfig>(configFilePath)) == null)
                {
                    api.StoreModConfig(Instance, configFilePath);
                }
                else
                {
                    Instance = modConfig;
                }
            }
            catch
            {
                api.StoreModConfig(Instance, configFilePath);
            }
        }

    }
}