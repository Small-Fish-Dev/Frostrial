using Sandbox;
using Sandbox.UI;

namespace Frostrial
{

	partial class Player : Sandbox.Player
	{

		[Net] public bool Shopping { get; set; } = false;

		public void HandleShopping()
		{

		}

	}

	public class Shop : Panel
	{

		public Shop()
		{


		}

		public override void Tick()
		{
		
		}

	}

}
