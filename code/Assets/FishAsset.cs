using Sandbox;
using System;
using System.Collections.Generic;

namespace Frostrial;

[GameResource( "Fish", "fish", "Frostrial Fish Asset", Icon = "set_meal" )]
public class FishAsset : GameResource
{
	public static IReadOnlyDictionary<string, FishAsset> All => _all;
	private static Dictionary<string, FishAsset> _all = new();

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
		return index switch
		{
			0 => WeightedZone1,
			1 => WeightedZone2,
			2 => WeightedZone3,
			_ => 0
		};
	}


	protected override void PostLoad()
	{
		base.PostLoad();

		if ( !_all.ContainsKey( ResourceName ) )
			_all.Add( ResourceName, this );
	}
}