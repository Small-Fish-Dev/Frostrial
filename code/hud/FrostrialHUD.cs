using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Frostrial
{

	public class HutIndicator : Panel
	{

		Panel arrow;

		public HutIndicator()
		{

			Panel hut = Add.Panel( "HutIndicator" );
			arrow = hut.Add.Panel( "Arrow" );

		}

		public override void Tick()
		{
			if ( Local.Pawn is not Player player )
				return;

			var hut = Game.Instance.HutEntity;
			var hutScreen = hut.Position.ToScreen();
			var left = hutScreen.x.Clamp( 0.21f, 0.75f );
			var top = hutScreen.y.Clamp( 0f, 1f );

			Style.Left = Length.Pixels( (left - 0.5f) * 3000 );
			Style.Top = Length.Pixels( (top + 0.1f) * 800 );


			if ( player.Camera is not IsometricCamera camera )
				return;

			var baseDistance = 1000f * camera.Zoom;
			var baseOpacity = 0.4f;
			var dangerLevel = 1 - player.Warmth;

			Style.Opacity = ((player.Position.Distance( hut.Position ) - baseDistance) / baseDistance).Clamp( 0, 1f ) * baseOpacity * (baseOpacity + 1 / baseOpacity * dangerLevel);

			var rotation = -((float)Math.Atan2( 0.5f - hutScreen.x, 0.5 - hutScreen.y )).RadianToDegree();

			var arrowRotate = new PanelTransform();
			arrowRotate.AddRotation( 0, 0, rotation );

			arrow.Style.Transform = arrowRotate;


		}

	}

	public class SpeechBubbles : Panel
	{
		Dictionary<int, BaseSpeechBubble> ActiveTags = new Dictionary<int, BaseSpeechBubble>();

		public class BaseSpeechBubble : Panel
		{
			internal enum State
			{
				Hidden,
				Printing,
				Fade
			}

			public Label TextLabel { get; set; }
			public float TextDelay { get; set; } = 2f;

			TimeUntil TimeUntilFadeAway;
			TimeSince TimeSinceSaid;
			string text;
			float duration;
			State s;

			public BaseSpeechBubble( Player player )
			{
				TextLabel = Add.Label();
				TextLabel.Style.Opacity = 0;
			}

			public override void Tick()
			{
				base.Tick();

				switch ( s )
				{
					case State.Hidden:
						break;
					case State.Printing:
						TextLabel.Text = text.Truncate( (int)(text.Length * Math.Min( 1, TimeSinceSaid / duration )) );
						if ( TextLabel.TextLength == text.Length )
						{
							TimeUntilFadeAway = TextDelay;
							s = State.Fade;
						}
						break;
					case State.Fade:
						if ( TimeUntilFadeAway <= 0 )
						{
							TextLabel.Style.Opacity = 0;
							s = State.Hidden;
						}
						break;
				}
			}

			public void Say( Monologue m )
			{
				text = m.Text;
				duration = m.Duration;

				TextLabel.Style.Opacity = 1;
				TimeSinceSaid = 0;
				s = State.Printing;
			}
		}

		public override void Tick()
		{
			base.Tick();

			foreach ( var player in Entity.All.OfType<Player>() )
			{
				UpdateNameTag( player );
			}
		}

		[Event( "frostrial.player.left" )]
		protected void OnPlayerLeave( Client cl )
		{
			ActiveTags[cl.Id].Delete();
			ActiveTags.Remove( cl.Id );
		}

		public virtual BaseSpeechBubble CreateNameTag( Player player )
		{
			if ( player.Client == null )
				return null;

			var tag = new BaseSpeechBubble( player );
			tag.Parent = this;
			return tag;
		}

		public void UpdateNameTag( Player player )
		{
			if ( player.Client == null )
				return;

			// TODO: would be nice to have but sadly in Frostrial death results in kick
			/*if ( player.LifeState != LifeState.Alive )
				return false;*/

			//
			// Where we putting the label, in world coords
			//
			var head = new Transform( player.EyePos );

			var labelPos = head.Position + Vector3.Up * 10; // FIXME: magic number!!!!!!!!!!!!!!!

			// TODO - can we see them

			var fontSize = 30 / ((Local.Pawn.Camera as IsometricCamera)?.Zoom ?? 1);

			if ( !ActiveTags.TryGetValue( player.Client.Id, out var tag ) )
			{
				tag = CreateNameTag( player );
				if ( tag == null )
				{
					Log.Error( $"Fatal: failed to make a speech bubble for {player.Client.Name} ({player.Client.Id})" );
					return;
				}
				ActiveTags[player.Client.Id] = tag;
			}

			var screenPos = labelPos.ToScreen();

			tag.Style.Left = Length.Fraction( screenPos.x );
			tag.Style.Top = Length.Fraction( screenPos.y );

			tag.Style.FontSize = Length.Pixels( fontSize );
		}

		public void Say( Client cl, Monologue m )
		{
			if ( !ActiveTags.ContainsKey( cl.Id ) || ActiveTags[cl.Id] is not BaseSpeechBubble bubble )
				return;

			bubble.Say( m );
		}
	}

	public class Interact : Panel
	{

		Panel interactContainer;
		Label interactTitle;

		public Interact()
		{
			interactContainer = Add.Panel( "Interact" ).Add.Panel( "InteractContainer" );
			interactTitle = interactContainer.Add.Label( "Lorem Ipsum", "InteractTitle" );
		}

		public override void Tick()
		{

			if ( Local.Pawn is not Player player )
				return;

			string text = "";

			if ( player.PlacingCampfire )
				text = "Click to place down the campfire";
			else if ( player.ItemsOpen )
				text = "Select an item to use";
			else if ( player.Fishing )
				text = "Let go to catch the fish as it bites";
			else if ( player.ShopOpen )
				text = "Buy items or Upgrades ( Hold SHIFT to buy 10 )";
			else if ( Game.NearestDescribableEntity( player.MouseWorldPosition, player.InteractionRange ) is IDescription describable )
				text = describable.Description;

			interactTitle.Text = text;

		}

	}

	public class Money : Panel
	{
		Panel moneyContainer;
		Label moneyTitle;
		Label moneyDifferenceTitle;
		Label secondaryMoneyDifferenceTitle;

		float currentDifference;
		float secondaryDifference;

		RealTimeUntil ProfitTime { get; set; } = 0f;

		public Money()
		{
			moneyContainer = Add.Panel( "Money" ).Add.Panel( "moneyContainer" );
			moneyTitle = moneyContainer.Add.Label( "€0", "moneyTitle" );
			moneyDifferenceTitle = moneyContainer.Add.Label( "", "difference" );
			secondaryMoneyDifferenceTitle = moneyContainer.Add.Label( "", "difference secondary" );
		}

		public override void Tick()
		{
			moneyDifferenceTitle.Style.Opacity = ProfitTime;
			secondaryMoneyDifferenceTitle.Style.Opacity = ProfitTime;
		}

		[Event( "frostrial.money" )]
		protected void MoneyEvent( float difference )
		{
			if ( ProfitTime <= 0 )
				currentDifference = 0;

			secondaryDifference = difference;
			currentDifference += difference;
			ProfitTime = 2f;

			RenderMoney();
		}

		protected void RenderMoney()
		{
			Player player = Local.Pawn as Player;

			moneyTitle.Text = $"€ { Math.Round( player.Money, 2 ) }";

			moneyDifferenceTitle.Text = MoneyToString( currentDifference );
			secondaryMoneyDifferenceTitle.Text = secondaryDifference.AlmostEqual( currentDifference )
				? ""
				: $"({MoneyToString( secondaryDifference )})";

			ChangePanelClasses( moneyDifferenceTitle, currentDifference );
			ChangePanelClasses( secondaryMoneyDifferenceTitle, secondaryDifference );
		}

		protected void ChangePanelClasses( Panel p, float value )
		{
			p.SetClass( "negative", value < 0 );
		}

		protected string MoneyToString( float value )
		{
			return $"€{(value > 0 ? "+" : "")}{Math.Round( value, 2 )}";
		}

	}

	public class Map : Panel
	{

		Panel mapPanel;
		Panel playerPanel;
		public Map()
		{

			mapPanel = Add.Panel( "Map" ).Add.Panel( "MapContainer" );
			playerPanel = mapPanel.Add.Panel( "Player" );

		}

		public override void Tick()
		{

			Player player = Local.Pawn as Player;
			var pos = player.Position;
			mapPanel.SetClass( "open", player.OpenMap );
			playerPanel.Style.Left = Length.Fraction( Math.Clamp( (pos.x - 550) / 9200 + 0.5f, 0.03f, 0.9f ) ); // This took a while to find the good map spot
			playerPanel.Style.Top = Length.Fraction( Math.Clamp( (-pos.y - 500) / 10000 + 0.5f, 0.03f, 0.9f ) );

		}

	}

	public class Curtains : Panel
	{

		Panel curtainsPanel;
		Label curtainsTitle;
		Label curtainsSubtitle;
		public Curtains()
		{

			curtainsPanel = Add.Panel( "Curtains" ).Add.Panel( "MapContainer" );
			curtainsTitle = curtainsPanel.Add.Panel( "titleContainer" ).Add.Label( "null", "curtainsTitle" );
			curtainsSubtitle = curtainsPanel.Add.Panel( "subtitleContainer" ).Add.Label( "null", "curtainsSubtitle" );

		}

		public override void Tick()
		{
			if ( Local.Pawn is not Player player )
				return;

			curtainsTitle.Text = Game.Instance.CurrentTitle;
			curtainsSubtitle.Text = Game.Instance.CurrentSubtitle;

			curtainsPanel.Parent.SetClass( "closed", !player.Curtains );

		}

	}

	public class Jumpscare : Panel
	{

		Panel jumpscarePanel;
		Dictionary<int, string> jumpscareImages = new();
		public Jumpscare()
		{

			jumpscarePanel = Add.Panel( "Jumpscare" );

			jumpscareImages[1] = "ui/yeti_jumpscare.jpg";
			jumpscareImages[2] = "ui/yeti_window.jpg";
			jumpscareImages[3] = "ui/takeoff.jpg";

		}

		public override void Tick()
		{

			var player = Local.Pawn as Player;
			float opacity = 0;

			switch ( player.Jumpscare )
			{

				case 0:
					opacity = 0;
					break;
				case 1:
					opacity = 1;
					break;
				case 2:
					opacity = 1 - player.JumpscareTimer / 3;
					break;
				case 3:
					opacity = 1 - Math.Min( player.JumpscareTimer, 3 ) / 3;
					break;

			}

			// TODO PLAY SOUNDS (PLANE, SCREAM, CREEPY)

			jumpscarePanel.Style.Opacity = opacity;

			if ( player.Jumpscare != 0 )
			{

				jumpscarePanel.Style.SetBackgroundImage( jumpscareImages[player.Jumpscare] );

			}

		}

	}

	public class FishCaught : Panel
	{

		RealTimeSince TimeSinceBorn = 0;
		public FishCaught() { }

		public FishCaught( string species, bool variant ) : this()
		{

			Style.ZIndex = (int)Time.Now * 100 + 3;
			Style.SetBackgroundImage( Game.FishPictures[species][variant ? 1 : 0] );

		}

		public override void Tick()
		{
			base.Tick();

			if ( TimeSinceBorn > 2 )
			{

				Delete();

			}

		}

	}

	public class NowPlaying : Panel
	{
		Panel InfoContainer;
		Label Song, Album, Artist, URL;
		Image AlbumCover;

		RealTimeSince TimeSinceBorn = 0;

		public NowPlaying()
		{
		}

		public NowPlaying( Music music ) : this()
		{
			AlbumCover = Add.Image( $"/ui/album-covers/{music.AlbumCover}" );
			InfoContainer = Add.Panel( "info" );
			{
				Log.Info( $"Now playing: {music.Song} from {music.Album} by {music.Artist} ({music.URL})" );
				Song = InfoContainer.Add.Label( "", "big" );
				Album = InfoContainer.Add.Label();
				Artist = InfoContainer.Add.Label();
				URL = InfoContainer.Add.Label();

				Song.Text = music.Song;
				Album.Text = music.Album;
				Artist.Text = $"by {music.Artist}";
				URL.Text = music.URL;
			}
		}

		public override void Tick()
		{
			base.Tick();

			if ( TimeSinceBorn > 5 )
				Delete();
		}
	}

	public partial class FrostrialHUD : Sandbox.HudEntity<RootPanel>
	{
		public SpeechBubbles SpeechBubbles { get; internal set; }

		public FrostrialHUD()
		{

			if ( !IsClient ) return;

			var player = Local.Pawn as Player;

			PostProcess.Add( new StandardPostProcess() );

			RootPanel.StyleSheet.Load( "hud/FrostrialHUD.scss" );

			RootPanel.AddChild<Interact>();
			RootPanel.AddChild<Money>();
			RootPanel.AddChild<Map>();
			RootPanel.AddChild<HutIndicator>();
			RootPanel.AddChild<Jumpscare>();
			RootPanel.AddChild<Curtains>();
			SpeechBubbles = RootPanel.AddChild<SpeechBubbles>();
			RootPanel.AddChild<Items>();
			RootPanel.AddChild<Shop>();

			PostProcess.Add( new FreezePostProcessEffect() );

		}

		[Event.Tick.Client]
		private void ClientTick()
		{
			var player = Local.Pawn as Player;

			var pp = PostProcess.Get<FreezePostProcessEffect>();

			pp.FreezeStrength = 1 - (player?.Warmth ?? 1);

		}

		[Event( "frostrial.next_song" )]
		public void NextSong( Music music )
		{
			var np = new NowPlaying( music );
			RootPanel.AddChild( np );
		}

		[Event( "frostrial.fish_caught" )]
		public void AddFish( string species, bool variant )
		{

			var fish = new FishCaught( species, variant );
			RootPanel.AddChild( fish );
		}

	}

}
