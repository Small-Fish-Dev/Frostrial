using Sandbox;

namespace Frostrial
{
	partial class Player
	{
		public InputButton Input_Use => InputButton.Attack2;
		public InputButton Input_Drill => InputButton.Attack1;
		public InputButton Input_Run => InputButton.Run;
		public InputButton Input_CameraCW => IsUsingController ? InputButton.SlotNext : InputButton.Menu;
		public InputButton Input_CameraCCW => IsUsingController ? InputButton.SlotPrev : InputButton.Use;
		public InputButton Input_CameraZoomIn => InputButton.Slot1;
		public InputButton Input_CameraZoomOut => InputButton.Slot3;
	}
}
