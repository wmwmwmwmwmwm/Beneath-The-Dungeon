#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class EditorOnlyManager : MonoBehaviour
{
	public Vector3 CameraPosition;
	[Button("씬 카메라 위치하기", ButtonHeight = 80)]
	void SceneCameraSetting()
	{
		SceneView ActiveSceneView = SceneView.lastActiveSceneView;
		float cameraDistance = (ActiveSceneView.pivot - ActiveSceneView.camera.transform.position).magnitude;
		ActiveSceneView.pivot = CameraPosition + Vector3.forward * cameraDistance;
		ActiveSceneView.rotation = Quaternion.LookRotation(Vector3.forward);
	}

	public Monster MonsterMonster;
	[Button("몬스터 Battle Sprite에 적용", ButtonHeight = 80)]
	void MonsterAppear()
	{
		EncounterEventManager.Instance.BattlePanel.SetActive(true);
		StartCoroutine(EncounterEventManager.Instance.SetMonsterSprite(MonsterMonster));
	}

	[Button("현재 포지션, 스케일 대입", ButtonHeight = 80)]
	void MonsterAssign()
	{
		MonsterMonster.PositionOffset = Player.Instance.BattleEnemyParent.localPosition;
		MonsterMonster.SpriteScale = Player.Instance.BattleEnemyParent.localScale.x;
		EditorUtility.SetDirty(MonsterMonster);
		AssetDatabase.SaveAssets();
	}

	//public List<Texture2D> Textures;
	//float blurPixelCount = 0;
	//public int BlurRadius = 2;
	//public int BlurIterations = 2;
	//[Button("가우시안 블러 적용", ButtonHeight = 80)]
	//void Blur()
	//{
	//    for (int a = 0; a < Textures.Count; a++)
	//    {
	//        Texture2D OneTexture = Textures[a];
	//        string TexturePath = LoadTexture(OneTexture);
	//        for (int i = 0; i < BlurIterations; i++)
	//        {
	//            OneTexture = BlurImage(OneTexture, BlurRadius, true);
	//            OneTexture = BlurImage(OneTexture, BlurRadius, false);
	//        }
	//        SaveTexture(TexturePath, OneTexture);
	//    }
	//    AssetDatabase.Refresh();
	//}

	//Texture2D BlurImage(Texture2D image, int blurSize, bool horizontal)
	//{
	//    float avgR, avgG, avgB;
	//    Texture2D blurred = new Texture2D(image.width, image.height);
	//    int _W = image.width;
	//    int _H = image.height;
	//    int xx, yy, x, y;
	//    if (horizontal)
	//    {
	//        for (yy = 0; yy < _H; yy++)
	//        {
	//            for (xx = 0; xx < _W; xx++)
	//            {
	//                ResetPixel();
	//                //Right side of pixel
	//                for (x = xx; (x < xx + blurSize && x < _W); x++)
	//                {
	//                    AddPixel(image.GetPixel(x, yy));
	//                }
	//                //Left side of pixel
	//                for (x = xx; (x > xx - blurSize && x > 0); x--)
	//                {
	//                    AddPixel(image.GetPixel(x, yy));
	//                }
	//                CalcPixel();
	//                for (x = xx; x < xx + blurSize && x < _W; x++)
	//                {
	//                    blurred.SetPixel(x, yy, new Color(avgR, avgG, avgB, 1.0f));
	//                }
	//            }
	//        }
	//    }
	//    else
	//    {
	//        for (xx = 0; xx < _W; xx++)
	//        {
	//            for (yy = 0; yy < _H; yy++)
	//            {
	//                ResetPixel();
	//                //Over pixel
	//                for (y = yy; (y < yy + blurSize && y < _H); y++)
	//                {
	//                    AddPixel(image.GetPixel(xx, y));
	//                }
	//                //Under pixel
	//                for (y = yy; (y > yy - blurSize && y > 0); y--)
	//                {
	//                    AddPixel(image.GetPixel(xx, y));
	//                }
	//                CalcPixel();
	//                for (y = yy; y < yy + blurSize && y < _H; y++)
	//                {
	//                    blurred.SetPixel(xx, y, new Color(avgR, avgG, avgB, 1.0f));
	//                }
	//            }
	//        }
	//    }
	//    blurred.Apply();
	//    return blurred;

	//    void AddPixel(Color pixel)
	//    {
	//        avgR += pixel.r;
	//        avgG += pixel.g;
	//        avgB += pixel.b;
	//        blurPixelCount++;
	//    }
	//    void ResetPixel()
	//    {
	//        avgR = 0.0f;
	//        avgG = 0.0f;
	//        avgB = 0.0f;
	//        blurPixelCount = 0;
	//    }
	//    void CalcPixel()
	//    {
	//        avgR /= blurPixelCount;
	//        avgG /= blurPixelCount;
	//        avgB /= blurPixelCount;
	//    }
	//}


	//[Button(ButtonHeight = 100, Name = "targetTextures 4의 배수로 리사이즈, 크런치 압축 적용")]
	//public void ResizeTexturesButton()
	//{
	//    foreach (Texture2D texture in Textures)
	//    {
	//        string texturePath = LoadTexture(texture);

	//        int oldSizeX = texture.width, oldSizeY = texture.height;
	//        int newSizeX = texture.width, newSizeY = texture.height;
	//        while (newSizeX % 4 != 0) newSizeX++;
	//        while (newSizeY % 4 != 0) newSizeY++;
	//        Texture2D newTexture = new Texture2D(newSizeX, newSizeY, TextureFormat.ARGB32, false);
	//        Color32[] clearColor = new Color32[newSizeX * newSizeY];
	//        for (int i = 0; i < clearColor.Length; i++)
	//        {
	//            clearColor[i] = Color.clear;
	//        }
	//        newTexture.SetPixels32(0, 0, newSizeX, newSizeY, clearColor);
	//        Color32[] originalPixels = texture.GetPixels32();
	//        newTexture.SetPixels32(0, 0, oldSizeX, oldSizeY, originalPixels);
	//        newTexture.Apply(false);
	//        SaveTexture(texturePath, newTexture);
	//    }
	//    AssetDatabase.Refresh();
	//    foreach (Texture2D texture in Textures)
	//    {
	//        string texturePath = AssetDatabase.GetAssetPath(texture);
	//        TextureImporter textureImporter = AssetImporter.GetAtPath(texturePath) as TextureImporter;
	//        textureImporter.textureType = TextureImporterType.Sprite;
	//        textureImporter.mipmapEnabled = false;
	//        textureImporter.crunchedCompression = true;
	//        textureImporter.isReadable = false;
	//        AssetDatabase.ImportAsset(texturePath, ImportAssetOptions.ForceUpdate);
	//    }
	//}

	//string LoadTexture(Texture2D tex)
	//{
	//    string texturePath = AssetDatabase.GetAssetPath(tex);
	//    TextureImporter textureImporter = AssetImporter.GetAtPath(texturePath) as TextureImporter;
	//    textureImporter.isReadable = true;
	//    AssetDatabase.ImportAsset(texturePath, ImportAssetOptions.ForceUpdate);
	//    return texturePath;
	//}
	//void SaveTexture(string texturePath, Texture2D tex)
	//{
	//    byte[] pngBytes = tex.EncodeToPNG();
	//    //AssetDatabase.DeleteAsset(texturePath);
	//    string absolutePath = Path.Combine(Directory.GetParent(Application.dataPath).FullName, texturePath);
	//    File.WriteAllBytes(absolutePath, pngBytes);
	//}
}
#endif