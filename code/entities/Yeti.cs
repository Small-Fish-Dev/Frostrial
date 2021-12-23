﻿using Sandbox;
using System;
using System.Collections.Generic;

namespace Frostrial
{

	public partial class Yeti : AnimEntity
	{

		[Net] public Entity Victim { get; set; }

		public override void Spawn()
		{

			base.Spawn();

			Scale = 1.7f;

			SetModel( "models/jorma/jorma.vmdl" );

			var suit = new ModelEntity();
			suit.SetModel( "models/randommodels/yeti/yeti.vmdl" );
			suit.SetParent( this, true );

			RenderColor = RenderColor.WithAlpha( 0 );

		}

		[Event.Tick.Server]
		public void OnTick()
		{

			Velocity = Rotation.Forward * 165f;
			Rotation = (Victim.Position.WithZ(0) - Position.WithZ(0)).EulerAngles.ToRotation();

			Position += Velocity * Time.Delta;


			SetAnimFloat( "move_x", Velocity.Length / Scale );

		}

	}

}