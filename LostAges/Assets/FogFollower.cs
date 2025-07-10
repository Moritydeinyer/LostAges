using UnityEngine;
using UnityEngine.VFX;

public class FogFollower : MonoBehaviour
{
    public VisualEffect vfxStartMitK; // Dein VFX GameObject
    public VisualEffect vfxStartOhneK; // Dein VFX GameObject
    public VisualEffect vfxK1; // Dein VFX GameObject
    public VisualEffect vfxK2; // Dein VFX GameObject
    public VisualEffect vfxK3; // Dein VFX GameObject

    public VisualEffect vfxPI; // Dein VFX GameObject
    public VisualEffect vfxPII; // Dein VFX GameObject
    public VisualEffect vfxPIII; // Dein VFX GameObject
    public VisualEffect vfxPIV; // Dein VFX GameObject

    public VisualEffect vfxTCB; // Dein VFX GameObject

    public Transform target; // z. B. Main Character oder Kamera

    void Update()
    {
        try 
            {
                vfxStartMitK.SetVector3("playerPosition", target.position);
            }
        catch (System.Exception e) {}
        try
            {
                vfxStartOhneK.SetVector3("playerPosition", target.position);
            }
        catch (System.Exception e) {}
        try
            {
                vfxK1.SetVector3("playerPosition", target.position);
            }
        catch (System.Exception e) {}
        try
            {
                vfxK2.SetVector3("playerPosition", target.position);
            }
        catch (System.Exception e) {}
        try
            {
                vfxK3.SetVector3("playerPosition", target.position);
            }
        catch (System.Exception e) {}
        try
            {
                vfxPI.SetVector3("playerPosition", target.position);
            }
        catch (System.Exception e) {}
        try
            {
                vfxPII.SetVector3("playerPosition", target.position);
            }
        catch (System.Exception e) {}
        try
            {
                vfxPIII.SetVector3("playerPosition", target.position);
            }
        catch (System.Exception e) {}
        try
            {
                vfxPIV.SetVector3("playerPosition", target.position);
            }
        catch (System.Exception e) {}
        try
            {
                vfxTCB.SetVector3("playerPosition", target.position);
            }
        catch (System.Exception e) {}
    }
}
