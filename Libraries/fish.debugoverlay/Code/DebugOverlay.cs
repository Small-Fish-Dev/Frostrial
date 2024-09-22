public enum DebugStyle : byte
{
	Line,
	Solid
}

public partial class DebugOverlay
{
	/// <summary>
	/// Renders a BBox for a specific amount of time.
	/// </summary>
	/// <param name="bbox"></param>
	/// <param name="style"></param>
	/// <param name="color"></param>
	/// <param name="time"></param>
	public static void BBox( BBox bbox, DebugStyle style = DebugStyle.Line, Color? color = null, float time = 0f )
		=> AddToQueue( () =>
		{
			Gizmo.Draw.Color = color ?? Color.Yellow;
			switch ( style )
			{
				case DebugStyle.Line: Gizmo.Draw.LineBBox( bbox ); break;
				case DebugStyle.Solid: Gizmo.Draw.SolidBox( bbox ); break;
			}
		}, time );

	/// <summary>
	/// Renders a Sphere for a specific amount of time.
	/// </summary>
	/// <param name="sphere"></param>
	/// <param name="style"></param>
	/// <param name="color"></param>
	/// <param name="time"></param>
	public static void Sphere( Sphere sphere, DebugStyle style = DebugStyle.Line, Color? color = null, float time = 0f )
		=> AddToQueue( () =>
		{
			Gizmo.Draw.Color = color ?? Color.Yellow;
			switch ( style )
			{
				case DebugStyle.Line: Gizmo.Draw.LineSphere( sphere ); break;
				case DebugStyle.Solid: Gizmo.Draw.SolidSphere( sphere.Center, sphere.Radius ); break;
			}
		}, time );

	/// <summary>
	/// Renders a line for a specific amount of time.
	/// </summary>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <param name="color"></param>
	/// <param name="time"></param>
	public static void Line( Vector3 a, Vector3 b, Color? color = null, float time = 0f )
		=> AddToQueue( () =>
		{
			Gizmo.Draw.Color = color ?? Color.Yellow;
			Gizmo.Draw.Line( a, b );
		}, time );

	/// <summary>
	/// Renders screenspace text at a 3D-position for a specific amount of time.
	/// </summary>
	/// <param name="text"></param>
	/// <param name="pos"></param>
	/// <param name="font"></param>
	/// <param name="size"></param>
	/// <param name="flags"></param>
	/// <param name="color"></param>
	/// <param name="time"></param>
	public static void Text( string text, Vector3 pos, string font = "Consolas", float size = 18, TextFlag flags = TextFlag.LeftTop, Color? color = null, float time = 0f )
		=> AddToQueue( () =>
		{
			var position = Game.ActiveScene.Camera.PointToScreenPixels( pos );
			Gizmo.Draw.Color = color ?? Color.Yellow;
			Gizmo.Draw.ScreenText( text, position, font, size, flags );
		}, time );

	/// <summary>
	/// Render screenspace text at a 2D-position for a specific amount of time.
	/// </summary>
	/// <param name="text"></param>
	/// <param name="pos"></param>
	/// <param name="font"></param>
	/// <param name="size"></param>
	/// <param name="flags"></param>
	/// <param name="color"></param>
	/// <param name="time"></param>
	public static void ScreenText( string text, Vector2 pos, string font = "Consolas", float size = 18, TextFlag flags = TextFlag.LeftTop, Color? color = null, float time = 0f )
		=> AddToQueue( () =>
		{
			Gizmo.Draw.Color = color ?? Color.Yellow;
			Gizmo.Draw.ScreenText( text, pos, font, size, flags );
		}, time );

	/// <summary>
	/// Renders the result of a SceneTrace for a specific amount of time.
	/// </summary>
	/// <param name="tr"></param>
	/// <param name="time"></param>
	public static void Trace( SceneTraceResult tr, float time = 0f )
		=> AddToQueue( () =>
		{
			Gizmo.Draw.Color = Color.Yellow;
			Gizmo.Draw.Line( tr.StartPosition, tr.EndPosition );

			Gizmo.Draw.Color = tr.Hit ? Color.Blue : Color.Red;
			Gizmo.Draw.LineSphere( new Sphere( tr.EndPosition, 2f ) );

			// If trace is hit.
			if ( tr.GameObject.IsValid() )
			{
				var position = Game.ActiveScene.Camera.PointToScreenPixels( tr.EndPosition ) + Vector2.Left * 30f;
				var text = $"{tr.GameObject.Name}\n{tr.Component}\n{tr.GameObject.Id}";
				Gizmo.Draw.Color = Color.Yellow;
				Gizmo.Draw.ScreenText( text, position, "Consolas", 18 );
			}
		}, time );

	/// <summary>
	/// Renders a Model for a specific amount of time.
	/// </summary>
	/// <param name="model"></param>
	/// <param name="transform"></param>
	/// <param name="time"></param>
	public static void Model( Model model, Transform transform, float time = 0f )
		=> AddToQueue( () =>
		{
			Gizmo.Draw.Model( model, transform );
		}, time );

	/// <summary>
	/// Renders a Texture for a specific amount of time.
	/// </summary>
	/// <param name="texture"></param>
	/// <param name="position"></param>
	/// <param name="size"></param>
	/// <param name="color"></param>
	/// <param name="time"></param>
	public static void Sprite( Texture texture, Vector3 position, float size, Color? color = null, float time = 0f )
		=> AddToQueue( () =>
		{
			Gizmo.Draw.Color = color ?? Color.White;
			Gizmo.Draw.Sprite( position, size, texture );
		}, time );

	/// <summary>
	/// Render a cone for a specific amount of time
	/// </summary>
	/// <param name="origin"></param>
	/// <param name="radius"></param>
	/// <param name="direction"></param>
	/// <param name="angle"></param>
	/// <param name="lines"></param>
	/// <param name="color"></param>
	/// <param name="time"></param>
	public static void Cone( Vector3 origin, float radius, Vector3 direction, float angle, int lines = 12, Color? color = null, float time = 0f )
		=> AddToQueue( () =>
		{
			Gizmo.Draw.Color = color ?? Color.White;
			var halfAngle = angle / 2f;
			var normalDir = direction.Normal;

			var baseCenter = origin + normalDir * (radius * MathF.Cos( MathX.DegreeToRadian( halfAngle ) ));
			var baseRadius = radius * MathF.Sin( MathX.DegreeToRadian( halfAngle ) );

			var upDirection = Vector3.Cross( normalDir, Vector3.Up ).Normal;
			var rightDirection = Vector3.Cross( normalDir, upDirection ).Normal;

			Vector3? previousPoint = null;
			var firstPoint = Vector3.Zero;

			for ( var edge = 0; edge < lines; edge++ )
			{
				var edgeAngle = MathF.PI * edge * 2f / lines;
				var edgeX = MathF.Cos( edgeAngle ) * baseRadius;
				var edgeY = MathF.Sin( edgeAngle ) * baseRadius;
				var edgePosition = baseCenter + rightDirection * edgeX + upDirection * edgeY;

				Gizmo.Draw.Line( origin, edgePosition );

				if ( edge == 0 )
					firstPoint = edgePosition;

				if ( previousPoint != null )
					Gizmo.Draw.Line( previousPoint.Value, edgePosition );

				if ( edge + 1 == lines )
					Gizmo.Draw.Line( edgePosition, firstPoint );

				previousPoint = edgePosition;
			}
		}, time );
}
