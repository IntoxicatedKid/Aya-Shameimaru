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
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle;
using System;

namespace AyaShameimaru.StatusEffects
{
    public sealed class AyaPassiveSeDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaPassiveSe);
        }

        public override LocalizationOption LoadLocalization()
        {
            var gl = new GlobalLocalization(directorySource);
            gl.DiscoverAndLoadLocFiles(this);
            return gl;
        }

        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("Resources.AyaPassiveSe.png", embeddedSource);
        }
        public override StatusEffectConfig MakeConfig()
        {
            var statusEffectConfig = new StatusEffectConfig(
                Index: sequenceTable.Next(typeof(StatusEffectConfig)),
                Id: "",
                Order: -1,
                Type: StatusEffectType.Special,
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
                Keywords: Keyword.Ethereal | Keyword.Replenish,
                RelativeEffects: new List<string>() { },
                VFX: "Default",
                VFXloop: "Default",
                SFX: "Default"
            );
            return statusEffectConfig;
        }


        [EntityLogic(typeof(AyaPassiveSeDef))]
        public sealed class AyaPassiveSe : StatusEffect
        {
            protected override void OnAdded(Unit unit)
            {
                foreach (Card card in Battle.EnumerateAllCards())
                {
                    if (card.CardType == CardType.Status && card.DebugName.Contains("News"))
                    {
                        card.DeltaDamage += 10;
                        card.IsEthereal = true;
                        card.IsReplenish = true;
                    }
                }
                HandleOwnerEvent(Battle.CardsAddedToDiscard, new GameEventHandler<CardsEventArgs>(OnAddCard));
                HandleOwnerEvent(Battle.CardsAddedToHand, new GameEventHandler<CardsEventArgs>(OnAddCard));
                HandleOwnerEvent(Battle.CardsAddedToExile, new GameEventHandler<CardsEventArgs>(OnAddCard));
                HandleOwnerEvent(Battle.CardsAddedToDrawZone, new GameEventHandler<CardsAddingToDrawZoneEventArgs>(OnAddCardToDraw));
                foreach (EnemyUnit enemyUnit in Battle.AllAliveEnemies)
                {
                    if (!enemyUnit.HasStatusEffect<AyaWalletSeDef.AyaWalletSe>())
                    {
                        int num = Math.Max(GameRun.GameRunEventRng.NextInt(enemyUnit.Config.PowerLoot.Min, enemyUnit.Config.PowerLoot.Max), 10);
                        React(new ApplyStatusEffectAction(typeof(AyaWalletSeDef.AyaWalletSe), enemyUnit, num, null, null, null, 0f, true));
                    }
                }
                HandleOwnerEvent(Battle.EnemySpawned, new GameEventHandler<UnitEventArgs>(OnEnemySpawned));
            }
            private void OnAddCard(CardsEventArgs args)
            {
                foreach (Card card in args.Cards)
                {
                    if (card.CardType == CardType.Status && card.DebugName.Contains("News"))
                    {
                        card.DeltaDamage += 10;
                        card.IsEthereal = true;
                        card.IsReplenish = true;
                    }
                }
            }
            private void OnAddCardToDraw(CardsAddingToDrawZoneEventArgs args)
            {
                foreach (Card card in args.Cards)
                {
                    if (card.CardType == CardType.Status && card.DebugName.Contains("News"))
                    {
                        card.DeltaDamage += 10;
                        card.IsEthereal = true;
                        card.IsReplenish = true;
                    }
                }
            }
            private void OnEnemySpawned(UnitEventArgs args)
            {
                EnemyUnit enemyUnit = args.Unit as EnemyUnit;
                if (!enemyUnit.HasStatusEffect<AyaWalletSeDef.AyaWalletSe>())
                {
                    React(new ApplyStatusEffectAction(typeof(AyaWalletSeDef.AyaWalletSe), enemyUnit, enemyUnit.Power, null, null, null, 0f, true));
                }
            }
        }
    }
}