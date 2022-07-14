//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using UnityEditor;
//using UnityEngine;

//public class SaveManagerEditor : EditorWindow
//{
//	static GUIStyle LeftPaneStyle, SearchBarStyle, SearchBarCancelButtonStyle, SelectedTypeButtonStyle, TypeButtonStyle;
//	static string TemplateFilePath, SerializationScriptPath, SearchFieldValue;
//	static Vector2 TypeListScrollPos, TypePaneScrollPos;
//	static List<TypeListItem> AllTypes;
//	static TypeListItem SelectedType;

//	[MenuItem("Window/Serialization Tool")]
//	public static void ShowSerializationToolWindow()
//	{
//		Init();
//		GetWindow<SaveManagerEditor>().Show();
//	}

//	static void Init()
//	{
//		TemplateFilePath = Application.dataPath + "/0Game/Script/Editor/Template.txt";
//		SerializationScriptPath = Application.dataPath + "/0Game/Script/SaveSystem/";

//		List<TypeListItem> TempTypes = new List<TypeListItem>();
//		Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(assembly => !assembly.FullName.Contains("Editor")).OrderBy(assembly => assembly.GetName().Name).ToArray();
//		foreach (Assembly assembly in assemblies)
//		{
//			Type[] assemblyTypes = assembly.GetTypes();
//			foreach (Type type in assemblyTypes)
//			{
//				if (type.IsGenericType || type.IsEnum || type.IsNotPublic || type.IsAbstract || type.IsInterface)
//					continue;
//				string typeName = type.Name;
//				if (typeName[0] == '$' || typeName[0] == '_' || typeName[0] == '<')
//					continue;
//				string TypeNamespace = type.Namespace;
//				string _NamespaceName = TypeNamespace == null ? "" : TypeNamespace.ToString();
//				TempTypes.Add(new TypeListItem()
//				{
//					Name = type.Name,
//					NamespaceName = _NamespaceName,
//					ThisType = type,
//					ShowInList = true,
//					AlreadyGenerated = File.Exists(string.Format("{0}{1}{2}", SerializationScriptPath, type.Name, ".Serialization.cs"))
//				});
//			}
//		}
//		AllTypes = TempTypes.OrderBy(type => type.Name).ToList();
//		PerformSearch();

//		float CancelButtonSize = EditorStyles.miniTextField.CalcHeight(new GUIContent(""), 20);
//		SearchBarCancelButtonStyle = new GUIStyle(EditorStyles.miniButton)
//		{
//			fixedWidth = CancelButtonSize,
//			fixedHeight = CancelButtonSize,
//			fontSize = 8,
//			padding = new RectOffset()
//		};
//		SearchBarStyle = new GUIStyle(EditorStyles.toolbarTextField)
//		{
//			stretchWidth = true
//		};
//		TypeButtonStyle = new GUIStyle(EditorStyles.largeLabel)
//		{
//			alignment = TextAnchor.MiddleLeft,
//			stretchWidth = false
//		};
//		SelectedTypeButtonStyle = new GUIStyle(TypeButtonStyle)
//		{
//			fontStyle = FontStyle.Bold
//		};
//		LeftPaneStyle = new GUIStyle
//		{
//			fixedWidth = 300,
//			clipping = TextClipping.Clip,
//			padding = new RectOffset(10, 10, 10, 10)
//		};
//	}

//	void OnGUI()
//	{
//		EditorGUILayout.BeginHorizontal();
//		EditorGUILayout.BeginVertical(LeftPaneStyle);
//		SearchBar();
//		TypeList();
//		EditorGUILayout.EndVertical();
//		EditorGUILayout.BeginVertical();
//		TypePane();
//		EditorGUILayout.EndVertical();
//		EditorGUILayout.EndHorizontal();
//	}

//	static void SearchBar()
//	{
//		EditorGUILayout.BeginHorizontal();
//		string CurrentSearchFieldValue = EditorGUILayout.TextField(SearchFieldValue, SearchBarStyle);
//		if (SearchFieldValue != CurrentSearchFieldValue)
//		{
//			SearchFieldValue = CurrentSearchFieldValue;
//			PerformSearch();
//		}
//		GUI.SetNextControlName("Clear");
//		EditorGUILayout.EndHorizontal();
//	}

//	static void PerformSearch()
//	{
//		string LowerCaseValue = SearchFieldValue.ToLowerInvariant();
//		foreach (TypeListItem item in AllTypes)
//		{
//			item.ShowInList = item.Name.ToLowerInvariant().Contains(LowerCaseValue);
//		}
//	}

//	static void TypeList()
//	{
//		if (!string.IsNullOrEmpty(SearchFieldValue))
//			GUILayout.Label("Search Results", EditorStyles.boldLabel);

//		TypeListScrollPos = EditorGUILayout.BeginScrollView(TypeListScrollPos);

//		if (!string.IsNullOrEmpty(SearchFieldValue))
//		{
//			foreach (TypeListItem item in AllTypes)
//			{
//				TypeButton(item);
//			}
//		}

//		EditorGUILayout.EndScrollView();
//	}

//	static void TypeButton(TypeListItem ListItem)
//	{
//		if (!ListItem.ShowInList)
//			return;
//		if (ListItem.AlreadyGenerated)
//			EditorGUILayout.BeginHorizontal();
//		GUIStyle ThisTypeButtonStyle = ListItem == SelectedType ? SelectedTypeButtonStyle : TypeButtonStyle;
//		if (GUILayout.Button(new GUIContent(ListItem.Name, ListItem.NamespaceName), ThisTypeButtonStyle))
//			SelectType(ListItem);
//		// Set the cursor.
//		Rect buttonRect = GUILayoutUtility.GetLastRect();
//		EditorGUIUtility.AddCursorRect(buttonRect, MouseCursor.Link);
//		if (ListItem.AlreadyGenerated)
//		{
//			GUILayout.Box(new GUIContent(checkmark, "Type is explicitly supported"), EditorStyles.largeLabel);
//			EditorGUILayout.EndHorizontal();
//		}
//	}

//	static void TypePane()
//	{
//		if (SelectedType == null) return;

//		EditorStyle style = EditorStyle.Get;
//		Type type = SelectedType.ThisType;

//		TypePaneScrollPos = EditorGUILayout.BeginScrollView(TypePaneScrollPos, style.area);
//		GUILayout.Label(SelectedType.Name, style.subheading);
//		GUILayout.Label(SelectedType.NamespaceName);

//		EditorGUILayout.BeginVertical(style.area);
//		bool hasParameterlessConstructor = type.has ES3Reflection.HasParameterlessConstructor(type);
//		bool isComponent = typeof(Component).IsAssignableFrom(type);
//		string path = GetOutputPath(types[SelectedType].type);
//		// An ES3Type file already exists.
//		if (File.Exists(path))
//		{
//			if (hasParameterlessConstructor || isComponent)
//			{
//				EditorGUILayout.BeginHorizontal();
//				if (GUILayout.Button("Reset to Default"))
//				{
//					SelectNone(true, true);
//					AssetDatabase.MoveAssetToTrash("Assets" + path.Remove(0, Application.dataPath.Length));
//					SelectType(SelectedType);
//				}
//				if (GUILayout.Button("Edit ES3Type Script"))
//					AssetDatabase.OpenAsset(AssetDatabase.LoadMainAssetAtPath("Assets" + path.Remove(0, Application.dataPath.Length)));
//				EditorGUILayout.EndHorizontal();
//			}
//			else
//			{
//				EditorGUILayout.HelpBox("This type has no public parameterless constructors.\n\nTo support this type you will need to modify the ES3Type script to use a specific constructor instead of the parameterless constructor.", MessageType.Info);
//				if (GUILayout.Button("Click here to edit the ES3Type script"))
//					AssetDatabase.OpenAsset(AssetDatabase.LoadMainAssetAtPath("Assets" + path.Remove(0, Application.dataPath.Length)));
//				if (GUILayout.Button("Reset to Default"))
//				{
//					SelectAll(true, true);
//					File.Delete(path);
//					AssetDatabase.Refresh();
//				}
//			}
//		}
//		// No ES3Type file and no fields.
//		else if (fields.Length == 0)
//		{
//			if (!hasParameterlessConstructor && !isComponent)
//				EditorGUILayout.HelpBox("This type has no public parameterless constructors.\n\nTo support this type you will need to create an ES3Type script and modify it to use a specific constructor instead of the parameterless constructor.", MessageType.Info);

//			if (GUILayout.Button("Create ES3Type Script"))
//				Generate();
//		}
//		// No ES3Type file, but fields are selectable.
//		else
//		{
//			if (!hasParameterlessConstructor && !isComponent)
//			{
//				EditorGUILayout.HelpBox("This type has no public parameterless constructors.\n\nTo support this type you will need to select the fields you wish to serialize below, and then modify the generated ES3Type script to use a specific constructor instead of the parameterless constructor.", MessageType.Info);
//				if (GUILayout.Button("Select all fields and generate ES3Type script"))
//				{
//					SelectAll(true, false);
//					Generate();
//				}
//			}
//			else
//			{
//				if (GUILayout.Button("Create ES3Type Script"))
//					Generate();
//			}
//		}
//		EditorGUILayout.EndVertical();

//		EditorGUILayout.BeginVertical(style.area);
//		GUILayout.Label("Fields", EditorStyles.boldLabel);
//		DisplayFieldsOrProperties(true, false);
//		EditorGUILayout.Space();
//		GUILayout.Label("Properties", EditorStyles.boldLabel);
//		DisplayFieldsOrProperties(false, true);
//		EditorGUILayout.EndVertical();

//		EditorGUILayout.EndScrollView();


//		static void DisplayFieldsOrProperties(bool showFields, bool showProperties)
//		{
//			// Get field and property counts.
//			int fieldCount = 0;
//			int propertyCount = 0;
//			for (int i = 0; i < fields.Length; i++)
//			{
//				if (fields[i].isProperty && showProperties)
//					propertyCount++;
//				else if ((!fields[i].isProperty) && showFields)
//					fieldCount++;
//			}

//			// If there is nothing to display, show message.
//			if (showFields && showProperties && fieldCount == 0 && propertyCount == 0)
//				GUILayout.Label("This type has no serializable fields or properties.");
//			else if (showFields && fieldCount == 0)
//				GUILayout.Label("This type has no serializable fields.");
//			else if (showProperties && propertyCount == 0)
//				GUILayout.Label("This type has no serializable properties.");

//			// Display Select All/Select None buttons only if there are fields to display.
//			if (fieldCount > 0 || propertyCount > 0)
//			{
//				EditorGUILayout.BeginHorizontal();

//				if (GUILayout.Button("Select All", selectAllNoneButtonStyle))
//				{
//					SelectAll(showFields, showProperties);
//					Generate();
//				}

//				if (GUILayout.Button("Select None", selectAllNoneButtonStyle))
//				{
//					SelectNone(showFields, showProperties);
//					Generate();
//				}
//				EditorGUILayout.EndHorizontal();
//			}

//			for (int i = 0; i < fields.Length; i++)
//			{
//				var field = fields[i];
//				if ((field.isProperty && !showProperties) || ((!field.isProperty) && !showFields))
//					continue;

//				EditorGUILayout.BeginHorizontal();

//				var content = new GUIContent(field.Name);

//				if (typeof(UnityEngine.Object).IsAssignableFrom(field.MemberType))
//					content.tooltip = field.MemberType.ToString() + "\nSaved by reference";
//				else
//					content.tooltip = field.MemberType.ToString() + "\nSaved by value";

//				bool selected = EditorGUILayout.ToggleLeft(content, fieldSelected[i]);
//				if (selected != fieldSelected[i])
//				{
//					fieldSelected[i] = selected;
//					unsavedChanges = true;
//				}
//				EditorGUILayout.EndHorizontal();
//			}
//		}
//	}

//	void SelectType(TypeListItem SelectedItem)
//	{
//		SelectedType = SelectedItem;

//		fields = ES3Reflection.GetSerializableMembers(SelectedType.ThisType, false);
//		fieldSelected = new bool[fields.Length];

//		var es3Type = ES3TypeMgr.GetES3Type(SelectedType.ThisType);
//		// If there's no ES3Type for this, only select fields which are supported by reflection.
//		if (es3Type == null)
//		{
//			var safeFields = ES3Reflection.GetSerializableMembers(SelectedType.ThisType, true);
//			for (int i = 0; i < fields.Length; i++)
//				fieldSelected[i] = safeFields.Any(item => item.Name == fields[i].Name);
//			return;
//		}

//		// Get fields and whether they're selected.
//		var selectedFields = new List<string>();
//		var propertyAttributes = es3Type.GetType().GetCustomAttributes(typeof(ES3PropertiesAttribute), false);
//		if (propertyAttributes.Length > 0)
//			selectedFields.AddRange(((ES3PropertiesAttribute)propertyAttributes[0]).members);

//		fieldSelected = new bool[fields.Length];

//		for (int i = 0; i < fields.Length; i++)
//			fieldSelected[i] = selectedFields.Contains(fields[i].Name);
//	}

//	static ES3ReflectedMember[] GetSerializableMembers(Type type, bool safe = true, string[] memberNames = null)
//	{
//		if (type == null)
//			return new ES3ReflectedMember[0];

//		var fieldInfos = GetSerializableFields(type, new List<FieldInfo>(), safe, memberNames);
//		var propertyInfos = GetSerializableProperties(type, new List<PropertyInfo>(), safe, memberNames);
//		var reflectedFields = new ES3ReflectedMember[fieldInfos.Count + propertyInfos.Count];

//		for (int i = 0; i < fieldInfos.Count; i++)
//			reflectedFields[i] = new ES3ReflectedMember(fieldInfos[i]);
//		for (int i = 0; i < propertyInfos.Count; i++)
//			reflectedFields[i + fieldInfos.Count] = new ES3ReflectedMember(propertyInfos[i]);

//		return reflectedFields;
//	}


//	public static List<FieldInfo> GetSerializableFields(Type type, List<FieldInfo> serializableFields = null, bool safe = true, string[] memberNames = null, BindingFlags bindings = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
//	{
//		if (type == null)
//			return new List<FieldInfo>();

//		var fields = type.GetFields(bindings);

//		if (serializableFields == null)
//			serializableFields = new List<FieldInfo>();

//		foreach (var field in fields)
//		{
//			var fieldName = field.Name;

//			// If a members array was provided as a parameter, only include the field if it's in the array.
//			if (memberNames != null)
//				if (!memberNames.Contains(fieldName))
//					continue;

//			var fieldType = field.FieldType;

//			if (AttributeIsDefined(field, es3SerializableAttributeType))
//			{
//				serializableFields.Add(field);
//				continue;
//			}

//			if (AttributeIsDefined(field, es3NonSerializableAttributeType))
//				continue;

//			if (safe)
//			{
//				// If the field is private, only serialize it if it's explicitly marked as serializable.
//				if (!field.IsPublic && !AttributeIsDefined(field, serializeFieldAttributeType))
//					continue;
//			}

//			// Exclude const or readonly fields.
//			if (field.IsLiteral || field.IsInitOnly)
//				continue;

//			// Don't store fields whose type is the same as the class the field is housed in unless it's stored by reference (to prevent cyclic references)
//			if (fieldType == type && !IsAssignableFrom(typeof(UnityEngine.Object), fieldType))
//				continue;

//			// If property is marked as obsolete or non-serialized, don't serialize it.
//			if (AttributeIsDefined(field, nonSerializedAttributeType) || AttributeIsDefined(field, obsoleteAttributeType))
//				continue;

//			if (!TypeIsSerializable(field.FieldType))
//				continue;

//			// Don't serialize member fields.
//			if (safe && fieldName.StartsWith(memberFieldPrefix) && field.DeclaringType.Namespace.Contains("UnityEngine"))
//				continue;

//			serializableFields.Add(field);
//		}

//		var baseType = BaseType(type);
//		if (baseType != null && baseType != typeof(System.Object) && baseType != typeof(UnityEngine.Object))
//			GetSerializableFields(BaseType(type), serializableFields, safe, memberNames);

//		return serializableFields;
//	}

//	public static List<PropertyInfo> GetSerializableProperties(Type type, List<PropertyInfo> serializableProperties = null, bool safe = true, string[] memberNames = null, BindingFlags bindings = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
//	{
//		bool isComponent = IsAssignableFrom(typeof(UnityEngine.Component), type);

//		// Only get private properties if we're not getting properties safely.
//		if (!safe)
//			bindings = bindings | BindingFlags.NonPublic;

//		var properties = type.GetProperties(bindings);

//		if (serializableProperties == null)
//			serializableProperties = new List<PropertyInfo>();

//		foreach (var p in properties)
//		{
//			if (AttributeIsDefined(p, es3SerializableAttributeType))
//			{
//				serializableProperties.Add(p);
//				continue;
//			}

//			if (AttributeIsDefined(p, es3NonSerializableAttributeType))
//				continue;

//			var propertyName = p.Name;

//			if (excludedPropertyNames.Contains(propertyName))
//				continue;

//			// If a members array was provided as a parameter, only include the property if it's in the array.
//			if (memberNames != null)
//				if (!memberNames.Contains(propertyName))
//					continue;

//			if (safe)
//			{
//				// If safe serialization is enabled, only get properties which are explicitly marked as serializable.
//				if (!AttributeIsDefined(p, serializeFieldAttributeType) && !AttributeIsDefined(p, es3SerializableAttributeType))
//					continue;
//			}

//			var propertyType = p.PropertyType;

//			// Don't store properties whose type is the same as the class the property is housed in unless it's stored by reference (to prevent cyclic references)
//			if (propertyType == type && !IsAssignableFrom(typeof(UnityEngine.Object), propertyType))
//				continue;

//			if (!p.CanRead || !p.CanWrite)
//				continue;

//			// Only support properties with indexing if they're an array.
//			if (p.GetIndexParameters().Length != 0 && !propertyType.IsArray)
//				continue;

//			// Check that the type of the property is one which we can serialize.
//			// Also check whether an ES3Type exists for it.
//			if (!TypeIsSerializable(propertyType))
//				continue;

//			// Ignore certain properties on components.
//			if (isComponent)
//			{
//				// Ignore properties which are accessors for GameObject fields.
//				if (propertyName == componentTagFieldName || propertyName == componentNameFieldName)
//					continue;
//			}

//			// If property is marked as obsolete or non-serialized, don't serialize it.
//			if (AttributeIsDefined(p, obsoleteAttributeType) || AttributeIsDefined(p, nonSerializedAttributeType))
//				continue;

//			serializableProperties.Add(p);
//		}

//		var baseType = BaseType(type);
//		if (baseType != null && baseType != typeof(System.Object))
//			GetSerializableProperties(baseType, serializableProperties, safe, memberNames);

//		return serializableProperties;
//	}

//	public class TypeListItem
//	{
//		public string Name;
//		public string NamespaceName;
//		public Type ThisType;
//		public bool ShowInList;
//		public bool AlreadyGenerated;
//	}
//}
