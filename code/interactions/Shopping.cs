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

		Button baitButton;
		Button campfireButton;
		Button coatButton;
		Button drillButton;
		Button rodButton;
		Button planeButton;
		Label baitText;
		Label campfireText;
		Label coatText;
		Label drillText;
		Label rodText;
		Label planeText;

		public Shop()
		{

			Player player = Local.Pawn as Player;

			Panel shopPanel = Add.Panel( "Shop" );

			Add.Panel( "Close" ).Add.Button( "", "button", () =>
			{

				Player.CloseShop();

			} ).Add.Label( "Close", "title" );

			baitButton = shopPanel.Add.Button( "", "button", () =>
			{


			} );

			baitText = baitButton.Add.Label( "Buy Bait", "title" );

			campfireButton = shopPanel.Add.Button( "", "button", () =>

			{


			} );

			campfireText = campfireButton.Add.Label( "Buy Campfire", "title" );

			coatButton = shopPanel.Add.Button( "", "button", () =>
			{


			} );

			coatText = coatButton.Add.Label( "Upgrade Coat", "title" );

			drillButton = shopPanel.Add.Button( "", "button", () =>

			{


			} );

			drillText = drillButton.Add.Label( "Upgrade Drill", "title" );

			rodButton = shopPanel.Add.Button( "", "button", () =>
			{


			} );

			rodText = rodButton.Add.Label( "Upgrade Rod", "title" );

			planeButton = shopPanel.Add.Button( "", "button", () =>

			{


			} );

			planeText = planeButton.Add.Label( "BUY PLANE TICKET", "title" );

		}

		public override void Tick()
		{
			Player player = Local.Pawn as Player;

			Parent.Style.PointerEvents = player.ShopOpen ? "all" : "visible";
			Style.Opacity = player.ShopOpen ? 1 : 0;

			baitText.Text = $"Buy Bait ({player.Baits})";
			campfireText.Text = $"Buy Campfire ({player.Campfires})";
			coatText.Text = $"Upgrade Coat ({0})"; //TODO Actual stuff + Do "BOUGHT" after buying
			drillText.Text = $"Upgrade Drill ({0})";
			rodText.Text = $"Upgrade Rod ({0})";
			planeText.Text = $"BUY PLANE TICKET";

		}

	}

}
