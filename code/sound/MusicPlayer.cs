using Sandbox;
using System.Collections.Generic;

namespace Frostrial
{
	public partial class MusicPlayer
	{
		public List<Music> Queue { get; internal set; } = new();
		public bool IsInitialized { get; internal set; } = false;
		public float TracksDelay { get; set; } = 30.0f;

		protected int currentQueuePosition = 0;
		protected bool isWaitingForTheNextTrack = false;
		protected RealTimeSince tsLastTrack = 0;
		protected RealTimeSince tsTrackStart = 0;

		public void Initialize()
		{
			foreach ( var m in Music.All )
			{
				Log.Info( m.Key );
				Queue.Add( m.Value );
			}
			Shuffle();

			PlayNext(false);

			IsInitialized = true;
		}

		public void Tick()
		{
			if ( !IsInitialized )
				return;

			if (isWaitingForTheNextTrack)
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

		public void PlayNext(bool increment = true)
		{
			if (increment)
			{
				int nextTrack = (currentQueuePosition + 1) % Queue.Count;
				if (nextTrack < currentQueuePosition)
				{
					Shuffle();
				}
				currentQueuePosition = nextTrack;
			}

			NowPlaying( Queue[currentQueuePosition] );

			tsTrackStart = 0;
			Queue[currentQueuePosition].Play();
		}

		protected void Shuffle()
		{
			Log.Info( $"Shuffle! {Queue.Count}" );
			for ( int i = 0; i < Queue.Count - 1; i++ )
			{
				for (int j = i + 1; j < Queue.Count; j++ )
				{
					if (System.Random.Shared.Int(0, 1) == 0)
					{
						(Queue[i], Queue[j]) = (Queue[j], Queue[i]);
					}
				}
			}
		}

		protected void NowPlaying(Music m)
		{
			// TOOD:
			Log.Info( $"Now playing: {m.Song} from {m.Album} by {m.Artist} ({m.URL})" );
		}
	}
}
