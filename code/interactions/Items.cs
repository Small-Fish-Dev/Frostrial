using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Frostrial
{

	partial class Player : Sandbox.Player
	{

		[Net] public int Baits { get; set; } = 10;
		[Net] public int Campfires { get; set; } = 10;
		[Net] public bool ItemsOpen { get; set; } = false;
		[Net] public bool PlacingCampfire { get; set; } = false;

		public void HandleItems()
		{

			if ( PlacingCampfire )
			{

				var mousePos = MouseWorldPosition;
				var playerDistance = mousePos.Distance( Position );
				bool canPlace = !Game.IsNearEntity( mousePos, 30f ) && playerDistance <= 100f && playerDistance >= 45f;

				if ( IsClient )
				{

					DebugOverlay.Circle( MouseWorldPosition + Vector3.Up, Rotation.FromPitch( 90 ), 20f, new Color( canPlace ? 0 : 1, canPlace ? 1 : 0, 0, 0.1f ), true, 0.1f);

				}
				else
				{

					if ( Input.Pressed( InputButton.Attack1 ) )
					{

						if ( canPlace )
						{

							PlacingCampfire = false;
							BlockMovement = false;

							var ent = new FishSpawner();
							ent.Position = MouseWorldPosition;

							Campfires--;

						}
						else
						{

							PlacingCampfire = false;
							BlockMovement = false;
							Hint( "I can't put that there...", 2 );

						}

					}

				}

			}

		}

		[ServerCmd]
		public static void CloseItems()
		{

			Player player = ConsoleSystem.Caller.Pawn as Player;

			player.ItemsOpen = false;
			player.BlockMovement = false;

		}

		[ServerCmd]
		public static void BaitSelected()
		{

			Player player = ConsoleSystem.Caller.Pawn as Player;

			if ( player.Baits > 0 )
			{

				player.ItemsOpen = false;
				player.BlockMovement = false;

				player.Baits--;

				player.Hint( "Oops, some slipped out", 1.5f );

				Particles.Create( "particles/worms_particle.vpcf", player.Position );

			}
			else
			{

				player.ItemsOpen = false;
				player.BlockMovement = false;

				player.Hint( "I don't have any of those...", 2 );

			}

		}

		[ServerCmd]
		public static void CampfireSelected()
		{

			Player player = ConsoleSystem.Caller.Pawn as Player;

			if ( player.Campfires > 0 )
			{

				player.ItemsOpen = false;
				player.PlacingCampfire = true;

			}
			else
			{

				player.ItemsOpen = false;
				player.BlockMovement = false;

				player.Hint( "I don't have any of those...", 2 );

			}

		}

	}

	public class Items : Panel
	{

		Button button1;
		Button button2;
		Label text1;
		Label text2;

		public Items()
		{

			Player player = Local.Pawn as Player;

			Panel itemPanel = Add.Panel( "Items" );

			button1 = itemPanel.Add.Button( "", "button", () =>
			{

				Player.BaitSelected();

			} );
			
			text1 = button1.Add.Label( "Bait", "title" );

			button2 = itemPanel.Add.Button( "", "button", () =>

			{

				Player.CampfireSelected();

			} );
			
			text2 = button2.Add.Label( "Campfire", "title" );

		}

		public override void OnButtonEvent( ButtonEvent e )
		{

			base.OnButtonEvent( e );

			if ( e.Pressed == true && e.Button == "mouseright")
			{

				Player.CloseItems();

			}

		}

		public override void Tick()
		{
			Player player = Local.Pawn as Player;

			Parent.Style.PointerEvents = player.ItemsOpen ? "all" : "visible";
			Style.Opacity = player.ItemsOpen ? 1 : 0;

			text1.Text = $"Bait ({player.Baits})"; // TODO Track amount of items
			text2.Text = $"Campfire ({player.Campfires})";

		}

	}

}
