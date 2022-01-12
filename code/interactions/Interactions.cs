using Sandbox;

namespace Frostrial
{

	partial class Player : Sandbox.Player
	{

		public float InteractionRange { get; set; } = 40f;
		public float InteractionMaxDistance { get; set; } = 120f;

		protected override void TickPlayerUse()
		{
			if ( !IsServer ) return;

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

			if ( MouseEntityPoint != null && MouseEntityPoint is IUse && MouseEntityPoint.IsValid ) // If we are pointing at the valid interactive entity
				return MouseEntityPoint; // then return it

			var selectedEntity = Game.NearestInteractiveEntity( MouseWorldPosition, InteractionRange );

			if ( selectedEntity is not WorldEntity || selectedEntity.Position.Distance( Position ) > InteractionMaxDistance )
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
