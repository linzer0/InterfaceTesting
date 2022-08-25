using InterfaceTesting;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class PopupWindow : EditorWindow
    {
        private GUIStyle _failedGUIStyle;
        private static TestReport _testReport;

        public static void ShowWindow()
        {
            GetWindow<PopupWindow>("Report info");
        }

        public static void ShowTest(TestReport testReport)
        {
            _testReport = testReport;
        }

        public void OnGUI()
        {
            if (_testReport.Name != null)
            {
                var report = "";
                report += $"{_testReport.Name} status is {_testReport.GetTestStatus()}\n{_testReport.Report}";
                GUILayout.Label(report);
            }
        }
    }
}