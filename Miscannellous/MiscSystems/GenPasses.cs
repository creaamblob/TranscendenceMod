using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using TranscendenceMod.Tiles.BigTiles.Rubbles;
using TranscendenceMod.Walls.Natural;
using TranscendenceMod.Tiles;
using System;
using TranscendenceMod.Items.Consumables.Placeables;
using TranscendenceMod.Items.Modifiers;
using System.Collections.Generic;
using TranscendenceMod.Tiles.TilesheetHell.Nature;
using Terraria.DataStructures;

namespace TranscendenceMod.Miscannellous
{
    public class CavinatorEX : GenPass
    {
        public CavinatorEX(string name, double loadWeight) : base(name, loadWeight)
        {
        }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Making Caves More Open";

            //Constant Wavy Caves
            double num956 = (double)Main.maxTilesX / 4200.0;
            num956 *= num956;
            int num957 = (int)(35.0 * num956);
            if (Main.remixWorld)
            {
                num957 /= 3;
            }
            int num958 = 0;
            int num959 = 80;
            for (int num960 = 0; num960 < num957; num960++)
            {
                double num961 = (double)num960 / (double)(num957 - 1);
                progress.Set(num961);
                int num962 = WorldGen.genRand.Next((int)Main.worldSurface + 100, Main.UnderworldLayer - 100);
                int num963 = 0;
                while (Math.Abs(num962 - num958) < num959)
                {
                    num963++;
                    if (num963 > 100)
                    {
                        break;
                    }
                    num962 = WorldGen.genRand.Next((int)Main.worldSurface + 100, Main.UnderworldLayer - 100);
                }
                num958 = num962;
                int num964 = 80;
                int startX = num964 + (int)((double)(Main.maxTilesX - num964 * 2) * num961);
                try
                {
                    WorldGen.WavyCaverer(startX, num962, 3f + WorldGen.genRand.Next(3, 6), 0.25 + WorldGen.genRand.NextDouble(), WorldGen.genRand.Next(300, 500), -1);
                }
                catch
                {
                }
            }

            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0 + (int)(Main.maxTilesY * 0.4f); j < (int)(Main.maxTilesY * 0.25f); j++)
                {
                    if (Main.rand.NextBool(4)) WorldGen.Cavinator(i, j, 15);

                    if (Main.rand.NextBool(20))
                    {
                        for (int k = 0; k < 40; k++)
                        {
                            WorldGen.Cavinator(i, j + (k * 15), 15);
                        }
                    }
                }
            }
        }
    }
    public class CrateMagnets : GenPass
    {
        public CrateMagnets(string name, double loadWeight) : base(name, loadWeight)
        {
        }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Littering the Ocean";

            for (int i = 0; i < 400; i++)
            {
                for (int j = 0; j < Main.maxTilesY / 3; j++)
                {
                    Tile tile = Main.tile[i, j];
                    Tile tile2 = Main.tile[i, j + 1];
                    if (tile.LiquidAmount == 255 && tile2.TileType == TileID.Sand && tile2.HasTile && Main.rand.NextBool(50))
                    {
                        WorldGen.PlaceTile(i, j, ModContent.TileType<CrateMagnetTile>(), false, true);
                    }
                }
            }

            for (int i = Main.maxTilesX; i > (Main.maxTilesX - 400); i--)
            {
                for (int j = 0; j < Main.maxTilesY / 3; j++)
                {
                    Tile tile = Main.tile[i, j];
                    Tile tile2 = Main.tile[i, j + 1];
                    if (tile.LiquidAmount == 255 && tile2.TileType == TileID.Sand && tile2.HasTile && Main.rand.NextBool(40))
                    {
                        WorldGen.PlaceTile(i, j, ModContent.TileType<CrateMagnetTile>(), false, true);
                    }
                }
            }

            for (int index = 0; index < Main.maxChests; index++)
            {
                if (index != -1)
                {
                    Chest chest = Main.chest[index];
                    if (chest != null)
                    {
                        Tile chestTile = Main.tile[chest.x, chest.y];

                        if (chestTile.TileType == TileID.Containers && chestTile.TileFrameX == 17 * 36)
                        {
                            for (int invSlot = 0; invSlot < Chest.maxItems; invSlot++)
                            {
                                if (chest.item[invSlot].type == ItemID.None)
                                {
                                    if (Main.rand.NextBool(4))
                                        chest.item[invSlot].SetDefaults(ModContent.ItemType<OceationItem>());
                                    if (Main.rand.NextBool(3))
                                        chest.item[invSlot].SetDefaults(ModContent.ItemType<EnchantedOrb>());
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    public class SunkenCatacombGen : GenPass
    {
        public SunkenCatacombGen(string name, double loadWeight) : base(name, loadWeight)
        {
        }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Spicing up the Dungeon";

            for (int i = 5; i < (Main.maxTilesX - 5); i++)
            {
                for (int j = 5; j < (Main.maxTilesY - 5); j++)
                {
                    Tile tile = Main.tile[i, j];

                    if (tile.TileType == TileID.BlueDungeonBrick || tile.TileType == TileID.GreenDungeonBrick || tile.TileType == TileID.PinkDungeonBrick)
                        ExtraTiles();

                    void ExtraTiles()
                    {
                        if (tile.HasTile && !Main.tile[i, j - 1].HasTile && Main.rand.NextBool(12))
                        {
                            if (Main.rand.NextBool(20))
                                WorldGen.PlaceTile(i, j - 1, ModContent.TileType<FishPendantTile>());
                            if (Main.rand.NextBool(20))
                                WorldGen.PlaceTile(i, j - 1, ModContent.TileType<CrateMagnetTile>());
                        }
                    }
                }
            }
        }
    }
    public class Ores : GenPass
    {
        public Ores(string name, double loadWeight) : base(name, loadWeight)
        {
        }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Volcanic Caves";

            //Carbon Ore
            for (int i = 200; i < (Main.maxTilesX - 200); i++)
            {
                for (int j = 0 + (int)(Main.maxTilesY / 3f); j < (Main.maxTilesY - 200); j++)
                {
                    Tile tile = Main.tile[i, j];
                    if ((tile.TileType == TileID.HardenedSand && Main.rand.NextBool(225)) && tile.HasTile)
                    {
                        WorldGen.OreRunner(i, j, 7, 22, (ushort)ModContent.TileType<CarbonOreTile>());
                    }
                }
            }

            //Evasion Stones
            for (int e = 0; e < 7; e++)
            {
                WorldGen.PlaceTile(WorldGen.genRand.Next(Main.spawnTileX - 350, Main.spawnTileX + 350),
                    WorldGen.genRand.Next((int)GenVars.worldSurfaceHigh, (int)GenVars.rockLayer), ModContent.TileType<Evasium>(), false, true);
            }

            //Volcanic Cave
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = Main.maxTilesY - 400; j < (Main.maxTilesY - 50); j++)
                {
                    Tile tile = Main.tile[i, j];
                    if (tile.TileType == TileID.Stone && tile.HasTile && !Main.rand.NextBool(8))
                    {
                        WorldGen.PlaceTile(i, j, ModContent.TileType<VolcanicStone>(), false, true);
                    }

                    //Turn soft blocks into ash
                    if ((tile.TileType == TileID.Dirt || tile.TileType == TileID.Silt || tile.TileType == TileID.Mud) && tile.HasTile)
                        WorldGen.PlaceTile(i, j, TileID.Ash, false, true);

                    //Turn grass into ash variants
                    if ((tile.TileType == TileID.Grass || tile.TileType == TileID.JungleGrass || tile.TileType == TileID.MushroomGrass) && tile.HasTile)
                        WorldUtils.Gen(new Point(i, j), new Shapes.Rectangle(1, 1), new Actions.SetTileKeepWall(TileID.AshGrass));

                    //Convert ores into Carbon
                    if ((tile.TileType == TileID.Copper || tile.TileType == TileID.Tin || tile.TileType == TileID.Tungsten || tile.TileType == TileID.Silver || tile.TileType == TileID.Iron
                        || tile.TileType == TileID.Lead || tile.TileType == TileID.Gold || tile.TileType == TileID.Platinum || tile.TileType == TileID.Demonite || tile.TileType == TileID.Crimtane) && tile.HasTile)
                    {
                        WorldGen.PlaceTile(i, j, ModContent.TileType<CarbonOreTile>(), false, true);
                    }

                    //Convert gems into Ruby
                    if ((tile.TileType == TileID.Emerald || tile.TileType == TileID.Topaz || tile.TileType == TileID.Diamond
                        || tile.TileType == TileID.Amethyst || tile.TileType == TileID.Sapphire) && tile.HasTile)
                    {
                        WorldGen.PlaceTile(i, j, TileID.Ruby, false, true);
                    }

                    //Hellstone veins
                    if (tile.TileType == ModContent.TileType<VolcanicStone>() && tile.HasTile && Main.rand.NextBool(120))
                        WorldGen.OreRunner(i, j, 4, 17, TileID.Hellstone);

                    //Convert water into lava
                    if (tile.LiquidType == LiquidID.Water)
                        tile.LiquidType = LiquidID.Lava;
                }
            }
        }
    }

    public class SpaceBiomeGenPass : GenPass
    {
        public SpaceBiomeGenPass(string name, double loadWeight) : base(name, loadWeight)
        {
        }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Revealing the Cosmos... Cosmic Islands";

            int sx = (int)(Main.maxTilesX / 3.75f);
            int sy = 110;
            int spy = 360;
            TranscendenceWorld.sx = sx;
            Point spacepoint = new Point(sx, sy);

            WorldUtils.Gen(new Point(sx - 275, spy - 275), new Shapes.Rectangle(550, 170), new Actions.Clear());

            //Generate islands
            for (int i = 0; i < 10; i++)
            {
                ShapeData shape = new ShapeData();


                WorldUtils.Gen(spacepoint, new Shapes.Circle(20, 6),
                    Actions.Chain(new GenAction[]
                    {
                        new Modifiers.Offset(-15 + (i * 30), i + (int)(Math.Sin(i) * 55)),
                        new Modifiers.Dither(0.2f),
                        new Actions.Blank().Output(shape)
                    }));
                
                WorldUtils.Gen(spacepoint, new Shapes.Circle(17, 5),
                    Actions.Chain(new GenAction[]
                    {
                        new Modifiers.Offset(-15 + (i * 30), i + (int)(Math.Sin(i) * 55)),
                        new Actions.Blank().Output(shape)
                    }));



                WorldUtils.Gen(spacepoint, new Shapes.Circle(20, 6),
                    Actions.Chain(new GenAction[]
                    {
                        new Modifiers.Offset(15 - (i * 30), i + (int)(Math.Sin(i) * 55)),
                        new Modifiers.Dither(0.2f),
                        new Actions.Blank().Output(shape)
                    }));
                
                WorldUtils.Gen(spacepoint, new Shapes.Circle(17, 5),
                    Actions.Chain(new GenAction[]
                    {
                        new Modifiers.Offset(15 - (i * 30), i + (int)(Math.Sin(i) * 55)),
                        new Actions.Blank().Output(shape)
                    }));
                


                WorldUtils.Gen(spacepoint, new Shapes.Circle(12, 3),
                    Actions.Chain(new GenAction[]
                    {
                        new Modifiers.Offset(25 + (i * 22), i - (int)(Math.Sin(i / 2f) * 75)),
                        new Modifiers.Dither(0.125f),
                        new Actions.Blank().Output(shape)
                    }));

                WorldUtils.Gen(spacepoint, new Shapes.Circle(12, 3),
                    Actions.Chain(new GenAction[]
                    {
                        new Modifiers.Offset(-25 - (i * 22), i - (int)(Math.Sin(i / 2f) * 75)),
                        new Modifiers.Dither(0.125f),
                        new Actions.Blank().Output(shape)
                    }));

                ushort rockTile = (ushort)ModContent.TileType<SpaceRock>();
                WorldUtils.Gen(new Point(spacepoint.X, spacepoint.Y), new ModShapes.All(shape), new Actions.SetTileKeepWall(rockTile));
            }



            for (int a = sx - 440; a < (sx + 440); a++)
            {
                for (int b = 5; b < (spy + 155); b++)
                {
                    if (Main.tile[a, b].TileType == TileID.Dirt && !Main.tile[a, b - 1].HasTile)
                    {
                        if (!Main.tile[a - 1, b].HasTile && !Main.tile[a + 1, b].HasTile)
                        {
                            Main.tile[a, b].ClearTile();
                            WorldGen.SquareTileFrame(a, b, true);
                            NetMessage.SendTileSquare(-1, a, b, 5);
                        }
                    }
                    if (Main.tile[a, b].TileType == ModContent.TileType<SpaceRock>() && !Main.tile[a, b - 1].HasTile)
                    {
                        //Place grass
                        WorldGen.PlaceTile(a, b - 1, (ushort)ModContent.TileType<SpaceRockGrass>());
                        if (Main.rand.NextBool(2))
                        {
                            WorldUtils.Gen(new Point(a, b), new Shapes.Rectangle(1, 1),
                                new Actions.SetTileKeepWall((ushort)ModContent.TileType<SpaceRockGrass>()));
                        }
                        WorldGen.SquareTileFrame(a, b, true);
                        NetMessage.SendTileSquare(-1, a, b, 5);
                    }
                }
            }

            bool rock(int x, int y)
            {
                return Main.tile[x, y].HasTile && (Main.tile[x, y].TileType == ModContent.TileType<SpaceRock>() || Main.tile[x, y].TileType == ModContent.TileType<SpaceRockGrass>());
            }
            for (int a = sx - 320; a < (sx - 40); a++)
            {
                for (int b = 10; b < 180; b++)
                {
                    if (Main.tile[a, b + 1].HasTile && Main.tile[a, b + 2].HasTile && !Main.tile[a, b - 2].HasTile && !Main.tile[a, b - 4].HasTile && !Main.tile[a, b - 6].HasTile && Main.tile[a, b].TileType == ModContent.TileType<SpaceRockGrass>() && Main.rand.NextBool(45))
                    {
                        int dir = Main.rand.NextFromList(-1, 1);
                        for (int e = 0; e < 15; e++)
                        {
                            WorldGen.PlaceTile(a + e * dir, b + 2 - 1 - e, ModContent.TileType<SpaceCrystal>(), true, true);
                            WorldGen.PlaceTile(a + e * dir, b + 2 - e, ModContent.TileType<SpaceCrystal>(), true, true);
                            WorldGen.PlaceTile(a + e * dir, b + 2 + 1 - e, ModContent.TileType<SpaceCrystal>(), true, true);
                            WorldGen.PlaceTile(a - (1 * dir) + e * dir, b + 2 + 3 - e, ModContent.TileType<SpaceCrystal>(), true, true);
                            WorldGen.PlaceTile(a - (2 * dir) + e * dir, b + 2 + 5 - e, ModContent.TileType<SpaceCrystal>(), true, true);
                            WorldGen.PlaceTile(a - (3 * dir) + e * dir, b + 2 + 7 - e, ModContent.TileType<SpaceCrystal>(), true, true);
                            WorldGen.PlaceTile(a - (4 * dir) + e * dir, b + 2 + 9 - e, ModContent.TileType<SpaceCrystal>(), true, true);
                        }
                    }

                    int am = 0;
                    for (int i = -6; i < 6; i++)
                    {
                        if (!Main.tile[a + i, b + i].HasTile)
                        {
                            am++;
                        }

                    }

                    if (am > 10 && Main.rand.NextBool(1350))
                    {
                        WorldUtils.Gen(new Point(a, b), new Shapes.Circle(5), Actions.Chain(new GenAction[]
                        {
                                new Modifiers.RadialDither(2, 4),
                                new Actions.PlaceTile((ushort)ModContent.TileType<ModMeteorite>())
                        }));
                    }

                    if (Main.tile[a, b].HasTile && Main.tile[a, b].TileType == ModContent.TileType<SpaceRock>())
                    {
                        if (Main.rand.NextBool(14) && Main.tile[a, b + 2].HasTile && Main.tile[a + 2, b + 2].HasTile && Main.tile[a - 2, b + 2].HasTile)
                        {
                            WorldUtils.Gen(new Point(a, b + 5), new Shapes.Circle(Main.rand.Next(4, 8)), new Actions.PlaceWall((ushort)ModContent.WallType<SpaceRockWall>()));
                        }
                        if (Main.rand.NextBool(20) && !Main.tile[a, b + 1].HasTile && !Main.tile[a, b + 2].HasTile && !Main.tile[a, b + 3].HasTile && !Main.tile[a, b + 4].HasTile
                            && Main.tile[a, b - 1].HasTile && Main.tile[a, b - 2].HasTile)
                        {
                            Dictionary<ushort, int> dictionary = new Dictionary<ushort, int>();
                            WorldUtils.Gen(new Point(a, b), new Shapes.Circle(30), new Actions.TileScanner((ushort)ModContent.TileType<AetherRoot>()).Output(dictionary));
                            int rootAmount = dictionary[(ushort)ModContent.TileType<AetherRoot>()];

                            if (rootAmount < 5)
                            {
                                for (int i = 0; i < 7; i++)
                                {
                                    int size = i > 4 ? 2 : 4;
                                    WorldUtils.Gen(new Point(a + Main.rand.Next(-2, 2), b + 2 + (i * (am - 1))), new Shapes.Circle(size - 1, size + 4), new Actions.PlaceTile((ushort)ModContent.TileType<AetherRoot>()));
                                }
                            }
                        }
                    }

                    if (Main.tile[a, b].TileType == ModContent.TileType<SpaceRockGrass>())
                    {
                        //Generate a big rubble if there are 3 Space Rocks below it
                        if (Main.tile[a - 1, b].HasTile && Main.tile[a + 1, b].HasTile && Main.rand.NextBool(4))
                        {
                            int rubble = Main.rand.NextBool(3) ? ModContent.TileType<SpaceBig01Natural>() : ModContent.TileType<SpaceBig02Natural>();
                            WorldGen.PlaceTile(a, b - 1, rubble, false, true);
                        }

                        //Generate a medium rubble if there are 2 Space Rocks below it
                        if ((Main.tile[a - 1, b].HasTile || Main.tile[a + 1, b].HasTile) && Main.rand.NextBool(6))
                        {
                            int rubble = Main.rand.NextBool(3) ? ModContent.TileType<SpaceMedium01Natural>() : ModContent.TileType<SpaceMedium02Natural>();
                            WorldGen.PlaceTile(a, b - 1, rubble, false, true);
                        }
                    }
                }
            }

            for (int a = sx + 40; a < (sx + 320); a++)
            {
                for (int b = 10; b < 180; b++)
                {
                    if (Main.tile[a, b + 1].HasTile && Main.tile[a, b + 2].HasTile && !Main.tile[a, b - 2].HasTile && !Main.tile[a, b - 4].HasTile && !Main.tile[a, b - 6].HasTile && Main.tile[a, b].TileType == ModContent.TileType<SpaceRockGrass>() && Main.rand.NextBool(45))
                    {
                        int dir = Main.rand.NextFromList(-1, 1);
                        for (int e = 0; e < 15; e++)
                        {
                            WorldGen.PlaceTile(a + e * dir, b + 2 - 1 - e, ModContent.TileType<SpaceCrystal>(), true, true);
                            WorldGen.PlaceTile(a + e * dir, b + 2 - e, ModContent.TileType<SpaceCrystal>(), true, true);
                            WorldGen.PlaceTile(a + e * dir, b + 2 + 1 - e, ModContent.TileType<SpaceCrystal>(), true, true);
                            WorldGen.PlaceTile(a - (1 * dir) + e * dir, b + 2 + 3 - e, ModContent.TileType<SpaceCrystal>(), true, true);
                            WorldGen.PlaceTile(a - (2 * dir) + e * dir, b + 2 + 5 - e, ModContent.TileType<SpaceCrystal>(), true, true);
                            WorldGen.PlaceTile(a - (3 * dir) + e * dir, b + 2 + 7 - e, ModContent.TileType<SpaceCrystal>(), true, true);
                            WorldGen.PlaceTile(a - (4 * dir) + e * dir, b + 2 + 9 - e, ModContent.TileType<SpaceCrystal>(), true, true);
                        }
                    }

                    int am = 0;
                    for (int i = -6; i < 6; i++)
                    {
                        if (!Main.tile[a + i, b + i].HasTile)
                        {
                            am++;
                        }

                    }

                    if (am > 10 && Main.rand.NextBool(1350))
                    {
                        WorldUtils.Gen(new Point(a, b), new Shapes.Circle(5), Actions.Chain(new GenAction[]
                        {
                                new Modifiers.RadialDither(2, 4),
                                new Actions.PlaceTile((ushort)ModContent.TileType<ModMeteorite>())
                        }));
                    }

                    if (Main.tile[a, b].HasTile && Main.tile[a, b].TileType == ModContent.TileType<SpaceRock>())
                    {
                        if (Main.rand.NextBool(14) && Main.tile[a, b + 2].HasTile && Main.tile[a + 2, b + 2].HasTile && Main.tile[a - 2, b + 2].HasTile)
                        {
                            WorldUtils.Gen(new Point(a, b + 5), new Shapes.Circle(Main.rand.Next(4, 8)), new Actions.PlaceWall((ushort)ModContent.WallType<SpaceRockWall>()));
                        }
                        if (Main.rand.NextBool(20) && !Main.tile[a, b + 1].HasTile && !Main.tile[a, b + 2].HasTile && !Main.tile[a, b + 3].HasTile && !Main.tile[a, b + 4].HasTile
                            && Main.tile[a, b - 1].HasTile && Main.tile[a, b - 2].HasTile)
                        {
                            Dictionary<ushort, int> dictionary = new Dictionary<ushort, int>();
                            WorldUtils.Gen(new Point(a, b), new Shapes.Circle(30), new Actions.TileScanner((ushort)ModContent.TileType<AetherRoot>()).Output(dictionary));
                            int rootAmount = dictionary[(ushort)ModContent.TileType<AetherRoot>()];

                            if (rootAmount < 5)
                            {
                                for (int i = 0; i < 7; i++)
                                {
                                    int size = i > 4 ? 2 : 4;
                                    WorldUtils.Gen(new Point(a + Main.rand.Next(-2, 2), b + 2 + (i * (am - 1))), new Shapes.Circle(size - 1, size + 4), new Actions.PlaceTile((ushort)ModContent.TileType<AetherRoot>()));
                                }
                            }
                        }
                    }
                    if (Main.tile[a, b].TileType == ModContent.TileType<SpaceRockGrass>())
                    {
                        //Generate a rubble if there are 3 Space Rocks below it
                        if (Main.tile[a - 1, b].HasTile && Main.tile[a + 1, b].HasTile && Main.rand.NextBool(6))
                        {
                            int rubble = Main.rand.NextBool(3) ? ModContent.TileType<SpaceBig01Natural>() : ModContent.TileType<SpaceMedium01Natural>();
                            WorldGen.PlaceTile(a, b - 1, rubble, false, true);
                        }
                    }
                }
            }
        }
    }
    public class Temple : GenPass
    {
        public Temple(string name, double loadWeight) : base(name, loadWeight)
        {
        }

        [JITWhenModsEnabled("StructureHelper")]
        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Building the Cosmic Cathedral";
            int sx = (int)(Main.maxTilesX / 3.75f);
            int sy = 135;

            StructureHelper.API.Generator.GenerateStructure("Miscannellous/CosmicChurch", new Point16(sx - 30, (sy - 78) - 15), TranscendenceMod.Instance, false, true);
        }
    }
    public class CosmicValleyGenPass : GenPass
    {
        public CosmicValleyGenPass(string name, double loadWeight) : base(name, loadWeight)
        {
        }
        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Revealing the Cosmos... Cosmic Valley";

            int sx = (int)(Main.maxTilesX / 3.75f);
            int spy = 410;
            TranscendenceWorld.sx = sx;

            //Flattening ground in preparation for the forest below the Space Biome
            WorldUtils.Gen(new Point(sx - 400, 5), new Shapes.Rectangle(800, 490), new Actions.Clear());
            WorldUtils.Gen(new Point(sx - 440, spy), new Shapes.Rectangle(885, 180), new Actions.SetTile(TileID.Dirt));

            WorldUtils.Gen(new Point(sx - 394, spy), new Shapes.Rectangle(794, 120), new Actions.PlaceTile(TileID.Dirt));
            WorldUtils.Gen(new Point(sx - 250, spy - 150), new Shapes.Rectangle(500, 200), new Actions.Clear());

            WorldGen.SquareTileFrame(sx, spy);
            NetMessage.SendTileSquare(-1, sx, spy, 250);

            //Hills at the edge
            WorldUtils.Gen(new Point(sx + 380, spy), new Shapes.Slime(59), new Actions.SetTileKeepWall(TileID.Dirt));
            WorldUtils.Gen(new Point(sx - 380, spy), new Shapes.Slime(59), new Actions.SetTileKeepWall(TileID.Dirt));

            //The actual hole
            for (int r = 0; r < 12; r++)
            {
                float amount = 18;
                WorldUtils.Gen(new Point(sx + (350 - (r * 30)), (int)(spy - (50 - (r * (amount - (r * 1.25f)))))), new Shapes.Circle(25, 50), new Actions.Clear());
                WorldUtils.Gen(new Point(sx - (350 - (r * 30)), (int)(spy - (50 - (r * (amount - (r * 1.25)))))), new Shapes.Circle(25, 50), new Actions.Clear());
            }

            //Place rocks, seperated so that trees and life crystals don't get replaced
            for (int a = sx - 350; a < (sx + 350); a++)
            {
                for (int b = spy - 250; b < (spy + 155); b++)
                {
                    if (Main.tile[a, b].TileType == TileID.Dirt && !Main.tile[a, b - 1].HasTile)
                    {
                        if (Main.rand.NextBool(25) && Main.tile[a, b + 2].HasTile && Main.tile[a + 2, b + 1].HasTile && Main.tile[a - 2, b + 1].HasTile)
                        {
                            WorldUtils.Gen(new Point(a, b - 1), new Shapes.Circle(Main.rand.Next(4, 9)), new Actions.SetTileKeepWall(TileID.Stone));
                        }
                    }
                }
            }

            //Place water, seperated so that trees and life crystals don't get fucked up   EDIT: NOW they shouldn't fuck up EDIT: They still fuck up
            for (int a = sx - 240; a < (sx + 240); a++)
            {
                for (int b = spy - 250; b < (spy + 155); b++)
                {
                    if (Main.tile[a, b].TileType == TileID.Dirt && !Main.tile[a, b - 1].HasTile && !Main.tile[a, b - 1].CheckingLiquid)
                    {
                        if (Main.rand.NextBool(85) && Main.tile[a, b + 2].HasTile && Main.tile[a + 2, b + 1].HasTile && Main.tile[a - 2, b + 1].HasTile)
                        {
                            int rand = Main.rand.Next(7, 16);
                            WorldGen.digTunnel(a, b, 0.75f, 0.25f, 10, 7, true);
                            /*WorldUtils.Gen(new Point(a, b + 4), new Shapes.Circle(rand, 7), Actions.Chain(
                                new Modifiers.Dither(0.1f), new Actions.ClearTile()));
                            WorldUtils.Gen(new Point(a, b + 4), new Shapes.Circle(rand, 7), Actions.Chain(
                                new Modifiers.Dither(0.1f), new Actions.SetLiquid(LiquidID.Water)));*/
                        }
                    }
                }
            }


            for (int a = sx - 360; a < (sx + 360); a++)
            {
                for (int b = spy - 250; b < (spy + 155); b++)
                {
                    if (Main.tile[a, b].TileType == TileID.Stone && Main.tile[a, b - 1].LiquidAmount == 0 && !Main.tile[a + 1, b - 1].HasTile && !Main.tile[a - 1, b - 1].HasTile && !Main.tile[a, b - 1].HasTile)
                    {
                        if (Main.rand.NextBool(6)) WorldGen.AddBuriedChest(new Point(a, b - 1), 0, true, 0);
                        else if (Main.rand.NextBool(20)) WorldGen.AddLifeCrystal(a, b);
                        //WorldGen.PlaceTile(a, b - 1, TileID.Pots, true, false, -1, 1);
                    }

                    if (Main.tile[a, b + 1].TileType == TileID.Dirt && Main.tile[a, b].TileType == TileID.Grass && !Main.tile[a, b - 1].HasTile)
                    {
                        if (Main.tile[a - 1, b].HasTile && Main.tile[a + 1, b].HasTile && Main.rand.NextBool(8))
                        {
                            int rubble = Main.rand.NextBool(2) ? TileID.LargePiles2 : TileID.LargePiles;
                            WorldGen.PlaceTile(a, b - 1, rubble, false, true, -1, rubble == TileID.LargePiles2 ? Main.rand.Next(14, Main.rand.NextBool(2) ? 18 : 17) : Main.rand.Next(9, 13));
                        }

                        if (Main.rand.NextBool(2) && Main.tile[a, b + 2].HasTile && Main.tile[a + 3, b + 2].HasTile && Main.tile[a - 3, b + 2].HasTile)
                        {
                            WorldUtils.Gen(new Point(a, b + 5), new Shapes.Circle(Main.rand.Next(3, 11)), new Actions.PlaceWall(WallID.LivingLeaf));
                        }

                        if (Main.rand.NextBool(2) && !Main.tile[a + 1, b - 1].HasTile && !Main.tile[a - 1, b - 1].HasTile && !Main.tile[a, b].CheckingLiquid)
                            WorldGen.GrowTree(a, b);
                        if (Main.rand.NextBool(2) && !Main.tile[a + 1, b - 1].HasTile && !Main.tile[a - 1, b - 1].HasTile && !Main.tile[a, b].CheckingLiquid)
                            WorldGen.GrowEpicTree(a, b);
                        if (Main.rand.NextBool(4) && !Main.tile[a + 1, b - 1].HasTile && !Main.tile[a, b - 1].HasTile)
                            WorldGen.PoundTile(a, b);

                    }
                }
            }

            for (int a = sx - 360; a < (sx + 360); a++)
            {
                for (int b = spy - 250; b < (spy + 155); b++)
                {
                    //Unfuck flying chests and life crystals
                    if ((Main.tile[a, b].TileType == TileID.Containers && Main.tile[a, b].TileType == TileID.Heart) && !Main.tile[a, b + 2].HasTile)
                        WorldGen.KillTile(a, b);
                }
            }


            /*
            //Preparations for the temples
            WorldUtils.Gen(spacepoint - new Point(13, 50), new Shapes.Rectangle(26, 150), new Actions.ClearTile());
            for (int e = 0; e < 23; e++)
            {
                for (int f = 0; f < 6; f++)
                {
                    int xl = spacepoint.X - 23 + e;
                    int yl = spacepoint.Y - 15 - e - 5;

                    int xr = spacepoint.X + 22 - e;
                    int yr = spacepoint.Y - 15 - e - 5;

                    WorldGen.PlaceTile(spacepoint.X - 23 + e, spacepoint.Y - 15 - e - f, ModContent.TileType<SpaceRock>(), true, true);
                    WorldGen.SlopeTile(xl, yl, 2);
                    WorldGen.SlopeTile(xl, yl + 5, 3);
                    WorldGen.PlaceTile(spacepoint.X + 22 - e, spacepoint.Y - 15 - e - f, ModContent.TileType<SpaceRock>(), true, true);
                    WorldGen.SlopeTile(xr, yr, 1);
                    WorldGen.SlopeTile(xr, yr + 5, 4);
                }
            }

            //Walls
            WorldUtils.Gen(spacepoint - new Point(15, 25), new Shapes.Rectangle(31, 50), new Actions.SetTile(TileID.DarkCelestialBrick));
            WorldUtils.Gen(spacepoint - new Point(14, 25), new Shapes.Rectangle(29, 50), new Actions.PlaceWall(WallID.AncientBlueBrickWall));
            WorldUtils.Gen(spacepoint - new Point(13, 23), new Shapes.Rectangle(27, 46), new Actions.ClearTile());

            //Window
            WorldUtils.Gen(spacepoint - new Point(1, 9), new Shapes.Rectangle(3, 6), new Actions.PlaceWall((ushort)ModContent.WallType<SpaceRockWallUnsafe>()));
            WorldUtils.Gen(spacepoint - new Point(6, 7), new Shapes.Rectangle(2, 5), new Actions.PlaceWall((ushort)ModContent.WallType<SpaceRockWallUnsafe>()));
            WorldUtils.Gen(spacepoint - new Point(-5, 7), new Shapes.Rectangle(2, 5), new Actions.PlaceWall((ushort)ModContent.WallType<SpaceRockWallUnsafe>()));

            WorldUtils.Gen(spacepoint - new Point(1, -17), new Shapes.Rectangle(3, 6), new Actions.PlaceWall((ushort)ModContent.WallType<SpaceRockWallUnsafe>()));
            WorldUtils.Gen(spacepoint + new Point(3, 16), new Shapes.Rectangle(2, 5), new Actions.PlaceWall((ushort)ModContent.WallType<SpaceRockWallUnsafe>()));
            WorldUtils.Gen(spacepoint - new Point(4, -16), new Shapes.Rectangle(2, 5), new Actions.PlaceWall((ushort)ModContent.WallType<SpaceRockWallUnsafe>()));

            WorldUtils.Gen(spacepoint - new Point(1, 8), new Shapes.Rectangle(3, 4), new Actions.PlaceWall(WallID.RainbowStainedGlass));
            WorldUtils.Gen(spacepoint - new Point(6, 6), new Shapes.Rectangle(2, 3), new Actions.PlaceWall(WallID.RainbowStainedGlass));
            WorldUtils.Gen(spacepoint - new Point(-5, 6), new Shapes.Rectangle(2, 3), new Actions.PlaceWall(WallID.RainbowStainedGlass));

            //Bottom windows
            WorldUtils.Gen(spacepoint - new Point(1, -18), new Shapes.Rectangle(3, 4), new Actions.PlaceWall(WallID.RainbowStainedGlass));
            WorldUtils.Gen(spacepoint + new Point(3, 17), new Shapes.Rectangle(2, 3), new Actions.PlaceWall(WallID.RainbowStainedGlass));
            WorldUtils.Gen(spacepoint - new Point(4, -17), new Shapes.Rectangle(2, 3), new Actions.PlaceWall(WallID.RainbowStainedGlass));

            //Triangles above platforms
            for (int e = 0; e < 4; e++)
            {
                int xl = spacepoint.X + 2 + e;
                int xr = spacepoint.X - 2 - e;
                int y = spacepoint.Y + 5 - e;

                WorldGen.PlaceTile(xl, y, TileID.DarkCelestialBrick, true, true);
                WorldGen.PlaceTile(xl - 1, y, TileID.DarkCelestialBrick, true, true);
                WorldGen.SlopeTile(xl, y, 3);
                WorldGen.SlopeTile(xl - 1, y, 2);


                WorldGen.PlaceTile(xr, y, TileID.DarkCelestialBrick, true, true);
                WorldGen.PlaceTile(xr + 1, y, TileID.DarkCelestialBrick, true, true);
                WorldGen.SlopeTile(xr, y, 4);
                WorldGen.SlopeTile(xr + 1, y, 1);

            }

            //Smoothen the ceiling
            WorldUtils.Gen(spacepoint - new Point(16, 23), new Shapes.Rectangle(5, 2), new Actions.SetTileKeepWall((ushort)ModContent.TileType<SpaceRock>()));
            WorldUtils.Gen(spacepoint + new Point(12, -23), new Shapes.Rectangle(5, 2), new Actions.SetTileKeepWall((ushort)ModContent.TileType<SpaceRock>()));

            for (int e = 0; e < 4; e++)
            {
                int xl = spacepoint.X - 13 + e;
                int xr = spacepoint.X + 13 - e;
                int y = spacepoint.Y - 21 - e;

                WorldGen.PlaceTile(xl, y, ModContent.TileType<SpaceRock>(), true, true);
                WorldGen.SlopeTile(xl, y, 3);


                WorldGen.PlaceTile(xr, y, ModContent.TileType<SpaceRock>(), true, true);
                WorldGen.SlopeTile(xr, y, 4);
            }

            //Seperate roof and third floor
            WorldUtils.Gen(spacepoint - new Point(16, 26), new Shapes.Rectangle(32, 3), new Actions.SetTile((ushort)ModContent.TileType<SpaceRock>()));

            //Make the 3D roof dome thing
            WorldUtils.Gen(spacepoint - new Point(1, 24), new Shapes.HalfCircle(17), new Actions.PlaceWall((ushort)ModContent.WallType<SpaceRockWallUnsafe>()));
            WorldUtils.Gen(spacepoint - new Point(0, 24), new Shapes.HalfCircle(17), new Actions.PlaceWall((ushort)ModContent.WallType<SpaceRockWallUnsafe>()));
            WorldUtils.Gen(spacepoint - new Point(19, 25), new Shapes.Rectangle(38, 8), new Actions.PlaceWall((ushort)ModContent.WallType<SpaceRockWallUnsafe>()));
            WorldUtils.Gen(spacepoint - new Point(21, 18), new Shapes.Rectangle(42, 1), new Actions.PlaceWall((ushort)ModContent.WallType<SpaceRockWallUnsafe>()));

            //Make a cross shaped window with a gigantic painting in the spot that Celestial Seraph will spawn in
            WorldUtils.Gen(spacepoint - new Point(13, 19), new Shapes.Rectangle(27, 2), new Actions.PlaceWall(WallID.Crystal));
            WorldUtils.Gen(spacepoint - new Point(6, 19), new Shapes.Rectangle(13, 8), new Actions.PlaceWall(WallID.Crystal));
            WorldUtils.Gen(spacepoint - new Point(5, 18), new Shapes.Rectangle(11, 6), new Actions.PlaceWall(WallID.BlueStarryGlassWall));

            WorldUtils.Gen(spacepoint - new Point(4, 21), new Shapes.Rectangle(9, 12), new Actions.PlaceWall(WallID.Crystal));
            WorldUtils.Gen(spacepoint - new Point(3, 20), new Shapes.Rectangle(7, 10), new Actions.PlaceWall(WallID.BlueStarryGlassWall));

            WorldGen.PlaceTile(spacepoint.X - 2, spacepoint.Y - 17, ModContent.TileType<GiganticSeraphPainting>(), false, true);

            //Make platforms for Plasma Lamps
            WorldUtils.Gen(spacepoint - new Point(9, 15), new Shapes.Rectangle(4, 1), new Actions.PlaceTile(spaceplat));
            WorldUtils.Gen(spacepoint - new Point(-6, 15), new Shapes.Rectangle(4, 1), new Actions.PlaceTile(spaceplat));

            //Make an entrance
            WorldUtils.Gen(spacepoint - new Point(19, 12), new Shapes.Rectangle(38, 4), new Actions.ClearTile());

            WorldGen.PlaceTile(spacepoint.X + 7, spacepoint.Y + 22, TileID.Bookcases, false, true, -1, 35);
            WorldGen.PlaceTile(spacepoint.X - 7, spacepoint.Y + 22, TileID.Bookcases, false, true, -1, 35);

            WorldUtils.Gen(spacepoint + new Point(8, 15), new Shapes.Rectangle(2, 8), new Actions.PlaceWall(WallID.Crystal));
            WorldUtils.Gen(spacepoint + new Point(-9, 15), new Shapes.Rectangle(2, 8), new Actions.PlaceWall(WallID.Crystal));

            WorldUtils.Gen(spacepoint + new Point(8, 19), new Shapes.Rectangle(4, 1), new Actions.PlaceTile(spaceplat));
            WorldGen.PlaceTile(spacepoint.X + 10, spacepoint.Y + 18, TileID.Candelabras, false, true, -1, 37);
            WorldGen.PlaceTile(spacepoint.X + 11, spacepoint.Y + 18, TileID.Books, false, true, -1);
            Main.tile[spacepoint.X + 11, spacepoint.Y + 18].TileFrameX = 90;

            for (int e = 0; e < 7; e++)
            {
                for (int f = 0; f < 3; f++)
                {
                    WorldUtils.ClearWall(spacepoint.X - 1 + f, spacepoint.Y + 7 + e);
                    WorldGen.PlaceWall(spacepoint.X - 1 + f, spacepoint.Y + 7 + e, WallID.Crystal);

                    WorldUtils.ClearWall(spacepoint.X, spacepoint.Y + 7 + e);
                    WorldGen.PlaceWall(spacepoint.X, spacepoint.Y + 7 + e, WallID.BlueStarryGlassWall);

                    WorldUtils.ClearWall(spacepoint.X - 14 + e, spacepoint.Y + 15 - e - (f * 4));
                    WorldUtils.ClearWall(spacepoint.X - 1 - e, spacepoint.Y + 15 - e - (f * 4));

                    WorldUtils.ClearWall(spacepoint.X + 1 + e, spacepoint.Y + 15 - e - (f * 4));
                    WorldUtils.ClearWall(spacepoint.X + 14 - e, spacepoint.Y + 15 - e - (f * 4));

                    WorldGen.PlaceWall(spacepoint.X - 14 + e, spacepoint.Y + 15 - e - (f * 4), WallID.Crystal);
                    WorldGen.PlaceWall(spacepoint.X - 1 - e, spacepoint.Y + 15 - e - (f * 4), WallID.Crystal);

                    WorldGen.PlaceWall(spacepoint.X + 1 + e, spacepoint.Y + 15 - e - (f * 4), WallID.Crystal);
                    WorldGen.PlaceWall(spacepoint.X + 14 - e, spacepoint.Y + 15 - e - (f * 4), WallID.Crystal);
                }
            }

            //Seperate the first floor
            WorldUtils.Gen(spacepoint - new Point(13, -14), new Shapes.Rectangle(28, 3), new Actions.SetTileKeepWall(TileID.DarkCelestialBrick));
            WorldUtils.Gen(spacepoint - new Point(5, -16), new Shapes.Rectangle(12, 1), new Actions.ClearTile());

            WorldGen.PlaceTile(spacepoint.X - 6, spacepoint.Y + 16, TileID.DarkCelestialBrick);
            Main.tile[spacepoint.X - 6, spacepoint.Y + 16].Get<TileWallWireStateData>().Slope = SlopeType.SlopeUpLeft;
            Main.tile[spacepoint.X + 9, spacepoint.Y + 16].Get<TileWallWireStateData>().Slope = SlopeType.SlopeUpLeft;

            WorldGen.PlaceTile(spacepoint.X + 6, spacepoint.Y + 16, TileID.DarkCelestialBrick);
            Main.tile[spacepoint.X + 6, spacepoint.Y + 16].Get<TileWallWireStateData>().Slope = SlopeType.SlopeUpRight;
            Main.tile[spacepoint.X - 9, spacepoint.Y + 16].Get<TileWallWireStateData>().Slope = SlopeType.SlopeUpRight;

            for (int e = 0; e < 7; e++)
            {
                WorldGen.PlaceTile(spacepoint.X - 3 + e, spacepoint.Y + 13, TileID.DarkCelestialBrick);
                Main.tile[spacepoint.X - 3 + e, spacepoint.Y + 13].Get<TileWallWireStateData>().BlockType = BlockType.HalfBlock;
            }

            //Seperate the second floor
            WorldUtils.Gen(spacepoint - new Point(13, 0), new Shapes.Rectangle(27, 2), new Actions.PlaceTile(TileID.DarkCelestialBrick));

            WorldGen.PlaceTile(spacepoint.X + 8, spacepoint.Y - 16, TileID.PlasmaLamp, false, true);
            WorldGen.PlaceTile(spacepoint.X - 7, spacepoint.Y - 16, TileID.PlasmaLamp, false, true);

            WorldGen.PlaceTile(spacepoint.X + 6, spacepoint.Y - 16, ModContent.TileType<ExtraTerrestrialCandle>(), false, true);
            WorldGen.PlaceTile(spacepoint.X - 6, spacepoint.Y - 16, ModContent.TileType<ExtraTerrestrialCandle>(), false, true);

            //Make candles with platforms
            WorldUtils.Gen(spacepoint - new Point(3, -8), new Shapes.Rectangle(2, 1), new Actions.PlaceTile(spaceplat));
            WorldUtils.Gen(spacepoint - new Point(-2, -8), new Shapes.Rectangle(2, 1), new Actions.PlaceTile(spaceplat));

            WorldGen.PlaceTile(spacepoint.X + 2, spacepoint.Y + 7, ModContent.TileType<ExtraTerrestrialCandle>(), false, true);
            WorldGen.PlaceTile(spacepoint.X - 2, spacepoint.Y + 7, ModContent.TileType<ExtraTerrestrialCandle>(), false, true);

            WorldGen.PlaceTile(spacepoint.X + 3, spacepoint.Y + 7, TileID.Books, false, true, -1, 0);
            WorldGen.PlaceTile(spacepoint.X - 3, spacepoint.Y + 7, TileID.Books, false, true, -1, 0);

            WorldUtils.Gen(spacepoint - new Point(13, 2), new Shapes.Rectangle(27, 3), new Actions.PlaceWall(WallID.Crystal));

            WorldUtils.Gen(spacepoint + new Point(-1, 23), new Shapes.Rectangle(3, 2), new Actions.ClearTile());
            WorldUtils.Gen(spacepoint + new Point(-1, 23), new Shapes.Rectangle(3, 1), new Actions.PlaceTile(spaceplat));

            //Place the altar
            WorldGen.PlaceTile(spacepoint.X, spacepoint.Y - 1, ModContent.TileType<ShimmerAltar>(), false, true);
            WorldGen.PlaceTile(spacepoint.X - 4, spacepoint.Y + 22, TileID.Benches, false, true, -1, 40);
            WorldGen.PlaceTile(spacepoint.X + 3, spacepoint.Y + 22, TileID.Pianos, false, true, -1, 37);
            WorldGen.PlaceTile(spacepoint.X, spacepoint.Y + 17, TileID.StinkbugHousingBlocker, false, true);

            //Make platforms to the second floor
            WorldUtils.Gen(spacepoint + new Point(-12, 14), new Shapes.Rectangle(3, 3), new Actions.ClearTile());
            WorldUtils.Gen(spacepoint + new Point(10, 14), new Shapes.Rectangle(3, 3), new Actions.ClearTile());

            WorldUtils.Gen(spacepoint + new Point(-12, 14), new Shapes.Rectangle(3, 1), new Actions.PlaceTile(spaceplat));
            WorldUtils.Gen(spacepoint + new Point(10, 14), new Shapes.Rectangle(3, 1), new Actions.PlaceTile(spaceplat));

            //Make a wall in the middle of the second floor
            WorldUtils.Gen(spacepoint + new Point(-1, 0), new Shapes.Rectangle(3, 9), new Actions.SetTileKeepWall(TileID.DarkCelestialBrick));
            WorldUtils.Gen(spacepoint + new Point(0, 1), new Shapes.Rectangle(1, 7), new Actions.SetTileKeepWall(TileID.BlueStarryGlassBlock));
            WorldUtils.Gen(spacepoint - new Point(5, 0), new Shapes.Rectangle(10, 1), new Actions.SetTileKeepWall(TileID.BlueStarryGlassBlock));
            for (int i = 0; i < 10; i++)
            {
                WorldGen.SquareTileFrame(sx - 6 + i, sy - 1, true);
                NetMessage.SendTileSquare(-1, sx - 6 + i, sy - 1, 1);

                for (int e = 0; e < 3; e++)
                {
                    WorldGen.SquareTileFrame(sx - 1 + e, sy - 1 + i, true);
                    NetMessage.SendTileSquare(-1, sx - 1 + e, sy - 1 + i, 1);
                }
            }

            //Make doors
            for (int k = 0; k < 4; k++)
            {
                WorldGen.PlaceTile(spacepoint.X - 15, spacepoint.Y - 12 + k, ModContent.TileType<SpaceRock>());
                WorldGen.PlaceTile(spacepoint.X + 15, spacepoint.Y - 12 + k, ModContent.TileType<SpaceRock>());

                WorldGen.PlaceWire(spacepoint.X - 15, spacepoint.Y - 12 + k);
                WorldGen.PlaceWire(spacepoint.X + 15, spacepoint.Y - 12 + k);

                WorldGen.PlaceActuator(spacepoint.X - 15, spacepoint.Y - 12 + k);
                WorldGen.PlaceActuator(spacepoint.X + 15, spacepoint.Y - 12 + k);

                WorldGen.PlaceWire(spacepoint.X - 16 + k, spacepoint.Y - 9);
                WorldGen.PlaceWire(spacepoint.X + 16 - k, spacepoint.Y - 9);

                WorldGen.PlaceTile(spacepoint.X - 14, spacepoint.Y - 9, TileID.WeightedPressurePlate, false, true, -1, 8);
                WorldGen.PlaceTile(spacepoint.X + 14, spacepoint.Y - 9, TileID.WeightedPressurePlate, false, true, -1, 8);
                Main.tile[spacepoint.X - 14, spacepoint.Y - 9].TileFrameY = 36;
                Main.tile[spacepoint.X + 14, spacepoint.Y - 9].TileFrameY = 36;

                WorldGen.KillTile(spacepoint.X - 16, spacepoint.Y - 8 + k, false, false, true);
                WorldGen.KillTile(spacepoint.X + 16, spacepoint.Y - 8 + k, false, false, true);

                WorldGen.PlaceTile(spacepoint.X - 13, spacepoint.Y - 8 + k, TileID.DarkCelestialBrick);
                WorldGen.PlaceTile(spacepoint.X + 13, spacepoint.Y - 8 + k, TileID.DarkCelestialBrick);

                WorldGen.PlaceTile(spacepoint.X - 16, spacepoint.Y - 8 + k, TileID.DarkCelestialBrick);
                WorldGen.PlaceTile(spacepoint.X + 16, spacepoint.Y - 8 + k, TileID.DarkCelestialBrick);

                WorldGen.PlaceTile(spacepoint.X - 16, spacepoint.Y - 9, TileID.WeightedPressurePlate, false, true, -1, 8);
                WorldGen.PlaceTile(spacepoint.X + 16, spacepoint.Y - 9, TileID.WeightedPressurePlate, false, true, -1, 8);
                Main.tile[spacepoint.X - 16, spacepoint.Y - 9].TileFrameY = 36;
                Main.tile[spacepoint.X + 16, spacepoint.Y - 9].TileFrameY = 36;
            }

            WorldGen.PlaceTile(spacepoint.X - 7, spacepoint.Y + 13, TileID.Lamps, false, true, -1, 37);
            WorldGen.PlaceTile(spacepoint.X - 5, spacepoint.Y + 13, ModContent.TileType<ExtraTerrestrialPot>(), false, true);
            //Bed for literally no one (dumbass "god of the cosmos" can't even fit on it)
            WorldGen.PlaceTile(spacepoint.X + 7, spacepoint.Y + 13, TileID.Beds, false, true, -1, 37);
            WorldGen.PlaceTile(spacepoint.X + 7, spacepoint.Y + 8, TileID.Painting3X3, false, true, -1, 82);

            //Smoothen second floor walls
            WorldUtils.Gen(spacepoint - new Point(13, -1), new Shapes.Rectangle(6, 2), new Actions.ClearTile());
            WorldUtils.Gen(spacepoint + new Point(8, 1), new Shapes.Rectangle(6, 2), new Actions.ClearTile());

            WorldUtils.Gen(spacepoint - new Point(14, -1), new Shapes.Rectangle(3, 2), new Actions.SetTileKeepWall(TileID.DarkCelestialBrick));
            WorldUtils.Gen(spacepoint + new Point(12, 1), new Shapes.Rectangle(3, 2), new Actions.SetTileKeepWall(TileID.DarkCelestialBrick));

            for (int e = 0; e < 4; e++)
            {
                int xl = spacepoint.X - 13 + e;
                int xr = spacepoint.X + 13 - e;
                int y = spacepoint.Y + 3 - e;

                WorldGen.PlaceTile(xl, y, TileID.DarkCelestialBrick, true, true);
                WorldGen.SlopeTile(xl, y, 3);


                WorldGen.PlaceTile(xr, y, TileID.DarkCelestialBrick, true, true);
                WorldGen.SlopeTile(xr, y, 4);
            }

            //Make platforms to the third (altar) floor
            WorldUtils.Gen(spacepoint + new Point(-10, 0), new Shapes.Rectangle(4, 2), new Actions.ClearTile());
            WorldUtils.Gen(spacepoint + new Point(7, 0), new Shapes.Rectangle(4, 2), new Actions.ClearTile());

            WorldUtils.Gen(spacepoint + new Point(-10, 0), new Shapes.Rectangle(4, 1), new Actions.PlaceTile(spaceplat));
            WorldUtils.Gen(spacepoint + new Point(7, 0), new Shapes.Rectangle(4, 1), new Actions.PlaceTile(spaceplat));

            //Spawn him
            if (!NPC.AnyNPCs(ModContent.NPCType<LateGameNPC>()))
                NPC.NewNPC(NPC.GetSource_None(), (int)spaceNPCPos.X, (int)spaceNPCPos.Y, ModContent.NPCType<LateGameNPC>());
            /*for (int a = 0 + (Main.maxTilesX / 2); a < (Main.maxTilesX - (Main.maxTilesX / 2)); a++)
            {
                for (int b = 0; b < Main.worldSurface; b++)
                {
                    if (Main.tile[a, b + 1].TileType == TileID.Stone && Main.tile[a, b].TileType == TileID.Stone && !Main.tile[a, b - 1].HasTile)
                    {
                        if (Main.tile[a - 1, b].HasTile && Main.tile[a + 1, b].HasTile && Main.rand.NextBool(10))
                        {
                            int rubble = ModContent.TileType<HardmetalBig01>();
                            WorldGen.PlaceTile(a, b - 1, rubble, false, true);
                        }
                    }
                }
                for (int b = 0 + (Main.maxTilesY / 2); b < (Main.maxTilesY - (Main.maxTilesY / 5)); b++)
                {
                    if (Main.rand.NextBool(15550))
                    {
                        int dir = Main.rand.NextBool(2) ? 1 : -1;
                        WorldUtils.Gen(new Point(a - 1, b), new Shapes.Rectangle(7, 48), new Actions.SetTile(TileID.WoodBlock));
                        WorldUtils.Gen(new Point(a, b + 22), new Shapes.Rectangle(60 * dir, 6), new Actions.Clear());

                        WorldUtils.Gen(new Point(a + (dir * 6), b + 28), new Shapes.Rectangle(54 * dir, 2), new Actions.SetTile(TileID.Dirt));
                        WorldUtils.Gen(new Point(a + (dir * 6), b + 27), new Shapes.Rectangle(54 * dir, 1), new Actions.SetTile(TileID.MinecartTrack));

                        WorldUtils.Gen(new Point(a + (dir * -6), b + 48), new Shapes.Rectangle(54 * -dir, 2), new Actions.SetTile(TileID.Dirt));
                        WorldUtils.Gen(new Point(a + (dir * -6), b + 47), new Shapes.Rectangle(54 * -dir, 1), new Actions.SetTile(TileID.MinecartTrack));

                        WorldUtils.Gen(new Point(a, b), new Shapes.Rectangle(5, 48), new Actions.Clear());
                        WorldUtils.Gen(new Point(a - 1, b), new Shapes.Rectangle(7, 48), new Actions.PlaceWall(WallID.Planked));
                        WorldUtils.Gen(new Point(a + 2, b), new Shapes.Rectangle(1, 47), new Actions.SetTileKeepWall(TileID.Chain));
                        WorldUtils.Gen(new Point(a + 1, b + 47), new Shapes.Rectangle(3, 2), new Actions.SetTileKeepWall(TileID.WoodBlock));
                    }
                    if (Main.tile[a, b + 1].TileType == TileID.Stone && Main.tile[a, b].TileType == TileID.Stone && !Main.tile[a, b - 1].HasTile)
                    {
                        if (Main.tile[a - 1, b].HasTile && Main.tile[a + 1, b].HasTile && Main.rand.NextBool(10))
                        {
                            int rubble = ModContent.TileType<HardmetalBig01>();
                            WorldGen.PlaceTile(a, b - 1, rubble, false, true);
                        }
                    }
                }
            }*/
        }
    }
}