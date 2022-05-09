using System.Collections.Generic;
using System.Linq;
using InterfaceTesting.Report;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Editor
{
    public class ReportEditor : EditorWindow
    {
        private static List<TestReport> _testReports;
        private GUIStyle _failedGUIStyle;

        [InitializeOnLoadMethod]
        static void Init()
        {
            ReportView.OnBringUpWindow = ShowWindow;
            ReportView.SetFailedTest = SetTests;
        }

        public static void ShowWindow()
        {
            GetWindow<ReportEditor>("Testing report");
        }

        public static void SetTests(List<TestReport> baseTest)
        {
            _testReports = baseTest;
            Debug.Log($"Tests updated\nSize is {baseTest.Count}");
        }

        private MultiColumnHeaderState _multiColumnHeaderState;
        private MultiColumnHeader _multiColumnHeader;

        private MultiColumnHeaderState.Column[] _columns;

        // Create a few test subjects.
        private void Initialize()
        {
            // We can move these columns into some ScriptableObject or some other data saving object/file to save their properties there, otherwise because of some events these settings will be recreated and state of the window won't be saved as expected.
            this._columns = new MultiColumnHeaderState.Column[]
            {
                new MultiColumnHeaderState.Column()
                {
                    headerContent = new GUIContent("Test Name"),
                    width = 200,
                    minWidth = 100,
                    maxWidth = 500,
                    autoResize = true,
                    headerTextAlignment = TextAlignment.Center
                },
                new MultiColumnHeaderState.Column()
                {
                    headerContent = new GUIContent("Description"),
                    width = 250,
                    minWidth = 100,
                    maxWidth = 500,
                    autoResize = true,
                    headerTextAlignment = TextAlignment.Center
                },
                new MultiColumnHeaderState.Column()
                {
                    headerContent = new GUIContent("Status"),
                    width = 250,
                    minWidth = 100,
                    maxWidth = 500,
                    autoResize = true,
                    headerTextAlignment = TextAlignment.Center
                },
            };

            _multiColumnHeaderState = new MultiColumnHeaderState(columns: this._columns);

            this._multiColumnHeader = new MultiColumnHeader(state: this._multiColumnHeaderState);

            this._multiColumnHeader.visibleColumnsChanged += (multiColumnHeader) => multiColumnHeader.ResizeToFit();

            this._multiColumnHeader.ResizeToFit();
        }

        private readonly Color _lighterColor = Color.white * 0.3f;
        private readonly Color _darkerColor = Color.white * 0.1f;

        private Vector2 _scrollPosition;

        private void OnGUI()
        {
            // After compilation and some other events data of the window is lost if it's not saved in some kind of container. Usually those containers are ScriptableObject(s).
            if (this._multiColumnHeader == null)
            {
                this.Initialize();
            }

            // Basically we just draw something. Empty space. Which is `FlexibleSpace` here on top of the window.
            // We need this for - `GUILayoutUtility.GetLastRect()` because it needs at least 1 thing to be drawn before it.
            GUILayout.FlexibleSpace();

            // Get automatically aligned rect for our multi column header component.
            Rect windowRect = GUILayoutUtility.GetLastRect();

            // Here we are basically assigning the size of window to our newly positioned `windowRect`.
            windowRect.width = this.position.width;
            windowRect.height = this.position.height;

            float columnHeight = EditorGUIUtility.singleLineHeight;

            // This is a rect for our multi column table.
            Rect columnRectPrototype = new Rect(source: windowRect)
            {
                height = columnHeight, // This is basically a height of each column including header.
            };

            // Just enormously large view if you want it to span for the whole window. This is how it works [shrugs in confusion].
            Rect positionalRectAreaOfScrollView = GUILayoutUtility.GetRect(0, float.MaxValue, 0, float.MaxValue);

            // Create a `viewRect` since it should be separate from `rect` to avoid circular dependency.
            Rect viewRect = new Rect(source: windowRect)
            {
                xMax = this._columns.Sum((column) =>
                    column.width) // Scroll max on X is basically a sum of width of columns.
            };

            this._scrollPosition = GUI.BeginScrollView(
                position: positionalRectAreaOfScrollView,
                scrollPosition: this._scrollPosition,
                viewRect: viewRect,
                alwaysShowHorizontal: false,
                alwaysShowVertical: false
            );

            // Draw header for columns here.
            this._multiColumnHeader.OnGUI(rect: columnRectPrototype, xScroll: 0.0f);

            // For each element that we have in object that we are modifying.
            //? I don't have an appropriate object here to modify, but this is just an example. In real world case I would probably use ScriptableObject here.
            if (_testReports == null)
            {
                return;
            }

            for (int i = 0; i < _testReports.Count; i++)
            {
                //! We draw each type of field here separately because each column could require a different type of field as seen here.
                // This can be improved if we want to have a more robust system. Like for example, we could have logic of drawing each field moved to object itself.
                // Then here we would be able to just iterate through array of these objects and call a draw methods for these fields and use this window for many types of objects.
                // But example with such a system would be too complicated for gamedev.stackexchange, so I have decided to not overengineer and just use hard coded indices for columns - `columnIndex`.

                Rect rowRect = new Rect(source: columnRectPrototype);

                rowRect.y += columnHeight * (i + 1);

                // Draw a texture before drawing each of the fields for the whole row.
                if (i % 2 == 0)
                    EditorGUI.DrawRect(rect: rowRect, color: this._darkerColor);
                else
                    EditorGUI.DrawRect(rect: rowRect, color: this._lighterColor);

                // Name field.
                int columnIndex = 0;

                if (this._multiColumnHeader.IsColumnVisible(columnIndex: columnIndex))
                {
                    int visibleColumnIndex = this._multiColumnHeader.GetVisibleColumnIndex(columnIndex: columnIndex);

                    Rect columnRect = this._multiColumnHeader.GetColumnRect(visibleColumnIndex: visibleColumnIndex);

                    // This here basically is a row height, you can make it any value you like. Or you could calculate the max field height here that your object has and store it somewhere then use it here instead of `EditorGUIUtility.singleLineHeight`.
                    // We move position of field on `y` by this height to get correct position.
                    columnRect.y = rowRect.y;

                    GUIStyle nameFieldGUIStyle = new GUIStyle(GUI.skin.label)
                    {
                        padding = new RectOffset(left: 10, right: 10, top: 2, bottom: 2)
                    };

                    EditorGUI.LabelField(
                        position: this._multiColumnHeader.GetCellRect(visibleColumnIndex: visibleColumnIndex,
                            columnRect),
                        label: new GUIContent(_testReports[i].Name),
                        style: nameFieldGUIStyle
                    );
                }

                // Health slider field.
                columnIndex = 1;

                if (this._multiColumnHeader.IsColumnVisible(columnIndex: columnIndex))
                {
                    int visibleColumnIndex = this._multiColumnHeader.GetVisibleColumnIndex(columnIndex: columnIndex);

                    Rect columnRect = this._multiColumnHeader.GetColumnRect(visibleColumnIndex: visibleColumnIndex);

                    columnRect.y = rowRect.y;

                    EditorGUI.LabelField(
                        position: this._multiColumnHeader.GetCellRect(visibleColumnIndex: visibleColumnIndex,
                            columnRect),
                        _testReports[i].GetDescription());
                }

                // Skin color field.
                columnIndex = 2;

                if (this._multiColumnHeader.IsColumnVisible(columnIndex: columnIndex))
                {
                    int visibleColumnIndex = this._multiColumnHeader.GetVisibleColumnIndex(columnIndex: columnIndex);

                    Rect columnRect = this._multiColumnHeader.GetColumnRect(visibleColumnIndex: visibleColumnIndex);

                    columnRect.y = rowRect.y;

                    if (GUI.Button(_multiColumnHeader.GetCellRect(visibleColumnIndex: visibleColumnIndex, columnRect),
                        _testReports[i].GetTestStatus()))
                    {
                        ShowTest(_testReports[i]);
                    }

                    // EditorGUI.LabelField(
                    // _multiColumnHeader.GetCellRect(visibleColumnIndex: visibleColumnIndex, columnRect),
                    // _testReports[i].GetTestStatus());
                }
            }

            GUI.EndScrollView(handleScrollWheel: true);
        }

        private void Awake()
        {
            this.Initialize();
        }

        private void ShowTest(TestReport testReport)
        {
            PopupWindow.ShowWindow();
            PopupWindow.ShowTest(testReport);
        }
    }
}