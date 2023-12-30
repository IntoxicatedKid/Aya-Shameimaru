using LBoL.ConfigData;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System;
using System.Collections.Generic;
using static AyaShameimaru.BepinexPlugin;
using UnityEngine;
using LBoL.Core;
using LBoL.Base;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.EntityLib.Exhibits;
using JetBrains.Annotations;
using AyaShameimaru.StatusEffects;
using LBoL.EntityLib.Cards.Enemy;

namespace AyaShameimaru.Exhibits
{
    public sealed class AyaGDef : ExhibitTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaG);
        }
        public override LocalizationOption LoadLocalization()
        {
            var gl = new GlobalLocalization(directorySource);
            gl.DiscoverAndLoadLocFiles(this);
            return gl;
        }
        public override ExhibitSprites LoadSprite()
        {
            // embedded resource folders are separated by a dot
            var folder = "";
            var exhibitSprites = new ExhibitSprites();
            Func<string, Sprite> wrap = (s) => ResourceLoader.LoadSprite(folder + GetId() + s + ".png", embeddedSource);
            exhibitSprites.main = wrap("");
            return exhibitSprites;
        }
        public override ExhibitConfig MakeConfig()
        {
            var exhibitConfig = new ExhibitConfig(
                Index: sequenceTable.Next(typeof(ExhibitConfig)),
                Id: "",
                Order: 10,
                IsDebug: false,
                IsPooled: false,
                IsSentinel: false,
                Revealable: false,
                Appearance: AppearanceType.Anywhere,
                Owner: "AyaPlayerUnit",
                LosableType: ExhibitLosableType.DebutLosable,
                Rarity: Rarity.Shining,
                Value1: 10,
                Value2: null,
                Value3: null,
                Mana: null,
                BaseManaRequirement: null,
                BaseManaColor: ManaColor.Green,
                BaseManaAmount: 1,
                HasCounter: false,
                InitialCounter: null,
                Keywords: Keyword.None,
                RelativeEffects: new List<string>() { },
                RelativeCards: new List<string>() { "AyaNews" }
            );
            return exhibitConfig;
        }
        [EntityLogic(typeof(AyaGDef))]
        [UsedImplicitly]
        public sealed class AyaG : ShiningExhibit
        {
            protected override void OnEnterBattle()
            {
                ReactBattleEvent(Battle.BattleStarted, new EventSequencedReactor<GameEventArgs>(OnBattleStarted));
                ReactBattleEvent(Battle.CardUsed, new EventSequencedReactor<CardUsingEventArgs>(OnCardUsed));
            }
            private IEnumerable<BattleAction> OnBattleStarted(GameEventArgs args)
            {
                Active = true;
                if (!Owner.HasStatusEffect<AyaPassiveSeDef.AyaPassiveSe>())
                {
                    yield return new ApplyStatusEffectAction(typeof(AyaPassiveSeDef.AyaPassiveSe), Owner, null, null, null, null, 0f, true);
                }
            }
            private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
            {
                if (Active && args.Card is AyaNews)
                {
                    NotifyActivating();
                    Active = false;
                    yield return new GainMoneyAction(Value1, SpecialSourceType.None);
                }
                yield break;
            }
            protected override void OnLeaveBattle()
            {
                Active = false;
            }
        }
    }
}