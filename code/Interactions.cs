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

				if ( selectedEntity == this || selectedEntity is Hole || selectedEntity is Hut )
				{

					if ( selectedEntity.Position.Distance( Position ) < InteractionMaxDistance )
					{



					}
					else
					{

						Hint( "That's too far away!", 2f );

					}

				}
				else
				{

					Hint( "What a beautiful world, I hate it.", 3f );

				}

			}

		}

	}

}
