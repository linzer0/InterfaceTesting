using System.Collections.Generic;
using InterfaceTesting.Report;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class ReportEditor : EditorWindow
    {
        private static List<TestReport> _testReports;
        private GUIStyle _failedGUIStyle;
        private List<string> _rowNames = new List<string>() {"Test Name", "Description", "Test Status", "Report"};

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

        void OnGUI()
        {
            GUI.backgroundColor = Color.cyan;
            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();
            foreach (var rowName in _rowNames)
            {
                GUILayout.Label(rowName);
            }

            EditorGUILayout.EndHorizontal();

            if (_testReports != null)
            {
                foreach (var test in _testReports)
                {
                    EditorGUILayout.BeginHorizontal();

                    GUILayout.Label($"{test.Name}");

                    GUILayout.Label($"{test.GetDescription()}");

                    GUI.contentColor = test.Failed ? Color.red : Color.green;
                    GUILayout.Label($"{test.GetTestStatus()}");

                    GUI.contentColor = Color.white;

                    GUILayout.Label($"{test.Report}");

                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.EndVertical();
            EndWindows();
        }
    }
}