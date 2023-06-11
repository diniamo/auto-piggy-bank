using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AutoPiggyBank
{
    public class AutoPiggyBankListener : GlobalItem
    {
        public override bool OnPickup(Item item, Player player)
        {
            var clientConfig = ModContent.GetInstance<AutoPiggyBankClientConfig>();
            if (!clientConfig.AutoPiggyBankToggle || clientConfig.TriggerOnMoneyTroughOpen) 
                return base.OnPickup(item, player);
            if (ModContent.GetInstance<AutoPiggyBankServerConfig>().RequireMoneyTrough && !Utils.HasItem(player.inventory, ItemID.MoneyTrough)) 
                return base.OnPickup(item, player);


            int type = item.type;

            if (Utils.IsCoin(type))
            {
                ulong totalMoney = Utils.CalculateCoinValue(type, (uint) item.stack);
                foreach (Item bItem in player.bank.item)
                {
                    totalMoney += Utils.CalculateCoinValue(bItem.type, (uint) bItem.stack);
                }

                List<Item> toPlace = Utils.ConvertCopperValueToCoins(totalMoney);
                Utils.ReplaceOrPlaceIntoChest(player.bank, toPlace);

                SoundEngine.PlaySound(SoundID.CoinPickup, player.position);
                return false;
            }


            return base.OnPickup(item, player);
        }

        public override bool? UseItem(Item item, Player player)
        {
            if (item.type != ItemID.MoneyTrough)
                return base.UseItem(item, player);

            var clientConfig = ModContent.GetInstance<AutoPiggyBankClientConfig>();
            if (!clientConfig.AutoPiggyBankToggle || !clientConfig.TriggerOnMoneyTroughOpen)
                return base.UseItem(item, player);


            ulong totalMoney = 0;
            for (int i = 0; i < player.inventory.Length; i++)
            {
                Item pItem = player.inventory[i];
                if (!Utils.IsCoin(pItem.type))
                    continue;

                totalMoney += Utils.CalculateCoinValue(pItem.type, (uint) pItem.stack);
                player.inventory[i].TurnToAir();

            }
            foreach (Item bItem in player.bank.item)
            {
                totalMoney += Utils.CalculateCoinValue(bItem.type, (uint) bItem.stack);
            }

            if (totalMoney == 0)
                return base.UseItem(item, player);

            List<Item> toPlace = Utils.ConvertCopperValueToCoins(totalMoney);
            Utils.ReplaceOrPlaceIntoChest(player.bank, toPlace);


            return base.UseItem(item, player);
        }
    }
}
