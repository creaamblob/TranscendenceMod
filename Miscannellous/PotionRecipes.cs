using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Items.Materials;

namespace TranscendenceMod
{
    public class PotionRecipes : ModSystem
    {
        public override void AddRecipes()
        {
            //Why the fuck I didn't just make a helper method to make these faster???
            Recipe transgernder = Recipe.Create(ItemID.GenderChangePotion);
            transgernder.AddIngredient(ModContent.ItemType<FlowerEssence>());
            transgernder.AddIngredient(ItemID.BottledWater);
            transgernder.AddTile(TileID.AlchemyTable);
            transgernder.DisableDecraft();
            transgernder.Register();

            Recipe rage = Recipe.Create(ItemID.RagePotion, 6);
            rage.AddIngredient(ModContent.ItemType<FlowerEssence>());
            rage.AddIngredient(ItemID.Hemopiranha, 3);
            rage.AddIngredient(ItemID.BottledWater);
            rage.AddTile(TileID.AlchemyTable);
            rage.DisableDecraft();
            rage.Register();

            Recipe wrath = Recipe.Create(ItemID.WrathPotion, 6);
            wrath.AddIngredient(ModContent.ItemType<FlowerEssence>());
            wrath.AddIngredient(ItemID.Ebonkoi, 3);
            wrath.AddIngredient(ItemID.BottledWater);
            wrath.AddTile(TileID.AlchemyTable);
            wrath.DisableDecraft();
            wrath.Register();

            Recipe ironskin = Recipe.Create(ItemID.IronskinPotion, 6);
            ironskin.AddIngredient(ModContent.ItemType<FlowerEssence>());
            ironskin.AddIngredient(ItemID.IronBar, 2);
            ironskin.AddIngredient(ItemID.BottledWater);
            ironskin.AddTile(TileID.AlchemyTable);
            ironskin.DisableDecraft();
            ironskin.Register();
            Recipe leadskin = Recipe.Create(ItemID.IronskinPotion, 6);
            leadskin.AddIngredient(ModContent.ItemType<FlowerEssence>());
            leadskin.AddIngredient(ItemID.LeadBar, 2);
            leadskin.AddIngredient(ItemID.BottledWater);
            leadskin.AddTile(TileID.AlchemyTable);
            leadskin.DisableDecraft();
            leadskin.Register();

            Recipe regeneration = Recipe.Create(ItemID.RegenerationPotion, 6);
            regeneration.AddIngredient(ModContent.ItemType<FlowerEssence>());
            regeneration.AddIngredient(ItemID.Mushroom, 3);
            regeneration.AddIngredient(ItemID.BottledWater);
            regeneration.AddTile(TileID.AlchemyTable);
            regeneration.DisableDecraft();
            regeneration.Register();

            Recipe gills = Recipe.Create(ItemID.GillsPotion, 6);
            gills.AddIngredient(ModContent.ItemType<FlowerEssence>());
            gills.AddIngredient(ItemID.Coral, 3);
            gills.AddIngredient(ItemID.BottledWater);
            gills.AddTile(TileID.AlchemyTable);
            gills.DisableDecraft();
            gills.Register();

            Recipe hunter = Recipe.Create(ItemID.HunterPotion, 6);
            hunter.AddIngredient(ModContent.ItemType<FlowerEssence>());
            hunter.AddIngredient(ItemID.SharkFin, 3);
            hunter.AddIngredient(ItemID.BottledWater);
            hunter.AddTile(TileID.AlchemyTable);
            hunter.DisableDecraft();
            hunter.Register();

            Recipe waterwalking = Recipe.Create(ItemID.WaterWalkingPotion, 6);
            waterwalking.AddIngredient(ModContent.ItemType<FlowerEssence>());
            waterwalking.AddIngredient(ItemID.Salmon, 3);
            waterwalking.AddIngredient(ItemID.BottledWater);
            waterwalking.AddTile(TileID.AlchemyTable);
            waterwalking.DisableDecraft();
            waterwalking.Register();

            Recipe archery = Recipe.Create(ItemID.ArcheryPotion, 6);
            archery.AddIngredient(ModContent.ItemType<FlowerEssence>());
            archery.AddIngredient(ItemID.Lens, 3);
            archery.AddIngredient(ItemID.BottledWater);
            archery.AddTile(TileID.AlchemyTable);
            archery.DisableDecraft();
            archery.Register();

            Recipe thorns = Recipe.Create(ItemID.ThornsPotion, 6);
            thorns.AddIngredient(ModContent.ItemType<FlowerEssence>());
            thorns.AddIngredient(ItemID.Cactus, 3);
            thorns.AddIngredient(ItemID.BottledWater);
            thorns.AddTile(TileID.AlchemyTable);
            thorns.DisableDecraft();
            thorns.Register();

            Recipe invisibility = Recipe.Create(ItemID.InvisibilityPotion, 6);
            invisibility.AddIngredient(ModContent.ItemType<FlowerEssence>());
            invisibility.AddIngredient(ItemID.FlinxFur);
            invisibility.AddIngredient(ItemID.BottledWater);
            invisibility.AddTile(TileID.AlchemyTable);
            invisibility.DisableDecraft();
            invisibility.Register();

            Recipe shine = Recipe.Create(ItemID.ShinePotion, 6);
            shine.AddIngredient(ModContent.ItemType<FlowerEssence>());
            shine.AddIngredient(ItemID.GlowingMushroom, 3);
            shine.AddIngredient(ItemID.BottledWater);
            shine.AddTile(TileID.AlchemyTable);
            shine.DisableDecraft();
            shine.Register();

            Recipe nightowl = Recipe.Create(ItemID.NightOwlPotion, 6);
            nightowl.AddIngredient(ModContent.ItemType<FlowerEssence>());
            nightowl.AddIngredient(ItemID.Lens);
            nightowl.AddIngredient(ItemID.BottledWater);
            nightowl.AddTile(TileID.AlchemyTable);
            nightowl.DisableDecraft();
            nightowl.Register();

            Recipe battle = Recipe.Create(ItemID.BattlePotion, 6);
            battle.AddIngredient(ModContent.ItemType<FlowerEssence>());
            battle.AddIngredient(ItemID.RottenChunk, 3);
            battle.AddIngredient(ItemID.BottledWater);
            battle.AddTile(TileID.AlchemyTable);
            battle.DisableDecraft();
            battle.Register();
            Recipe battle2 = Recipe.Create(ItemID.BattlePotion, 6);
            battle2.AddIngredient(ModContent.ItemType<FlowerEssence>());
            battle2.AddIngredient(ItemID.Vertebrae, 3);
            battle2.AddIngredient(ItemID.BottledWater);
            battle2.AddTile(TileID.AlchemyTable);
            battle2.DisableDecraft();
            battle2.Register();

            Recipe calming = Recipe.Create(ItemID.CalmingPotion, 6);
            calming.AddIngredient(ModContent.ItemType<FlowerEssence>());
            calming.AddIngredient(ItemID.Damselfish, 3);
            calming.AddIngredient(ItemID.BottledWater);
            calming.AddTile(TileID.AlchemyTable);
            calming.DisableDecraft();
            calming.Register();

            Recipe spelunker = Recipe.Create(ItemID.SpelunkerPotion, 6);
            spelunker.AddIngredient(ModContent.ItemType<FlowerEssence>());
            spelunker.AddIngredient(ItemID.Emerald);
            spelunker.AddIngredient(ItemID.Sapphire);
            spelunker.AddIngredient(ItemID.BottledWater);
            spelunker.AddTile(TileID.AlchemyTable);
            spelunker.DisableDecraft();
            spelunker.Register();

            Recipe obsidianskin = Recipe.Create(ItemID.ObsidianSkinPotion, 6);
            obsidianskin.AddIngredient(ModContent.ItemType<FlowerEssence>());
            obsidianskin.AddIngredient(ItemID.Obsidian, 5);
            obsidianskin.AddIngredient(ItemID.BottledWater);
            obsidianskin.AddTile(TileID.AlchemyTable);
            obsidianskin.DisableDecraft();
            obsidianskin.Register();

            Recipe manaregen = Recipe.Create(ItemID.ManaRegenerationPotion, 6);
            manaregen.AddIngredient(ModContent.ItemType<FlowerEssence>());
            manaregen.AddIngredient(ItemID.FallenStar, 4);
            manaregen.AddIngredient(ItemID.BottledWater);
            manaregen.AddTile(TileID.AlchemyTable);
            manaregen.DisableDecraft();
            manaregen.Register();
            Recipe manapower = Recipe.Create(ItemID.MagicPowerPotion, 6);
            manapower.AddIngredient(ModContent.ItemType<FlowerEssence>());
            manapower.AddIngredient(ItemID.FallenStar, 3);
            manapower.AddIngredient(ItemID.BottledWater);
            manapower.AddTile(TileID.AlchemyTable);
            manapower.DisableDecraft();
            manapower.Register();

            Recipe featherfall = Recipe.Create(ItemID.FeatherfallPotion, 6);
            featherfall.AddIngredient(ModContent.ItemType<FlowerEssence>());
            featherfall.AddIngredient(ItemID.Feather, 3);
            featherfall.AddIngredient(ItemID.BottledWater);
            featherfall.AddTile(TileID.AlchemyTable);
            featherfall.DisableDecraft();
            featherfall.Register();

            Recipe gravity = Recipe.Create(ItemID.GravitationPotion, 6);
            gravity.AddIngredient(ModContent.ItemType<FlowerEssence>());
            gravity.AddIngredient(ItemID.RainCloud, 3);
            gravity.AddIngredient(ItemID.BottledWater);
            gravity.AddTile(TileID.AlchemyTable);
            gravity.DisableDecraft();
            gravity.Register();

            Recipe fishing = Recipe.Create(ItemID.FishingPotion, 6);
            fishing.AddIngredient(ModContent.ItemType<FlowerEssence>());
            fishing.AddIngredient(ItemID.CrispyHoneyBlock, 3);
            fishing.AddIngredient(ItemID.BottledWater);
            fishing.AddTile(TileID.AlchemyTable);
            fishing.DisableDecraft();
            fishing.Register();

            Recipe crate = Recipe.Create(ItemID.CratePotion, 6);
            crate.AddIngredient(ModContent.ItemType<FlowerEssence>());
            crate.AddIngredient(ItemID.Amber, 6);
            crate.AddIngredient(ItemID.BottledWater);
            crate.AddTile(TileID.AlchemyTable);
            crate.DisableDecraft();
            crate.Register();

            Recipe builder = Recipe.Create(ItemID.BuilderPotion, 6);
            builder.AddIngredient(ModContent.ItemType<FlowerEssence>());
            builder.AddIngredient(ItemID.RedBrick, 24);
            builder.AddIngredient(ItemID.BottledWater);
            builder.AddTile(TileID.AlchemyTable);
            builder.DisableDecraft();
            builder.Register();

            Recipe titan = Recipe.Create(ItemID.TitanPotion, 6);
            titan.AddIngredient(ModContent.ItemType<FlowerEssence>());
            titan.AddIngredient(ItemID.Bone, 12);
            titan.AddIngredient(ItemID.BottledWater);
            titan.AddTile(TileID.AlchemyTable);
            titan.DisableDecraft();
            titan.Register();

            Recipe summoning = Recipe.Create(ItemID.SummoningPotion, 6);
            summoning.AddIngredient(ModContent.ItemType<FlowerEssence>());
            summoning.AddIngredient(ItemID.SpiderFang, 6);
            summoning.AddIngredient(ItemID.BottledWater);
            summoning.AddTile(TileID.AlchemyTable);
            summoning.DisableDecraft();
            summoning.Register();

            Recipe heartreach = Recipe.Create(ItemID.HeartreachPotion, 6);
            heartreach.AddIngredient(ModContent.ItemType<FlowerEssence>());
            heartreach.AddIngredient(ItemID.Vine, 3);
            heartreach.AddIngredient(ItemID.BottledWater);
            heartreach.AddTile(TileID.AlchemyTable);
            heartreach.DisableDecraft();
            heartreach.Register();

            Recipe flipper = Recipe.Create(ItemID.FlipperPotion, 6);
            flipper.AddIngredient(ModContent.ItemType<FlowerEssence>());
            flipper.AddIngredient(ItemID.Starfish, 3);
            flipper.AddIngredient(ItemID.BottledWater);
            flipper.AddTile(TileID.AlchemyTable);
            flipper.DisableDecraft();
            flipper.Register();

            Recipe mining = Recipe.Create(ItemID.MiningPotion, 6);
            mining.AddIngredient(ModContent.ItemType<FlowerEssence>());
            mining.AddIngredient(ModContent.ItemType<CarbonBar>(), 6);
            mining.AddIngredient(ItemID.BottledWater);
            mining.AddTile(TileID.AlchemyTable);
            mining.DisableDecraft();
            mining.Register();

            Recipe sonar = Recipe.Create(ItemID.SonarPotion, 6);
            sonar.AddIngredient(ModContent.ItemType<FlowerEssence>());
            sonar.AddIngredient(ItemID.AtlanticCod, 3);
            sonar.AddIngredient(ItemID.BottledWater);
            sonar.AddTile(TileID.AlchemyTable);
            sonar.DisableDecraft();
            sonar.Register();

            Recipe dangersense = Recipe.Create(ItemID.TrapsightPotion, 6);
            dangersense.AddIngredient(ModContent.ItemType<FlowerEssence>());
            dangersense.AddIngredient(ItemID.AntlionMandible, 3);
            dangersense.AddIngredient(ItemID.BottledWater);
            dangersense.AddTile(TileID.AlchemyTable);
            dangersense.DisableDecraft();
            dangersense.Register();

            Recipe inferno = Recipe.Create(ItemID.InfernoPotion, 6);
            inferno.AddIngredient(ModContent.ItemType<FlowerEssence>());
            inferno.AddIngredient(ItemID.HellstoneBar, 12);
            inferno.AddIngredient(ItemID.BottledWater);
            inferno.AddTile(TileID.AlchemyTable);
            inferno.DisableDecraft();
            inferno.Register();

            Recipe endurance = Recipe.Create(ItemID.EndurancePotion, 6);
            endurance.AddIngredient(ModContent.ItemType<FlowerEssence>());
            endurance.AddIngredient(ItemID.SoulofMight, 3);
            endurance.AddIngredient(ItemID.BottledWater);
            endurance.AddTile(TileID.AlchemyTable);
            endurance.DisableDecraft();
            endurance.Register();

            Recipe lifeforce = Recipe.Create(ItemID.LifeforcePotion, 6);
            lifeforce.AddIngredient(ModContent.ItemType<FlowerEssence>());
            lifeforce.AddIngredient(ItemID.LifeCrystal, 3);
            lifeforce.AddIngredient(ItemID.BottledWater);
            lifeforce.AddTile(TileID.AlchemyTable);
            lifeforce.DisableDecraft();
            lifeforce.Register();

            Recipe ammo = Recipe.Create(ItemID.AmmoReservationPotion, 6);
            ammo.AddIngredient(ModContent.ItemType<FlowerEssence>());
            ammo.AddIngredient(ItemID.DoubleCod, 3);
            ammo.AddIngredient(ItemID.BottledWater);
            ammo.AddTile(TileID.AlchemyTable);
            ammo.DisableDecraft();
            ammo.Register();

            Recipe teleportation = Recipe.Create(ItemID.TeleportationPotion, 6);
            teleportation.AddIngredient(ModContent.ItemType<FlowerEssence>());
            teleportation.AddIngredient(ItemID.SpecularFish, 6);
            teleportation.AddIngredient(ItemID.BottledWater);
            teleportation.AddTile(TileID.AlchemyTable);
            teleportation.DisableDecraft();
            teleportation.Register();

            Recipe warmth = Recipe.Create(ItemID.WarmthPotion, 6);
            warmth.AddIngredient(ModContent.ItemType<FlowerEssence>());
            warmth.AddIngredient(ItemID.FlinxFur, 2);
            warmth.AddIngredient(ItemID.BottledWater);
            warmth.AddTile(TileID.AlchemyTable);
            warmth.DisableDecraft();
            warmth.Register();

            Recipe biomesight = Recipe.Create(ItemID.BiomeSightPotion, 6);
            biomesight.AddIngredient(ModContent.ItemType<FlowerEssence>());
            biomesight.AddIngredient(ItemID.CorruptSeeds, 6);
            biomesight.AddIngredient(ItemID.BottledWater);
            biomesight.AddTile(TileID.AlchemyTable);
            biomesight.DisableDecraft();
            biomesight.Register();
            Recipe biomesight2 = Recipe.Create(ItemID.BiomeSightPotion, 6);
            biomesight2.AddIngredient(ModContent.ItemType<FlowerEssence>());
            biomesight2.AddIngredient(ItemID.CrimsonSeeds, 6);
            biomesight2.AddIngredient(ItemID.BottledWater);
            biomesight2.AddTile(TileID.AlchemyTable);
            biomesight2.DisableDecraft();
            biomesight2.Register();

            Recipe luck1 = Recipe.Create(ItemID.LuckPotionLesser, 6);
            luck1.AddIngredient(ModContent.ItemType<FlowerEssence>());
            luck1.AddIngredient(ItemID.WhitePearl, 2);
            luck1.AddIngredient(ItemID.BottledWater);
            luck1.AddTile(TileID.AlchemyTable);
            luck1.DisableDecraft();
            luck1.Register();

            Recipe luck2 = Recipe.Create(ItemID.LuckPotion, 6);
            luck2.AddIngredient(ModContent.ItemType<FlowerEssence>());
            luck2.AddIngredient(ItemID.WhitePearl, 2);
            luck2.AddIngredient(ItemID.BottledWater);
            luck2.AddTile(TileID.AlchemyTable);
            luck2.DisableDecraft();
            luck2.Register();

            Recipe luck3 = Recipe.Create(ItemID.LuckPotionGreater, 6);
            luck3.AddIngredient(ModContent.ItemType<FlowerEssence>());
            luck3.AddIngredient(ItemID.PinkPearl, 2);
            luck3.AddIngredient(ItemID.BottledWater);
            luck3.AddTile(TileID.AlchemyTable);
            luck3.DisableDecraft();
            luck3.Register();

            Recipe returnpot = Recipe.Create(ItemID.PotionOfReturn, 6);
            returnpot.AddIngredient(ModContent.ItemType<FlowerEssence>());
            returnpot.AddIngredient(ItemID.LightShard, 3);
            returnpot.AddIngredient(ItemID.RecallPotion);
            returnpot.AddTile(TileID.AlchemyTable);
            returnpot.DisableDecraft();
            returnpot.Register();

            Recipe love = Recipe.Create(ItemID.LovePotion, 6);
            love.AddIngredient(ModContent.ItemType<FlowerEssence>());
            love.AddIngredient(ItemID.PrincessFish, 3);
            love.AddIngredient(ItemID.RecallPotion);
            love.AddTile(TileID.AlchemyTable);
            love.DisableDecraft();
            love.Register();

            Recipe stink = Recipe.Create(ItemID.StinkPotion, 6);
            stink.AddIngredient(ModContent.ItemType<FlowerEssence>());
            stink.AddIngredient(ItemID.Stinkfish, 3);
            stink.AddIngredient(ItemID.RecallPotion);
            stink.AddTile(TileID.AlchemyTable);
            stink.DisableDecraft();
            stink.Register();
        }
    }
}