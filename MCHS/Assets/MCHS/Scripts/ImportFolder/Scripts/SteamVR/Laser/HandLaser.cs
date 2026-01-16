using UnityEngine.UI;
using Valve.VR.InteractionSystem;


namespace MySteamVR.Laser
{
    public class HandLaser : SteamVR_LaserPointer
    {
        public override void OnPointerIn(PointerEventArgs e)
        {
            base.OnPointerIn(e);
            if (e.target.TryGetComponent(out IPointerIn pointer))
            {
                pointer.OnPointerIn();
            }
        }

        public override void OnPointerClick(PointerEventArgs e)
        {
            base.OnPointerClick(e);
            if (e.target.TryGetComponent(out IPointerClick pointer))
            {
                pointer.OnPointerClick();
            }

            if (e.target.GetComponent<UIElement>())
            {
                e.target.GetComponent<Button>().onClick?.Invoke();
            }
        }

        public override void OnPointerOut(PointerEventArgs e)
        {
            base.OnPointerOut(e);
            if (e.target.TryGetComponent(out IPointerOut pointer))
            {
                pointer.OnPointerOut();
            }
        }
    }
}