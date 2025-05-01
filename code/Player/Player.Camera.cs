using System;
using Sandbox;

namespace Frostrial;

partial class Player
{
    [Property] [Category("Camera")] public float CameraRotateDelay = 0.3f;
    [Property] [Category("Camera")] public float CameraRotateRate = 5.0f;
    [Property] [Category("Camera")] public float CameraPitch = 30f;
    [Property] [Category("Camera")] public float CameraDistance = 500f;
    [Property] [Category("Camera")] public SoundEvent CrankSound;

    private TimeUntil CanRotateCamera = 0;
    private int _targetRotation;

    private void InitCamera()
    {
        _targetRotation = 1; // 90deg
    }

    private void UpdateCamera()
    {
        if (CanRotateCamera)
        {
            var cameraChanged = false;
            if (Input.Pressed(InputAction.CameraRotateCW))
            {
                _targetRotation = (_targetRotation + 1) % 4;
                cameraChanged = true;
            }

            if (Input.Pressed(InputAction.CameraRotateCCW))
            {
                _targetRotation = (4 + _targetRotation - 1) % 4;
                cameraChanged = true;
            }

            if (cameraChanged)
            {
                CanRotateCamera = CameraRotateDelay;
                Sound.Play(CrankSound);
            }
        }

        var targetRotation = Rotation.From(CameraPitch, _targetRotation * 90f, 0);
        Camera.LocalRotation = Rotation.Slerp(Camera.LocalRotation,
            targetRotation,
            CameraRotateRate * Time.Delta);
        Camera.LocalPosition = Camera.LocalRotation.Backward * CameraDistance;
    }
}