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
using static AyaShameimaru.BepinexPlugin;
using System.Linq;
using LBoL.EntityLib.Cards.Enemy;

namespace AyaShameimaru.Cards
{
    public sealed class AyaTakePictureDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaTakePicture);
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
               Rarity: Rarity.Common,
               Type: CardType.Attack,
               TargetType: TargetType.SingleEnemy,
               Colors: new List<ManaColor>() { ManaColor.Green },
               IsXCost: false,
               Cost: new ManaGroup() { Green = 2 },
               UpgradedCost: new ManaGroup() { Green = 1 },
               MoneyCost: null,
               Damage: 12,
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
               RelativeCards: new List<string>() { "AyaNews" },
               UpgradedRelativeCards: new List<string>() { "AyaNews" },
               Owner: "AyaPlayerUnit",
               Unfinished: false,
               Illustrator: "満福神社",
               SubIllustrator: new List<string>() { }
            );

            return cardConfig;
        }
    }
    [EntityLogic(typeof(AyaTakePictureDef))]
    public sealed class AyaTakePicture : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return PerformAction.Animation(Battle.Player, "skill1", 1f, null, 0f, -1);
            yield return new DamageAction(Battle.Player, selector.SelectedEnemy, DamageInfo.Attack(Damage.Amount, false), "Instant", GunType.Single);
            List<Card> cards = Library.CreateCards<AyaNews>(Value1).ToList<Card>();
            yield return new AddCardsToDrawZoneAction(cards, DrawZoneTarget.Top);
            yield return new DrawManyCardAction(Value1);
        }
    }
}
