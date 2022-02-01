using Sandbox;
using System.Collections.Generic;

namespace Frostrial
{
	[Library("fish"), AutoGenerate]
	public partial class FishAsset : Asset
	{
		public static IReadOnlyList<FishAsset> All => _all;
		internal static List<FishAsset> _all = new();

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



		protected override void PostLoad()
		{
			base.PostLoad();

			if ( !_all.Contains( this ) )
				_all.Add( this );
		}
	}
}
