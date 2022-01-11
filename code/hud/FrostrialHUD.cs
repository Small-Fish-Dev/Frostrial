using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;
using System.Collections.Generic;

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

			var hut = Game.HutEntity;
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

	public class Hint : Panel
	{

		Panel hintContainer;
		Label hintTitle;

		public Hint()
		{

			Player player = Local.Pawn as Player;

			hintContainer = Add.Panel( "Hint" ).Add.Panel( "HintContainer" );
			hintTitle = hintContainer.Add.Label( "Lorem Ipsum", "HintTitle" );

		}

		public override void Tick()
		{

			float fadeTime = 1f;
			float textSpeed = 20f; // Letters per second

			if ( Local.Pawn is not Player { Camera: IsometricCamera camera } player )
				return;

			hintTitle.Text = player.HintText.Truncate( (int)(Math.Max( Time.Now - player.HintLifeTime, 0 ) * textSpeed) );
			hintTitle.Style.FontSize = 20 / camera.Zoom;
			hintTitle.Style.TextStrokeWidth = 3 / camera.Zoom;
			hintTitle.Style.TextStrokeColor = Color.Black;
			hintContainer.Style.Top = Length.Pixels( 360 * camera.Zoom - 1000 );

			// Don't punish me, RealTimeSince doesn't seem to work when networked
			Style.Opacity = Math.Clamp( player.HintLifeDuration + fadeTime - (Time.Now - player.HintLifeTime), 0, 1 );
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
			var player = Local.Pawn as Player;

			curtainsTitle.Text = Game.CurrentTitle;
			curtainsSubtitle.Text = Game.CurrentSubtitle;

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
			RootPanel.AddChild<Hint>();
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
