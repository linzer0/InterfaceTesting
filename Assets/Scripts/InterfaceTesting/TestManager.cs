using System.Collections.Generic;
using InterfaceTesting.Report;
using InterfaceTesting.Tests;
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
            // Debug.Log($"{_currentTest.name} completed");
        }

        private void TestFailed(string report)
        {
            CreateTestReport(_index, _currentTest.name, true, report);
            // Debug.Log($"{_currentTest.name} is failed\nReport {report}");
        }
    }
}