﻿using System;
using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace TokenAuthWebApiCore.Server.IntegrationTest.Setup
{
    public class PriorityOrderer : ITestCaseOrderer
    {
        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases)
            where TTestCase : ITestCase
        {
            var sortedMethods = new SortedDictionary<int, TTestCase>(new DuplicateKeyComparer<int>());

            foreach (TTestCase testCase in testCases)
            {
                IAttributeInfo attribute = testCase.TestMethod.Method.
                GetCustomAttributes((typeof(TestPriorityAttribute)
                .AssemblyQualifiedName)).FirstOrDefault();

                var priority = attribute.GetNamedArgument<int>("Priority");
                sortedMethods.Add(priority, testCase);
            }

            return sortedMethods.Values;
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TestPriorityAttribute : Attribute
    {
        private int Priority { get; }

        public TestPriorityAttribute(int priority)
        {
            Priority = priority;
        }
    }

    public class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable
    {
        public int Compare(TKey x, TKey y)
        {
            int result = x.CompareTo(y);

            if (result == 0)
                return 1;
            else
                return result;
        }
    }
}