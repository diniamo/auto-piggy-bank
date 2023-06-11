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
        [Tooltip("If enabled, the mod only puts the coins into your piggy once you interact with the Money Trough item")]
        [DefaultValue(false)]
        public bool TriggerOnMoneyTroughOpen;

        [Label("Invert Money Placement Order")]
        [Tooltip("Inverts the order that money is placed into your piggy bank (this is only useful when using Trigger On Money Trough Open)")]
        [DefaultValue(false)]
        public bool InvertMoneyPlacementOrder;
    }

    public class AutoPiggyBankServerConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Label("Require Money Trough")]
        [Tooltip("If enabled, players have to have a Money Trough in their inventory for the mod to take effect")]
        [DefaultValue(true)]
        public bool RequireMoneyTrough;
    }
}
