using UnityEngine;

public class OutlineMaterialScript : MonoBehaviour
{
    [SerializeField] private Material outlineMaterial;
    private Material[] originalMaterials;
    private Renderer objectRenderer;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        originalMaterials = objectRenderer.materials;
    }

    public void ApplyOutlineMaterial()
    {
        if (outlineMaterial == null || originalMaterials == null) return;
        
        Material[] materialsWithOutline = new Material[originalMaterials.Length + 1];
        for (int i = 0; i < originalMaterials.Length; i++)
        {
            materialsWithOutline[i] = originalMaterials[i];
        }
        materialsWithOutline[originalMaterials.Length] = outlineMaterial;
        
        objectRenderer.materials = materialsWithOutline;
    }

    public void RemoveOutlineMaterial()
    {
        if (objectRenderer == null) return;
        
        objectRenderer.materials = originalMaterials;
    }
}
