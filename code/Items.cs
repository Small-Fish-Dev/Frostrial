using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Frostrial
{

	partial class Player : Sandbox.Player
	{

		[Net] public bool ItemsOpen { get; set; } = false;

		public void HandleItems()
		{

		}

		[ServerCmd]
		public static void ItemSelected()
		{

			Player player = ConsoleSystem.Caller.Pawn as Player;

			player.ItemsOpen = false;
			player.BlockMovement = false;

		}

	}

	public class Items : Panel
	{

		Label button1;
		Label button2;

		public Items()
		{

			Player player = Local.Pawn as Player;

			Panel itemPanel = Add.Panel( "Items" );

			button1 = itemPanel.Add.Button( "", "button", () =>
			{

				Player.ItemSelected();

			} ).Add.Label( "Bait", "title" );

			button2 = itemPanel.Add.Button( "", "button", () =>

			{

				Player.ItemSelected();

			} ).Add.Label( "Campfire", "title" );

		}

		public override void Tick()
		{
			Player player = Local.Pawn as Player;

			Parent.Style.PointerEvents = player.ItemsOpen ? "all" : "visible";
			Style.Opacity = player.ItemsOpen ? 1 : 0;

			button1.Text = $"Bait ({0})"; // TODO Track amount of items
			button2.Text = $"Campfire ({0})";

		}

	}

}
