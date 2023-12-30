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
using System.Collections;
using LBoL.Presentation;
using LBoL.Core.StatusEffects;
using System;
using AyaShameimaru.StatusEffects;

namespace AyaShameimaru.Cards
{
    public sealed class AyaFmaDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaFma);
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
                Cost: new ManaGroup() { Any = 1, Red = 2 },
                UpgradedCost: null,
                MoneyCost: null,
                Damage: null,
                UpgradedDamage: null,
                Block: null,
                UpgradedBlock: null,
                Shield: null,
                UpgradedShield: null,
                Value1: 2,
                UpgradedValue1: 3,
                Value2: 20,
                UpgradedValue2: 30,
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

                RelativeEffects: new List<string>() { "Firepower", "AyaAccelerationSe" },
                UpgradedRelativeEffects: new List<string>() { "Firepower", "AyaAccelerationSe" },
                RelativeCards: new List<string>() { },
                UpgradedRelativeCards: new List<string>() { },
                Owner: "AyaPlayerUnit",
                Unfinished: false,
                Illustrator: "Kihou Kanshouzai",
                SubIllustrator: new List<string>() { }
             );

            return cardConfig;
        }
        [EntityLogic(typeof(AyaFmaDef))]
        public sealed class AyaFma : Card
        {
            public override int AdditionalValue1
            {
                get
                {
                    return (int)Math.Truncate((decimal)GetSeLevel<AyaAccelerationSeDef.AyaAccelerationSe>() / 4);
                }
            }
            protected override void OnEnterBattle(BattleController battle)
            {
                ReactBattleEvent(Battle.BattleStarted, delegate (GameEventArgs args)
                {
                    GameMaster.Instance.StartCoroutine(Trigger());
                    return null;
                });
            }
            private IEnumerator Trigger()
            {
                FreeCost = true;
                yield return new WaitForSecondsRealtime(Value2);
                NotifyChanged();
                FreeCost = false;
            }
            protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
            {
                yield return BuffAction<Firepower>(Value1, 0, 0, 0, 0.2f);
                yield break;
            }
        }
    }
}