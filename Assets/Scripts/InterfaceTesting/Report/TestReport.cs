using System;

namespace InterfaceTesting
{
    [Serializable]
    public struct TestReport
    {
        public string Name;
        public int Id;
        public bool Failed;
        public string Report;

        public BaseTest BaseTest;

        public string GetTestStatus()
        {
            return Failed ? "Failed" : "Completed";
        }

        public string GetDescription()
        {
            return BaseTest.GetDescription();
        }

        public TestReport(int id, string name, bool failed, string report, BaseTest baseTest)
        {
            Name = name;
            Id = id;
            Failed = failed;
            Report = report;
            BaseTest = baseTest;
        }
    }
}