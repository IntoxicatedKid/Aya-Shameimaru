using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System.Collections.Generic;
using LBoL.Core.StatusEffects;
using static AyaShameimaru.BepinexPlugin;
using UnityEngine;
using LBoL.Presentation;
using System.Collections;
using LBoL.Core.Units;
using LBoL.EntityLib.StatusEffects.Enemy;
using static AyaShameimaru.Cards.AyaQuickdrawSeDef;
using LBoL.Base.Extensions;
using System.Linq;
using LBoL.Presentation.UI.Panels;
using LBoL.Presentation.UI;

namespace AyaShameimaru.Cards
{
    public sealed class AyaMultiCameraDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaMultiCamera);
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
               GunName: "丑时参拜",
               GunNameBurst: "丑时参拜B",
               DebugLevel: 0,
               Revealable: false,
               IsPooled: true,
               HideMesuem: false,
               IsUpgradable: true,
               Rarity: Rarity.Uncommon,
               Type: CardType.Attack,
               TargetType: TargetType.SingleEnemy,
               Colors: new List<ManaColor>() { ManaColor.Green },
               IsXCost: false,
               Cost: new ManaGroup() { Any = 2, Green = 1 },
               UpgradedCost: null,
               MoneyCost: null,
               Damage: 18,
               UpgradedDamage: 24,
               Block: null,
               UpgradedBlock: null,
               Shield: null,
               UpgradedShield: null,
               Value1: null,
               UpgradedValue1: null,
               Value2: null,
               UpgradedValue2: null,
               Mana: new ManaGroup() { Any = 1 },
               UpgradedMana: new ManaGroup() { Any = 1 },
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
               RelativeKeyword: Keyword.TempMorph,
               UpgradedRelativeKeyword: Keyword.TempMorph,

               RelativeEffects: new List<string>() { },
               UpgradedRelativeEffects: new List<string>() { },
               RelativeCards: new List<string>() { },
               UpgradedRelativeCards: new List<string>() { },
               Owner: "AyaPlayerUnit",
               Unfinished: false,
               Illustrator: "zsefvgy",
               SubIllustrator: new List<string>() { }
            );

            return cardConfig;
        }
        [EntityLogic(typeof(AyaMultiCameraDef))]
        public sealed class AyaMultiCamera : Card
        {
            protected override void OnEnterBattle(BattleController battle)
            {
                ReactBattleEvent(Battle.CardExiled, new EventSequencedReactor<CardEventArgs>(OnCardExiled));
            }
            private IEnumerable<BattleAction> OnCardExiled(CardEventArgs args)
            {
                if (args.Card.CardType == CardType.Status && Zone == CardZone.Discard)
                {
                    yield return new MoveCardAction(this, CardZone.Hand);
                    SetTurnCost(Mana);
                }
                yield break;
            }
        }
    }
}
