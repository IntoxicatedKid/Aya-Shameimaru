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
using LBoL.EntityLib.Cards.Enemy;
using HarmonyLib;
using LBoL.EntityLib.Cards.Character.Reimu;
using LBoL.Base.Extensions;
using LBoL.Presentation.UI.Panels;
using LBoL.EntityLib.Cards.Character.Sakuya;

namespace AyaShameimaru.Cards
{
    public sealed class AyaEffortlessFlightDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaEffortlessFlight);
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
               Colors: new List<ManaColor>() { ManaColor.Red },
               IsXCost: false,
               Cost: new ManaGroup() { Red = 2 },
               UpgradedCost: new ManaGroup() { Red = 2 },
               MoneyCost: null,
               Damage: null,
               UpgradedDamage: null,
               Block: null,
               UpgradedBlock: null,
               Shield: null,
               UpgradedShield: null,
               Value1: 3,
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

               RelativeEffects: new List<string>() { },
               UpgradedRelativeEffects: new List<string>() { },
               RelativeCards: new List<string>() { },
               UpgradedRelativeCards: new List<string>() { },
               Owner: "AyaPlayerUnit",
               Unfinished: false,
               Illustrator: "kasuya baian",
               SubIllustrator: new List<string>() { }
            );

            return cardConfig;
        }
    }
    [EntityLogic(typeof(AyaEffortlessFlightDef))]
    public sealed class AyaEffortlessFlight : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<AyaEffortlessFlightSeDef.AyaEffortlessFlightSe>(Value1, 0, 0, 0, 0.2f);
        }
    }
    public sealed class AyaEffortlessFlightSeDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaEffortlessFlightSe);
        }

        public override LocalizationOption LoadLocalization()
        {
            var gl = new GlobalLocalization(directorySource);
            gl.DiscoverAndLoadLocFiles(this);
            return gl;
        }

        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("Resources.AyaEffortlessFlightSe.png", embeddedSource);
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
        [EntityLogic(typeof(AyaEffortlessFlightSeDef))]
        public sealed class AyaEffortlessFlightSe : StatusEffect
        {
            protected override void OnAdded(Unit unit)
            {
                ReactOwnerEvent(Battle.RoundStarted, new EventSequencedReactor<GameEventArgs>(OnRoundStarted));
            }
            private IEnumerable<BattleAction> OnRoundStarted(GameEventArgs args)
            {
                NotifyActivating();
                yield return new ApplyStatusEffectAction<AyaAccelerationSeDef.AyaAccelerationSe>(Owner, Level, null, null, null, 0f, true);
            }
        }
    }
}