using Sandbox;

namespace Frostrial
{
	public class VoiceLinePlayer
	{
		protected Sound currentSound;
		protected TimeUntil TimeUntilCanSkip = 0;
		protected Player player;

		public VoiceLinePlayer( Player player )
		{
			this.player = player;
		}

		public void Delay( float time )
		{
			TimeUntilCanSkip = time;
		}

		protected Monologue? GetLineAndStopSound( VoiceLine m )
		{
			if ( !Player.VoiceLines.ContainsKey( m ) )
			{
				Log.Warning( $"Warning: a voice line for {m} is not found!" );
				return null;
			}

			var vl = Player.VoiceLines[m];

			if ( vl.CanSkip && TimeUntilCanSkip > 0 )
				return null;

			if ( !vl.CanSkip )
				TimeUntilCanSkip = vl.Duration;

			if ( currentSound.Index != 0 )
				currentSound.Stop();

			return vl;
		}

		public void PlayVoiceLine( VoiceLine m )
		{
			if ( GetLineAndStopSound( m ) is not { } vl )
				return;

			currentSound = player.PlaySound( vl.Sound );
			Game.Instance.HudEntity.SpeechBubbles.Say( player.Client, vl ); // TODO: make a 3d subtitle panel
		}
	}
}
