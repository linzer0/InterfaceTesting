using System.Collections.Generic;
using System.Linq;
using InterfaceTesting;
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
            _columns = new []
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

            _multiColumnHeader = new MultiColumnHeader(state: this._multiColumnHeaderState);

            _multiColumnHeader.visibleColumnsChanged += (multiColumnHeader) => multiColumnHeader.ResizeToFit();

            _multiColumnHeader.ResizeToFit();
        }

        private readonly Color _lighterColor = Color.white * 0.3f;
        private readonly Color _darkerColor = Color.white * 0.1f;

        private Vector2 _scrollPosition;

        private void OnGUI()
        {
            if (_multiColumnHeader == null)
            {
                Initialize();
            }

            GUILayout.FlexibleSpace();

            Rect windowRect = GUILayoutUtility.GetLastRect();

            windowRect.width = position.width;
            windowRect.height = position.height;

            float columnHeight = EditorGUIUtility.singleLineHeight;

            Rect columnRectPrototype = new Rect(source: windowRect)
            {
                height = columnHeight, 
            };

            Rect positionalRectAreaOfScrollView = GUILayoutUtility.GetRect(0, float.MaxValue, 0, float.MaxValue);

            Rect viewRect = new Rect(source: windowRect)
            {
                xMax = _columns.Sum((column) =>
                    column.width)
            };

            _scrollPosition = GUI.BeginScrollView(
                position: positionalRectAreaOfScrollView,
                scrollPosition: this._scrollPosition,
                viewRect: viewRect,
                alwaysShowHorizontal: false,
                alwaysShowVertical: false
            );

            _multiColumnHeader.OnGUI(rect: columnRectPrototype, xScroll: 0.0f);

            if (_testReports == null)
            {
                return;
            }

            for (int i = 0; i < _testReports.Count; i++)
            {

                Rect rowRect = new Rect(source: columnRectPrototype);

                rowRect.y += columnHeight * (i + 1);

                if (i % 2 == 0)
                    EditorGUI.DrawRect(rect: rowRect, color: _darkerColor);
                else
                    EditorGUI.DrawRect(rect: rowRect, color: _lighterColor);

                int columnIndex = 0;

                if (_multiColumnHeader.IsColumnVisible(columnIndex: columnIndex))
                {
                    int visibleColumnIndex = _multiColumnHeader.GetVisibleColumnIndex(columnIndex: columnIndex);

                    Rect columnRect = _multiColumnHeader.GetColumnRect(visibleColumnIndex: visibleColumnIndex);

                    columnRect.y = rowRect.y;

                    GUIStyle nameFieldGUIStyle = new GUIStyle(GUI.skin.label)
                    {
                        padding = new RectOffset(left: 10, right: 10, top: 2, bottom: 2)
                    };

                    EditorGUI.LabelField(
                        position: _multiColumnHeader.GetCellRect(visibleColumnIndex: visibleColumnIndex,
                            columnRect),
                        label: new GUIContent(_testReports[i].Name),
                        style: nameFieldGUIStyle
                    );
                }

                columnIndex = 1;

                if (_multiColumnHeader.IsColumnVisible(columnIndex: columnIndex))
                {
                    int visibleColumnIndex = _multiColumnHeader.GetVisibleColumnIndex(columnIndex: columnIndex);

                    Rect columnRect = _multiColumnHeader.GetColumnRect(visibleColumnIndex: visibleColumnIndex);

                    columnRect.y = rowRect.y;

                    EditorGUI.LabelField(
                        position: this._multiColumnHeader.GetCellRect(visibleColumnIndex: visibleColumnIndex,
                            columnRect),
                        _testReports[i].GetDescription());
                }

                columnIndex = 2;

                if (_multiColumnHeader.IsColumnVisible(columnIndex: columnIndex))
                {
                    int visibleColumnIndex = _multiColumnHeader.GetVisibleColumnIndex(columnIndex: columnIndex);

                    Rect columnRect = _multiColumnHeader.GetColumnRect(visibleColumnIndex: visibleColumnIndex);

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
            Initialize();
        }

        private void ShowTest(TestReport testReport)
        {
            PopupWindow.ShowWindow();
            PopupWindow.ShowTest(testReport);
        }
    }
}