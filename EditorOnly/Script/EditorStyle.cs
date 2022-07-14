#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class EditorStyle
{
	static EditorStyle style = null;

	public GUIStyle area;
	public GUIStyle areaPadded;

	public GUIStyle menuButton;
	public GUIStyle menuButtonSelected;
	public GUIStyle smallSquareButton;

	public GUIStyle heading;
	public GUIStyle subheading;
	public GUIStyle subheading2;

	public GUIStyle boldLabelNoStretch;

	public GUIStyle link;

	public GUIStyle toggle;

	public Texture2D saveIconSelected;
	public Texture2D saveIconUnselected;

	public static EditorStyle Get { get { if (style == null) style = new EditorStyle(); return style; } }

	public EditorStyle()
	{
		// An area with padding.
		area = new GUIStyle
		{
			padding = new RectOffset(10, 10, 10, 10),
			wordWrap = true
		};

		// An area with more padding.
		areaPadded = new GUIStyle
		{
			padding = new RectOffset(20, 20, 20, 20),
			wordWrap = true
		};

		// Unselected menu button.
		menuButton = new GUIStyle(EditorStyles.toolbarButton)
		{
			fontStyle = FontStyle.Normal,
			fontSize = 14,
			fixedHeight = 24
		};

		// Selected menu button.
		menuButtonSelected = new GUIStyle(menuButton)
		{
			fontStyle = FontStyle.Bold
		};

		// Main Headings
		heading = new GUIStyle(EditorStyles.label)
		{
			fontStyle = FontStyle.Bold,
			fontSize = 24
		};

		subheading = new GUIStyle(heading)
		{
			fontSize = 18
		};

		subheading2 = new GUIStyle(heading)
		{
			fontSize = 14
		};

		boldLabelNoStretch = new GUIStyle(EditorStyles.label)
		{
			stretchWidth = false,
			fontStyle = FontStyle.Bold
		};

		link = new GUIStyle
		{
			fontSize = 16
		};
		if (EditorGUIUtility.isProSkin)
			link.normal.textColor = new Color(0.262f, 0.670f, 0.788f);
		else
			link.normal.textColor = new Color(0.129f, 0.129f, 0.8f);

		toggle = new GUIStyle(EditorStyles.toggle)
		{
			stretchWidth = false
		};
	}
}
#endif