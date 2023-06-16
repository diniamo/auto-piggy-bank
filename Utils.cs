using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AutoPiggyBank
{
    public static class Utils
    {
        public const ulong COPPER_VALUE = 1;
        public const ulong SILVER_VALUE = 100;
        public const ulong GOLD_VALUE = 100 * 100;
        public const ulong PLATINUM_VALUE = 100 * 100 * 100;

        /// <summary>
        /// Checks if an item is a coin.
        /// </summary>
        /// <param name="type">The item's type id</param>
        /// <returns>True if yes, false if no</returns>
        public static bool IsCoin(int type)
        {
            return 71 <= type && type <= 74;
        }

        /// <summary>
        /// Calculates
        /// </summary>
        /// <param name="type">The type of coin. Must be between 71-74.</param>
        /// <param name="stack">The amount of coins</param>
        /// <returns>The value in copper coins, if type is valid, otherwise 0.</returns>
        public static ulong CalculateCoinValue(int type, uint stack)
        {
            switch (type)
            {
                case ItemID.CopperCoin:
                    return stack * COPPER_VALUE;
                case ItemID.SilverCoin:
                    return stack * SILVER_VALUE;
                case ItemID.GoldCoin:
                    return stack * GOLD_VALUE;
                case ItemID.PlatinumCoin:
                    return stack * PLATINUM_VALUE;
                default:
                    return 0;
            }
            /*
                // Alternative implementation:
            if (IsCoin(type))
            {
                return stack * (ulong)Math.Pow(100, type - 71);
            } else
            {
                return 0;
            }
            */
        }

        /// <summary>
        /// Converts a copper value to the 4 types of coins like in-game.
        /// </summary>
        /// <param name="value">The copper value</param>
        /// <returns>An Item list of coin stacks in the following format: [copper, silver, gold, platinum]</returns>
        public static List<Item> ConvertCopperValueToCoins(ulong value)
        {
            (ulong plat, ulong plat_rem) = Math.DivRem(value, PLATINUM_VALUE);
            (ulong gold, ulong gold_rem) = Math.DivRem(plat_rem, GOLD_VALUE);
            (ulong silver, ulong copper) = Math.DivRem(gold_rem, SILVER_VALUE);


            var toReturn = new List<Item>();

            if (!ModContent.GetInstance<ClientConfig>().InvertMoneyPlacementOrder)
            {
                if (copper > 0) toReturn.Add(new Item(ItemID.CopperCoin, (int)copper));
                if (silver > 0) toReturn.Add(new Item(ItemID.SilverCoin, (int)silver));
                if (gold > 0) toReturn.Add(new Item(ItemID.GoldCoin, (int)gold));

                while (plat > 0)
                {
                    toReturn.Add(new Item(ItemID.PlatinumCoin, Math.Min((int)plat, 999)));
                    plat -= Math.Min(plat, 999);
                }
            } else
            {
                while (plat > 0)
                {
                    toReturn.Add(new Item(ItemID.PlatinumCoin, Math.Min((int)plat, 999)));
                    plat -= Math.Min(plat, 999);
                }

                if (gold > 0) toReturn.Add(new Item(ItemID.GoldCoin, (int)gold));
                if (silver > 0) toReturn.Add(new Item(ItemID.SilverCoin, (int)silver));
                if (copper > 0) toReturn.Add(new Item(ItemID.CopperCoin, (int)copper));
            }

            return toReturn;
        }

        /// <summary>
        /// Places items into a chest in a specific way. If there is already a stack of the current item in the chest, then it replaces that stack, otherwise it places the stack into the first empty slot.
        /// </summary>
        /// <param name="chest">The chest object to put the items into</param>
        /// <param name="items">The items to put into the chest</param>
        public static void ReplaceOrPlaceIntoChest(Chest chest, List<Item> items)
        {
            var toIgnore = new List<int>();

            foreach (Item item in items)
            {
                for (int i = 0; i < chest.item.Length; i++)
                {
                    if (toIgnore.Contains(i)) continue;

                    if (chest.item[i].type == item.type)
                    {
                        chest.item[i] = item;
                        toIgnore.Add(i);
                        goto outer_end;
                    }
                }

                for (int i = 0; i < chest.item.Length; i++)
                {
                    if (toIgnore.Contains(i)) continue;
                 
                    if (chest.item[i].stack == 0)
                    {
                        chest.item[i] = item;
                        toIgnore.Add(i);
                        goto outer_end;
                    }
                }

            outer_end:;
            }
        }

        /// <summary>
        /// Checks if a list of Items contains a specific type
        /// </summary>
        /// <param name="items">The list of items</param>
        /// <param name="type">The type to check for</param>
        /// <returns>True if yes, false if no</returns>
        public static bool HasItem(Item[] items, int type)
        {
            foreach (Item item in items)
            {
                if (item.type == type) return true;
            }

            return false;
        }
    }
}