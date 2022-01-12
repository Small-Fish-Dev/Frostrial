using Sandbox;
using System.Linq;

namespace Frostrial
{
	public enum VoiceLine
	{
		NotDrillingHere,
		CantDrillOnThere,
		LetsSee,
		Idiot,
		ImAlmostThere,
		OldYetiHand,
		FinnishYeti,
		CaughtSmallFish,
		CaughtFish,
		CaughtBigFish,
		AngrySwear,
		HappySwear,
		TooFarAway,
		ClickOnGround,
		CantPlace,
		UsedBait,
		NoItems,
		Intro,
		FoundBait,
		Outro,
		WakeUp,
		WhatHappened,
		LeFishe,
		Fishing,
		YetiJumpscare
	};

	public struct Monologue
	{
		public string Text { get; internal set; }
		public string Sound { get; internal set; }
		public float Duration { get; internal set; }
		public bool CanSkip { get; internal set; }

		private const float WPS = 225f / 60; // Average WPM for an adult - 225

		public Monologue( string text, string sound = null, bool canSkip = true, float duration = 0 )
		{
			Text = text;
			Sound = sound;
			CanSkip = canSkip;

			Duration = duration > 0 ? duration : Text.Split( ' ' ).Length / WPS + Text.Count( char.IsWhiteSpace ) * 0.2f;

			if ( sound != null )
				Precache.Add( $"sounds/voicelines/{sound}.vsnd" );
		}
	}
}
