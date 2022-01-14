using Sandbox;

namespace Frostrial
{

	partial class Player : Sandbox.Player
	{

		public float InteractionRange { get; set; } = 40f;
		public float InteractionMaxDistance { get; set; } = 120f;

		protected override void TickPlayerUse()
		{
			if ( !IsServer || BlockMovement ) return;

			using ( Prediction.Off() )
			{
				if ( Input.Pressed( InputButton.Attack2 ) )
				{
					PlayClick();

					var found = FindUsable();

					if ( found is WorldEntity )
					{
						Say( VoiceLine.ClickOnGround );
						return;
					}

					if ( found == null )
					{
						Say( VoiceLine.TooFarAway );
						return;
					}

					if ( found is IUse use && use.OnUse( this ) )
						return;
				}
			}
		}

		protected override Entity FindUsable()
		{
			Host.AssertServer(  );

			var mep = MouseEntityPoint;
			var mwp = MouseWorldPosition;
			if ( mep != null && mep is IUse && mep.IsValid && mwp.Distance( Position ) <= InteractionMaxDistance ) // If we are pointing at the valid interactive entity
				return mep; // then return it

			var selectedEntity = Game.NearestInteractiveEntity( mwp, InteractionRange );

			if ( selectedEntity is not WorldEntity && mwp.Distance( Position ) > InteractionMaxDistance )
				return null;

			return selectedEntity;
		}

		[ClientRpc]
		public static void PlayClick()
		{

			Sound.FromScreen( "button_click" );

		}

		[ClientRpc]
		public static void Play3D( string sound, Entity source )
		{

			Sound.FromEntity( sound, source );

		}

		[ClientRpc]
		public static void FishPoopoo()
		{

			Event.Run( "frostrial.fish_caught", "fishaupoopoocaca", false );

		}

	}

}
