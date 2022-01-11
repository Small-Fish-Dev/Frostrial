using Sandbox;

namespace Frostrial
{
	
	public partial class Player
	{
		[Net] public string HintText { get; set; } = "";
		[Net] public float HintLifeTime { get; set; }
		[Net] public float HintLifeDuration { get; set; }
		[Net] public int Jumpscare { get; set; } = 0;
		[Net] public RealTimeUntil JumpscareTimer { get; set; } = 0;
		[Net] public float? ForceUnskippable { get; set; }
		public bool Curtains { get; set; } = true;
		public bool OpenMap { get; set; }
		[Net] public RealTimeSince SpawnedSince { get; set; } = 0f;

		public void Hint( string text, float duration = 1f, bool unskippable = false ) // "Unskippable" dialog will be skipped by other unskippale dialogs
		{

			if ( ForceUnskippable == null || Time.Now >= ForceUnskippable )
			{

				HintText = text;
				HintLifeDuration = duration;
				HintLifeTime = Time.Now;

			}

			if ( unskippable )
			{

				ForceUnskippable = Time.Now + duration;

				HintText = text;
				HintLifeDuration = duration;
				HintLifeTime = Time.Now;

			}

		}

		public void HandleHUD()
		{

			if ( IsClient )
			{

				OpenMap = Input.Down( InputButton.Score );

			}

			if ( SpawnedSince >= 4f && SpawnedSince <= 5f ) //Just so I can be lazy and be able to put it back later on
			{
				Hint( "Today is the day I buy my way out of here.", 5, true );
				Curtains = false;

			}

		}

	}
}
