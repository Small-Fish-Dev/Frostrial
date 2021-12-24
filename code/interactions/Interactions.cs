using Sandbox;
using System.Collections.Generic;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Frostrial
{

	partial class Player : Sandbox.Player
	{

		public float InteractionRange { get; set; } = 40f;
		public float InteractionMaxDistance { get; set; } = 120f;

		public void HandleInteractions()
		{

			if ( IsClient ) return;

			if ( Input.Pressed( InputButton.Attack2 ) )
			{

				var selectedEntity = Game.NearestEntity( MouseWorldPosition, InteractionRange );

				if ( selectedEntity is not WorldEntity )
				{

					if ( selectedEntity.Position.Distance( Position ) < InteractionMaxDistance )
					{

						if ( selectedEntity is Player )
						{

							if ( selectedEntity == this )
							{

								ItemsOpen = true;
								BlockMovement = true;

								Hint( "Let's see...", 1.2f );
								PlayClick();

							}
							else
							{

								Hint( "Idiot.", 1f );
								PlayClick();

							}

						}

						if ( selectedEntity is Hole )
						{

							Fishing = true;
							BlockMovement = true;

							CurrentHole = selectedEntity;

							Hole hole = CurrentHole as Hole;
							hole.Bobber = true;

							Hint( ".   .   .   .   .", 1f );
							Play3D( "rod_woosh", this );
							Play3D( "rod_throw", selectedEntity );
							PlayClick();

						}

						if ( selectedEntity is YetiHand )
						{

							selectedEntity.Delete();

							AddMoney( 800f );

							Hint( "This Yeti Hand is old, lucky", 2f, true );
							PlayClick();

						}

						if ( selectedEntity is YetiScalp )
						{

							selectedEntity.Delete();

							AddMoney( 2500f );

							var yeti = new Yeti()
							{
								Position = new Vector3( 3275f, 3511.5f, 8f ),
								Victim = this as Entity

							};

							Play3D( "yeti_roar", yeti );

							Hint( "It's the Finnish Yeti! I must head back to the cabin!", 4f, true );
							PlayClick();

						}

						if ( selectedEntity is FishAuPoopooCaca )
						{

							selectedEntity.Delete();

							AddMoney( 19.84f );

							Hint( "I can always count on French Cuisine", 2.5f );
							PlayClick();

						}

						if ( selectedEntity is Hut )
						{


							ShopOpen = true;
							BlockMovement = true;
							Hint( "I'm almost there", 2f );
							PlayClick();

						}

						if ( selectedEntity is DeadFish )
						{

							DeadFish selectedFish = selectedEntity as DeadFish;


							switch ( selectedFish.Rarity )
							{

								case <= 0.3f:
									Hint( "This isn't going to cut it", 1.7f );
									break;

								case <= 0.6f:
									Hint( "There we go!", 1f );
									break;

								case > 0.6f:
									Hint( "That's a big one!", 1.2f );
									break;

							}

							Play3D( "fish_flop", selectedFish );
							PlayClick();

							AddMoney( selectedFish.Value );

							selectedFish.Delete();

						}


					}
					else
					{

						Hint( "That's too far away!", 2f );
						PlayClick();

					}

				}
				else
				{

					Hint( "I hate this place.", 1f );
					PlayClick();

				}

			}

		}

		[ClientRpc]
		public static void PlayClick()
		{

			Sound.FromScreen( "button_click" );

		}

		[ClientRpc]
		public static void Play3D( string sound, Entity source )
		{

			Sound.FromEntity( sound, source );

		}

	}

}
