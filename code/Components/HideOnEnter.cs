using Sandbox;

namespace Frostrial;

public sealed class HideOnEnter : Component, Component.ITriggerListener
{
	[Property] [RequireComponent] public ModelRenderer Renderer { get; set; }
	
	public void OnTriggerEnter( Collider other )
	{
		if ( !Active ) return;
		if ( !other.GameObject.Components.TryGet<Player>( out var player, FindMode.Enabled | FindMode.InAncestors ) ) return;

		Renderer.Tint = new Color(1f, 1f, 1f, 0f);
	}
}
