using Sandbox;
using System;
using System.Collections.Generic;

namespace Frostrial
{
	[Library("fish"), AutoGenerate]
	public partial class FishAsset : Asset
	{
		public static IReadOnlyDictionary<string, FishAsset> All => _all;
		internal static Dictionary<string, FishAsset> _all = new();

		[Property]
		public string FishName { get; set; }
		[Property]
		public string CommonName { get; set; }
		[Property]
		public string ScientificName { get; set; }
		[Property]
		public string Description { get; set; }
		[Property, ResourceType( "vmdl" )]
		public string Model { get; set; }
		[Property, ResourceType( "png" )]
		public string Preview { get; set; }
		[Property]
		public float Size { get; set; }
		[Property]
		public float ModelWorldSizeMultiplier { get; set; }
		[Property]
		public float Rarity { get; set; }
		[Property]
		public string VariantName { get; set; }
		[Property]
		public string VariantCommonName { get; set; }
		[Property]
		public string VariantScientificName { get; set; }
		[Property]
		public string VariantDescription { get; set; }
		[Property]
		public string VariantSkin { get; set; }
		[Property, ResourceType( "png" )]
		public string VariantPreview { get; set; }
		[Property]
		public int WeightedZone1 { get; set; }
		[Property]
		public int WeightedZone2 { get; set; }
		[Property]
		public int WeightedZone3 { get; set; }

		public int ZoneValue( int index )
		{

			switch ( index )
			{

				case 0:
					return WeightedZone1;
				case 1:
					return WeightedZone2;
				case 2:
					return WeightedZone3;
				default:
					return 0;

			}

		}


		protected override void PostLoad()
		{
			base.PostLoad();

			if ( !_all.ContainsKey( Name ) )
				_all.Add( Name, this );

			//// Cheeky workaround while Rubat fixes my issue
			foreach ( var kvp in _all )
			{
				
				Preview = Preview.Replace( "jpg", "png" );
				VariantPreview = VariantPreview.Replace( "jpg", "png" );

			}

		}
	}
}
