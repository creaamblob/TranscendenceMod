using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;
using TranscendenceMod.NPCs.Boss.Dragon;

namespace TranscendenceMod.Miscannellous.UI
{
    public class INCOMINGATTACK : UIElement
    {
        public NPC npc2;
        public bool Hovering => Main.MouseWorld.Distance(new Vector2(Main.screenPosition.X + Main.screenWidth / 2, Main.screenPosition.Y + Main.screenHeight / 2 + 50)) < 8;
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Texture2D sprite = ModContent.Request<Texture2D>("TranscendenceMod/Miscannellous/UI/INCOMINGATTACK").Value;
            Player player = Main.LocalPlayer;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc != null && npc.active && npc.type == ModContent.NPCType<WindDragon>())
                    npc2 = npc;
            }

            if (player != null && player.active && npc2 != null && npc2.active && npc2.ModNPC is WindDragon boss && boss.Timer_AI < 90)
            {
                int x = Main.screenWidth / 2;
                int y = Main.screenHeight / 2;

                Rectangle rec = new Rectangle(x - (sprite.Width / 2), y - 85, sprite.Width, sprite.Height);

                spriteBatch.Draw(sprite, rec, Color.White);

                Vector2 warningPos = new Vector2(x, y - 80);

                string attackName = boss.CurrentAttack;
                ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value,
                    attackName, new Vector2(warningPos.X - (FontAssets.MouseText.Value.MeasureString(attackName).X / 2 * 0.75f),
                    warningPos.Y), Color.White, 0, Vector2.Zero, Vector2.One * 0.75f);
            }
        }
    }

    public class INCOMINGATTACKState : UIState
    {
        public INCOMINGATTACK parry;
        public override void OnInitialize()
        {
            parry = new INCOMINGATTACK();
            Append(parry);
        }
    }
}
namespace TranscendenceMod.Miscannellous.UI
{
    [Autoload(Side = ModSide.Client)]
    public class INCOMINGATTACKDraw : ModSystem
    {
        internal INCOMINGATTACKState parryBar;
        private UserInterface parryBar2;

        public override void Load()
        {
            parryBar = new INCOMINGATTACKState();
            parryBar.Activate();

            parryBar2 = new UserInterface();
            parryBar2.SetState(parryBar);
        }
        public override void UpdateUI(GameTime gameTime)
        {
            parryBar2?.Update(gameTime);
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {/*
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("TranscendenceMod: INCOMING ATTACK", delegate
                {
                    parryBar2.Draw(Main.spriteBatch, new GameTime());
                    return true;
                }, InterfaceScaleType.UI));
            }*/
        }
    }
}
