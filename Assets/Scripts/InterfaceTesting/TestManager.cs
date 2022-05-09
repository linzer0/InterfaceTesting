using System.Collections.Generic;
using System.Threading;
using InterfaceTesting.Report;
using InterfaceTesting.Tests;
using UnityEditor;
using UnityEngine;

namespace InterfaceTesting
{
    public class TestManager : MonoBehaviour
    {
        [SerializeField] private List<BaseTest> _interfaceTests;
        [SerializeField] private ReportView _reportView;

        private int _failedTestAmount;
        private int _completedTestAmount;

        private BaseTest _currentTest;
        private int _index;

        private List<BaseTest> _failedTests;

        private List<TestReport> _testReports;

        public void Start()
        {
            _failedTests = new List<BaseTest>();
            _testReports = new List<TestReport>();
            RunTests();
            TestRunningBar();
        }

        private void TestRunningBar(float workTime = 2.0f)
        {
            var step = 0.1f;
            for (float t = 0; t < workTime; t += step)
            {
                EditorUtility.DisplayProgressBar("Test Running", "Wait for testing to complete...", t / workTime);
                Thread.Sleep((int) (step * 1000.0f));
            }

            EditorUtility.ClearProgressBar();
        }


        public void RunTests()
        {
            _failedTests.Clear();
            _index = 0;
            foreach (var interfaceTest in _interfaceTests)
            {
                _currentTest = interfaceTest;
                interfaceTest.OnTestCompleted += TestCompleted;
                interfaceTest.OnTestFail += TestFailed;

                interfaceTest.RunTest();

                interfaceTest.OnTestCompleted -= TestCompleted;
                interfaceTest.OnTestFail -= TestFailed;
                _index++;
            }

            MakeReport();
        }

        private void CreateTestReport(int index, string testName, bool testFailed, string reportText = "")
        {
            var testReport = new TestReport(index, testName, testFailed, reportText, _currentTest);
            _testReports.Add(testReport);
        }

        private void MakeReport()
        {
            _reportView.GenerateReport(_testReports);
        }

        private void TestCompleted()
        {
            CreateTestReport(_index, _currentTest.name, false);
        }

        private void TestFailed(string report)
        {
            CreateTestReport(_index, _currentTest.name, true, report);
        }
    }
}