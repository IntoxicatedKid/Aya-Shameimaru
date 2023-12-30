using LBoL.ConfigData;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System.Collections.Generic;
using static AyaShameimaru.BepinexPlugin;
using UnityEngine;
using LBoL.Core;
using LBoL.Base;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using JetBrains.Annotations;
using AyaShameimaru.StatusEffects;
using LBoL.Core.Randoms;
using System.Linq;
using System;

namespace AyaShameimaru.Cards
{
    public sealed class AyaArmoredRavenDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaArmoredRaven);
        }
        public override CardImages LoadCardImages()
        {
            var imgs = new CardImages(embeddedSource);
            imgs.AutoLoad(this, ".png", relativePath: "Resources.");
            return imgs;
        }

        public override LocalizationOption LoadLocalization()
        {
            var gl = new GlobalLocalization(directorySource);
            gl.DiscoverAndLoadLocFiles(this);
            return gl;
        }
        public override CardConfig MakeConfig()
        {
            var cardConfig = new CardConfig(
               Index: sequenceTable.Next(typeof(CardConfig)),
               Id: "",
               ImageId: "",
               UpgradeImageId: "",
               Order: 10,
               AutoPerform: true,
               Perform: new string[0][],
               GunName: "Simple1",
               GunNameBurst: "Simple1",
               DebugLevel: 0,
               Revealable: false,
               IsPooled: true,
               HideMesuem: false,
               IsUpgradable: true,
               Rarity: Rarity.Rare,
               Type: CardType.Ability,
               TargetType: TargetType.Self,
               Colors: new List<ManaColor>() { ManaColor.Red, ManaColor.Green },
               IsXCost: false,
               Cost: new ManaGroup() { Any = 1, Red = 2, Green = 2 },
               UpgradedCost: new ManaGroup() { Any = 1, Red = 2, Green = 2 },
               MoneyCost: null,
               Damage: null,
               UpgradedDamage: null,
               Block: null,
               UpgradedBlock: null,
               Shield: null,
               UpgradedShield: null,
               Value1: 1,
               UpgradedValue1: null,
               Value2: null,
               UpgradedValue2: null,
               Mana: null,
               UpgradedMana: null,
               Scry: null,
               UpgradedScry: null,
               ToolPlayableTimes: null,
               Loyalty: null,
               UpgradedLoyalty: null,
               PassiveCost: null,
               UpgradedPassiveCost: null,
               ActiveCost: null,
               UpgradedActiveCost: null,
               UltimateCost: null,
               UpgradedUltimateCost: null,

               Keywords: Keyword.Ethereal,
               UpgradedKeywords: Keyword.None,
               EmptyDescription: false,
               RelativeKeyword: Keyword.None,
               UpgradedRelativeKeyword: Keyword.None,

               RelativeEffects: new List<string>() { "Weak" },
               UpgradedRelativeEffects: new List<string>() { "Weak" },
               RelativeCards: new List<string>() { },
               UpgradedRelativeCards: new List<string>() { },
               Owner: "AyaPlayerUnit",
               Unfinished: false,
               Illustrator: "vandana",
               SubIllustrator: new List<string>() { }
            );

            return cardConfig;
        }
    }
    [EntityLogic(typeof(AyaArmoredRavenDef))]
    public sealed class AyaArmoredRaven : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return PerformAction.Sfx("DroneSummon", 0f);
            yield return BuffAction<AyaArmoredRavenSeDef.AyaArmoredRavenSe>(Value1, 0, 0, 0, 0.2f);
            yield break;
        }
    }
    public sealed class AyaArmoredRavenSeDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaArmoredRavenSe);
        }

        public override LocalizationOption LoadLocalization()
        {
            var gl = new GlobalLocalization(directorySource);
            gl.DiscoverAndLoadLocFiles(this);
            return gl;
        }

        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("Resources.AyaArmoredRavenSe.png", embeddedSource);
        }
        public override StatusEffectConfig MakeConfig()
        {
            var statusEffectConfig = new StatusEffectConfig(
                Index: sequenceTable.Next(typeof(StatusEffectConfig)),
                Id: "",
                Order: 10,
                Type: StatusEffectType.Positive,
                IsVerbose: false,
                IsStackable: true,
                StackActionTriggerLevel: null,
                HasLevel: true,
                LevelStackType: StackType.Add,
                HasDuration: false,
                DurationStackType: StackType.Add,
                DurationDecreaseTiming: DurationDecreaseTiming.Custom,
                HasCount: false,
                CountStackType: StackType.Keep,
                LimitStackType: StackType.Keep,
                ShowPlusByLimit: false,
                Keywords: Keyword.None,
                RelativeEffects: new List<string>() { },
                VFX: "Default",
                VFXloop: "Default",
                SFX: "Default"
            );
            return statusEffectConfig;
        }
        [EntityLogic(typeof(AyaArmoredRavenSeDef))]
        public sealed class AyaArmoredRavenSe : StatusEffect
        {
            [UsedImplicitly]
            public int Value
            {
                get
                {
                    return 30;
                }
            }
            private bool attacked;
            [UsedImplicitly]
            public float latestDamage;
            protected override void OnAdded(Unit unit)
            {
                HandleOwnerEvent(Owner.DamageDealing, new GameEventHandler<DamageDealingEventArgs>(OnDamageDealing));
                ReactOwnerEvent(Owner.DamageDealt, new EventSequencedReactor<DamageEventArgs>(OnDamageDealt));
                ReactOwnerEvent(Owner.StatisticalTotalDamageDealt, new EventSequencedReactor<StatisticalDamageEventArgs>(OnStatisticalTotalDamageDealt));
                HandleOwnerEvent(Owner.StatusEffectAdding, new GameEventHandler<StatusEffectApplyEventArgs>(OnStatusEffectAdding));
            }
            private void OnDamageDealing(DamageDealingEventArgs args)
            {
                DamageInfo damageInfo = args.DamageInfo;
                if (damageInfo.DamageType == DamageType.Attack)
                {
                    damageInfo.Damage = damageInfo.Amount * (1f - Value / 100f);
                    args.DamageInfo = damageInfo;
                    args.AddModifier(this);
                }
            }
            private IEnumerable<BattleAction> OnDamageDealt(DamageEventArgs args)
            {
                if (args.DamageInfo.DamageType == DamageType.Attack)
                {
                    attacked = true;
                    latestDamage = args.DamageInfo.Amount;
                }
                yield break;
            }
            private IEnumerable<BattleAction> OnStatisticalTotalDamageDealt(StatisticalDamageEventArgs args)
            {
                if (attacked)
                {
                    for (int i = 0; i < Level; i++)
                    {
                        yield return new DamageAction(Owner, args.ArgsTable.Keys, new DamageInfo(latestDamage, DamageType.Reaction, false, false, false));
                    }
                    attacked = false;
                }
            }
            private void OnStatusEffectAdding(StatusEffectApplyEventArgs args)
            {
                if (args.Effect is Weak)
                {
                    args.CancelBy(this);
                    NotifyActivating();
                }
            }
        }
    }
}