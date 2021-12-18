using Sandbox;
using System.Collections.Generic;

namespace Frostrial
{
	partial class Player : Sandbox.Player
	{

		private Dictionary<string, ModelEntity> clothes = new();

		public void SetClothing( string clothingSlot, string modelPath )
		{

			// Delete pre-existing clothes on the same slot
			if ( clothes.ContainsKey( clothingSlot ) )
			{

				clothes[clothingSlot].Delete();

			}

			var entity = new ModelEntity();

			entity.SetModel( modelPath );
			entity.SetParent( this, true );

			clothes[clothingSlot] = entity;

		}
		public void BasicClothes()
		{

			SetClothing( "tool", "models/tools/basic_fishingrod.vmdl" );
			SetClothing( "hat", "models/clothing/hats/ushanka.vmdl" );
			SetClothing( "jacket", "models/clothing/jackets/parka.vmdl" );
			SetClothing( "trousers", "models/clothing/trousers/fishing_trousers.vmdl" );
			SetClothing( "gloves", "models/citizen_clothes/gloves/gloves_workgloves.vmdl" );
			//SetClothing( "boots", "models/citizen_clothes/shoes/shoes_securityboots.vmdl" ); // They too big!

		}

	}

}
