using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AutoPiggyBank
{
    public static class Utils
    {
        public const ulong COPPER_VALUE = Item.copper;
        public const ulong SILVER_VALUE = Item.silver;
        public const ulong GOLD_VALUE = Item.gold;
        public const ulong PLATINUM_VALUE = Item.platinum;

        public static ulong PLATINUM_MAX_STACK = (ulong)ContentSamples.ItemsByType[ItemID.PlatinumCoin].maxStack;

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
                toReturn.Add(new Item(ItemID.CopperCoin, (int)copper));
                toReturn.Add(new Item(ItemID.SilverCoin, (int)silver));
                toReturn.Add(new Item(ItemID.GoldCoin, (int)gold));

                while (plat > 0)
                {
                    ulong used = Math.Min(plat, PLATINUM_MAX_STACK);
                    toReturn.Add(new Item(ItemID.PlatinumCoin, (int)used));
                    plat -= used;
                }
            } else
            {
                while (plat > 0)
                {
                    ulong used = Math.Min(plat, PLATINUM_MAX_STACK);
                    toReturn.Add(new Item(ItemID.PlatinumCoin, (int)used));
                    plat -= used;
                }

                toReturn.Add(new Item(ItemID.GoldCoin, (int)gold));
                toReturn.Add(new Item(ItemID.SilverCoin, (int)silver));
                toReturn.Add(new Item(ItemID.CopperCoin, (int)copper));
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