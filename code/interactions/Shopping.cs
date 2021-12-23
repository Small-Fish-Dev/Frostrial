using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Frostrial
{

	partial class Player : Sandbox.Player
	{

		[Net] public float Money { get; set; } = 0f;
		[Net] public bool ShopOpen { get; set; } = false;
		[Net] public bool UpgradedDrill { get; set; } = false;
		[Net] public bool UpgradedRod { get; set; } = false;
		[Net] public bool UpgradedCoat { get; set; } = false;
		[Net] public float LastProfit { get; set; } = 0f;
		[Net] public RealTimeUntil ProfitTime { get; set; } = 0f;

		public void HandleShopping()
		{

			SetClothing( "jacket", UpgradedCoat ? "models/clothing/jackets/parka.vmdl" : "models/clothing/jackets/jumper.vmdl" );

			if ( Jumpscare == 3 )
			{

				if ( JumpscareTimer <= -2 )
				{

					Game.CurrentTitle = "Thank you for playing";
					Game.CurrentSubtitle = "Game made by SmallFish and friends for JamBox 2021";
					Curtains = true;

				}

				if ( JumpscareTimer <= -9 )
				{

					Curtains = false;
					Jumpscare = 1;

				}

			}

			if ( Jumpscare == 1 )
			{

				if ( JumpscareTimer <= -9.8f )
				{

					Client.Kick();

				}

			}

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

			Player player = ConsoleSystem.Caller.Pawn as Player;

			if ( player.Money >= Game.Prices["bait"] )
			{

				player.Baits++;
				player.AddMoney( -Game.Prices["bait"] );

			}

		}

		[ServerCmd]
		public static void BuyCampfire()
		{

			Player player = ConsoleSystem.Caller.Pawn as Player;

			if ( player.Money >= Game.Prices["campfire"] )
			{

				player.Campfires++;
				player.AddMoney( -Game.Prices["campfire"] );

			}

		}

		[ServerCmd]
		public static void UpgradeCoat()
		{

			Player player = ConsoleSystem.Caller.Pawn as Player;

			if ( player.Money >= Game.Prices["coat"] )
			{

				player.UpgradedCoat = true;
				player.AddMoney( -Game.Prices["coat"] );

			}

		}

		[ServerCmd]
		public static void UpgradeDrill()
		{

			Player player = ConsoleSystem.Caller.Pawn as Player;

			if ( player.Money >= Game.Prices["drill"] )
			{

				player.UpgradedDrill = true;
				player.AddMoney( -Game.Prices["drill"] );

			}

		}

		[ServerCmd]
		public static void UpgradeRod()
		{

			Player player = ConsoleSystem.Caller.Pawn as Player;

			if ( player.Money >= Game.Prices["rod"] )
			{

				player.UpgradedRod = true;
				player.AddMoney( -Game.Prices["rod"] );

			}


		}

		[ServerCmd]
		public static void Win()
		{

			Player player = ConsoleSystem.Caller.Pawn as Player;

			if ( player.Money >= Game.Prices["plane"] )
			{

				player.BlockMovement = true;
				player.Jumpscare = 3;
				player.JumpscareTimer = 6f;

				player.BlockMovement = true;
				player.Hint( "Goodbye fishes.", 3, true );

				player.AddMoney( -Game.Prices["plane"] );

			}

		}

		public void AddMoney( float amount )
		{

			Money += amount;

			LastProfit = amount;
			ProfitTime = 2f;

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

				Player.BuyBait();

			} );

			baitText = baitButton.Add.Label( "Buy Bait", "title" );

			campfireButton = shopPanel.Add.Button( "", "button", () =>
			{

				Player.BuyCampfire();

			} );

			campfireText = campfireButton.Add.Label( "Buy Campfire", "title" );

			coatButton = shopPanel.Add.Button( "", "button", () =>
			{

				Player.UpgradeCoat();

			} );

			coatText = coatButton.Add.Label( "Upgrade Coat", "title" );

			drillButton = shopPanel.Add.Button( "", "button", () =>
			{

				Player.UpgradeDrill();

			} );

			drillText = drillButton.Add.Label( "Upgrade Drill", "title" );

			rodButton = shopPanel.Add.Button( "", "button", () =>
			{

				Player.UpgradeRod();

			} );

			rodText = rodButton.Add.Label( "Upgrade Rod", "title" );

			planeButton = shopPanel.Add.Button( "", "button", () =>
			{

				Player.CloseShop();
				Player.Win();

			} );

			planeText = planeButton.Add.Label( "BUY PLANE TICKET", "title" );

		}

		public override void Tick()
		{
			Player player = Local.Pawn as Player;

			Parent.Style.PointerEvents = player.ShopOpen ? "all" : "visible";
			Style.Opacity = player.ShopOpen ? 1 : 0;

			baitText.Text = $"( ${Game.Prices["bait"]} ) Buy Bait ({player.Baits})";
			campfireText.Text = $"( ${Game.Prices["campfire"]} ) Buy Campfire ({player.Campfires})";
			coatText.Text = player.UpgradedCoat ? "[BOUGHT]" : $"( ${Game.Prices["coat"]} ) Upgrade Coat";
			drillText.Text = player.UpgradedDrill ? "[BOUGHT]" : $"( ${Game.Prices["drill"]} ) Upgrade Drill";
			rodText.Text = player.UpgradedRod ? "[BOUGHT]" :  $"( ${Game.Prices["rod"]} ) Upgrade Rod";
			planeText.Text = $"( ${Game.Prices["plane"]} ) Buy Plane Ticket";

		}

	}

}
