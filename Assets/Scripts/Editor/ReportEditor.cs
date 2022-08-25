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

        private void Initialize()
        {
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
            if (this._multiColumnHeader == null)
            {
                this.Initialize();
            }

            GUILayout.FlexibleSpace();

            Rect windowRect = GUILayoutUtility.GetLastRect();

            windowRect.width = this.position.width;
            windowRect.height = this.position.height;

            float columnHeight = EditorGUIUtility.singleLineHeight;

            Rect columnRectPrototype = new Rect(source: windowRect)
            {
                height = columnHeight, 
            };

            Rect positionalRectAreaOfScrollView = GUILayoutUtility.GetRect(0, float.MaxValue, 0, float.MaxValue);

            Rect viewRect = new Rect(source: windowRect)
            {
                xMax = this._columns.Sum((column) =>
                    column.width)
            };

            this._scrollPosition = GUI.BeginScrollView(
                position: positionalRectAreaOfScrollView,
                scrollPosition: this._scrollPosition,
                viewRect: viewRect,
                alwaysShowHorizontal: false,
                alwaysShowVertical: false
            );

            this._multiColumnHeader.OnGUI(rect: columnRectPrototype, xScroll: 0.0f);

            if (_testReports == null)
            {
                return;
            }

            for (int i = 0; i < _testReports.Count; i++)
            {

                Rect rowRect = new Rect(source: columnRectPrototype);

                rowRect.y += columnHeight * (i + 1);

                if (i % 2 == 0)
                    EditorGUI.DrawRect(rect: rowRect, color: this._darkerColor);
                else
                    EditorGUI.DrawRect(rect: rowRect, color: this._lighterColor);

                int columnIndex = 0;

                if (this._multiColumnHeader.IsColumnVisible(columnIndex: columnIndex))
                {
                    int visibleColumnIndex = this._multiColumnHeader.GetVisibleColumnIndex(columnIndex: columnIndex);

                    Rect columnRect = this._multiColumnHeader.GetColumnRect(visibleColumnIndex: visibleColumnIndex);

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