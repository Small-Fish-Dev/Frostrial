using Sandbox;
using System.Collections.Generic;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Frostrial
{

	partial class Player : Sandbox.Player
	{

		public float InteractionRange { get; set; } = 80f;
		public float InteractionMaxDistance { get; set; } = 100f;

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

							}
							else
							{

								Hint( "Idiot.", 1f );

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

						}

						if ( selectedEntity is YetiHand )
						{

							selectedEntity.Delete();

							AddMoney( 800f );

							Hint( "This Yeti Hand is old, lucky", 2f );

						}

						if ( selectedEntity is YetiScalp )
						{

							selectedEntity.Delete();

							AddMoney( 2500f );

							new Yeti()
							{
								Position = new Vector3( 3275f, 3511.5f, 8f ),
								Victim = this as Entity

							};

							Hint( "The blood on this Yeti Scalp is still fresh", 3f );

						}


						if ( selectedEntity is Hut )
						{


							ShopOpen = true;
							BlockMovement = true;
							Hint( "I'm almost there", 2f );

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

							AddMoney( selectedFish.Value );

							selectedFish.Delete();

						}


					}
					else
					{

						Hint( "That's too far away!", 2f );

					}

				}
				else
				{

					Hint( "I hate this place.", 1f );

				}

			}

		}

	}

}
