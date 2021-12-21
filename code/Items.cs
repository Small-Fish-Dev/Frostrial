using Sandbox;
using Sandbox.UI;

namespace Frostrial
{

	partial class Player : Sandbox.Player
	{

		[Net] public bool Items { get; set; } = false;

		public void HangleItems()
		{

		}

	}

	public class Items : Panel
	{

		public Items()
		{


		}

		public override void Tick()
		{

		}

	}

}
