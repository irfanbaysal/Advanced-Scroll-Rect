using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Editor
{
	public sealed class UGUIToolbar : EditorWindow
	{
		private sealed class Data
		{
			public Type   Type        { get; }
			public string CommandRoot { get; }
			public string CommandName { get; }

			public Data( Type type, string commandRoot, string commandName )
			{
				Type        = type;
				CommandRoot = commandRoot;
				CommandName = commandName;
			}
		}

		private const string Title         = "uGUI";
		private const float  WindowWidth  = 32;
		private const float  WindowHeight = 28;

		private static readonly GUILayoutOption[] ButtonOptions =
		{
			GUILayout.MinWidth( 28 ),
			GUILayout.MaxWidth( 48 ),
			GUILayout.Height( 24 ),
		};

		private static readonly Data[] DataList =
		{
			new Data( typeof( GameObject ), "GameObject/", "Create Empty Child" ),
			new Data( typeof( Text ), "GameObject/UI/", "Text - TextMeshPro" ),
			new Data( typeof( Image ), "GameObject/UI/", "Image" ),
			new Data( typeof( RawImage ), "GameObject/UI/", "Raw Image" ),
			new Data( typeof( Button ), "GameObject/UI/", "Button" ),
			new Data( typeof( Toggle ), "GameObject/UI/", "Toggle" ),
			new Data( typeof( Slider ), "GameObject/UI/", "Slider" ),
			new Data( typeof( Scrollbar ), "GameObject/UI/", "Scrollbar" ),
			new Data( typeof( Dropdown ), "GameObject/UI/", "Dropdown" ),
			new Data( typeof( InputField ), "GameObject/UI/", "Input Field" ),
			new Data( typeof( Canvas ), "GameObject/UI/", "Canvas" ),
			new Data( typeof( ScrollRect ), "GameObject/UI/", "Scroll View" ),
			new Data( typeof( Shadow ), "Component/UI/Effects/", "Shadow" ),
			new Data( typeof( Outline ), "Component/UI/Effects/", "Outline" ),
			new Data( typeof( PositionAsUV1 ), "Component/UI/Effects/", "Position As UV1" ),
			new Data( typeof( Mask ), "Component/UI/", "Mask" ),
			new Data( typeof( RectMask2D ), "Component/UI/", "Rect Mask 2D" ),
		};

		private bool IsVertical => false;

		[MenuItem( "Window/UniUGUIToolbar" )]
		private static void Init()
		{
			var win = GetWindow<UGUIToolbar>( Title );

			var pos = win.position;
			pos.width    = 640;
			pos.height   = WindowHeight;
			win.position = pos;

			win.minSize = new Vector2( WindowWidth, WindowHeight );

			var maxSize = win.maxSize;
			maxSize.y   = WindowHeight;
			win.maxSize = maxSize;
		}

		private void OnGUI()
		{
			if ( IsVertical )
			{
				GUILayout.BeginVertical();
			}
			else
			{
				GUILayout.BeginHorizontal();
			}

			foreach ( var n in DataList )
			{
				var type = n.Type;
				if ( type == null )
				{
					var options = IsVertical
							? new[] { GUILayout.MinWidth( 28 ), GUILayout.MaxWidth( 48 ), GUILayout.Height( 1 ) }
							: new[] { GUILayout.Height( 24 ), GUILayout.Width( 1 ) }
						;
					GUILayout.Box( string.Empty, options );
					continue;
				}

				var commandName = n.CommandName;
				var src         = EditorGUIUtility.ObjectContent( null, type );
				var content = new GUIContent( src )
				{
					text    = string.Empty,
					tooltip = commandName,
				};
				if ( !GUILayout.Button( content, ButtonOptions ) ) continue;
				var commandRoot  = n.CommandRoot;
				var menuItemPath = $"{commandRoot}{commandName}";
				EditorApplication.ExecuteMenuItem( menuItemPath );
			}

			if ( IsVertical )
			{
				GUILayout.EndVertical();
			}
			else
			{
				GUILayout.EndHorizontal();
			}
		}

	}
}