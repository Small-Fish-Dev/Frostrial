using Sandbox;
using System;
using System.Collections.Generic;

namespace Frostrial
{

	public partial class Player
	{
		[Net] public int Jumpscare { get; set; } = 0;
		[Net] public RealTimeUntil JumpscareTimer { get; set; } = 0;
		public bool Curtains { get; set; } = true;
		public bool OpenMap { get; set; }
		[Net] public RealTimeSince SpawnedSince { get; set; } = 0f;

		// Dear ubre:
		// please uncomment these lines as soon as ceitine makes a v/o for them
		public static readonly Dictionary<VoiceLine, Monologue> VoiceLines = new()
		{
			{ VoiceLine.AngrySwear, new( "*********!", "angryswear" ) },
			{ VoiceLine.HappySwear, new( "Holy ****!", "happyswear" ) },
			{ VoiceLine.CantDrillOnThere, new( "I can't drill on there!", "cantdrillonthere" ) },
			{ VoiceLine.CaughtSmallFish, new Monologue( "This isn't going to cut it", "" ) },
			{ VoiceLine.CaughtFish, new Monologue( "There we go!", "" ) },
			{ VoiceLine.CaughtBigFish, new Monologue( "That's a big one!", "" ) },
			{ VoiceLine.LeFishe, new ( "I can always count on French Cuisine", "" ) },
			{ VoiceLine.Fishing, new( ".   .   .   .   ." ) },
			{ VoiceLine.ClickOnGround, new( "I hate this place.", "clickonground" ) },
			{ VoiceLine.TooFarAway, new( "That's too far away!", "toofaraway" ) },
			{ VoiceLine.Intro, new( "Today is the day I buy my way out of here.", "", false ) },
			{ VoiceLine.ImAlmostThere, new( "I'm almost there", "imalmostthere" ) },
			{ VoiceLine.Outro, new( "Goodbye fishes.", "" ) },
			{ VoiceLine.OldYetiHand, new( "This Yeti Hand is old, lucky", "", false ) },
			{ VoiceLine.NotDrillingHere, new( "I'm not drilling there.", "notdrillinghere" ) },
			{ VoiceLine.FoundBait, new( "I found a worm, good for bait I guess.", "" ) },
			{ VoiceLine.NoItems, new( "I don't have any of those...", "noitems" ) },
			{ VoiceLine.CantPlace, new( "I can't put that there...", "" ) },
			{ VoiceLine.UsedBait, new( "Oops, some slipped out", "usedbait" ) },
			{ VoiceLine.FinnishYeti, new( "It's the Finnish Yeti! I must head back to the cabin!", "finnishyeti", false ) },
			{ VoiceLine.YetiJumpscare, new( "********* ! ! !", "", false ) }
		};

		public static void CheckVoiceLines()
		{
			foreach ( VoiceLine e in Enum.GetValues( typeof( VoiceLine ) ) )
			{
				if ( !VoiceLines.ContainsKey( e ) )
				{
					Log.Warning( $"Warning: a voice line for {e} is not found!" );
				}
			}
		}

		[ClientRpc]
		public void Say( VoiceLine vl )
		{
			vlp.PlayVoiceLine( vl );
		}

		[ClientRpc]
		public void Delay( float time )
		{
			vlp.Delay( time );
		}

		public void HandleHUD()
		{

			if ( IsClient )
			{

				if ( Input.Pressed( Input_Map ) )
				{

					OpenMap = !OpenMap;

				}

				if ( Curtains && SpawnedSince >= 4f ) //Just so I can be lazy and be able to put it back later on
				{
					Say( VoiceLine.Intro );
					Curtains = false;

				}
			}

		}

	}
}
