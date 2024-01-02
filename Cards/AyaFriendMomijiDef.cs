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
using LBoL.Core.Units;
using LBoL.Base.Extensions;
using LBoL.Core.Battle.Interactions;
using static AyaShameimaru.BepinexPlugin;
using JetBrains.Annotations;
using LBoL.Core.StatusEffects;
using System.Linq;
using static UnityEngine.GraphicsBuffer;
using System;
using AyaShameimaru.StatusEffects;
using System.Collections;

namespace AyaShameimaru.Cards
{
    public sealed class AyaFriendMomijiDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaFriendMomiji);
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
               GunName: "元鬼玉",
               GunNameBurst: "元鬼玉B",
               DebugLevel: 0,
               Revealable: false,
               IsPooled: true,
               HideMesuem: false,
               IsUpgradable: true,
               Rarity: Rarity.Rare,
               Type: CardType.Friend,
               TargetType: TargetType.Nobody,
               Colors: new List<ManaColor>() { ManaColor.Red },
               IsXCost: false,
               Cost: new ManaGroup() { Any = 2, Red = 2 },
               UpgradedCost: new ManaGroup() { Any = 2, Red = 2 },
               MoneyCost: null,
               Damage: 12,
               UpgradedDamage: 12,
               Block: 12,
               UpgradedBlock: 12,
               Shield: null,
               UpgradedShield: null,
               Value1: null,
               UpgradedValue1: null,
               Value2: 10,
               UpgradedValue2: 10,
               Mana: null,
               UpgradedMana: null,
               Scry: null,
               UpgradedScry: null,
               ToolPlayableTimes: null,
               Loyalty: 5,
               UpgradedLoyalty: 5,
               PassiveCost: 2,
               UpgradedPassiveCost: 4,
               ActiveCost: null,
               UpgradedActiveCost: null,
               UltimateCost: -4,
               UpgradedUltimateCost: -4,

               Keywords: Keyword.None,
               UpgradedKeywords: Keyword.None,
               EmptyDescription: false,
               RelativeKeyword: Keyword.Block | Keyword.Expel | Keyword.Accuracy,
               UpgradedRelativeKeyword: Keyword.Block | Keyword.Expel | Keyword.Accuracy,

               RelativeEffects: new List<string>() { },
               UpgradedRelativeEffects: new List<string>() { },
               RelativeCards: new List<string>() { },
               UpgradedRelativeCards: new List<string>() { },
               Owner: "AyaPlayerUnit",
               Unfinished: false,
               Illustrator: "kazami ryouya",
               SubIllustrator: new List<string>() { }
            );

            return cardConfig;
        }
    }
    [EntityLogic(typeof(AyaFriendMomijiDef))]
    public sealed class AyaFriendMomiji : Card
    {
        public DamageInfo Damage2
        {
            get
            {
                return DamageInfo.Attack(Damage.Amount * 2, true);
            }
        }

        public FriendCostInfo FriendP2
        {
            get
            {
                return new FriendCostInfo(-2, FriendCostType.Passive);
            }
        }


        public int PassiveBlock
        {
            get
            {
                return base.ConfigBlock;
            }
        }

        protected override void OnEnterBattle(BattleController battle)
        {
            ReactBattleEvent(Battle.Player.StatisticalTotalDamageDealt, new EventSequencedReactor<StatisticalDamageEventArgs>(OnPlayerStatisticalTotalDamageDealt));
            HandleBattleEvent(Battle.Player.DamageTaking, OnPlayerDamageTaking, (GameEventPriority)(45));
        }


        private IEnumerable<BattleAction> AttackPassive(IEnumerable<Unit> targets)
        {
            NotifyActivating();
            Loyalty += FriendP2.Cost;
            for (int i = 0; i < Battle.FriendPassiveTimes; i++)
            {
                if (Battle.BattleShouldEnd)
                {
                    yield break;
                }
                yield return PerformAction.Sfx("FairySupport", 0f);
                yield return new DamageAction(Battle.Player, targets, Damage, "YoumuKan", GunType.Single);
            }
        }

        private IEnumerable<BattleAction> ParryPassive(DamageEventArgs args = null) 
        {
            NotifyActivating();
            Loyalty += FriendP2.Cost;
            for (int i = 0; i < Battle.FriendPassiveTimes; i++)
            {
                if (Battle.BattleShouldEnd)
                {
                    yield break;
                }
                yield return PerformAction.Sfx("FairySupport", 0f);
                yield return new CastBlockShieldAction(Battle.Player, Block.Block, 0, BlockShieldType.Direct, false);
                if (args != null)
                {
                    args.DamageInfo = args.DamageInfo.BlockBy(Block.Block);
                    args.AddModifier(this);
                }
            }
        }




        private IEnumerable<BattleAction> OnPlayerStatisticalTotalDamageDealt(StatisticalDamageEventArgs args)
        {
            if (!Battle.BattleShouldEnd && Zone == CardZone.Hand && Summoned && Loyalty >= -FriendP2.Cost)
            {
                var targets = args.ArgsTable.Where(kv => kv.Value.Any(e => e.DamageInfo.DamageType == DamageType.Attack)).Select(kv => kv.Key);

                if(targets.Count() > 0)
                    foreach (var a in AttackPassive(targets)) yield return a;
            }
        }
        private void OnPlayerDamageTaking(DamageEventArgs args)
        {
            if (args.Cause != ActionCause.OnlyCalculate && !Battle.BattleShouldEnd 
                && Zone == CardZone.Hand && Summoned && Loyalty >= -FriendP2.Cost 
                && args.DamageInfo.DamageType == DamageType.Attack
                && args.DamageInfo.Damage > 0f && !args.DamageInfo.IsGrazed
                )
            {
                foreach (var a in ParryPassive(args)) { React(a); }
            }
        }
        public override IEnumerable<BattleAction> OnTurnStartedInHand()
        {
            if (!Summoned || Battle.BattleShouldEnd)
            {
                yield break;
            }
            NotifyActivating();
            Loyalty += PassiveCost;
            yield break;
        }
        public override IEnumerable<BattleAction> GetPassiveActions()
        {
            if (!Summoned || Battle.BattleShouldEnd)
            {
                yield break;
            }
            NotifyActivating();
            Loyalty += PassiveCost;
            if (Loyalty >= -FriendP2.Cost)
            {
                foreach (var a in AttackPassive(Battle.AllAliveEnemies)) yield return a;
            }
            if (Loyalty >= -FriendP2.Cost)
            {
                foreach (var a in ParryPassive()) yield return a;
            }
        }

        public override IEnumerable<BattleAction> SummonActions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            GameRun.CanViewDrawZoneActualOrder += 1;
            foreach (BattleAction battleAction in base.SummonActions(selector, consumingMana, precondition))
            {
                yield return battleAction;
            }
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            Loyalty += UltimateCost;
            UltimateUsed = true;
            EnemyUnit enemyUnit = Battle.EnemyGroup.Alives.MinBy((EnemyUnit unit) => unit.Hp);
            yield return PerformAction.Sfx("Wolf", 0f);
            yield return PerformAction.Gun(Battle.Player, enemyUnit, "盛宴", 0f);
            yield return AttackAction(enemyUnit, Damage2, "结界猛击B");
            if (!enemyUnit.IsAlive)
            {
                yield return new HealAction(enemyUnit, Battle.Player, Value2, HealType.Vampire, 0.2f);
            }
        }
        protected override void OnLeaveBattle()
        {
            if (Summoned)
            {
                GameRun.CanViewDrawZoneActualOrder -= 1;
            }
        }
    }
}
