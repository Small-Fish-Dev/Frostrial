using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Frostrial
{

	partial class Player : Sandbox.Player
	{

		[Net] public int Baits { get; set; } = 0;
		[Net] public int Campfires { get; set; } = 0;
		[Net] public RealTimeUntil BaitEffect { get; set; } = 0;
		[Net] public bool ItemsOpen { get; set; } = false;
		[Net] public bool PlacingCampfire { get; set; } = false;

		public void HandleItems()
		{

			if ( PlacingCampfire )
			{

				var mousePos = MouseWorldPosition;
				var playerDistance = mousePos.Distance( Position );
				bool canPlace = !Game.IsNearEntity( mousePos, 30f ) && playerDistance <= 110f && playerDistance >= 30f;

				if ( IsClient )
				{

					DebugOverlay.Circle( MouseWorldPosition + Vector3.Up, Rotation.FromPitch( 90 ), 20f, new Color( canPlace ? 0 : 1, canPlace ? 1 : 0, 0, 0.1f ), 0.1f);

				}
				else
				{

					if ( Input.Pressed( InputButton.PrimaryAttack ) )
					{

						if ( canPlace )
						{

							PlacingCampfire = false;
							BlockMovement = false;

							var ent = new Campfire();
							ent.Position = MouseWorldPosition;

							Campfires--;

						}
						else
						{

							PlacingCampfire = false;
							BlockMovement = false;
							Say( VoiceLine.CantPlace );

						}

					}

				}

			}

		}

		[ConCmd.Server]
		public static void CloseItems()
		{

			Player player = ConsoleSystem.Caller.Pawn as Player;

			player.ItemsOpen = false;
			player.BlockMovement = false;

		}

		[ConCmd.Server]
		public static void BaitSelected()
		{

			Player player = ConsoleSystem.Caller.Pawn as Player;

			if ( player.Baits > 0 )
			{

				player.ItemsOpen = false;
				player.BlockMovement = false;
				player.BaitEffect = 15f;

				player.Baits--;

				player.Say( VoiceLine.UsedBait );

				Particles.Create( "particles/worms_particle.vpcf", player.Position );

			}
			else
			{

				player.ItemsOpen = false;
				player.BlockMovement = false;

				player.Say( VoiceLine.NoItems );

			}

		}

		[ConCmd.Server]
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

				player.Say( VoiceLine.NoItems );

			}

		}

	}

	public class Items : Panel
	{

		Button baitButton;
		Button campfireButton;
		Label baitText;
		Label campfireText;

		public Items()
		{

			Player player = Local.Pawn as Player;

			Panel itemPanel = Add.Panel( "Items" );

			Add.Panel( "Close" ).Add.Button( "", "button", () =>
			{

				Sound.FromScreen( "button_click" );
				Player.CloseItems();

			} ).Add.Label( "Close", "title" );

			baitButton = itemPanel.Add.Button( "", "button", () =>
			{

				Sound.FromScreen( "button_click" );
				Player.BaitSelected();

			} );
			
			baitText = baitButton.Add.Label( "Bait", "title" );

			campfireButton = itemPanel.Add.Button( "", "button", () =>

			{

				Sound.FromScreen( "button_click" );
				Player.CampfireSelected();

			} );
			
			campfireText = campfireButton.Add.Label( "Campfire", "title" );

		}

		public override void Tick()
		{
			Player player = Local.Pawn as Player;

			Parent.Style.PointerEvents = player.ItemsOpen ? "all" : "visible";
			Style.Opacity = player.ItemsOpen ? 1 : 0;

			baitText.Text = $"Bait ({player.Baits})";
			campfireText.Text = $"Campfire ({player.Campfires})";

		}

	}

}
