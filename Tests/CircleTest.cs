using System;
using System.Collections.Generic;
using NUnit.Framework;
using infogr_raytracer;
using OpenTK;

namespace Tests
{
    public class CircleTest
    {
        public struct CircleTestCase
        {
            public Ray Ray;
            public Circle Circle;
            public bool ExpectedResult;
            public string Description;
        }


        // private static Circle circle = new Circle()
        // {
        //     Position = new Vector2(2f,2f),
        //     Radius = 1f,
        // };
        //
        // private static List<(Ray, bool, string)> cases = new List<(Ray, bool, string)>()
        // {
        //     (new Ray() {Origin = new Vector2(0, 2), Direction = new Vector2(1, 0)}, true, "A"),
        //     (new Ray() {Origin = new Vector2(0, 2), Direction = new Vector2(1, 0)}, true, "B"),
        //     (new Ray() {Origin = new Vector2(3, 0), Direction = new Vector2(-1, 0), true, ""})
        // };
        
        private static List<CircleTestCase> testCases = new List<CircleTestCase>()
        {
            new CircleTestCase()
            {
                Circle = new Circle() { Position = new Vector2(0, 0), Radius = 1f},
                Ray = new Ray() { Origin = new Vector2(4, 0), Direction = new Vector2(-1, 0)},
                ExpectedResult = true,
                Description = "Intersection near origin"
            }
        };
        
        [Test]
        [TestCaseSource("testCases")]
        public void TestIntersect(CircleTestCase testCase)
        {
            Assert.AreEqual(testCase.Circle.Intersects(testCase.Ray), testCase.ExpectedResult, testCase.Description);
        }
    }
}