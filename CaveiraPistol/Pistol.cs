using System;
using System.Linq;
using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Item;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items.Firearms.Attachments;
using PlayerRoles;
using PlayerStatsSystem;

namespace CaveiraPistol
{
    [CustomItem(ItemType.GunCOM15)]
    public class Com18BoostItem : CustomWeapon
    {
        public override ItemType Type { get; set; } = ItemType.GunCOM15;
        public override uint Id { get; set; } = 122;
        public override string Name { get; set; } = "Caveira Pistol";
        public override string Description { get; set; } = "Kill Everyone!";
        public override float Damage { get; set; } = Main.Instance.Config.Damage;
        public override byte ClipSize { get; set; } = 12;
        public override bool FriendlyFire { get; set; } = true;
        public override float Weight { get; set; } = 1f;
        public float DamageMultiplier { get; set; } = Main.Instance.Config.RampageDamageMultiplier;
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            DynamicSpawnPoints = Main.Instance.Config.SpawnLocations
                .Select(entry => new DynamicSpawnPoint
                {
                    Location = entry.Key,
                    Chance = entry.Value
                }).ToList()
        };
        public override AttachmentName[] Attachments { get; set; } = new[]
        {
        AttachmentName.SoundSuppressor,
        };
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
            Exiled.Events.Handlers.Player.ChangedItem += OnChangedItem;
            Exiled.Events.Handlers.Item.ChangingAttachments += ChangingAttachmentEvent;

            base.SubscribeEvents();
        }
        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            Exiled.Events.Handlers.Player.ChangedItem -= OnChangedItem;
            Exiled.Events.Handlers.Item.ChangingAttachments -= ChangingAttachmentEvent;

            base.UnsubscribeEvents();
        }
        public void ChangingAttachmentEvent(ChangingAttachmentsEventArgs ev)
        {
            if (Check(ev.Item))
            {
                if (TryGet(ev.Player.CurrentItem, out var item) && item.Id == 122)
                {
                    ev.IsAllowed = false;
                }
            }
        }
        private void OnChangedItem(ChangedItemEventArgs ev)
        {
            if (!Check(ev.Player.CurrentItem))
            {
                ev.Player.DisableEffect(EffectType.MovementBoost);
                ev.Player.DisableEffect(EffectType.SilentWalk);
                ev.Player.DisableEffect(EffectType.Scanned);
            }
        }
        protected override void OnHurting(HurtingEventArgs ev)
        {
            if (ev.Attacker != ev.Player && ev.DamageHandler.Base is FirearmDamageHandler firearmDamageHandler && firearmDamageHandler.WeaponType == ev.Attacker.CurrentItem.Type && Check(ev.Attacker.CurrentItem))
            {
                ev.DamageHandler.Damage = Damage;

                if(ev.Attacker.IsEffectActive<SilentWalk>())
                    ev.DamageHandler.Damage = DamageMultiplier * Damage;

                if (!Main.Instance.Config.Scp207 && ev.Attacker.IsEffectActive<Scp207>())
                    ev.DamageHandler.Damage = Damage;

                if (!Main.Instance.Config.Scp1853 && ev.Attacker.IsEffectActive<Scp1853>())
                    ev.DamageHandler.Damage = Damage;

                if (!Main.Instance.Config.Antiscp207 && ev.Attacker.IsEffectActive<AntiScp207>())
                    ev.DamageHandler.Damage = Damage;

                if (ev.Player.Role.Team == Team.SCPs)
                    ev.DamageHandler.Damage = Damage;
            }

            if (!Check(ev.Player.CurrentItem))
                return;

            if (ev.Attacker != ev.Player)
            {
                if (!Main.Instance.Config.Scp207 && ev.Player.IsEffectActive<Scp207>())
                    return;
                if (!Main.Instance.Config.Scp1853 && ev.Player.IsEffectActive<Scp1853>())
                    return;
                if (!Main.Instance.Config.Antiscp207 && ev.Player.IsEffectActive<AntiScp207>())
                    return;
                if (Main.Instance.Config.Hint)
                    ev.Player.ShowHint("<color=red>Rampage Activated</color>", 2f);
                ev.Player.EnableEffect(EffectType.MovementBoost, 40, Main.Instance.Config.RampageDuration);
                ev.Player.EnableEffect(EffectType.SilentWalk, 10, Main.Instance.Config.RampageDuration);
                ev.Player.EnableEffect(EffectType.Scanned, 10, Main.Instance.Config.RampageDuration);
            }
        }
    }
} 