using LBoL.ConfigData;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System.Collections.Generic;
using static AyaShameimaru.BepinexPlugin;
using LBoL.Core;
using LBoL.Base;
using LBoL.Core.Battle;
using AyaShameimaru.StatusEffects;
using LBoL.Core.Battle.BattleActions;
using LBoL.Presentation.UI.Panels;
using LBoL.Core.StatusEffects;
using UnityEngine;
using LBoL.Core.Units;
using static AyaShameimaru.Cards.AyaReignofWindSeDef;

namespace AyaShameimaru.Cards
{
    public sealed class AyaReignofWindDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaReignofWind);
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
                Rarity: Rarity.Uncommon,
                Type: CardType.Ability,
                TargetType: TargetType.Self,
                Colors: new List<ManaColor>() { ManaColor.White },
                IsXCost: false,
                Cost: new ManaGroup() { Any = 3, White = 1 },
                UpgradedCost: null,
                MoneyCost: null,
                Damage: null,
                UpgradedDamage: null,
                Block: null,
                UpgradedBlock: null,
                Shield: null,
                UpgradedShield: null,
                Value1: 2,
                UpgradedValue1: 4,
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

                RelativeEffects: new List<string>() { "Graze" },
                UpgradedRelativeEffects: new List<string>() { "Graze" },
                RelativeCards: new List<string>() { },
                UpgradedRelativeCards: new List<string>() { },
                Owner: "AyaPlayerUnit",
                Unfinished: false,
                Illustrator: "emerane",
                SubIllustrator: new List<string>() { }
            );

            return cardConfig;
        }
        [EntityLogic(typeof(AyaReignofWindDef))]
        public sealed class AyaReignofWind : Card
        {
            protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
            {
                yield return BuffAction<AyaReignofWindSe>(0, 0, 0, 0, 0.2f);
                yield return BuffAction<Graze>(Value1, 0, 0, 0, 0.2f);
                yield break;
            }
        }

    }
    public sealed class AyaReignofWindSeDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaReignofWindSe);
        }

        public override LocalizationOption LoadLocalization()
        {
            var gl = new GlobalLocalization(directorySource);
            gl.DiscoverAndLoadLocFiles(this);
            return gl;
        }

        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("Resources.AyaReignofWindSe.png", embeddedSource);
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
                HasLevel: false,
                LevelStackType: StackType.Add,
                HasDuration: false,
                DurationStackType: StackType.Add,
                DurationDecreaseTiming: DurationDecreaseTiming.Custom,
                HasCount: false,
                CountStackType: StackType.Keep,
                LimitStackType: StackType.Keep,
                ShowPlusByLimit: false,
                Keywords: Keyword.None,
                RelativeEffects: new List<string>() { "Graze" },
                VFX: "Default",
                VFXloop: "Default",
                SFX: "Default"
            );
            return statusEffectConfig;
        }
        [EntityLogic(typeof(AyaReignofWindSeDef))]
        public sealed class AyaReignofWindSe : StatusEffect
        {
            protected override void OnAdded(Unit unit)
            {
                foreach (EnemyUnit enemyUnit in Battle.AllAliveEnemies)
                {
                    if (enemyUnit.HasStatusEffect<Graze>())
                    {
                        int num = enemyUnit.GetStatusEffect<Graze>().Level;
                        React(new RemoveStatusEffectAction(enemyUnit.GetStatusEffect<Graze>(), true, 0.1f));
                        React(new ApplyStatusEffectAction(typeof(Graze), Battle.Player, num, null, null, null, 0f, true));
                    }
                    ReactOwnerEvent(enemyUnit.StatusEffectAdding, new EventSequencedReactor<StatusEffectApplyEventArgs>(OnEnemyStatusEffectAdding));
                }
                HandleOwnerEvent(Battle.EnemySpawned, new GameEventHandler<UnitEventArgs>(OnEnemySpawned));
            }
            private IEnumerable<BattleAction> OnEnemyStatusEffectAdding(StatusEffectApplyEventArgs args)
            {
                if (args.Effect is Graze)
                {
                    int num = args.Effect.Level;
                    args.ForceCancelBecause(CancelCause.Reaction);
                    NotifyActivating();
                    yield return new ApplyStatusEffectAction(typeof(Graze), Battle.Player, num, null, null, null, 0f, true);
                }
            }
            private void OnEnemySpawned(UnitEventArgs args)
            {
                ReactOwnerEvent(args.Unit.StatusEffectAdded, new EventSequencedReactor<StatusEffectApplyEventArgs>(OnEnemyStatusEffectAdding));
            }
        }
    }
}