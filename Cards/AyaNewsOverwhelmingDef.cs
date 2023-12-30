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
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using System.Linq;
using LBoL.EntityLib.Cards.Enemy;
using HarmonyLib;
using LBoL.Core.Battle.BattleActions;
using JetBrains.Annotations;
using LBoL.EntityLib.StatusEffects.Others;

namespace AyaShameimaru.Cards
{
    public sealed class AyaNewsOverwhelmingDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaNewsOverwhelming);
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
                Colors: new List<ManaColor>() { ManaColor.Green },
                IsXCost: false,
                Cost: new ManaGroup() { Green = 3 },
                UpgradedCost: new ManaGroup() { Green = 1 },
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

                Keywords: Keyword.None,
                UpgradedKeywords: Keyword.None,
                EmptyDescription: false,
                RelativeKeyword: Keyword.None,
                UpgradedRelativeKeyword: Keyword.None,

                RelativeEffects: new List<string>() { },
                UpgradedRelativeEffects: new List<string>() { },
                RelativeCards: new List<string>() { },
                UpgradedRelativeCards: new List<string>() { },
                Owner: "AyaPlayerUnit",
                Unfinished: false,
                Illustrator: "mariarose753",
                SubIllustrator: new List<string>() { }
             );

            return cardConfig;
        }
        [EntityLogic(typeof(AyaNewsOverwhelmingDef))]
        public sealed class AyaNewsOverwhelming : Card
        {
            protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
            {
                yield return BuffAction<AyaNewsOverwhelmingSeDef.AyaNewsOverwhelmingSe>(Value1, 0, 0, 0, 0.2f);
                yield break;
            }
        }

    }
    public sealed class AyaNewsOverwhelmingSeDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaNewsOverwhelmingSe);
        }

        public override LocalizationOption LoadLocalization()
        {
            var gl = new GlobalLocalization(directorySource);
            gl.DiscoverAndLoadLocFiles(this);
            return gl;
        }

        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("Resources.AyaNewsOverwhelmingSe.png", embeddedSource);
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
        [EntityLogic(typeof(AyaNewsOverwhelmingSeDef))]
        public sealed class AyaNewsOverwhelmingSe : StatusEffect
        {
            protected override void OnAdded(Unit unit)
            {
                ReactOwnerEvent(Owner.DamageDealt, new EventSequencedReactor<DamageEventArgs>(OnDamageDealt));
            }
            private IEnumerable<BattleAction> OnDamageDealt(DamageEventArgs args)
            {
                if (Battle.BattleShouldEnd)
                {
                    yield break;
                }
                if (args.Target.IsAlive)
                {
                    Card card = args.ActionSource as Card;
                    DamageInfo damageInfo = args.DamageInfo;
                    if (card.CardType == CardType.Status && damageInfo.DamageType == DamageType.Attack && damageInfo.Damage > 0f)
                    {
                        NotifyActivating();
                        switch (card.Config.Rarity)
                        {
                            case Rarity.Common:
                                yield return new ApplyStatusEffectAction<AyaNewsOverwhelmedSeDef.AyaNewsOverwhelmedSe>(args.Target, new int?(Level), null, null, null, 0f, true);
                                break;
                            case Rarity.Uncommon:
                                yield return new ApplyStatusEffectAction<AyaNewsOverwhelmedSeDef.AyaNewsOverwhelmedSe>(args.Target, new int?(Level), null, null, null, 0f, true);
                                break;
                            case Rarity.Rare:
                                yield return new ApplyStatusEffectAction<AyaNewsOverwhelmedSeDef.AyaNewsOverwhelmedSe>(args.Target, new int?(Level), null, null, null, 0f, true);
                                break;
                            default:
                                break;
                        }
                    }
                }
                yield break;
            }
        }
    }
    public sealed class AyaNewsOverwhelmedSeDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaNewsOverwhelmedSe);
        }

        public override LocalizationOption LoadLocalization()
        {
            var gl = new GlobalLocalization(directorySource);
            gl.DiscoverAndLoadLocFiles(this);
            return gl;
        }

        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("Resources.AyaNewsOverwhelmedSe.png", embeddedSource);
        }
        public override StatusEffectConfig MakeConfig()
        {
            var statusEffectConfig = new StatusEffectConfig(
                Index: sequenceTable.Next(typeof(StatusEffectConfig)),
                Id: "",
                Order: 10,
                Type: StatusEffectType.Negative,
                IsVerbose: false,
                IsStackable: true,
                StackActionTriggerLevel: 8,
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
        [EntityLogic(typeof(AyaNewsOverwhelmedSeDef))]
        public sealed class AyaNewsOverwhelmedSe : StatusEffect
        {
            public override IEnumerable<BattleAction> StackAction(Unit targetOwner, int targetLevel)
            {
                EnemyUnit enemy = targetOwner as EnemyUnit;
                enemy._turnMoves.Clear();
                enemy.ClearIntentions();
                var stun = Intention.Stun();
                stun.Source = enemy;
                enemy._turnMoves.Add(new SimpleEnemyMove(stun, new EnemyMoveAction[] { new EnemyMoveAction(enemy, "Wasted", true) }));
                enemy.Intentions.Add(stun);
                enemy.NotifyIntentionsChanged();
                yield break;
            }
        }
    }
}