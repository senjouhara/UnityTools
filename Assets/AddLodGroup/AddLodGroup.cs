using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddLodGroup : MonoBehaviour
{

    private MeshRenderer[] mrs;
    public float lodvalue;

    [ContextMenu("AddLOD")]
    void AddLOD()
    {
        mrs = GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < mrs.Length; i++)
        {
            var group = mrs[i].gameObject.GetComponent<LODGroup>();
            if (!group)
            {
                group = mrs[i].gameObject.AddComponent<LODGroup>();
            }
            LOD[] lods = new LOD[1];

            Renderer[] renderers = new Renderer[1];
            renderers[0] = mrs[i];
            //if (lodvalue != 0)
            //{
            //    lods[0] = new LOD(0.05f, renderers);
            //}
            //else
            //{
            //    lods[0] = new LOD(lodvalue, renderers);
            //}
            lods[0] = new LOD(0.1f, renderers);
            group.SetLODs(lods);
        }

    }

    [ContextMenu("Remove")]
    void Remove()
    {
        var lgs = GetComponentsInChildren<LODGroup>();
        for (int i = 0; i < lgs.Length; i++)
        {
            DestroyImmediate(lgs[i]);
        }

    }
}
