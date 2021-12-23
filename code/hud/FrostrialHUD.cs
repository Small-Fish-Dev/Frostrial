using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System.Collections.Generic;
using System;
using System.Numerics;

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

			Game current = Game.Current as Game;
			var player = Local.Pawn as Player;
			var hut = current.HutEntity;
			var hutScreen = hut.Position.ToScreen();
			var left = MathX.Clamp( hutScreen.x, 0.21f, 0.75f );
			var top = MathX.Clamp( hutScreen.y, 0f, 1f);

			Style.Left = Length.Pixels( ( left - 0.5f ) * 3000 );
			Style.Top = Length.Pixels( ( top + 0.1f ) * 800 );


			var camera = player.Camera as IsometricCamera;
			var baseDistance = 1000f * camera.Zoom;
			var baseOpacity = 0.4f;
			var dangerLevel = 1 - player.Warmth;

			Style.Opacity = MathX.Clamp( ( player.Position.Distance( hut.Position ) - baseDistance ) / baseDistance , 0, 1f ) * baseOpacity * ( baseOpacity + 1 / baseOpacity * dangerLevel );

			var rotation = -MathX.RadianToDegree( (float)Math.Atan2( 0.5f - hutScreen.x, 0.5 - hutScreen.y ) );

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

			Player player = Local.Pawn as Player;
			IsometricCamera camera = player.Camera as IsometricCamera;

			hintTitle.Text = player.HintText.Truncate( (int)( Math.Max( Time.Now - player.HintLifeTime, 0 ) * textSpeed ) );
			hintTitle.Style.FontSize = 20 / camera.Zoom;
			hintTitle.Style.TextStrokeWidth = 3 / camera.Zoom;
			hintTitle.Style.TextStrokeColor = Color.Black;
			hintContainer.Style.Top = Length.Pixels( 260 * camera.Zoom - 650 );

			// Don't punish me, RealTimeSince doesn't seem to work when networked
			Style.Opacity = Math.Clamp( player.HintLifeDuration + fadeTime - ( Time.Now - player.HintLifeTime ), 0, 1 );
		}

	}


	public class Interact : Panel
	{

		Panel interactContainer;
		Label interactTitle;

		public Interact()
		{

			Player player = Local.Pawn as Player;

			interactContainer = Add.Panel( "Interact" ).Add.Panel( "InteractContainer" );
			interactTitle = interactContainer.Add.Label( "Lorem Ipsum", "InteractTitle" );

		}

		public override void Tick()
		{

			Player player = Local.Pawn as Player;

			string type = Game.NearestEntity( player.MouseWorldPosition, player.InteractionRange ).GetType().Name;
			string text = Game.InteractionsText.ContainsKey( type ) ? Game.InteractionsText[type] : "";

			if ( player.PlacingCampfire )
			{

				text = "Click to place down the campfire";

			}

			if ( player.ItemsOpen )
			{

				text = "Select an item to use";

			}

			if ( player.Fishing )
			{

				text = "Let go to catch the fish as it bites";

			}

			if ( player.ShopOpen )
			{

				text = "Buy items or Upgrades";

			}

			interactTitle.Text = text;

		}

	}

	public class Money : Panel
	{
		Panel moneyContainer;
		Label moneyTitle;
		Label moreMoneyTitle;
		Label lessMoneyTitle;

		public Money()
		{

			Player player = Local.Pawn as Player;

			moneyContainer = Add.Panel( "Money" ).Add.Panel( "moneyContainer" );
			moneyTitle = moneyContainer.Add.Label( "Lorem Ipsum", "moneyTitle" );
			moreMoneyTitle = moneyContainer.Add.Label( "Lorem Ipsum", "moreMoney" );
			lessMoneyTitle = moneyContainer.Add.Label( "Lorem Ipsum", "lessMoney" );

		}

		public override void Tick()
		{

			Player player = Local.Pawn as Player;

			double text = Math.Round( player.Money, 2 );
			moneyTitle.Text = $"$ { text }";

			moreMoneyTitle.Text = player.LastProfit > 0 ? $"$+{player.LastProfit}" : "" ;
			lessMoneyTitle.Text = player.LastProfit < 0 ? $"${player.LastProfit}" : "" ;

			moreMoneyTitle.Style.Opacity = player.ProfitTime;
			lessMoneyTitle.Style.Opacity = player.ProfitTime;

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
			playerPanel.Style.Left = Length.Fraction( Math.Clamp( ( pos.x - 550 ) / 9200 + 0.5f, 0.03f, 0.9f ) ); // This took a while to find the good map spot
			playerPanel.Style.Top = Length.Fraction( Math.Clamp( ( -pos.y - 500 )  / 10000 + 0.5f, 0.03f, 0.9f ) );

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
			curtainsTitle = curtainsPanel.Add.Panel( "titleContainer" ).Add.Label( "Wake Up", "curtainsTitle" );
			curtainsSubtitle = curtainsPanel.Add.Panel( "subtitleContainer" ).Add.Label( "Today might just be your last day here", "curtainsSubtitle" );

		}

		public override void Tick()
		{
			var player = Local.Pawn as Player;

			curtainsPanel.Parent.SetClass( "closed", !player.Curtains );

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

			pp.FreezeStrength = 1 - player.Warmth;

		}

	}

	partial class Player : Sandbox.Player
	{
		[Net] public string HintText { get; set; } = "";
		[Net] public float HintLifeTime { get; set; } = 0f;
		[Net] public float HintLifeDuration { get; set; } = 0f;
		public bool Curtains { get; set; } = true;
		public bool OpenMap { get; set; } = false;
		RealTimeSince spawnedSince { get; set; } = 0f; 

		public void Hint( string text, float duration = 1f )
		{

			HintText = text;
			HintLifeDuration = duration;
			HintLifeTime = Time.Now;

		}

		public void HandleHUD()
		{

			if ( IsClient )
			{

				if ( Input.Down( InputButton.Score ) )
				{

					OpenMap = true;

				}
				else
				{

					OpenMap = false;

				}

				if ( spawnedSince >= 4f && spawnedSince <= 5f ) //Just so I can be lazy and be able to put it back later on
				{

					Curtains = false;

				}

			}

		}

		[ClientRpc]
		public static void EndGameOnClients()
		{

			Player player = Local.Pawn as Player;

			player.Curtains = true;

		}

	}

}
