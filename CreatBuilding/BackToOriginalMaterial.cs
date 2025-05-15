using System.Collections.Generic;
using UnityEngine;

public static class BackToOriginalMaterial
{
    // 材质备份存储结构
    public struct MaterialBackup
    {
        public GameObject targetObject;
        public Dictionary<Renderer, Material[]> rendererMaterials;
    }

    // 备份材质（可处理多个物体）
    public static MaterialBackup[] BackupMaterials(GameObject[] targets)
    {
        List<MaterialBackup> backups = new List<MaterialBackup>();

        foreach (var go in targets)
        {
            MaterialBackup backup = new MaterialBackup
            {
                targetObject = go,
                rendererMaterials = new Dictionary<Renderer, Material[]>()
            };

            Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
            foreach (var renderer in renderers)
            {
                // 使用sharedMaterials避免创建实例
                backup.rendererMaterials[renderer] = renderer.sharedMaterials;
            }

            backups.Add(backup);
        }

        return backups.ToArray();
    }

    // 应用临时材质到动态物体列表
    public static void ApplyTempMaterials(GameObject[] targets, Material tempMaterial)
    {
        foreach (var go in targets)
        {
            if (go == null) continue;

            Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
            foreach (var renderer in renderers)
            {
                Material[] newMats = new Material[renderer.sharedMaterials.Length];
                System.Array.Fill(newMats, tempMaterial);
                renderer.sharedMaterials = newMats;
            }
        }
    }

    // 从备份恢复材质
    public static void RestoreMaterials(MaterialBackup[] backups)
    {
        foreach (var backup in backups)
        {
            if (backup.targetObject == null) continue;

            foreach (var pair in backup.rendererMaterials)
            {
                if (pair.Key != null) // 检查Renderer是否仍然存在
                {
                    pair.Key.sharedMaterials = pair.Value;
                }
            }
        }
    }
}
