global using Sandbox;
global using System;
global using System.Collections.Generic;

/// <summary>
/// Badass library for rendering debug information.
/// </summary>
public sealed partial class DebugOverlay : GameObjectSystem
{
	private static List<(
		Action Callback,
		TimeSince SinceCreated,
		float Duration
	)> _queue = new();

	public DebugOverlay( Scene scene ) : base( scene )
	{
		_queue.Clear();
		Listen( Stage.FinishUpdate, 0, RenderQueue, "DebugOverlay" );
	}

	private static void RenderQueue()
	{
		var tx = Gizmo.Settings == null ? Transform.Zero : Gizmo.Transform;
		if ( Gizmo.Settings != null ) Gizmo.Transform = Transform.Zero;

		for ( int i = 0; i < _queue.Count; i++ )
		{
			var obj = _queue[i];
			if ( obj.SinceCreated >= obj.Duration )
			{
				_queue.RemoveAt( i );
				continue;
			}

			obj.Callback();
		}

		if ( Gizmo.Settings != null ) Gizmo.Transform = tx;
	}

	private static void AddToQueue( Action callback, float time )
	{
		// ? XD
		if ( Gizmo.Draw == null )
			return;

		if ( time <= 0f )
		{
			var tx = Gizmo.Settings == null ? Transform.Zero : Gizmo.Transform;
			if ( Gizmo.Settings != null ) Gizmo.Transform = Transform.Zero;
			callback();
			if ( Gizmo.Settings != null ) Gizmo.Transform = tx;
			return;
		}

		_queue.Add( (
			Callback: callback,
			SinceCreated: 0f,
			Duration: time
		) );
	}
}
