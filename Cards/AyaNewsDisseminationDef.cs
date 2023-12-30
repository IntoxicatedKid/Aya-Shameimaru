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
using LBoL.Core.StatusEffects;
using System.Linq;
using UnityEngine;
using LBoL.Core.Units;
using HarmonyLib;
using LBoL.EntityLib.Cards.Enemy;

namespace AyaShameimaru.Cards
{
    public sealed class AyaNewsDisseminationDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaNewsDissemination);
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
                Type: CardType.Skill,
                TargetType: TargetType.Nobody,
                Colors: new List<ManaColor>() { ManaColor.Green },
                IsXCost: false,
                Cost: new ManaGroup() { Green = 1 },
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
                Mana: new ManaGroup() { Green = 3, Colorless = 1 },
                UpgradedMana: new ManaGroup() { Green = 3, Philosophy = 1 },
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
                RelativeCards: new List<string>() { "Xuanguang" },
                UpgradedRelativeCards: new List<string>() { "Xuanguang" },
                Owner: "AyaPlayerUnit",
                Unfinished: false,
                Illustrator: "Noumin Joemanyodw",
                SubIllustrator: new List<string>() { }
            );

            return cardConfig;
        }
        [EntityLogic(typeof(AyaNewsDisseminationDef))]
        public sealed class AyaNewsDissemination : Card
        {
            protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
            {
                List<Card> list = Library.CreateCards<Xuanguang>(Value1, false) as List<Card>;
                //list.Do(card => card.IsReplenish = true);
                if (!IsUpgraded)
                {
                    yield return new AddCardsToDrawZoneAction(list, DrawZoneTarget.Random);
                }
                else
                {
                    yield return new AddCardsToDiscardAction(list);
                }
                yield return new GainManaAction(Mana);
                yield break;
            }
        }
    }
}