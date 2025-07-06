using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Projectiles.Modifiers;

namespace TranscendenceMod.Items.Consumables
{
    public class MysticTalismanPickup : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(7, 3));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            ItemID.Sets.IsAPickup[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 96;
            Item.height = 96;
            Item.rare = ItemRarityID.Purple;
        }
        public override Color? GetAlpha(Color lightColor) => Color.White;
        public override bool OnPickup(Player player)
        {
            player.statMana += 100;
            player.ManaEffect(100);
            return false;
        }
        private int TimeBeforeDisappear;
        private float Size = 1f;
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Main.GetItemDrawFrame(Item.type, out var sprite, out var frame);

            var eff = ModContent.Request<Effect>("TranscendenceMod/Miscannellous/Assets/Shaders/Effects/SeraphOutlineShader", AssetRequestMode.ImmediateLoad).Value;
            eff.Parameters["uOpacity"].SetValue(0.25f);
            eff.Parameters["uSaturation"].SetValue(0.5f);

            eff.Parameters["uRotation"].SetValue(1f);
            eff.Parameters["uTime"].SetValue(0f);
            eff.Parameters["uDirection"].SetValue(1f);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, eff, Main.GameViewMatrix.TransformationMatrix);


            spriteBatch.Draw(sprite, Item.Bottom - Main.screenPosition - new Vector2(0, frame.Height * 0.5f),
                frame, Color.White, rotation, frame.Size() * 0.5f, scale * 5f * Size, SpriteEffects.None, 0f);


            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);


            spriteBatch.Draw(sprite, Item.Bottom - Main.screenPosition - new Vector2(0, frame.Height * 0.5f),
                frame, Color.White, rotation, frame.Size() * 0.5f, scale * Size, SpriteEffects.None, 0f); 

            return false;
        }
        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            base.Update(ref gravity, ref maxFallSpeed);

            Item.velocity = Vector2.Lerp(Item.velocity, Item.DirectionTo(Main.MouseWorld + Main.rand.NextVector2Circular(50f, 50f)) * 150f, 1f / 150f);

            if (++TimeBeforeDisappear > 90)
                Size -= 0.1f;

            if (Size < 0.1f)
            {
                Item.active = false;
                Projectile.NewProjectile(Item.GetSource_FromThis(), Item.Center, Vector2.Zero, ModContent.ProjectileType<MysticBlast>(), 100, 4f, Main.LocalPlayer.whoAmI);
            }
        }
    }
}
