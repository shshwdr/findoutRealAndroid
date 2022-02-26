//using UnityEngine;
//using UnityEditor;
//using System.Collections.Generic;
//using UnityEditorInternal;
//using System.IO;

//// https://forum.unity.com/threads/sprite-editor-automatic-slicing-by-script.320776/
//// http://www.sarpersoher.com/a-custom-asset-importer-for-unity/
//public class SpritePostprocessor : AssetPostprocessor
//{

//    /// <summary>
//    /// Default all textures to 2D sprites, pivot at bottom, mip-mapped, uncompressed.
//    /// </summary>
//    private void OnPreprocessTexture()
//    {
//        Debug.Log("OnPreprocessTexture overwriting defaults");

//        TextureImporter importer = assetImporter as TextureImporter;
//        importer.isReadable = true;
//        importer.filterMode = FilterMode.Point;
//        importer.spritePixelsPerUnit = 100;
//        importer.maxTextureSize = 3000;
//       // importer.alphaSource = TextureImporterAlphaSource.FromInput;
//        importer.textureType = TextureImporterType.Sprite;

//        importer.spriteImportMode = SpriteImportMode.Multiple;

//        importer.mipmapEnabled = false;
//        importer.textureCompression = TextureImporterCompression.Uncompressed;
//    }
//    public void OnPostprocessTexture(Texture2D texture)
//    {
//        TextureImporter importer = assetImporter as TextureImporter;
//        if (importer.spriteImportMode != SpriteImportMode.Multiple)
//        {
//            return;
//        }

//        Debug.Log("OnPostprocessTexture generating sprites");
//        int startSpriteId = 112;
//        int endSpriteId = 135;
//        Vector2 minimumSpriteSize = new Vector2(48,96);
//        //Rect[] rects = InternalSpriteUtility.GenerateAutomaticSpriteRectangles(texture, minimumSpriteSize, extrudeSize);
//        Rect[] rects = InternalSpriteUtility.GenerateGridSpriteRectangles(texture,Vector2.zero,minimumSpriteSize,Vector2.zero,true);
//        List<Rect> rectsList = new List<Rect>(rects);
//       // rectsList = SortRects(rectsList, texture.width);

//        string filenameNoExtension = Path.GetFileNameWithoutExtension(assetPath);
//        List<SpriteMetaData> metas = new List<SpriteMetaData>();
//        int rectNum = 0;

//        for(int i= 0;i< rectsList.Count;i++)
//        {
//            if(i< startSpriteId || i > endSpriteId)
//            {
//                continue;
//            }
//            var rect = rectsList[i];
//            SpriteMetaData meta = new SpriteMetaData();
//            meta.rect = rect;
//            meta.name = filenameNoExtension + "_" + rectNum++;
//            metas.Add(meta);
//        }

//        importer.spritesheet = metas.ToArray();
//        Debug.Log(importer.spritesheet.Length);
//        AssetDatabase.ForceReserializeAssets(new List<string>() { assetImporter.assetPath });
//    }

//    public void OnPostprocessSprites(Texture2D texture, Sprite[] sprites)
//    {
//        Debug.Log("OnPostprocessSprites sprites: " + sprites.Length);
//    }

//    private List<Rect> SortRects(List<Rect> rects, float textureWidth)
//    {
//        List<Rect> list = new List<Rect>();
//        while (rects.Count > 0)
//        {
//            Rect rect = rects[rects.Count - 1];
//            Rect sweepRect = new Rect(0f, rect.yMin, textureWidth, rect.height);
//            List<Rect> list2 = this.RectSweep(rects, sweepRect);
//            if (list2.Count <= 0)
//            {
//                list.AddRange(rects);
//                break;
//            }
//            list.AddRange(list2);
//        }
//        return list;
//    }

//    private List<Rect> RectSweep(List<Rect> rects, Rect sweepRect)
//    {
//        List<Rect> result;
//        if (rects == null || rects.Count == 0)
//        {
//            result = new List<Rect>();
//        }
//        else
//        {
//            List<Rect> list = new List<Rect>();
//            foreach (Rect current in rects)
//            {
//                if (current.Overlaps(sweepRect))
//                {
//                    list.Add(current);
//                }
//            }
//            foreach (Rect current2 in list)
//            {
//                rects.Remove(current2);
//            }
//            list.Sort((Rect a, Rect b) => a.x.CompareTo(b.x));
//            result = list;
//        }
//        return result;
//    }
//}
