using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PhotoCreate : MonoBehaviour {

    public GameObject target;
    public Transform photoCameraBase;
    public RenderTexture renderTexture;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private CameraBase[] cameraList;

    // Use this for initialization
    void Start () {
        meshFilter = target.GetComponentInChildren<MeshFilter>();
        meshRenderer = target.GetComponentInChildren<MeshRenderer>();

        var bounds = meshFilter.mesh.bounds;

    }

    float v, r;
    public float vmin = -70;
    public float vmax = 70;
    public float vstep = 5;
    public float rstep = 5;

    enum Phase {
        Init,
        RLoop,
        VLoop,
        End
    }

    Phase phase = Phase.Init;

    bool enablecreate = false;

    string time;

    // Update is called once per frame
    void Update () {
        
        switch(phase) {
            case Phase.Init: {
                enablecreate = false;
                r = 0;
                v = vmin;
                time = System.DateTime.Now.ToString("yyyyMMdd_HHmm");
                phase = Phase.RLoop;
                break;
            }
            case Phase.RLoop: {
                if (enablecreate) break;
                photoCameraBase.rotation = Quaternion.Euler(v, r, 0);
                r += rstep;
                enablecreate = true;
                if (r >= 360f) {
                    phase = Phase.VLoop;
                }
                break;
            }
            case Phase.VLoop: {
                v += vstep;
                if (v <= vmax) {
                    r = 0;
                    phase = Phase.RLoop;
                    break;
                }
                phase = Phase.End;
                break;
            }
            case Phase.End: {
                break;
            }
        }
		
	}

    private void OnRenderObject() {
        if (!enablecreate) return;
        var tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        RenderTexture.active = renderTexture;
        tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tex.Apply();
        byte[] png = tex.EncodeToPNG();
        Destroy(tex);
        var pathbase = Path.Combine(Application.dataPath, "../" + time + "/" + string.Format("{1:D3}_{0:D3}", (int)r, (int)v) + ".png");
        if (!Directory.Exists(Path.GetDirectoryName(pathbase))) {
            Directory.CreateDirectory(Path.GetDirectoryName(pathbase));
        }
        File.WriteAllBytes(pathbase, png);
        enablecreate = false;
    }
}
