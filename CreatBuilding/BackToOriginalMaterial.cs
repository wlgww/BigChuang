using System.Collections.Generic;
using UnityEngine;

public static class BackToOriginalMaterial
{
    // ���ʱ��ݴ洢�ṹ
    public struct MaterialBackup
    {
        public GameObject targetObject;
        public Dictionary<Renderer, Material[]> rendererMaterials;
    }

    // ���ݲ��ʣ��ɴ��������壩
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
                // ʹ��sharedMaterials���ⴴ��ʵ��
                backup.rendererMaterials[renderer] = renderer.sharedMaterials;
            }

            backups.Add(backup);
        }

        return backups.ToArray();
    }

    // Ӧ����ʱ���ʵ���̬�����б�
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

    // �ӱ��ݻָ�����
    public static void RestoreMaterials(MaterialBackup[] backups)
    {
        foreach (var backup in backups)
        {
            if (backup.targetObject == null) continue;

            foreach (var pair in backup.rendererMaterials)
            {
                if (pair.Key != null) // ���Renderer�Ƿ���Ȼ����
                {
                    pair.Key.sharedMaterials = pair.Value;
                }
            }
        }
    }
}
