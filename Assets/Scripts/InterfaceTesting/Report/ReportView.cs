using System.Collections.Generic;
using UnityEngine;

namespace InterfaceTesting
{
    public class ReportView : MonoBehaviour
    {
        public delegate void BringWindow();

        public static BringWindow OnBringUpWindow;

        public delegate void SetTests(List<TestReport> testReports);

        public static SetTests SetFailedTest;


        public void GenerateReport(List<TestReport> testReports)
        {
            if (OnBringUpWindow != null)
            {
                SetFailedTest(testReports);
                OnBringUpWindow();
            }
        }
    }
}