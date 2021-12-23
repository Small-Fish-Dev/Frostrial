using Sandbox;
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

			if ( Victim.Position.Distance( Position ) < 40f )
			{

				Player player = Victim as Player;

				if ( player.Jumpscare == 0 )
				{

					player.BlockMovement = true;
					player.Jumpscare = 1;
					player.JumpscareTimer = 4f;

				}

				if ( player.JumpscareTimer <= 0f )
				{

					player.Client.Kick();

				}

			}

			if ( Game.IsInside( Victim.Position, new Vector3( -1395, -2745, 0 ), new Vector3( -1164, -2394, 40 ) ) )
			{

				if ( Position.Distance( Game.HutEntity.Position ) <= 210 )
				{

					Player player = Victim as Player;

					if ( player.Jumpscare == 0 )
					{

						player.JumpscareTimer = 3f;
						player.BlockMovement = true;

						player.Jumpscare = 2;

					}

					if ( player.JumpscareTimer <= -4f )
					{

						player.BlockMovement = false;
						player.Jumpscare = 0;
						Delete();

					}

					Velocity = Vector3.Zero;


				}

			}

			Position += Velocity * Time.Delta;

			SetAnimFloat( "move_x", Velocity.Length / Scale );

		}

	}

}
