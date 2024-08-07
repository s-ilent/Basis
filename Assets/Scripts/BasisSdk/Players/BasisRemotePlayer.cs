using Assets.Scripts.Avatar;
using Assets.Scripts.Drivers;
using Assets.Scripts.TransformBinders.BoneControl;
using Assets.Scripts.UI.NamePlate;
using UnityEngine;
using static SerializableDarkRift;
namespace Assets.Scripts.BasisSdk.Players
{
    public class BasisRemotePlayer : BasisPlayer
    {
        public BasisRemoteBoneDriver RemoteBoneDriver;
        public BasisRemoteAvatarDriver RemoteAvatarDriver;
        public GameObject AudioSourceGameobject;
        public BasisBoneControl MouthControl;
        public bool HasEvents = false;
        public async void RemoteInitialize(string AvatarURL, PlayerMetaDataMessage PlayerMetaDataMessage)
        {
            DisplayName = PlayerMetaDataMessage.playerDisplayName;
            UUID = PlayerMetaDataMessage.playerUUID;
            IsLocal = false;
            RemoteBoneDriver.CreateInitialArrays(RemoteBoneDriver.transform);
            RemoteBoneDriver.Initialize();
            if (HasEvents == false)
            {
                RemoteAvatarDriver.CalibrationComplete += RemoteCalibration;
                HasEvents = true;
            }
            if (Avatar == null)
            {
                CreateAvatar(AvatarURL);
            }
            RemoteBoneDriver.FindBone(out MouthControl, BasisBoneTrackedRole.Mouth);
            await BasisRemoteNamePlate.LoadRemoteNamePlate(this);
        }
        public void OnDestroy()
        {
            if (HasEvents)
            {
                if (RemoteAvatarDriver != null)
                {
                    RemoteAvatarDriver.CalibrationComplete -= RemoteCalibration;
                    HasEvents = false;
                }
            }
        }
        public void UpdateTransform(Vector3 position, Quaternion rotation)
        {
            AudioSourceGameobject.transform.SetPositionAndRotation(position, rotation);
        }
        public async void CreateAvatar(string Loader = BasisAvatarFactory.LoadingAvatar)
        {
            if (string.IsNullOrEmpty(Loader))
            {
                Debug.Log("Avatar Load string was null or empty using fallback!");
                await BasisAvatarFactory.LoadAvatar(this, BasisAvatarFactory.LoadingAvatar);
            }
            else
            {
                Debug.Log("loading avatar from " + Loader);
                await BasisAvatarFactory.LoadAvatar(this, Loader);
            }
        }
        public void RemoteCalibration()
        {
            RemoteBoneDriver.OnCalibration(this);
        }
    }
}