using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Frostrial
{

	partial class Player : Sandbox.Player
	{
		[Net] public bool ShopOpen { get; set; } = false;

		public void HandleShopping()
		{

		}

		[ServerCmd]
		public static void CloseShop()
		{

			Player player = ConsoleSystem.Caller.Pawn as Player;

			player.ShopOpen = false;
			player.BlockMovement = false;

		}

		[ServerCmd]
		public static void BuyBait()
		{

		}

	}

	public class Shop : Panel
	{

		Button button1;
		Button button2;
		Button button3;
		Button button4;
		Button button5;
		Button button6;
		Label text1;
		Label text2;
		Label text3;
		Label text4;
		Label text5;
		Label text6;

		public Shop()
		{

			Player player = Local.Pawn as Player;

			Panel shopPanel = Add.Panel( "Shop" );

			button1 = shopPanel.Add.Button( "", "button", () =>
			{


			} );

			text1 = button1.Add.Label( "Buy Bait", "title" );

			button2 = shopPanel.Add.Button( "", "button", () =>

			{


			} );

			text2 = button2.Add.Label( "Buy Campfire", "title" );

			button3 = shopPanel.Add.Button( "", "button", () =>
			{


			} );

			text3 = button3.Add.Label( "Upgrade Coat", "title" );

			button4 = shopPanel.Add.Button( "", "button", () =>

			{


			} );

			text4 = button4.Add.Label( "Upgrade Drill", "title" );

			button5 = shopPanel.Add.Button( "", "button", () =>
			{


			} );

			text5 = button5.Add.Label( "Upgrade Rod", "title" );

			button6 = shopPanel.Add.Button( "", "button", () =>

			{


			} );

			text6 = button6.Add.Label( "BUY PLANE TICKET", "title" );

		}

		public override void OnButtonEvent( ButtonEvent e )
		{

			base.OnButtonEvent( e );

			if ( e.Pressed == true && e.Button == "mouseright" )
			{

				Player.CloseShop();

			}

		}

		public override void Tick()
		{
			Player player = Local.Pawn as Player;

			Parent.Style.PointerEvents = player.ShopOpen ? "all" : "visible";
			Style.Opacity = player.ShopOpen ? 1 : 0;

			text1.Text = $"Buy Bait ({player.Baits})";
			text2.Text = $"Buy Campfire ({player.Campfires})";
			text3.Text = $"Upgrade Coat ({0})"; //TODO Actual stuff + Do "BOUGHT" after buying
			text4.Text = $"Upgrade Drill ({0})";
			text5.Text = $"Upgrade Rod ({0})";
			text6.Text = $"BUY PLANCE TICKET";

		}

	}

}
