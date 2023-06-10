using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace AutoPiggyBank
{
    public class AutoPiggyBankClientConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;


        [Label("Auto Piggy Bank Toggle")]
        [Tooltip("Toggles the mod's functionality for the current player")]
        [DefaultValue(true)]
        public bool AutoPiggyBankToggle;

        [Label("Trigger On Money Trough Interact")]
        [Tooltip("If enabled, the mod only puts the coins into the player's piggy once they interact with the Money Trough item")]
        [DefaultValue(false)]
        public bool TriggerOnPiggyBankOpen;
    }

    public class AutoPiggyBankServerConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Label("Require Money Trough")]
        [Tooltip("If enabled, the player has to have to have a Money Trough in their inventory for the mod to take effect")]
        [DefaultValue(true)]
        public bool RequireMoneyTrough;
    }
}
