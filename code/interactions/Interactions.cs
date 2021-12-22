using Sandbox;
using System.Collections.Generic;

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

							Hint( ".   .   .   .   .", 1f );

						}

						if ( selectedEntity is YetiHand )
						{

							selectedEntity.Delete();
							//TODO GIVE YOU MONEY

							Hint( "This Yeti hand is old, lucky", 2f );

						}

						if ( selectedEntity is YetiScalp )
						{

							selectedEntity.Delete();

							//TODO GIVE YOU MONEY and play sound maybe? lol

							Hint( "The blood on this Yeti scalp is still fresh", 3f );

						}


						if ( selectedEntity is Hut )
						{

							Hint( "I'm almost there", 2f );

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
