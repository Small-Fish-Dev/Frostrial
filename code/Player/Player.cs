using Sandbox;

namespace Frostrial;

public sealed partial class Player : Component
{
    public static Player Local { get; private set; }
    
    [Property]
    [Category( "Components" )]
    public PlayerController Controller { get; set; }
    
    [Property]
    [Category( "Components" )]
    public CameraComponent Camera { get; set; }

    protected override void OnStart()
    {
        if (!IsProxy)
        {
            Local = this;
            
            InitCamera();
        }
    }

    protected override void OnDestroy()
    {
        if (!IsProxy)
        {
            Local = null;
        }
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        if (IsProxy) return;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (IsProxy) return;
        
        UpdateCamera();
    }
}