using Sandbox;
using System.Collections.Generic;

namespace Frostrial
{
	public partial class MusicPlayer
	{
		public List<Music> Queue { get; internal set; } = new();
		public bool IsInitialized { get; internal set; }
		public float TracksDelay { get; set; } = 30f;
		public float Volume { get; set; } = 0.5f;

		protected int currentQueuePosition = 0;
		protected bool isWaitingForTheNextTrack = false;
		protected RealTimeSince tsLastTrack = 0;
		protected RealTimeSince tsTrackStart = 0;

		public void Initialize()
		{
			if ( IsInitialized )
				return;

			foreach ( var (key, value) in Music.All )
			{
				Log.Info( key );
				Queue.Add( value );
			}
			Shuffle( true );

			PlayNext( false );

			IsInitialized = true;
		}

		public void Tick()
		{
			if ( !IsInitialized )
				return;

#if false
			DebugOverlay.ScreenText( $"{isWaitingForTheNextTrack}\n{tsLastTrack}:{TracksDelay}\n{tsTrackStart}:{Queue[currentQueuePosition].Length}" );
#endif

			if ( isWaitingForTheNextTrack )
			{
				if ( tsLastTrack >= TracksDelay )
				{
					isWaitingForTheNextTrack = false;
					PlayNext();
				}
			}
			else if ( tsTrackStart >= Queue[currentQueuePosition].Length )
			{
				isWaitingForTheNextTrack = true;
				tsLastTrack = 0;
			}
		}

		public void PlayNext( bool increment = true )
		{
			if ( increment )
			{
				var nextTrack = (currentQueuePosition + 1) % Queue.Count;
				if ( nextTrack < currentQueuePosition )
				{
					Shuffle();
				}
				currentQueuePosition = nextTrack;
			}

			NowPlaying( Queue[currentQueuePosition] );

			tsTrackStart = 0;
			Queue[currentQueuePosition].Play( Volume );
		}

		protected void Shuffle(bool ignoreFLRepeat = false)
		{
			Log.Info( $"Shuffle! {Queue.Count}" );
			Music lastTrack = Queue[^1];
			for ( var i = 0; i < Queue.Count - 1; i++ )
			{
				for ( var j = i + 1; j < Queue.Count; j++ )
				{
					if ( System.Random.Shared.Int( 0, 1 ) != 0 )
						continue;

					if ( !ignoreFLRepeat
					     && i == 0 && j == Queue.Count - 1
					     && Queue[j].ResourceId == lastTrack.ResourceId ) // If we are swapping the first and the last queue entry, make sure we won't make the listener to hear the same song twice
						continue;
				
					(Queue[i], Queue[j]) = (Queue[j], Queue[i]);
				}
			}
		}

		protected void NowPlaying( Music m )
		{
			Event.Run( "frostrial.next_song", m );
		}
	}
}
