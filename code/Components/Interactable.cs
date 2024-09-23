using System.Text.Json.Serialization;
using Sandbox;

namespace Frostrial;

public sealed class Interactable : Component
{
    public const string Tag = "interactable";
    
    /// <summary>
    /// The action that is performed when interacted with
    /// </summary>
    [Property, Category( "Required" )]
    public InteractionEvent Action { get; set; }

    public delegate void InteractionEvent( Player subject );

    /// <summary>
    /// Defines whether a player can use this object or not
    /// </summary>
    [Property, Category( "Optional" )]
    protected CanUseEvent CanUse { get; set; } = _ => true;

    public delegate bool CanUseEvent( Player subject );

    /// <summary>
    /// The max distance you can use this interaction from
    /// </summary>
    [Property, Category( "Optional" )]
    public float InteractDistance { get; set; } = 150f;

    /// <summary>
    /// The UI description displayed when hovering over the object
    /// </summary>
    [Property, Category( "Optional" )]
    public string Name { get; set; } = "Something";

    public bool IsPressed => Input.Pressed( InputAction.Interact );

    public bool IsUsable( Player subject ) => CanUse?.Invoke( subject ) ?? true;

    protected override void OnEnabled()
    {
        GameObject.Tags.Add( Tag );
    }

    protected override void OnDisabled()
    {
        GameObject.Tags.Remove( Tag );
    }
}