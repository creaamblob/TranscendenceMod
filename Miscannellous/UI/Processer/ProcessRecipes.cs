using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;
using TranscendenceMod.Items.Consumables.Placeables.SpaceBiome;
using TranscendenceMod.Items.Materials;
using TranscendenceMod.Items.Materials.MobDrops;
using TranscendenceMod.Items.Modifiers;
using TranscendenceMod.Items.Modifiers.Upgrades;
using TranscendenceMod.Items.Weapons.Melee;
using TranscendenceMod.Miscannellous.UI.Processer;

namespace TranscendenceMod.Miscannellous.UI.Processer
{
    public class ProcessRecipes : ModSystem
    {
        public static int[] TinOre()
        {
            int c = ItemID.CopperOre;
            int f = ItemID.Shimmerfly;

            return new int[] {
                0, f, 0, 0,
                0, c, c, f,
                f, c, c, 0,
                0, 0, f, 0,
                2
            };
        }
        public static int[] CopperOre()
        {
            int t = ItemID.TinOre;
            int f = ItemID.Shimmerfly;

            return new int[] {
                0, 0, f, 0,
                f, t, t, 0,
                0, t, t, f,
                0, f, 0, 0,
                2
            };
        }

        public static int[] LeadOre()
        {
            int i = ItemID.IronOre;
            int f = ItemID.Shimmerfly;

            return new int[] {
                0, f, 0, 0,
                0, i, i, f,
                f, i, i, 0,
                0, 0, f, 0,
                2
            };
        }
        public static int[] IronOre()
        {
            int l = ItemID.LeadOre;
            int f = ItemID.Shimmerfly;

            return new int[] {
                0, 0, f, 0,
                f, l, l, 0,
                0, l, l, f,
                0, f, 0, 0,
                2
            };
        }

        public static int[] TungstenOre()
        {
            int s = ItemID.SilverOre;
            int f = ItemID.Shimmerfly;

            return new int[] {
                0, f, 0, 0,
                0, s, s, f,
                f, s, s, 0,
                0, 0, f, 0,
                2
            };
        }
        public static int[] SilverOre()
        {
            int t = ItemID.TungstenOre;
            int f = ItemID.Shimmerfly;

            return new int[] {
                0, 0, f, 0,
                f, t, t, 0,
                0, t, t, f,
                0, f, 0, 0,
                2
            };
        }

        public static int[] PlatinumOre()
        {
            int g = ItemID.GoldOre;
            int f = ItemID.Shimmerfly;

            return new int[] {
                0, f, 0, 0,
                0, g, g, f,
                f, g, g, 0,
                0, 0, f, 0,
                2
            };
        }
        public static int[] GoldOre()
        {
            int p = ItemID.PlatinumOre;
            int f = ItemID.Shimmerfly;

            return new int[] {
                0, 0, f, 0,
                f, p, p, 0,
                0, p, p, f,
                0, f, 0, 0,
                2
            };
        }

        public static int[] SunburntAlloy()
        {
            int h = ItemID.HellstoneBar;
            int c = ModContent.ItemType<CarbonBar>();

            int s = ItemID.SunplateBlock;
            int f = ItemID.SoulofFlight;

            return new int[] {
                0, 0, 0, 0,
                s, h, h, s,
                s, c, c, s,
                f, f, f, f,
                2
            };
        }

        public static int[] Luminite()
        {
            int l = ItemID.LunarOre;
            int v = ModContent.ItemType<VoidFragment>();
            int m = ItemID.MeteoriteBar;

            return new int[] {
                0, l, l, 0,
                l, l, l, l,
                m, v, v, m,
                m, m, m, m,
                4
            };
        }


        public static int[] GalaxyAlloy()
        {
            int l = ItemID.LunarBar;
            int sb = ModContent.ItemType<SunburntAlloy>();
            int s = ItemID.FragmentSolar;
            int v = ItemID.FragmentVortex;
            int n = ItemID.FragmentNebula;
            int st = ItemID.FragmentStardust;
            int r = ModContent.ItemType<PulverizedPlanet>();

            return new int[] {
                l, l, l, l,
                l, l, l, l,
                s, v, n, st,
                sb, r, r, sb,
                1
            };
        }

        public static int[] StarcraftedAlloy()
        {
            int o = ModContent.ItemType<ShimmerChunk>();
            int sb = ModContent.ItemType<SunburntAlloy>();
            int h = ItemID.HallowedBar;
            int g = ModContent.ItemType<GalaxyAlloy>();

            return new int[] {
                0, 0, 0, 0,
                o, o, o, o,
                o, g, g, o,
                h, h, h, h,
                1
            };
        }
    }
}