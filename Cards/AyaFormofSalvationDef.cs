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

namespace AyaShameimaru.Cards
{
    public sealed class AyaFormofSalvationDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaFormofSalvation);
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
                IsPooled: false,
                HideMesuem: true,
                IsUpgradable: true,
                Rarity: Rarity.Uncommon,
                Type: CardType.Defense,
                TargetType: TargetType.Self,
                Colors: new List<ManaColor>() { ManaColor.Red },
                IsXCost: false,
                Cost: new ManaGroup() { Any = 1, Red = 1 },
                UpgradedCost: null,
                MoneyCost: null,
                Damage: null,
                UpgradedDamage: null,
                Block: 18,
                UpgradedBlock: 24,
                Shield: null,
                UpgradedShield: null,
                Value1: 1,
                UpgradedValue1: 2,
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

                RelativeEffects: new List<string>() { "AyaPerfectEvasionSe", "AyaEvasionSe", "Graze" },
                UpgradedRelativeEffects: new List<string>() { "AyaPerfectEvasionSe", "AyaEvasionSe", "Graze" },
                RelativeCards: new List<string>() { },
                UpgradedRelativeCards: new List<string>() { },
                Owner: "AyaPlayerUnit",
                Unfinished: false,
                Illustrator: "Hyuuga Azuri",
                SubIllustrator: new List<string>() { }
            );

            return cardConfig;
        }
        [EntityLogic(typeof(AyaFormofSalvationDef))]
        public sealed class AyaFormofSalvation : Card
        {
            protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
            {
                yield return BuffAction<AyaPerfectEvasionSeDef.AyaPerfectEvasionSe>(Value1, 0, 0, 0, 0.2f);
                yield break;
            }
        }

    }
}