using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddLodGroup : MonoBehaviour
{

    private MeshRenderer[] renderers;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    [ContextMenu("AddLOD")]
    private void AddLOD()
    {
        renderers = GetComponentsInChildren<MeshRenderer>();
        LOD[] lods = new LOD[2];
        
        for (int i = 0; i < renderers.Length; i++)
        {
            var group = renderers[i].GetComponent<LODGroup>();
            Renderer[] mr = new Renderer[1];
            mr[0] = renderers[i];
            if (!group)
            {
                group = renderers[i].gameObject.AddComponent<LODGroup>();
            }
            lods[0] = new LOD(0.1f, mr);

            group.SetLODs(lods);

        }

    }
}
