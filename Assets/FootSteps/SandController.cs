using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandController : MonoBehaviour
{
    Camera cam;
    public Shader paintShader;
    public LayerMask layer;
    public Texture2D _FootTex;
    private Material paintMat;
    private Material sandMat;
    private RenderTexture _DispTex;
    public float _SquareSize;
    public float _Angle;
    void Start()
    {
        cam = Camera.main;
        paintMat = new Material(paintShader);
        paintMat.SetVector("_Color", Color.red);
        paintMat.SetFloat("_SquareSize", _SquareSize);
        paintMat.SetTexture("_FootTex", _FootTex);
        paintMat.SetFloat("_Angle", _Angle);
        sandMat = this.GetComponent<MeshRenderer>().material;
        _DispTex = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat);
        sandMat.SetTexture("_DispTex", _DispTex);
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if(Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition),out hit,100f,layer))
            {
                paintMat.SetVector("_Coord",new Vector4(hit.textureCoord.x, hit.textureCoord.y,0.0f,0.0f));
                RenderTexture temp = RenderTexture.GetTemporary(_DispTex.width, _DispTex.height, 0, RenderTextureFormat.ARGBFloat);
                Graphics.Blit(_DispTex, temp);
                Graphics.Blit(temp, _DispTex, paintMat);
                RenderTexture.ReleaseTemporary(temp);
                sandMat.SetTexture("_DispTex", _DispTex);
            }
        }
    }
    
    public void PrintFoot(Ray ray,float radiantRotation)
    {
        paintMat.SetFloat("_Angle", radiantRotation);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, layer))
        {
            paintMat.SetVector("_Coord", new Vector4(hit.textureCoord.x, hit.textureCoord.y, 0.0f, 0.0f));
            RenderTexture temp = RenderTexture.GetTemporary(_DispTex.width, _DispTex.height, 0, RenderTextureFormat.ARGBFloat);
            Graphics.Blit(_DispTex, temp);
            Graphics.Blit(temp, _DispTex, paintMat);
            RenderTexture.ReleaseTemporary(temp);
            sandMat.SetTexture("_DispTex", _DispTex);
        }
    }

}
