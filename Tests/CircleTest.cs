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

            public override string ToString()
            {
                return Description;
            }
        }

        private static List<CircleTestCase> testCases = new List<CircleTestCase>()
        {
            new CircleTestCase()
            {
                Circle = new Circle() { Position = new Vector2(0, 0), Radius = 1f},
                Ray = new Ray() { Origin = new Vector2(4, 0), Direction = new Vector2(-1, 0)},
                ExpectedResult = true,
                Description = "Intersection near origin"
            },
            new CircleTestCase()
            {
                Circle = new Circle() { Position = new Vector2(2, 2), Radius = 1f},
                Ray = new Ray() { Origin = new Vector2(0, 2), Direction = new Vector2(1, 0)},
                ExpectedResult = true,
                Description = "Orthogonal from left"
            },
            new CircleTestCase()
            {
                Circle = new Circle() { Position = new Vector2(2, 2), Radius = 1f},
                Ray = new Ray() { Origin = new Vector2(2, 4), Direction = new Vector2(0, -1)},
                ExpectedResult = true,
                Description = "Orthogonal from top"
            },
            new CircleTestCase()
            {
                Circle = new Circle() { Position = new Vector2(2, 2), Radius = 1f},
                Ray = new Ray() { Origin = new Vector2(4, 2), Direction = new Vector2(-1, 0)},
                ExpectedResult = true,
                Description = "Orthogonal from right"
            },
            new CircleTestCase()
            {
                Circle = new Circle() { Position = new Vector2(2, 2), Radius = 1f},
                Ray = new Ray() { Origin = new Vector2(2, -1), Direction = new Vector2(0, 1)},
                ExpectedResult = true,
                Description = "Orthogonal from bottom"
            },
            new CircleTestCase()
            {
                Circle = new Circle() { Position = new Vector2(0, 0), Radius = 1f},
                Ray = new Ray() { Origin = new Vector2(4, 4), Direction = new Vector2(1, -1)},
                ExpectedResult = true,
                Description = "Orthogonal from top left"
            },
            new CircleTestCase()
            {
                Circle = new Circle() { Position = new Vector2(0, 0), Radius = 1f},
                Ray = new Ray() { Origin = new Vector2(4, -4), Direction = new Vector2(1, 1)},
                ExpectedResult = true,
                Description = "Orthogonal from bottom left"
            },
            new CircleTestCase()
            {
                Circle = new Circle() { Position = new Vector2(0, 0), Radius = 1f},
                Ray = new Ray() { Origin = new Vector2(4, 4), Direction = new Vector2(-1, -1)},
                ExpectedResult = true,
                Description = "Orthogonal from top right"
            },
            new CircleTestCase()
            {
                Circle = new Circle() { Position = new Vector2(0, 0), Radius = 1f},
                Ray = new Ray() { Origin = new Vector2(4, -4), Direction = new Vector2(1, -1)},
                ExpectedResult = true,
                Description = "Orthogonal from bottom right"
            }
        };
        
        [Test]
        [TestCaseSource("testCases")]
        public void TestIntersect(CircleTestCase testCase)
        {
            Console.WriteLine(testCase.Description);
            Assert.AreEqual(testCase.Circle.Intersects(testCase.Ray), testCase.ExpectedResult, testCase.Description);
        }
    }
}